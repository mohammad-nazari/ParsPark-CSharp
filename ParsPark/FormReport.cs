using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExportFileLib;
using System.Drawing.Printing;
using System.Globalization;
using PrinterLib;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormReport : MetroForm
	{
		// The class that will do the printing process.
		DataGridViewPrinter _myDataGridViewPrinter;
		private long _totalCost;

		public FormReport()
		{
			InitializeComponent();
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (cbFromDate.Checked || cbToDate.Checked)
			{
				lblRecordsCount.Text = "";
				lblRecordsCost.Text = "";
				dgvReport.Rows.Clear();

				try
				{
					try
					{
						parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

						var logsList = parsPark.logs.GroupJoin(parsPark.subscription.Join(parsPark.car.Join(parsPark.driver, carObj => carObj.driverid, driverObj => driverObj.id, (carObj, driverObj) => new
						{
							idi = carObj.id,
							orgnamei = driverObj.orgname,
							orgvali = driverObj.orgval
						}), subObj => subObj.drivercarid, carDriverObj => carDriverObj.idi, (subObj, carDriverObj) => new { subObj, carDriverObj })
													.Select(t => new OrganSubInfo()
													{
														Code = t.subObj.code,
														Orgname = t.carDriverObj.orgnamei,
														Orgval = t.carDriverObj.orgvali
													}), logObj => logObj.code, subObj => subObj.Code, (logObj, subObj) =>
						new { logObj, subObj }).SelectMany(selection => selection.subObj.DefaultIfEmpty(), (selection, result) => new { selection.logObj, selection.subObj, result }).Select(selectResult => new { ID = selectResult.logObj.id, Code = selectResult.logObj.code, EnLicense = selectResult.logObj.enlicense, ExLicense = selectResult.logObj.exlicense, EnTime = selectResult.logObj.enter, ExTime = selectResult.logObj.exit.Value, ExUser = selectResult.logObj.exuser.Value, Cost = selectResult.logObj.cost.Value, Type = selectResult.result != null ? selectResult.result.Orgname + " (" + selectResult.result.Orgval + ")" : "عادی" });

						// Start date time
						if (cbFromDate.Checked)
						{
							if (dtpStartTime.Value != null)
							{
								logsList = logsList.Where(l => l.ExTime >= dtpStartTime.Value.Value);
							}
						}

						// End date time
						if (cbToDate.Checked)
						{
							if (dtpEndTime.Value != null)
							{
								logsList = logsList.Where(l => l.ExTime <= dtpEndTime.Value.Value);
							}
						}

						// users
						if (chlbUsers.CheckedItems.Count > 0 && chlbUsers.CheckedItems.Count < chlbUsers.Items.Count)
						{
							List<string> usersList = new List<string>();
							foreach (var user in chlbUsers.CheckedItems)
							{
								usersList.Add(Tools.Splitter(" (", user.ToString())[1].Replace(")", ""));
							}

							logsList = logsList.Where(l => usersList.Contains(l.ExUser.ToString()));
						}

						if (rbTypePublic.Checked)
						{
							logsList = logsList.Where(l => l.Type == "عادی");
						}
						else if (rbTypeSubscribe.Checked)
						{
							if (chlbOrgs.CheckedItems.Count > 0 && chlbOrgs.CheckedItems.Count < chlbOrgs.Items.Count)
							{
								List<string> organList = new List<string>();
								foreach (var org in chlbOrgs.CheckedItems)
								{
									organList.Add(org.ToString());
								}
								logsList = logsList.Where(l => organList.Contains(l.Type));
							}
							else
							{
								logsList = logsList.Where(l => l.Type != "عادی");
							}
						}

						if (rbCarOk.Checked)
						{
							logsList = logsList.GroupJoin(parsPark.blacklist, logObj => logObj.EnLicense, blObj => blObj.license, (logObj, blObj) => new { logObj, blObj }).SelectMany(selection => selection.blObj.DefaultIfEmpty(),
									(selection, result) => new { selection.logObj, selection.blObj, result }).Where(bl => bl.result == null).Select(result => result.logObj);
						}
						else if (rbCarBlackList.Checked)
						{

							logsList = logsList.GroupJoin(parsPark.blacklist, logObj => logObj.EnLicense, blObj => blObj.license, (logObj, blObj) => new { logObj, blObj }).SelectMany(selection => selection.blObj.DefaultIfEmpty(),
									(selection, result) => new { selection.logObj, selection.blObj, result }).Where(bl => bl.result != null).Select(result => result.logObj);
						}

						if (logsList.Any())
						{
							int allCost = 0;
							foreach (var log in logsList)
							{
								int rowIndex = dgvReport.Rows.Add();
								dgvReport.Rows[rowIndex].Cells["Row"].Value = rowIndex + 1;
								dgvReport.Rows[rowIndex].Cells["Code"].Value = log.Code;
								dgvReport.Rows[rowIndex].Cells["EnLicense"].Value = log.EnLicense;
								dgvReport.Rows[rowIndex].Cells["ExLicense"].Value = log.ExLicense;
								dgvReport.Rows[rowIndex].Cells["EnterTime"].Value = DateTimeClass.ToPersianFormat(log.EnTime);
								dgvReport.Rows[rowIndex].Cells["ExitTime"].Value = DateTimeClass.ToPersianFormat(log.ExTime);
								dgvReport.Rows[rowIndex].Cells["Duration"].Value = (log.ExTime - log.EnTime).ToString();
								dgvReport.Rows[rowIndex].Cells["Cost"].Value = log.Cost;
								dgvReport.Rows[rowIndex].Cells["Type"].Value = log.Type;
								dgvReport.Rows[rowIndex].Cells["Details"].Value = "جزئیات";
								dgvReport.Rows[rowIndex].Cells["Print"].Value = "پرینت";
								allCost += log.Cost;
								dgvReport.Rows[rowIndex].Tag = log.ID;
							}
							lblRecordsCount.Text = logsList.Count().ToString(@"0,0");
							lblRecordsCost.Text = allCost.ToString(@"0,0");
						}
						else
						{
							MessageBox.Show(@"هیچ رکوردی یافت نشد.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(@"خطا در پایگاه داده." + Environment.NewLine + ex.Message, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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

		private void FillUsersList()
		{
			chlbUsers.Items.Clear();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var userList = (parsPark.users.Select(d => new
				{
					d.id,
					d.username
				}));

				foreach (var userObject in userList)
				{
					chlbUsers.Items.Add(userObject.username + " (" + userObject.id + ")");
				}
			}
			catch
			{
				// ignored
			}
		}

		private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex >= 0)
			{
				if (dgvReport.Columns[e.ColumnIndex].Name == "Print")
				{
					if (dgvReport.Rows[e.RowIndex].Cells["ExitTime"].Value != null)
					{
						Printer printerObject = new Printer
						{
							LicensePlate = dgvReport.Rows[e.RowIndex].Cells["EnLicense"].Value != null && dgvReport.Rows[e.RowIndex].Cells["EnLicense"].Value.ToString() != "" ? dgvReport.Rows[e.RowIndex].Cells["EnLicense"].Value.ToString() : dgvReport.Rows[e.RowIndex].Cells["ExLicense"].Value != null && dgvReport.Rows[e.RowIndex].Cells["ExLicense"].Value.ToString() != "" ? dgvReport.Rows[e.RowIndex].Cells["ExLicense"].Value.ToString() : "مجهول",
							Cost = dgvReport.Rows[e.RowIndex].Cells["Cost"].Value?.ToString() ?? "",
							Name = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name,
							EnterTime = dgvReport.Rows[e.RowIndex].Cells["EnterTime"].Value?.ToString() ?? "",
							ExitTime = dgvReport.Rows[e.RowIndex].Cells["ExitTime"].Value.ToString(),
							Duration = dgvReport.Rows[e.RowIndex].Cells["Duration"].Value.ToString(),
							Lyric = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric,
							Type = dgvReport.Rows[e.RowIndex].Cells["Type"].Value.ToString(),
							Support = "پشتیبانی 09177382160"
						};

						printerObject.Run();
					}
				}
				else if (dgvReport.Columns[e.ColumnIndex].Name == "Details")
				{
					try
					{
						parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
						logs logObject = parsPark.logs.Find(dgvReport.Rows[e.RowIndex].Tag);

						if (logObject != null)
						{
							FormDetailes formDetails = new FormDetailes() { LogDetail = logObject };

							formDetails.ShowDialog();
						}
						else
						{
							MessageBox.Show(@"خطا در بارگذاری اطلاعات.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
					catch
					{
						MessageBox.Show(@"خطا در برقراری ارتباط با پایگاه داده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
			}
		}

		private void FormReport_Load(object sender, EventArgs e)
		{
			dtpStartTime.Value = DateTime.Now;
			dtpStartTime.Value = DateTimeClass.SetTime(dtpStartTime.Value.Value, 0, 0, 0);

			dtpEndTime.Value = dtpStartTime.Value;
			dtpEndTime.Value = DateTimeClass.SetTime(dtpEndTime.Value.Value, 23, 59, 0);

			FillOrgansList();
			FillUsersList();
		}

		private void FillOrgansList()
		{
			chlbOrgs.Items.Clear();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var organsList = (parsPark.driver.Select(d => new
				{
					d.orgname,
					d.orgval
				})).Distinct();

				foreach (var organsObject in organsList)
				{
					chlbOrgs.Items.Add(organsObject.orgname + " (" + organsObject.orgval + ")");
				}
			}
			catch
			{
				// ignored
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
								try
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
									MessageBox.Show(@"گزارش در فایل با موفقیت ذخیره گردید.", @"پیام", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
								catch (Exception ex)
								{
									MessageBox.Show(@"خطا در ذخیره سازی گزارش." + Environment.NewLine + ex.Message, @"پیام", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
							}
						}
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
			sfdSaveReport.FileName = "";
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

		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (dgvReport.Rows.Count > 0)
			{
				if (SetupThePrinting())
				{
					ppdlgReport.Document = pdocReport;
					ppdlgReport.ShowDialog();
				}
			}
			else
			{
				MessageBox.Show(@"لطفا دست کم یک سطر را انتخاب کنید", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		// The printing setup function
		private bool SetupThePrinting()
		{
			pdlgReport.AllowCurrentPage = false;
			pdlgReport.AllowPrintToFile = false;
			pdlgReport.AllowSelection = false;
			pdlgReport.AllowSomePages = false;
			pdlgReport.PrintToFile = false;
			pdlgReport.ShowHelp = false;
			pdlgReport.ShowNetwork = false;

			if (pdlgReport.ShowDialog() != DialogResult.OK)
				return false;

			pdocReport.DocumentName = "Customers Report";
			pdocReport.PrinterSettings = pdlgReport.PrinterSettings;
			pdocReport.DefaultPageSettings = pdlgReport.PrinterSettings.DefaultPageSettings;
			pdocReport.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

			DataGridView finalData = GetFinalData();

			_myDataGridViewPrinter = new DataGridViewPrinter(finalData, pdocReport, true, true, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name, new Font("B Nazanin", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
			return true;
		}

		// The PrintPage action for the PrintDocument control
		private void pdocReport_PrintPage(object sender, PrintPageEventArgs e)
		{

			bool more = _myDataGridViewPrinter.DrawDataGridView(e.Graphics);
			if (more)
			{
				e.HasMorePages = true;
			}
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
					_totalCost += long.Parse((string)result.Rows[rowIndex].Cells["Cost"].Value, NumberStyles.AllowThousands);
				}
			}

			return result;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{

		}

		private void cbFromDate_CheckedChanged(object sender, EventArgs e)
		{
			dtpStartTime.Enabled = cbFromDate.Checked;
		}

		private void cbToDate_CheckedChanged(object sender, EventArgs e)
		{
			dtpEndTime.Enabled = cbToDate.Checked;
		}

		private void btnPrintReceipt_Click(object sender, EventArgs e)
		{
			if (dgvReport.Rows.Count > 0)
			{
				DataGridView finalData = GetFinalData();
				if (finalData.Rows.Count > 1)
				{
					foreach (DataGridViewRow row in finalData.Rows)
					{
						if (row.Cells["ExitTime"].Value != null)
						{
							Printer printerObject = new Printer
							{
								LicensePlate = row.Cells["EnLicense"].Value != null && row.Cells["EnLicense"].Value.ToString() != "" ? row.Cells["EnLicense"].Value.ToString() : row.Cells["ExLicense"].Value != null && row.Cells["ExLicense"].Value.ToString() != "" ? row.Cells["ExLicense"].Value.ToString() : "مجهول",
								Cost = row.Cells["Cost"].Value?.ToString() ?? "",
								Name = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name,
								EnterTime = row.Cells["EnterTime"].Value?.ToString() ?? "",
								ExitTime = row.Cells["ExitTime"].Value.ToString(),
								Duration = row.Cells["Duration"].Value.ToString(),
								Lyric = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric,
								Type = row.Cells["Duration"].Value.ToString(),
								Support = "پشتیبانی 09177382160",
							};
							printerObject.Run();
						}
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

		private void rbTypeSubscribe_CheckedChanged(object sender, EventArgs e)
		{
			chlbOrgs.Enabled = rbTypeSubscribe.Checked;
		}

		private void clbOrgs_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
	}
}
