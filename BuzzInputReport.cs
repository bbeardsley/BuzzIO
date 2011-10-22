namespace BuzzIO
{
    /// <summary>
    /// Input report for Buzz Handset device
    /// </summary>
    public class BuzzInputReport : InputReport
    {
        /// <summary>Number of handsets</summary>
        public const int NumHandsets = 4;

        /// <summary>
        /// Decoded button states
        /// </summary>
        public ButtonStates[] Buttons { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BuzzInputReport()
        {
            // Create the button states
            Buttons = new ButtonStates[NumHandsets];
            for (var i = 0; i < NumHandsets; i++)
                Buttons[i] = new ButtonStates();
        }
        /// <summary>
        /// Process the raw data from the actual input report bytes
        /// </summary>
        public override void ProcessData()
        {
            var arrData = Buffer;
            Buttons[0].Red = ((arrData[3] & 0x01) != 0);
            Buttons[0].Yellow = ((arrData[3] & 0x02) != 0);
            Buttons[0].Green = ((arrData[3] & 0x04) != 0);
            Buttons[0].Orange = ((arrData[3] & 0x08) != 0);
            Buttons[0].Blue = ((arrData[3] & 0x10) != 0);

            Buttons[1].Red = ((arrData[3] & 0x20) != 0);
            Buttons[1].Yellow = ((arrData[3] & 0x40) != 0);
            Buttons[1].Green = ((arrData[3] & 0x80) != 0);
            Buttons[1].Orange = ((arrData[4] & 0x01) != 0);
            Buttons[1].Blue = ((arrData[4] & 0x02) != 0);

            Buttons[2].Red = ((arrData[4] & 0x04) != 0);
            Buttons[2].Yellow = ((arrData[4] & 0x08) != 0);
            Buttons[2].Green = ((arrData[4] & 0x10) != 0);
            Buttons[2].Orange = ((arrData[4] & 0x20) != 0);
            Buttons[2].Blue = ((arrData[4] & 0x40) != 0);

            Buttons[3].Red = ((arrData[4] & 0x80) != 0);
            Buttons[3].Yellow = ((arrData[5] & 0x01) != 0);
            Buttons[3].Green = ((arrData[5] & 0x02) != 0);
            Buttons[3].Orange = ((arrData[5] & 0x04) != 0);
            Buttons[3].Blue = ((arrData[5] & 0x08) != 0);
        }
    }
}
