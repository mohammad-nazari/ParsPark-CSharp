using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormLogin : MetroForm
	{
		public users UserInfo { get; set; } = new users();

		public bool IsOk { get; set; }

		public string Errors { get; set; }

		public bool ShowSettings { get; set; }

		public FormMain FormMainObject { get; set; }

		public FormLogin()
		{
			InitializeComponent();
			IsOk = true;
			Errors = "";
			pnlSettings.Visible = ShowSettings;

			txtDBSever.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Server;
			nudPort.Value = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Port;
			txtDBName.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Database;
			txtDBUser.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Username;
			txtDBPassword.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Password;

			chbActiveODR.Checked = (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes);
		}

		private void btnSignIn_Click(object sender, EventArgs e)
		{
			IsOk = true;
			Errors = "";

			if (txtUserName.Text.Trim() == "")
			{
				Errors += @"لطفا نام کاربری را وارد نمائید." + Environment.NewLine;
				IsOk = false;
			}
			if (txtPassWord.Text.Trim() == "")
			{
				Errors += @"لطفا گذر واژه را وارد نمائید." + Environment.NewLine;
				IsOk = false;
			}

			if (!IsOk)
			{
				MessageBox.Show(Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			else
			{
				GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Server = txtDBSever.Text;
				GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Port = (int)nudPort.Value;
				GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Database = txtDBName.Text;
				GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Username = txtDBUser.Text;
				GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Password = txtDBPassword.Text;

				GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow = chbActiveODR.Checked ? PermissionAllow.Yes : PermissionAllow.No;

				FormMainObject.ChangeEntityFrameworkConnectionString();

				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					string passMd5 = MyMD5.ToMD5(txtPassWord.Text.Trim());
					UserInfo = (from l in parsPark.users where l.username == txtUserName.Text.Trim() && l.password == passMd5 select l).FirstOrDefault();
					if (UserInfo != null)
					{
						DialogResult = DialogResult.OK;
						Close();
					}
					else
					{
						MessageBox.Show(@"نام کاربری یا گذرواژه اشتباه می باشد.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در برقراری ارتباط با پایگاه داده." + Environment.NewLine + @"لطفا ارتباط شبکه و یا اطلاعات پایگاه داده را چک کنید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					UserInfo = null;
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void FormLogin_Load(object sender, EventArgs e)
		{
		}

		private void btnSettings_Click(object sender, EventArgs e)
		{
			ShowSettings = !ShowSettings;
			pnlSettings.Visible = ShowSettings;
			btnSettings.Image = ShowSettings ? ilIcons.Images["down.png"] : ilIcons.Images["left.png"];
		}

		private void lblSettings_Click(object sender, EventArgs e)
		{
			btnSettings_Click(sender, e);
		}
	}
}
