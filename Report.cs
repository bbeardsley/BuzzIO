namespace BuzzIO
{
    /// <summary>
    /// Base class for report types. Simply wraps a byte buffer.
    /// </summary>
    public abstract class Report
    {
        #region Properties
        /// <summary>
        /// Buffer for the raw report bytes
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// Length of the report
        /// </summary>
        public int BufferLength { get; private set; }

        #endregion

        /// <summary>
        /// Sets the raw byte array.
        /// </summary>
        /// <param name="arrBytes">Raw report bytes</param>
        protected void SetBuffer(byte[] arrBytes)
        {
            Buffer = arrBytes;
            BufferLength = Buffer.Length;
        }

	}
}
