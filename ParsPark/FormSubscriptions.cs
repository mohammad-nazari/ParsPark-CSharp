using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormSubscriptions : MetroForm
	{
		/*public CardReaderEnter EnterCardReader { get; set; } = new CardReaderEnter();

		public CardReaderExit ExitCardReader { get; set; } = new CardReaderExit();*/
		public FormMain FormMainObject { get; set; }

		public FormSubscriptions()
		{
			InitializeComponent();
		}

		private void FormSubscriptions_Load(object sender, EventArgs e)
		{
			InitializeSubscriptionGridView();
		}

		private void InitializeSubscriptionGridView()
		{
			dgvSubscriptions.Rows.Clear();

			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var subscriptions = (from s in parsPark.subscription select s);

				if (rbCredit.Checked)
				{
					DateTime now = DateTime.Now;
					subscriptions = subscriptions.Where(s => s.startdate < now && s.enddate > now);
				}
				else if (rbExpired.Checked)
				{
					DateTime now = DateTime.Now;
					subscriptions = subscriptions.Where(s => s.enddate < now);
				}

				if (subscriptions.Any())
				{
					foreach (subscription sub in subscriptions)
					{
						var rowIndex = dgvSubscriptions.Rows.Add();

						parsparkoEntities parsPark2 = new parsparkoEntities(GlobalVariables.ConnectionString);

						var driverCarObject = (from d in parsPark2.driver
											   join c in parsPark2.car on d.id equals c.driverid
											   where c.id == sub.drivercarid
											   select new
											   {
												   c.license,
												   d.fname,
												   d.lname,
												   d.orgname,
												   d.orgval
											   }).FirstOrDefault();

						if (driverCarObject != null)
						{
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionRow"].Value = (rowIndex + 1).ToString();
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionCode"].Value = sub.code;
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionFname"].Value = driverCarObject.fname;
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionLname"].Value = driverCarObject.lname;
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionLicense"].Value = driverCarObject.license;
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionOrg"].Value = driverCarObject.orgname + "(" + driverCarObject.orgval + ")";
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionStart"].Value = DateTimeClass.ToPersianFormat(sub.startdate);
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionEnd"].Value = DateTimeClass.ToPersianFormat(sub.enddate);
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionDetails"].Value = "جزئیات";
							dgvSubscriptions.Rows[rowIndex].Cells["SubscriptionCancel"].Value = "لغو اشتراک";
							dgvSubscriptions.Rows[rowIndex].Tag = sub.id.ToString(CultureInfo.InvariantCulture);
						}
					}
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در بازیابی فهرست.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnAddSubscription_Click(object sender, EventArgs e)
		{
			FormEditSubscription editSubscription = new FormEditSubscription
			{
				/*EnterCardReader = EnterCardReader,
				ExitCardReader = ExitCardReader,*/
				FormMainObject = FormMainObject,
				IsNew = true
			};

			editSubscription.ShowDialog();

			InitializeSubscriptionGridView();
		}

		private void dgvSubscriptions_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvSubscriptions.Columns[e.ColumnIndex].Name == "SubscriptionDetails")
			{
				FormEditSubscription editSubscription = new FormEditSubscription
				{
					CarLicense = dgvSubscriptions.Rows[e.RowIndex].Cells["SubscriptionLicense"].Value.ToString(),
					SubId = dgvSubscriptions.Rows[e.RowIndex].Tag.ToString(),
					IsNew = false
				};

				editSubscription.ShowDialog();
				InitializeSubscriptionGridView();
			}
			else if (dgvSubscriptions.Columns[e.ColumnIndex].Name == "SubscriptionCancel")
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var subObject = parsPark.subscription.Find(Convert.ToInt64(dgvSubscriptions.Rows[e.RowIndex].Tag));

				if (subObject != null && subObject.id > 0)
				{
					// Car is for another driver
					DialogResult dResult = MessageBox.Show(@"آیا از حذف این مشترک مطمئن هستید؟", @"هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					if (dResult == DialogResult.Yes)
					{
						try
						{
							parsPark.subscription.Remove(subObject);
							if (parsPark.SaveChanges() > 0)
							{
								MessageBox.Show(@"اطلاعات مشترک مورد نظر حذف گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							}
						}
						catch
						{
							MessageBox.Show(@"خطا در حذف اطلاعات مشترک", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
				}
				else
				{
					MessageBox.Show(@"خطا در حذف اطلاعات مشترک", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
				InitializeSubscriptionGridView();
			}
		}

		private void rbAll_CheckedChanged(object sender, EventArgs e)
		{
			InitializeSubscriptionGridView();
		}

		private void rbCredit_CheckedChanged(object sender, EventArgs e)
		{
			InitializeSubscriptionGridView();
		}

		private void rbExpired_CheckedChanged(object sender, EventArgs e)
		{
			InitializeSubscriptionGridView();
		}
	}
}
