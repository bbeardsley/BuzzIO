namespace BuzzIO
{
    /// <summary>
    /// Defines a base class for output reports. To use output reports, just put the bytes into the raw buffer.
    /// </summary>
    public abstract class OutputReport : Report
    {
        /// <summary>
        /// Construction. Setup the buffer with the correct output report length dictated by the device
        /// </summary>
        /// <param name="oDev">Creating device</param>
        protected OutputReport(HIDDevice oDev)
        {
            SetBuffer(new byte[oDev.OutputReportLength]);
        }
    }
}
