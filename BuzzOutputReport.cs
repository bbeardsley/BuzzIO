namespace BuzzIO
{
    /// <summary>
    /// Output report for Buzz device
    /// </summary>
    public class BuzzOutputReport : OutputReport
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Device constructing this report</param>
        public BuzzOutputReport(HIDDevice oDev)
            : base(oDev)
        {
        }

        /// <summary>
        /// Sets the states of the lights
        /// </summary>
        /// <param name="bLight1">State of light on handset 1</param>
        /// <param name="bLight2">State of light on handset 2</param>
        /// <param name="bLight3">State of light on handset 3</param>
        /// <param name="bLight4">State of light on handset 4</param>
        public void SetLightStates(bool bLight1, bool bLight2, bool bLight3, bool bLight4)
        {
            var arrBuff = Buffer;
            arrBuff[2] = (byte)(bLight1 ? 0xff : 0);
            arrBuff[3] = (byte)(bLight2 ? 0xff : 0);
            arrBuff[4] = (byte)(bLight3 ? 0xff : 0);
            arrBuff[5] = (byte)(bLight4 ? 0xff : 0);
        }
    }
}
