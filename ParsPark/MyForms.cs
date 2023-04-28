using System;
using System.Windows.Forms;
using ToolsLib;
using TextBox = Atf.UI.TextBox;

namespace ParsPark
{
	public class OrganSubInfo
	{
		public long Id;
		public string Code;
		public string Orgname;
		public string Orgval;
	}
	public class MyForms
	{
		public static string GetLicenseFromTextBox(TextBox TxtLpNo1, TextBox TxtLpAlpha, TextBox TxtLpNo2, TextBox TxtLpNo3)
		{
			return TxtLpNo2.Text + TxtLpNo3.Text + TxtLpAlpha.Text + TxtLpNo1.Text;
		}
		public static string GetLicenseFromMaskedTextBox(MaskedTextBox TxtLpNo1, MaskedTextBox TxtLpAlpha, MaskedTextBox TxtLpNo2, MaskedTextBox TxtLpNo3)
		{
			return TxtLpNo2.Text + TxtLpNo3.Text + TxtLpAlpha.Text + TxtLpNo1.Text;
		}

		public static void FillLicenseMaskedTextBox(LicensePlate LicenseNumber, ref MaskedTextBox TxtLpNo1, ref MaskedTextBox TxtLpAlpha, ref MaskedTextBox TxtLpNo2, ref MaskedTextBox TxtLpNo3)
		{
			TxtLpNo1.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber1;
			});
			TxtLpAlpha.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpAlpha;
			});
			TxtLpNo2.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber2;
			});
			TxtLpNo3.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber3;
			});
		}

		public static void FillLicenseTextBox(LicensePlate LicenseNumber, ref TextBox TxtLpNo1, ref TextBox TxtLpAlpha, ref TextBox TxtLpNo2, ref TextBox TxtLpNo3)
		{
			TxtLpNo1.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber1;
			});
			TxtLpAlpha.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpAlpha;
			});
			TxtLpNo2.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber2;
			});
			TxtLpNo3.InvokeIfRequired(c =>
			{
				c.Text = LicenseNumber.LpNumber3;
			});
		}

		public static string SelectCamera()
		{
			string camAddressEn = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow ==
									  PermissionAllow.Yes
					  ? GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL : "";
			string camAddressEx = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow ==
								PermissionAllow.Yes
				? GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.URL : "";

			if (camAddressEn != "" && camAddressEx != "")
			{
				DialogResult dialogResult = MessageBox.Show(@"از دوربین ورودی (Yes)" + Environment.NewLine + @"از دوربین خروجی (No)", @"ثبت عکس", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				if (dialogResult == DialogResult.No)
				{
					camAddressEn = camAddressEx;
				}
				else if (dialogResult == DialogResult.Cancel)
				{
					camAddressEn = "";
				}
			}
			else if (camAddressEx != "")
			{
				camAddressEn = camAddressEx;
			}
			return camAddressEn;
		}
	}
}
