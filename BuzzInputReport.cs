using System;

namespace BuzzWin
{
    /// <summary>
    /// Input report for Buzz Handset device
    /// </summary>
    public class BuzzInputReport : InputReport
    {
        /// <summary>Number of handsets</summary>
        public const int NUM_HANDSETS = 4;
        /// <summary>Decoded button states</summary>
        private ButtonStates[] _buttons;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Constructing device</param>
        public BuzzInputReport(HIDDevice oDev)
            : base(oDev)
        {
            // Create the button states
            _buttons = new ButtonStates[NUM_HANDSETS];
            for (int i = 0; i < NUM_HANDSETS; i++)
                _buttons[i] = new ButtonStates();
        }
        /// <summary>
        /// Process the raw data from the actual input report bytes
        /// </summary>
        public override void ProcessData()
        {
            byte[] arrData = Buffer;
            _buttons[0].Red = ((arrData[3] & 0x01) != 0);
            _buttons[0].Yellow = ((arrData[3] & 0x02) != 0);
            _buttons[0].Green = ((arrData[3] & 0x04) != 0);
            _buttons[0].Orange = ((arrData[3] & 0x08) != 0);
            _buttons[0].Blue = ((arrData[3] & 0x10) != 0);

            _buttons[1].Red = ((arrData[3] & 0x20) != 0);
            _buttons[1].Yellow = ((arrData[3] & 0x40) != 0);
            _buttons[1].Green = ((arrData[3] & 0x80) != 0);
            _buttons[1].Orange = ((arrData[4] & 0x01) != 0);
            _buttons[1].Blue = ((arrData[4] & 0x02) != 0);

            _buttons[2].Red = ((arrData[4] & 0x04) != 0);
            _buttons[2].Yellow = ((arrData[4] & 0x08) != 0);
            _buttons[2].Green = ((arrData[4] & 0x10) != 0);
            _buttons[2].Orange = ((arrData[4] & 0x20) != 0);
            _buttons[2].Blue = ((arrData[4] & 0x40) != 0);

            _buttons[3].Red = ((arrData[4] & 0x80) != 0);
            _buttons[3].Yellow = ((arrData[5] & 0x01) != 0);
            _buttons[3].Green = ((arrData[5] & 0x02) != 0);
            _buttons[3].Orange = ((arrData[5] & 0x04) != 0);
            _buttons[3].Blue = ((arrData[5] & 0x08) != 0);
        }
        /// <summary>
        /// Accessor for the button states
        /// </summary>
        public ButtonStates[] Buttons
        {
            get { return _buttons; }
        }
    }
}
