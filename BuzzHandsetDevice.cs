using System;
using System.Collections.Generic;

namespace BuzzWin
{
	/// <summary>
	/// Class that defines a controller of Buzz handsets : Representation of the USB hardware device
	/// </summary>
    public class BuzzHandsetDevice : HIDDevice
    {
		#region Public attributes/methods
		/// <summary>
		/// Event fired when one or more button state changes
		/// </summary>
		public event BuzzButtonChangedEventHandler OnButtonChanged;
		/// <summary>
		/// Creates an input report for use in the HID device framework
		/// </summary>
		/// <returns>A new input report for this device</returns>
		public override InputReport CreateInputReport()
		{
			return new BuzzInputReport(this);
		}
		/// <summary>
		/// Sets the states of the handsets lamps on or off.
		/// </summary>
		/// <param name="bLight1">Turn light on handset 1 on/off</param>
		/// <param name="bLight2">Turn light on handset 2 on/off</param>
		/// <param name="bLight3">Turn light on handset 3 on/off</param>
		/// <param name="bLight3">Turn light on handset 4 on/off</param>
		public void SetLights(bool bLight1, bool bLight2, bool bLight3, bool bLight4)
        {
            BuzzOutputReport oRep = new BuzzOutputReport(this);	// create output report
			oRep.SetLightStates(bLight1, bLight2, bLight3, bLight4);	// set the lights states
			try
			{
				Write(oRep); // write the output report
			}
			catch(HIDDeviceException)
			{
				// Device may have been removed!
			}
        }
		/// <summary>
		/// Finds the Buzz handset. 
		/// </summary>
		/// <returns>A new BuzzHandsetDevice or null if not found.</returns>
        public static BuzzHandsetDevice FindBuzzHandset()
        {
			// VID and PID for Buzz wired device are 0x054c and 0x1000 respectively
            // VID and PID for Buzz wireless device are 0x054c and 2 respectively
            BuzzHandsetDevice device = (BuzzHandsetDevice)FindDevice(0x054c, 0x1000, typeof(BuzzHandsetDevice));
            if (device == null)
                device = (BuzzHandsetDevice)FindDevice(0x054c, 2, typeof(BuzzHandsetDevice));

            return device;
        }

        /// <summary>
        /// Finds multiple Buzz handsets.
        /// </summary>
        /// <returns>A list of BuzzHandsetDevice instances or empty list if none found.</returns>
        public static List<BuzzHandsetDevice> FindBuzzHandsets()
        {
            List<BuzzHandsetDevice> list = new List<BuzzHandsetDevice>();

            foreach (HIDDevice device in HIDDevice.FindDevices(0x54c, 0x1000, typeof(BuzzHandsetDevice)))
                list.Add((BuzzHandsetDevice)device);

            foreach (HIDDevice device in HIDDevice.FindDevices(0x54c, 2, typeof(BuzzHandsetDevice)))
                list.Add((BuzzHandsetDevice)device);

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
			// Fire the event handler if assigned
			if (OnButtonChanged != null)
			{
				BuzzInputReport oBuzIn = (BuzzInputReport)oInRep;
				OnButtonChanged(this, new BuzzButtonChangedEventArgs(oBuzIn.Buttons));
			}
		}
		/// <summary>
		/// Dispose.
		/// </summary>
		/// <param name="bDisposing">True if object is being disposed - else is being finalised</param>
		protected override void Dispose(bool bDisposing)
		{
			if ( bDisposing )
			{
				// before we go, turn all lights off
				SetLights(false, false, false, false);
			}
			base.Dispose(bDisposing);
		}
		#endregion
    }
}
