namespace BuzzIO
{
    /// <summary>
    /// Defines a base class for input reports. To use input reports, use the SetData method and override the
    /// ProcessData method.
    /// </summary>
    public abstract class InputReport : Report
    {
        /// <summary>
        /// Call this to set the buffer given a raw input report. Calls an overridable method to
        /// should automatically parse the bytes into meaningul structures.
        /// </summary>
        /// <param name="arrData">Raw input report.</param>
        public void SetData(byte[] arrData)
        {
            SetBuffer(arrData);
            ProcessData();
        }
        /// <summary>
        /// Override this to process the input report into something useful
        /// </summary>
        public abstract void ProcessData();
    }
}
