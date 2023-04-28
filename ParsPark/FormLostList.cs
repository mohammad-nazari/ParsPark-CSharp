using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ExportFileLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormLostList : MetroForm
	{
		// The class that will do the printing process.
		private long _totalCost;

		public users User { get; set; }

		public string ExitImageName { get; set; }

		public FormLostList()
		{
			InitializeComponent();
		}

		private void cbFromDate_CheckedChanged(object sender, EventArgs e)
		{
			dtpStartTime.Enabled = cbFromDate.Checked;
		}

		private void cbToDate_CheckedChanged(object sender, EventArgs e)
		{
			dtpEndTime.Enabled = cbToDate.Checked;
		}

		private void FormLostList_Load(object sender, EventArgs e)
		{
			dtpStartTime.Value = DateTime.Now;
			dtpStartTime.Value = DateTimeClass.SetTime(dtpStartTime.Value.Value, 0, 0, 0);

			dtpEndTime.Value = dtpStartTime.Value;
			dtpEndTime.Value = DateTimeClass.SetTime(dtpEndTime.Value.Value, 23, 59, 0);
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (cbFromDate.Checked || cbToDate.Checked)
			{

				lblRecordsCount.Text = @"";
				lblRecordsCost.Text = @"";
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					var lostList = (from l in parsPark.lostcard select l);

					if (cbFromDate.Checked)
					{
						if (dtpStartTime.Value != null)
						{
							lostList = lostList.Where(l => l.submitdate >= dtpStartTime.Value.Value);
						}
					}
					if (cbToDate.Checked)
					{
						if (dtpEndTime.Value != null)
						{
							lostList = lostList.Where(l => l.submitdate <= dtpEndTime.Value.Value);
						}
					}

					dgvReport.Rows.Clear();

					lblRecordsCount.Text = @"0";
					lblRecordsCost.Text = @"0";

					decimal allCost = 0;

					if (lostList.Any())
					{
						foreach (var log in lostList)
						{
							var rowIndex = dgvReport.Rows.Add();
							dgvReport.Rows[rowIndex].Cells["ReportRow"].Value = rowIndex + 1;
							dgvReport.Rows[rowIndex].Cells["ReportCode"].Value = log.cardid;
							dgvReport.Rows[rowIndex].Cells["ReportDriverName"].Value = log.fname + " " + log.lname;
							dgvReport.Rows[rowIndex].Cells["ReportDriverNID"].Value = log.driverID;
							dgvReport.Rows[rowIndex].Cells["ReportDriverDID"].Value = log.dlicense;
							dgvReport.Rows[rowIndex].Cells["ReportCarLicense"].Value = log.license;
							dgvReport.Rows[rowIndex].Cells["ReportCardType"].Value = log.type;
							dgvReport.Rows[rowIndex].Cells["ReportFine"].Value = log.fine;
							dgvReport.Rows[rowIndex].Cells["ReportLostDate"].Value = log.submitdate;
							dgvReport.Rows[rowIndex].Cells["ReportDescription"].Value = log.description;
							allCost += log.fine;
						}
						lblRecordsCount.Text = lostList.Count().ToString(@"0,0");
						lblRecordsCost.Text = allCost.ToString(@"0,0");
					}
					else
					{
						MessageBox.Show(@"هیچ رکوردی یافت نشد.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در پایگاه داده.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"لطفا دست کم یک روش جستجو را انتخاب کنید", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void bsbSaveReport_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.WORD);
		}

		private void tsmiSaveExcel_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.EXCEL);
		}

		private void tsmiSavePDF_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.PDF);
		}

		private void tsmiSaveXML_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.XML);
		}

		private void tsmiSaveCSV_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.CSV);
		}

		private void tsmiSaveHTML_Click(object sender, EventArgs e)
		{
			ExportReport(FileFormatNames.HTML);
		}

		private void ExportReport(FileFormatNames ExportFileType)
		{
			if (dgvReport.Rows.Count > 0)
			{
				DataGridView finalData = GetFinalData();
				if (finalData.Rows.Count > 1)
				{
					string strFilePath = GetSaveFileName(ExportFileType);
					if (!string.IsNullOrEmpty(strFilePath))
					{
						if (dtpStartTime.Value != null)
						{
							if (dtpEndTime.Value != null)
							{
								ExportFile exportObject = new ExportFile()
								{
									DgvObject = finalData,
									FileName = strFilePath,
									ParkingName = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name,
									DateTimeInfo = @"تاریخ گزارش گیری : " + DateTimeClass.ToPersianFormat(DateTime.Now, "yyyy/MM/dd ساعت HH:mm") + Environment.NewLine + Environment.NewLine + @"از تاریخ: " + DateTimeClass.ToPersianFormat(dtpStartTime.Value.Value, "yyyy/MM/dd ساعت HH:mm") + Environment.NewLine + Environment.NewLine + @" تا تاریخ: " + DateTimeClass.ToPersianFormat(dtpEndTime.Value.Value, "yyyy/MM/dd ساعت HH:mm"),
									NumberOfRecords = @"تعداد رکوردها: " + (finalData.Rows.Count - 1).ToString(@"0,0") + @" عدد",
									TotalCost = @"مجموع درآمد: " + _totalCost.ToString(@"0,0") + @"  تومان"
								};

								switch (ExportFileType)
								{
									case FileFormatNames.WORD:
										{
											exportObject.ExportToWord();
											break;
										}
									case FileFormatNames.EXCEL:
										{
											exportObject.ExportToExcel();
											break;
										}
									case FileFormatNames.PDF:
										{
											exportObject.ExportToPdf();
											break;
										}
									case FileFormatNames.XML:
										{
											exportObject.ExportToXml();
											break;
										}
									case FileFormatNames.HTML:
										{
											exportObject.ExportToHtml();
											break;
										}
									case FileFormatNames.CSV:
										{
											exportObject.ExportToCsv();
											break;
										}
								}
							}
						}

						MessageBox.Show(@"گزارش در فایل با موفقیت ذخیره گردید.", @"پیام", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				else
				{
					MessageBox.Show(@"لطفا دست کم یک سطر را انتخاب کنید", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"لطفا دست کم یک سطر را انتخاب کنید", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private string GetSaveFileName(FileFormatNames FormatName)
		{
			sfdSaveReport.FileName = @"";
			switch (FormatName)
			{
				case FileFormatNames.CSV:
					{
						sfdSaveReport.Filter = @"CSV File|*.csv";
						break;
					}
				case FileFormatNames.EXCEL:
					{
						sfdSaveReport.Filter = @"Excel Worksheets|*.xlsx;*.xls";
						break;
					}
				case FileFormatNames.WORD:
					{
						sfdSaveReport.Filter = @"Word Documents|*.docx;*.doc";
						break;
					}
				case FileFormatNames.PDF:
					{
						sfdSaveReport.Filter = @"PDF File|*.pdf";
						break;
					}
				case FileFormatNames.XML:
					{
						sfdSaveReport.Filter = @"XML File|*.xml";
						break;
					}
				case FileFormatNames.HTML:
					{
						sfdSaveReport.Filter = @"HTML File|*.html;*.htm";
						break;
					}
			}

			if (sfdSaveReport.ShowDialog() == DialogResult.OK)
			{
				return sfdSaveReport.FileName;
			}
			return null;
		}

		private DataGridView GetFinalData()
		{
			DataGridView result = new DataGridView();
			_totalCost = 0;

			for (int j = 1; j < dgvReport.Columns.Count - 2; j++)
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				result.Columns.Add(dgvReport.Columns[j].Clone() as DataGridViewColumn);
			}

			for (int i = 0; i < dgvReport.Rows.Count; i++)
			{
				if (dgvReport.Rows[i].Cells[0].Value != null && (bool)dgvReport.Rows[i].Cells[0].Value)
				{
					var rowIndex = result.Rows.Add();
					for (int j = 1; j < dgvReport.Columns.Count - 2; j++)
					{
						result.Rows[rowIndex].Cells[j - 1].Value = dgvReport.Rows[i].Cells[j].Value != null ? dgvReport.Rows[i].Cells[j].Value.ToString() : "";
					}
					result.Rows[rowIndex].Cells["Row"].Value = (rowIndex + 1).ToString();
					_totalCost += long.Parse((string)result.Rows[rowIndex].Cells["ReportFine"].Value, NumberStyles.AllowThousands);
				}
			}

			return result;
		}

		private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			dgvReport.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			FormLostCard formLostCard = new FormLostCard()
			{
				//ExitCardReader = _exitCardReader,
				AllowAnpr = Tools.AllowUsingAnpr(),
				AllowCamera = Tools.AllowExitCamera(),
				User = User,
				ExitImageName = ExitImageName
			};
			formLostCard.ShowDialog();
			if (cbFromDate.Checked || cbToDate.Checked)
			{
				btnSearch_Click(sender, e);
			}
		}
	}
}
