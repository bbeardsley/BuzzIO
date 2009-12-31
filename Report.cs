using System;

namespace BuzzWin
{
	/// <summary>
	/// Base class for report types. Simply wraps a byte buffer.
	/// </summary>
	public abstract class Report
	{
		#region Member variables
		/// <summary>Buffer for raw report bytes</summary>
		private byte[] m_arrBuffer;
		/// <summary>Length of the report</summary>
		private int m_nLength;
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oDev">Constructing device</param>
		public Report(HIDDevice oDev)
		{
			// Do nothing
		}

		/// <summary>
		/// Sets the raw byte array.
		/// </summary>
		/// <param name="arrBytes">Raw report bytes</param>
		protected void SetBuffer(byte[] arrBytes)
		{
			m_arrBuffer = arrBytes;
			m_nLength = m_arrBuffer.Length;
		}

		/// <summary>
		/// Accessor for the raw byte buffer
		/// </summary>
		public byte[] Buffer
		{
			get
			{
				return m_arrBuffer;
			}
		}
		/// <summary>
		/// Accessor for the buffer length
		/// </summary>
		public int BufferLength
		{
			get
			{
				return m_nLength;
			}
		}
	}
}
