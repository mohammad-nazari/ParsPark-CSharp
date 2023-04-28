using System;
using System.Windows.Forms;
using CardReaderEnterLib;
using CardReaderExitLib;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormTestCardReader : MetroForm
	{
		public bool EnterCard { get; set; } = true;

		public CardReaderEnter EnterCardReader { get; set; } = new CardReaderEnter();

		public CardReaderExit ExitCardReader { get; set; } = new CardReaderExit();

		public FormTestCardReader()
		{
			InitializeComponent();
		}

		private void FormTestCardReader_Load(object sender, EventArgs e)
		{

		}

		private void timerReadCard_Tick(object sender, EventArgs e)
		{
			// Wait until card read
			if(EnterCard)
			{
				EnterCardReader.ReadCardData();
			}
			else
			{
				ExitCardReader.ReadCardData();
			}

			string strCardSerialNumber = EnterCard ? EnterCardReader.LastCardSerialNumber : ExitCardReader.LastCardSerialNumber;

			if(strCardSerialNumber != "")
			{
				txtCardNumber.Text = strCardSerialNumber;
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{

		}
	}
}
