using System;
using System.Drawing;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormEditUser : MetroForm
	{
		public int UserId { get; set; } = 0;

		users _userObject = new users();

		public FormEditUser()
		{
			InitializeComponent();
		}

		private void pbUser_Click(object sender, EventArgs e)
		{
			if (ofdPicture.ShowDialog() == DialogResult.OK)
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
			if (tbPassword.Text != tbPasswordR.Text)
			{
				ErrorResult += @"پسورد و تکرار آن با هم برابر نیستند." + Environment.NewLine;
			}

			if (ErrorResult != "")
			{
				ErrorResult = @"خطا: " + Environment.NewLine + ErrorResult;
				return false;
			}
			return true;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string errorResult = "";
			if (validation_Form(ref errorResult))
			{
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					_userObject = parsPark.users.Find(_userObject.id);

					if (tbPassword.Text != "")
					{
						_userObject.password = MyMD5.ToMD5(tbPassword.Text);
					}
					_userObject.fname = tbFname.Text != "" ? tbFname.Text : "";
					_userObject.lname = tbLname.Text != "" ? tbLname.Text : "";
					_userObject.phone = mtbPhone.Text != "" ? mtbPhone.Text : "0";
					_userObject.address = tbAddress.Text != "" ? tbAddress.Text : "";
					_userObject.picture = pbUser.Image != null ? ImageProcess.ImageToByteArray(pbUser.Image) : null;
					_userObject.type = rbEmploee.Checked ? UserType.emploee.ToString() : UserType.manager.ToString();

					parsPark.Entry(_userObject).CurrentValues.SetValues(_userObject);

					if (parsPark.SaveChanges() > 0)
					{
						MessageBox.Show(@"کاربر با نام کاربری: " + _userObject.username + @"ویرایش گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
					else
					{
						MessageBox.Show(@"خطا در ویرایش کاربر: " + _userObject.username, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در ویرایش کاربر", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(errorResult, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void FormEditUser_Load(object sender, EventArgs e)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				_userObject = parsPark.users.Find(UserId);

				if (_userObject.id > 0)
				{
					tbUsername.Text = _userObject.username;
					tbFname.Text = _userObject.fname != "" ? _userObject.fname : "";
					tbLname.Text = _userObject.lname != "" ? _userObject.lname : "";
					mtbPhone.Text = _userObject.phone.ToString();
					tbAddress.Text = _userObject.address != "" ? tbAddress.Text : "";
					pbUser.Image = _userObject.picture != null ? ImageProcess.ByteArrayToImage(_userObject.picture) : null;
					if (_userObject.type == UserType.emploee.ToString())
					{
						rbEmploee.Checked = true;
					}
					else
					{
						rbAdmin.Checked = true;
					}
				}
			}
			catch
			{
				MessageBox.Show(@"خطا", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}
	}
}
