using System;

namespace BuzzIO
{
    public interface IBuzzHandsetDevice
    {
        /// <summary>
        /// Event fired when one or more button state changes
        /// </summary>
        event BuzzButtonChangedEventHandler ButtonChanged;

        /// <summary>
        /// Sets the states of the handsets lamps on or off.
        /// </summary>
        /// <param name="bLight1">Turn light on handset 1 on/off</param>
        /// <param name="bLight2">Turn light on handset 2 on/off</param>
        /// <param name="bLight3">Turn light on handset 3 on/off</param>
        /// <param name="bLight4">Turn light on handset 4 on/off</param>
        void SetLights(bool bLight1, bool bLight2, bool bLight3, bool bLight4);

        /// <summary>
        /// Event handler called when device has been removed
        /// </summary>
        event EventHandler DeviceRemoved;

        string ProductString { get; }
    }
}