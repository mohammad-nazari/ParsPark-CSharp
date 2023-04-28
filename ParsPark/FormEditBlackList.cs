using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormEditBlackList : MetroForm
	{
		public bool IsNew { get; set; }

		public string CarLicense { get; set; }

		public object Id { get; set; }

		public FormEditBlackList()
		{
			InitializeComponent();
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			CreateLicensePlateString();
			if (!IsNew)
			{
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					blacklist blackList = parsPark.blacklist.Find(Id);

					if (blackList != null)
					{
						blackList.description = tbDescription.Text != "" ? tbDescription.Text : "";

						parsPark.Entry(blackList).CurrentValues.SetValues(blackList);

						if (parsPark.SaveChanges() > 0)
						{
							MessageBox.Show(@"ماشین با پلاک : " + blackList.license + @"ویرایش گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
						else
						{
							MessageBox.Show(@"خطا در ویرایش پلاک: " + blackList.license, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
					else
					{
						MessageBox.Show(@"خطا در ویرایش پلاک: " + blackList.license, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در ویرایش پلاک", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{

				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					blacklist blackList = new blacklist
					{
						license = CarLicense,
						description = tbDescription.Text
					};

					blackList = parsPark.blacklist.Add(blackList);
					if (parsPark.SaveChanges() > 0)
					{
						MessageBox.Show(@"ماشین با پلاک: " + blackList.license + @"ثبت گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
					else
					{
						MessageBox.Show(@"خطا در ثبت پلاک: " + blackList.license, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در ثبت پلاک: ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
		}

		private void FormEditBlackList_Load(object sender, EventArgs e)
		{
			if (IsNew == false)
			{
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					blacklist blackList = parsPark.blacklist.Find(Id);

					if (blackList != null)
					{
						FillFormData(blackList.license, true);
						tbDescription.Text = blackList.description;
					}
					else
					{
						MessageBox.Show(@"این شماره پلاک در لیست سیاه قرار دارد.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در جستجوی پایگاه داده", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				FillFormData(CarLicense, CarLicense != "");
				Text = @"ثبت پلاک جدید";
			}
		}

		private void FillFormData(string LicenseNumber, bool ReadOnlyFormat = false)
		{
			if (LicenseNumber != null && LicenseNumber.Length >= 8)
			{
				mtxtLPNumberFirst.Text = LicenseNumber.Substring(6, 2);
				mtxtLPNumberAlpha.Text = LicenseNumber.Substring(5, 1);
				mtxtLPNumberSecond.Text = LicenseNumber.Substring(0, 3);
				mtxtLPNumberThird.Text = LicenseNumber.Substring(3, 2);
			}
			mtxtLPNumberFirst.ReadOnly = ReadOnlyFormat;
			mtxtLPNumberFirst.Enabled = !ReadOnlyFormat;
			mtxtLPNumberAlpha.ReadOnly = ReadOnlyFormat;
			mtxtLPNumberAlpha.Enabled = !ReadOnlyFormat;
			mtxtLPNumberSecond.ReadOnly = ReadOnlyFormat;
			mtxtLPNumberSecond.Enabled = !ReadOnlyFormat;
			mtxtLPNumberThird.ReadOnly = ReadOnlyFormat;
			mtxtLPNumberThird.Enabled = !ReadOnlyFormat;
		}

		private void CreateLicensePlateString()
		{
			CarLicense = mtxtLPNumberSecond.Text + mtxtLPNumberThird.Text + mtxtLPNumberAlpha.Text + mtxtLPNumberFirst.Text;
		}
	}
}
