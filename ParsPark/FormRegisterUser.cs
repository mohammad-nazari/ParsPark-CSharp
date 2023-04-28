using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormRegisterUser : MetroForm
	{
		public FormRegisterUser()
		{
			InitializeComponent();
		}

		private void pbUser_Click(object sender, EventArgs e)
		{
			if(ofdPicture.ShowDialog() == DialogResult.OK)
			{
				pbUser.Image = Image.FromFile(ofdPicture.FileName);
			}
		}

		private void pbUser_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(pbUser, @"برای بارگذاری عکس کلیک کنید");
		}

		private bool validation_Form(ref string ErrorResult)
		{
			if (ErrorResult == null) throw new ArgumentNullException(nameof(ErrorResult));
			ErrorResult = "";
			Regex regex = new Regex(@"^(?=[A-Za-z0-9])(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,15}$");
			if(tbUsername.Text != "")
			{
				if(!(regex.Match(tbUsername.Text).Success))
				{
					ErrorResult += @"لطفا نام کاربری معتبر وارد نمائید." + Environment.NewLine;
				}
			}
			else
			{
				ErrorResult += @"لطفا نام کاربری را وارد نمائید." + Environment.NewLine;
			}

			if(tbPassword.Text != "")
			{
				if(tbPasswordR.Text != "")
				{
					if(tbPassword.Text != tbPasswordR.Text)
					{
						ErrorResult += @"پسورد و تکرار آن با هم برابر نیستند." + Environment.NewLine;
					}
				}
				else
				{
					ErrorResult += @"لطفا پسورد را مجدد وارد نمائید." + Environment.NewLine;
				}
			}
			else
			{
				ErrorResult += @"لطفا پسورد را وارد نمائید." + Environment.NewLine;
				if(tbPasswordR.Text == "")
				{
					ErrorResult += @"لطفا پسورد را مجدد وارد نمائید." + Environment.NewLine;
				}
			}

			if(ErrorResult != "")
			{
				ErrorResult = @"خطا: " + Environment.NewLine + ErrorResult;
				return false;
			}
			return true;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string errorResult = "";
			if(validation_Form(ref errorResult))
			{
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					users userObject = new users
					{
						username = tbUsername.Text,
						password = MyMD5.ToMD5(tbPassword.Text),
						fname = tbFname.Text != "" ? tbFname.Text : "",
						lname = tbLname.Text != "" ? tbLname.Text : "",
						phone = mtbPhone.Text != "" ? mtbPhone.Text : "0",
						address = tbAddress.Text != "" ? tbAddress.Text : "",
						picture = pbUser.Image != null ? ImageProcess.ImageToByteArray(pbUser.Image) : null,
						type = rbEmploee.Checked ? UserType.emploee.ToString() : UserType.manager.ToString()
					};


					userObject = parsPark.users.Add(userObject);
					if(parsPark.SaveChanges() > 0)
					{
						MessageBox.Show(@"کاربر با نام کاربری: " + userObject.username + @"ثبت گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
					else
					{
						MessageBox.Show(@"خطا در ثبت کاربر: " + userObject.username, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در ثبت کاربر: ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(errorResult, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void FormRegisterUser_Load(object sender, EventArgs e)
		{

		}
	}
}
