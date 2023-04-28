using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormBlackList : MetroForm
	{
		public FormBlackList()
		{
			InitializeComponent();
		}

		private void FormBlackList_Load(object sender, EventArgs e)
		{
			try
			{

				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var blackList = (from bl in parsPark.blacklist select bl);

				if (blackList.Any())
				{
					foreach (var bl in blackList)
					{
						var rowIndex = dgvCars.Rows.Add();
						dgvCars.Rows[rowIndex].Cells["BlackListRow"].Value = (rowIndex + 1).ToString();
						dgvCars.Rows[rowIndex].Cells["BlackListLicense"].Value = bl.license;
						dgvCars.Rows[rowIndex].Cells["BlackListDescription"].Value = bl.description;
						dgvCars.Rows[rowIndex].Cells["BlackListEdit"].Value = "ویرایش";
						dgvCars.Rows[rowIndex].Cells["BlackListDelete"].Value = "حذف";
						dgvCars.Rows[rowIndex].Tag = bl.id;
					}
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در ارتباط با پایگاه داده", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void dgvCars_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvCars.Columns[e.ColumnIndex].Name == "BlackListEdit")
			{
				FormEditBlackList editBlackList = new FormEditBlackList
				{
					CarLicense = dgvCars.Rows[e.RowIndex].Cells["BlackListLicense"].Value.ToString(),
					Id = dgvCars.Rows[e.RowIndex].Tag,
					IsNew = false
				};

				editBlackList.ShowDialog();

				InitializeBlackListGridView();
			}
			else if (dgvCars.Columns[e.ColumnIndex].Name == "BlackListDelete")
			{
				DeleteBlackList(e);
			}
		}

		private void InitializeBlackListGridView()
		{
			dgvCars.Rows.Clear();

			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var blackList = (from bl in parsPark.blacklist select bl);

				foreach (var bl in blackList)
				{
					var rowIndex = dgvCars.Rows.Add();
					dgvCars.Rows[rowIndex].Cells["BlackListRow"].Value = (rowIndex + 1).ToString();
					dgvCars.Rows[rowIndex].Cells["BlackListLicense"].Value = bl.license;
					dgvCars.Rows[rowIndex].Cells["BlackListDescription"].Value = bl.description;
					dgvCars.Rows[rowIndex].Cells["BlackListEdit"].Value = "ویرایش";
					dgvCars.Rows[rowIndex].Cells["BlackListDelete"].Value = "حذف";
					dgvCars.Rows[rowIndex].Tag = bl.id;
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در ارتباط با پایگاه داده", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void DeleteBlackList(DataGridViewCellEventArgs e)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var blackListObject = parsPark.blacklist.Find(dgvCars.Rows[e.RowIndex].Tag);

				if (blackListObject != null && blackListObject.id > 0)
				{
					DialogResult result = MessageBox.Show(@"آیا می خواهید پلاک  " + blackListObject.license + @"را حذف نمائید؟", @"اخطار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					if (result == DialogResult.Yes)
					{
						blackListObject = parsPark.blacklist.Remove(blackListObject);
						if (parsPark.SaveChanges() > 0)
						{
							MessageBox.Show(@"ماشین به شماره پلاک: " + blackListObject.license + @"حذف گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
						else
						{
							MessageBox.Show(@"خطا در حذف پلاک : " + blackListObject.license, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
				}
				InitializeBlackListGridView();
			}
			catch
			{
				MessageBox.Show(@"خطا در حذف پلاک", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnAddCar_Click(object sender, EventArgs e)
		{
			FormEditBlackList editBlackList = new FormEditBlackList
			{
				CarLicense = "",
				IsNew = true
			};

			editBlackList.ShowDialog();

			InitializeBlackListGridView();
		}
	}
}
