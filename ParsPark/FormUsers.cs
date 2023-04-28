using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormUsers : MetroForm
	{
		public FormUsers()
		{
			InitializeComponent();
		}

		private void FormUsers_Load(object sender, EventArgs e)
		{
			// Load data into DataGridView
			InitializeUserGridView();
		}

		private void InitializeUserGridView()
		{
			dgvUsers.Rows.Clear();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				IQueryable<users> usersList = (from u in parsPark.users where u.username != @"admin" select u);

				foreach (users user in usersList)
				{
					var rowIndex = dgvUsers.Rows.Add();
					dgvUsers.Rows[rowIndex].Cells["UserRow"].Value = (rowIndex + 1).ToString();
					dgvUsers.Rows[rowIndex].Cells["UserCode"].Value = user.id;
					dgvUsers.Rows[rowIndex].Cells["UserFname"].Value = user.fname;
					dgvUsers.Rows[rowIndex].Cells["UserLname"].Value = user.lname;
					dgvUsers.Rows[rowIndex].Cells["UserUname"].Value = user.username;
					dgvUsers.Rows[rowIndex].Cells["UserType"].Value = user.type;
					if (user.picture != null)
					{
						dgvUsers.Rows[rowIndex].Cells["UserImage"].Value = user.picture;
					}
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در بازیابی فهرست کاربران", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnAddUser_Click(object sender, EventArgs e)
		{
			FormRegisterUser registerUser = new FormRegisterUser();
			registerUser.ShowDialog();

			InitializeUserGridView();
		}

		private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvUsers.Columns[e.ColumnIndex].Name == "UserEdit")
			{
				FormEditUser editUser = new FormEditUser
				{
					UserId = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["UserCode"].Value.ToString())
				};

				editUser.ShowDialog();

				InitializeUserGridView();
			}
			else if (dgvUsers.Columns[e.ColumnIndex].Name == "UserDelete")
			{
				DeleteUser(e);
			}
		}

		private void DeleteUser(DataGridViewCellEventArgs e)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var userObject = parsPark.users.Find(Convert.ToInt64(dgvUsers.Rows[e.RowIndex].Cells["UserCode"].Value.ToString()));

				if (userObject != null && userObject.id > 0)
				{
					DialogResult result = MessageBox.Show(@"آیا می خواهید کاربر " + userObject.username + @"را حذف نمائید؟", @"اخطار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					if (result == DialogResult.Yes)
					{
						userObject = parsPark.users.Remove(userObject);
						if (parsPark.SaveChanges() > 0)
						{
							MessageBox.Show(@"کاربر با نام کاربری: " + userObject.username + @"حذف گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
						else
						{
							MessageBox.Show(@"خطا در حذف کاربر: " + userObject.username, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
				}
				InitializeUserGridView();
			}
			catch
			{
				MessageBox.Show(@"خطا در حذف کاربر", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}
	}
}
