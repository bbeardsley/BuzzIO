using System;
using System.IO;
using System.Text;

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
		private ButtonStates[] m_arrButtons;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oDev">Constructing device</param>
		public BuzzInputReport(HIDDevice oDev) : base(oDev)
		{
			// Create the button states
			m_arrButtons = new ButtonStates[NUM_HANDSETS];
			for(int i = 0; i < NUM_HANDSETS; i++)
			{
				m_arrButtons[i] = new ButtonStates();
			}
		}
		/// <summary>
		/// Process the raw data from the actual input report bytes
		/// </summary>
		public override void ProcessData()
		{
			byte[] arrData = Buffer;
			m_arrButtons[0].Red    = ((arrData[3] & 0x01) != 0);
			m_arrButtons[0].Yellow = ((arrData[3] & 0x02) != 0);
			m_arrButtons[0].Green  = ((arrData[3] & 0x04) != 0);
			m_arrButtons[0].Orange = ((arrData[3] & 0x08) != 0);
			m_arrButtons[0].Blue   = ((arrData[3] & 0x10) != 0);

			m_arrButtons[1].Red    = ((arrData[3] & 0x20) != 0);
			m_arrButtons[1].Yellow = ((arrData[3] & 0x40) != 0);
			m_arrButtons[1].Green  = ((arrData[3] & 0x80) != 0);
			m_arrButtons[1].Orange = ((arrData[4] & 0x01) != 0);
			m_arrButtons[1].Blue   = ((arrData[4] & 0x02) != 0);

			m_arrButtons[2].Red    = ((arrData[4] & 0x04) != 0);
			m_arrButtons[2].Yellow = ((arrData[4] & 0x08) != 0);
			m_arrButtons[2].Green  = ((arrData[4] & 0x10) != 0);
			m_arrButtons[2].Orange = ((arrData[4] & 0x20) != 0);
			m_arrButtons[2].Blue   = ((arrData[4] & 0x40) != 0);
			
			m_arrButtons[3].Red    = ((arrData[4] & 0x80) != 0);
			m_arrButtons[3].Yellow = ((arrData[5] & 0x01) != 0);
			m_arrButtons[3].Green  = ((arrData[5] & 0x02) != 0);
			m_arrButtons[3].Orange = ((arrData[5] & 0x04) != 0);
			m_arrButtons[3].Blue   = ((arrData[5] & 0x08) != 0);
		}
		/// <summary>
		/// Accessor for the button states
		/// </summary>
		public ButtonStates[] Buttons
		{
			get
			{
				return m_arrButtons;
			}
		}
	}
}
