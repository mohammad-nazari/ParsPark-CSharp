using System;
using System.Windows.Forms;
using SerialPortLib;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormTestBoard : MetroForm
	{
		public FormTestBoard()
		{
			InitializeComponent();
		}

		private void btnSendCost_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormTestBoard_Load(object sender, EventArgs e)
		{

		}
	}
}
