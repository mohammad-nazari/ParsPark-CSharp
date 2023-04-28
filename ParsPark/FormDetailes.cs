using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormDetailes : MetroForm
	{
		public logs LogDetail { get; set; } = new logs();

		public FormDetailes()
		{
			InitializeComponent();
		}

		private void FormDetailes_Load(object sender, EventArgs e)
		{
			pbEnter.Image = LogDetail.enpicture != null ? StringConvert.ByteArrayToImage(LogDetail.enpicture) : null;

			pbExit.Image = LogDetail.expicture != null ? StringConvert.ByteArrayToImage(LogDetail.expicture) : null;

			if (LogDetail.enlicense != null)
			{
				LicensePlate lp = new LicensePlate();
				lp.LoadLicense(LogDetail.enlicense);
				MyForms.FillLicenseMaskedTextBox(lp, ref mtxtEnterLPNumberFirst, ref mtxtEnterLPNumberAlpha, ref mtxtEnterLPNumberSecond, ref mtxtEnterLPNumberThird);
			}

			if (LogDetail.exlicense != null)
			{
				LicensePlate lp = new LicensePlate();
				lp.LoadLicense(LogDetail.exlicense);
				MyForms.FillLicenseMaskedTextBox(lp, ref mtxtExitLPNumberFirst, ref mtxtExitLPNumberAlpha, ref mtxtExitLPNumberSecond, ref mtxtExitLPNumberThird);
			}

			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				users userObject = parsPark.users.Find(LogDetail.enuser);
				if (userObject != null) txtEnterUser.Text = userObject.username;
			}
			catch
			{
				txtEnterUser.Text = "";
			}

			try
			{
				parsparkoEntities parsPark1 = new parsparkoEntities(GlobalVariables.ConnectionString);
				users userObject1 = parsPark1.users.Find(LogDetail.exuser);
				if (userObject1 != null) txtExitUser.Text = userObject1.username;
			}
			catch
			{
				txtExitUser.Text = "";
			}

			txtEnterDateTime.Text = DateTimeClass.ToPersianFormat(LogDetail.enter);

			txtExitDateTime.Text = LogDetail.exit != null ? DateTimeClass.ToPersianFormat(LogDetail.exit.Value) : @"خارج نشده";

			txtCardCode.Text = LogDetail.code ?? "";

			if (LogDetail.enter != null && LogDetail.exit != null)
			{
				txtDuration.Text = (LogDetail.exit - LogDetail.enter).Value.ToString();
			}

			txtCost.Text = LogDetail.cost != null ? LogDetail.cost.ToString() : @"خارج نشده";
		}
	}
}
