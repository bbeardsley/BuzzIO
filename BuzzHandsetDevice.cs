using System.Collections.Generic;
using System.Linq;

namespace BuzzIO
{
    /// <summary>
    /// Class that defines a controller of Buzz handsets : Representation of the USB hardware device
    /// </summary>
    public class BuzzHandsetDevice : HIDDevice, IBuzzHandsetDevice
    {
        #region Public attributes/methods
        /// <summary>
        /// Event fired when one or more button state changes
        /// </summary>
        public event BuzzButtonChangedEventHandler ButtonChanged;
        protected virtual void OnButtonChanged(BuzzButtonChangedEventArgs e)
        {
            var handler = ButtonChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Creates an input report for use in the HID device framework
        /// </summary>
        /// <returns>A new input report for this device</returns>
        protected override InputReport CreateInputReport()
        {
            return new BuzzInputReport();
        }
        /// <summary>
        /// Sets the states of the handsets lamps on or off.
        /// </summary>
        /// <param name="bLight1">Turn light on handset 1 on/off</param>
        /// <param name="bLight2">Turn light on handset 2 on/off</param>
        /// <param name="bLight3">Turn light on handset 3 on/off</param>
        /// <param name="bLight4">Turn light on handset 4 on/off</param>
        public void SetLights(bool bLight1, bool bLight2, bool bLight3, bool bLight4)
        {
            if (_deviceWasRemoved)
                return;

            var oRep = new BuzzOutputReport(this);	// create output report
            oRep.SetLightStates(bLight1, bLight2, bLight3, bLight4);	// set the lights states
            try
            {
                Write(oRep); // write the output report
            }
            catch
            {
                // Device may have been removed!
            }
        }
        /// <summary>
        /// Finds the Buzz handset. 
        /// </summary>
        /// <returns>A new BuzzHandsetDevice or null if not found.</returns>
        public static IBuzzHandsetDevice FindBuzzHandset()
        {
            // VID and PID for Buzz wired device are 0x054c and 0x1000 respectively
            // VID and PID for Buzz wireless device are 0x054c and 2 respectively
            var device = (IBuzzHandsetDevice)FindDevice(0x054c, 0x1000, typeof(BuzzHandsetDevice)) ??
                         (IBuzzHandsetDevice)FindDevice(0x054c, 2, typeof(BuzzHandsetDevice));

            return device;
        }

        /// <summary>
        /// Finds multiple Buzz handsets.
        /// </summary>
        /// <returns>A list of BuzzHandsetDevice instances or empty list if none found.</returns>
        public static List<IBuzzHandsetDevice> FindBuzzHandsets()
        {
            var list = FindDevices(0x54c, 0x1000, typeof (BuzzHandsetDevice)).Cast<IBuzzHandsetDevice>().ToList();
            list.AddRange(FindDevices(0x54c, 2, typeof (BuzzHandsetDevice)).Cast<IBuzzHandsetDevice>());
            return list;
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Fired when data has been received over USB
        /// </summary>
        /// <param name="oInRep">Input report received</param>
        protected override void HandleDataReceived(InputReport oInRep)
        {
            OnButtonChanged(new BuzzButtonChangedEventArgs(((BuzzInputReport)oInRep).Buttons));
        }

        private bool _deviceWasRemoved;
        protected override void HandleDeviceRemoved()
        {
            _deviceWasRemoved = true;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">True if object is being disposed - else is being finalized</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // before we go, turn all lights off
                SetLights(false, false, false, false);
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
