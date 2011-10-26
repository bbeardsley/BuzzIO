using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace BuzzIO
{
    /// <summary>
    /// Abstract HID device : Derive your new device controller class from this
    /// </summary>
    public abstract class HIDDevice : Win32Usb, IDisposable
    {
        #region Properties
        /// <summary>Filestream we can use to read/write from</summary>
        private FileStream _file;

        /// <summary>Handle to the device</summary>
        private IntPtr _handle;

        /// <summary>
        /// Length of output report : device gives us this
        /// </summary>
        public int OutputReportLength { get; private set; }

        /// <summary>
        /// Length of input report : device gives us this
        /// </summary>
        public int InputReportLength { get; private set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string ProductString { get; private set; }

        #endregion Properties

        #region IDisposable Members
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposer called by both dispose and finalise
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)	// if we are disposing, need to close the managed resources
            {
                if (_file != null)
                {
                    _file.Close();
                    _file = null;
                }
            }
            if (_handle != IntPtr.Zero)	// Dispose and finalize, get rid of unmanaged resources
                CloseHandle(_handle);
        }
        #endregion

        #region Privates/protected
        /// <summary>
        /// Initialises the device
        /// </summary>
        /// <param name="strPath">Path to the device</param>
        private void Initialise(string strPath)
        {
            // Create the file from the device path
            _handle = CreateFile(strPath, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            if (_handle != InvalidHandleValue)	// if the open worked...
            {
                IntPtr lpData;
                if (HidD_GetPreparsedData(_handle, out lpData))	// get windows to read the device data into an internal buffer
                {
                    try
                    {
                        var buffer = Marshal.AllocHGlobal(126);//max alloc for string;
                        ProductString = HidD_GetProductString(_handle, buffer, 126) ? Marshal.PtrToStringAuto(buffer) : string.Empty;
                        Marshal.FreeHGlobal(buffer); 

                        HidCaps oCaps;
                        HidP_GetCaps(lpData, out oCaps);	// extract the device capabilities from the internal buffer
                        InputReportLength = oCaps.InputReportByteLength;	// get the input...
                        OutputReportLength = oCaps.OutputReportByteLength;	// ... and output report lengths
                        _file = new FileStream(new Microsoft.Win32.SafeHandles.SafeFileHandle(_handle, true), FileAccess.Read | FileAccess.Write, InputReportLength, true);	// wrap the file handle in a .Net file stream
                        BeginAsyncRead();	// kick off the first asynchronous read
                    }
                    finally
                    {
                        HidD_FreePreparsedData(ref lpData);	// before we quit the funtion, we must free the internal buffer reserved in GetPreparsedData
                    }
                }
                else	// GetPreparsedData failed? Chuck an exception
                {
                    throw HIDDeviceException.GenerateWithWinError("GetPreparsedData failed");
                }
            }
            else	// File open failed? Chuck an exception
            {
                _handle = IntPtr.Zero;
                throw HIDDeviceException.GenerateWithWinError("Failed to create device file");
            }
        }
        /// <summary>
        /// Kicks off an asynchronous read which completes when data is read or when the device
        /// is disconnected. Uses a callback.
        /// </summary>
        private void BeginAsyncRead()
        {
            var arrInputReport = new byte[InputReportLength];
            // put the buff we used to receive the stuff as the async state then we can get at it when the read completes
            _file.BeginRead(arrInputReport, 0, InputReportLength, ReadCompleted, arrInputReport);
        }
        /// <summary>
        /// Callback for above. Care with this as it will be called on the background thread from the async read
        /// </summary>
        /// <param name="iResult">Async result parameter</param>
        protected void ReadCompleted(IAsyncResult iResult)
        {
            var arrBuff = (byte[])iResult.AsyncState;	// retrieve the read buffer
            try
            {
                _file.EndRead(iResult);	// call end read : this throws any exceptions that happened during the read
                try
                {
                    var oInRep = CreateInputReport();	// Create the input report for the device
                    oInRep.SetData(arrBuff);	// and set the data portion - this processes the data received into a more easily understood format depending upon the report type
                    HandleDataReceived(oInRep);	// pass the new input report on to the higher level handler
                }
                finally
                {
                    BeginAsyncRead();	// when all that is done, kick off another read for the next report
                }
            }
            catch (IOException)	// if we got an IO exception, the device was removed
            {
                _handle = IntPtr.Zero;
                HandleDeviceRemoved();
                OnDeviceRemoved(EventArgs.Empty);
                Dispose();
            }
        }
        /// <summary>
        /// Write an output report to the device.
        /// </summary>
        /// <param name="oOutRep">Output report to write</param>
        protected void Write(OutputReport oOutRep)
        {
            try
            {
                _file.Write(oOutRep.Buffer, 0, oOutRep.BufferLength);
            }
            catch (IOException)
            {
                // The device was removed!
                _handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// virtual handler for any action to be taken when data is received. Override to use.
        /// </summary>
        /// <param name="oInRep">The input report that was received</param>
        protected abstract void HandleDataReceived(InputReport oInRep);

        /// <summary>
        /// Virtual handler for any action to be taken when a device is removed. Override to use.
        /// </summary>
        protected abstract void HandleDeviceRemoved();

        /// <summary>
        /// Virtual method to create an input report for this device. Override to use.
        /// </summary>
        /// <returns>A shiny new input report</returns>
        protected abstract InputReport CreateInputReport();

            /// <summary>
        /// Helper method to return the device path given a DeviceInterfaceData structure and an InfoSet handle.
        /// Used in 'FindDevice' so check that method out to see how to get an InfoSet handle and a DeviceInterfaceData.
        /// </summary>
        /// <param name="hInfoSet">Handle to the InfoSet</param>
        /// <param name="oInterface">DeviceInterfaceData structure</param>
        /// <returns>The device path or null if there was some problem</returns>
        private static string GetDevicePath(IntPtr hInfoSet, ref DeviceInterfaceData oInterface)
        {
            uint nRequiredSize = 0;
            // Get the device interface details
            if (!SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero))
            {
                var oDetail = new DeviceInterfaceDetailData
                {
                    Size = 5	// hardcoded to 5! Sorry, but this works and trying more future proof versions by setting the size to the struct sizeof failed miserably. If you manage to sort it, mail me! Thx
                };

                if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero))
                    return oDetail.DevicePath;
            }
            return null;
        }
        #endregion

        #region Public static
        /// <summary>
        /// Finds a device given its PID and VID
        /// </summary>
        /// <param name="nVid">Vendor id for device (VID)</param>
        /// <param name="nPid">Product id for device (PID)</param>
        /// <param name="oType">Type of device class to create</param>
        /// <returns>A new device class of the given type or null</returns>
        public static HIDDevice FindDevice(int nVid, int nPid, Type oType)
        {
            var searchPath = GetSearchPath(nVid, nPid); // first, build the path search string
            Guid gHid;
            HidD_GetHidGuid(out gHid);	// next, get the GUID from Windows that it uses to represent the HID USB interface
            var hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);	// this gets a list of all HID devices currently connected to the computer (InfoSet)
            try
            {
                var oInterface = new DeviceInterfaceData();	// build up a device interface data block
                oInterface.Size = Marshal.SizeOf(oInterface);
                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs
                // to get device details for each device connected
                for (var i = 0; SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)i, ref oInterface); i++)	// this gets the device interface information for a device at index 'i' in the memory block
                {
                    var strDevicePath = GetDevicePath(hInfoSet, ref oInterface);	// get the device path (see helper method 'GetDevicePath')
                    if (strDevicePath.IndexOf(searchPath) < 0)
                        continue;

                    var oNewDevice = (HIDDevice)Activator.CreateInstance(oType);	// create an instance of the class for this device
                    oNewDevice.Initialise(strDevicePath);	// initialise it with the device path
                    return oNewDevice;	// and return it
                }
            }
            finally
            {
                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs
                SetupDiDestroyDeviceInfoList(hInfoSet);
            }
            return null;	// oops, didn't find our device
        }

        public static List<HIDDevice> FindDevices(int nVid, int nPid, Type oType)
        {
            var list = new List<HIDDevice>();
            var searchPath = GetSearchPath(nVid, nPid); // first, build the path search string
            Guid gHid;
            HidD_GetHidGuid(out gHid);	// next, get the GUID from Windows that it uses to represent the HID USB interface
            var hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);	// this gets a list of all HID devices currently connected to the computer (InfoSet)
            try
            {
                var oInterface = new DeviceInterfaceData();	// build up a device interface data block
                oInterface.Size = Marshal.SizeOf(oInterface);
                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs
                // to get device details for each device connected
                for (var i = 0; SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)i, ref oInterface); i++)	// this gets the device interface information for a device at index 'nIndex' in the memory block
                {
                    var strDevicePath = GetDevicePath(hInfoSet, ref oInterface);	// get the device path (see helper method 'GetDevicePath')
                    if (strDevicePath.IndexOf(searchPath) < 0)
                        continue;

                    var oNewDevice = (HIDDevice)Activator.CreateInstance(oType);	// create an instance of the class for this device
                    try
                    {
                        oNewDevice.Initialise(strDevicePath);	// initialise it with the device path
                        list.Add(oNewDevice);
                    }
                    catch
                    {
                    }
                }
            }
            finally
            {
                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs
                SetupDiDestroyDeviceInfoList(hInfoSet);
            }
            return list;
        }

        private static string GetSearchPath(int nVid, int nPid)
        {
            return string.Format("vid_{0:x4}&pid_{1:x4}", nVid, nPid);
        }

        #endregion

        #region Events
        /// <summary>
        /// Event handler called when device has been removed
        /// </summary>
        public event EventHandler DeviceRemoved;
        protected virtual void OnDeviceRemoved(EventArgs e)
        {
            if (DeviceRemoved != null)
                DeviceRemoved(this, e);
        }

        #endregion
    }
}
