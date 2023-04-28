using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CameraLib;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormLostCard : MetroForm
	{
		public bool AllowCamera { get; set; } = true;

		public bool AllowAnpr { get; set; } = true;

		public users User { get; set; }

		public FormMain FormMainObject { set; get; }

		public string ExitImageName { get; set; }

		public long RowId { get; set; }

		public FormLostCard()
		{
			InitializeComponent();
		}

		private void FormLostCard_Load(object sender, EventArgs e)
		{
			dtpEnter.Value = DateTime.Now;
			dtpExit.Value = dtpEnter.Value;

			if (Tools.AllowUsingAnpr() && Tools.AllowCamera())
			{
				string camAddress = MyForms.SelectCamera();
				if (camAddress != "")
				{
					Camera cameraObject = new Camera { SourceUrl = camAddress };
					cameraObject.GetPicture();

					if (cameraObject.PictureObject != null)
					{
						if (Tools.IsServer())
						{
							// Start ANPR
							cameraObject.InitializeCamera();
							// Using ANPR
							cameraObject.ProgressPictureFromCamera();
						}
						else
						{
							cameraObject.License = FormMainObject.GetLpFromServer(cameraObject.PictureObject);
						}
					}
					else
					{
						MessageBox.Show(@"خطا در ثبت تصویر", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error,
							MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
					if (cameraObject.License != null && cameraObject.License.LicenseNumber != "")
					{
						// Load information
						MyForms.FillLicenseMaskedTextBox(cameraObject.License, ref mtxtLPNo1, ref mtxtLPAlpha, ref mtxtLPNo2,
							ref mtxtLPNo3);

						GetLogInfo(cameraObject.License);
					}
				}
			}

			FillList();
		}

		private void FillList()
		{
			// Clean list
			dgvDetails.Rows.Clear();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var logList = from l in parsPark.enterlogs
							  where l.exit == null
							  select
							  new { l.exit, l.enter, l.id, l.code, l.enlicense, l.exlicense, l.cost, l.type };
				foreach (var log in logList)
				{
					string type = log.type == LogType.pub.ToString() ? "عادی" : "مشترک";

					dgvDetails.InvokeIfRequired(d =>
					{
						int rowIndex = d.Rows.Add();
						d.Rows[rowIndex].Cells["Code"].Value = log.code;
						d.Rows[rowIndex].Cells["EnLicense"].Value = log.enlicense;
						d.Rows[rowIndex].Cells["ExLicense"].Value = log.exlicense ?? "";
						d.Rows[rowIndex].Cells["EnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
						d.Rows[rowIndex].Cells["ExitTime"].Value = log.exit != null ? DateTimeClass.ToPersianFormat(log.exit.Value) : "";
						d.Rows[rowIndex].Cells["Duration"].Value = log.exit != null ? (log.exit - log.enter).Value.ToString() : null;
						d.Rows[rowIndex].Cells["Cost"].Value = (log.exit != null ? log.cost.ToString() : "");
						d.Rows[rowIndex].Cells["Type"].Value = type;
						d.Rows[rowIndex].Cells["Detailes"].Value = "جزئیات";
						d.Rows[rowIndex].Cells["Selecting"].Value = "انتخاب";
						d.Rows[rowIndex].Tag = log.id;
					});
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در بازیابی اطلاعات.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnGetInfoFromLicense_Click(object sender, EventArgs e)
		{
			if (mtxtLPNo1.Text != "" && mtxtLPAlpha.Text != "" && mtxtLPNo2.Text != "" && mtxtLPNo3.Text != "")
			{
				LicensePlate license = new LicensePlate()
				{
					LpNumber1 = mtxtLPNo1.Text,
					LpAlpha = mtxtLPAlpha.Text,
					LpNumber2 = mtxtLPNo2.Text,
					LpNumber3 = mtxtLPNo3.Text,
				};
				license.LicenseNumber = license.GetStandardString();

				GetLogInfo(license);
			}
			else
			{
				MessageBox.Show(@"لطفا شماره پلاک را وارد نمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (txtCardNumber.Text != "" && mtxtLPNo1.Text != "" && mtxtLPAlpha.Text != "" && mtxtLPNo2.Text != "" && mtxtLPNo3.Text != "" && txtCardType.Text != "" && txtFName.Text != "" && txtLName.Text != "" && txtDriverLicense.Text != "" && mtxtFine.Text != "" && mtxtDriverID.Text != "" && mtxtPhone.Text != "")
			{
				lostcard lostCard = new lostcard();

				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					string licensePlate = mtxtLPNo2.Text + mtxtLPNo3.Text + mtxtLPAlpha.Text + mtxtLPNo1.Text;

					// Generate database record and save
					lostCard.cardid = txtCardNumber.Text;
					lostCard.license = licensePlate;
					lostCard.type = txtCardType.Text == @"مشترک" ? LogType.sub.ToString() : LogType.pub.ToString();
					lostCard.fname = txtFName.Text;
					lostCard.lname = txtLName.Text;
					lostCard.driverID = Convert.ToInt64(mtxtDriverID.Text);
					lostCard.dlicense = txtDriverLicense.Text;
					lostCard.phone = mtxtPhone.Text;
					lostCard.fine = Convert.ToInt32(mtxtFine.Text);
					lostCard.description = rtxtDescription.Text;

					parsPark.lostcard.Add(lostCard);
					if (parsPark.SaveChanges() > 0)
					{
						try
						{
							parsparkoEntities parsPark2 = new parsparkoEntities(GlobalVariables.ConnectionString);

							enterlogs logsObject = parsPark2.enterlogs.Find(RowId);

							var dateQuery = parsPark2.Database.SqlQuery<DateTime>("SELECT NOW()");

							DateTime exitTime = dateQuery.AsEnumerable().FirstOrDefault();
							if (logsObject != null && logsObject.enter < exitTime)
							{
								SubscriptionInfo subInfo = new SubscriptionInfo();
								long data = subInfo.GetSubscriptionInfo(txtCardNumber.Text, logsObject.enlicense);

								// Generate database record and save
								logsObject.cost = data > 0 ? 0 : int.Parse(mtxtCost.Text);
								logsObject.exlicense = licensePlate;
								logsObject.exit = exitTime;
								logsObject.exuser = User.id;
								logsObject.type = data > 0 ? LogType.sub.ToString() : LogType.pub.ToString();

								if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Save)
								{
									Stream stream = File.Open(ExitImageName, FileMode.Open);
									Image pictureObject = Image.FromStream(stream);
									logsObject.expicture = StringConvert.ImageToByteArray(pictureObject);
									stream.Close();
								}

								parsPark2.Entry(logsObject).CurrentValues.SetValues(logsObject);

								if (parsPark2.SaveChanges() > 0)
								{
									MessageBox.Show(@"کارت گم شده جدید ثبت شد.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
								else
								{
									MessageBox.Show(@"خطا در ثبت کارت گم شده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
							}
							else
							{
								MessageBox.Show(@"خطا در ثبت کارت گم شده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							}
						}
						catch (Exception exception)
						{
							MessageBox.Show(@"خطا در ثبت کارت گم شده." + Environment.NewLine + exception.Message, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
					else
					{
						MessageBox.Show(@"خطا در ثبت کارت گم شده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				catch
				{
					MessageBox.Show(@"خطا در ثبت کارت گم شده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"لطفا همه موارد را وارد نمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}

			FillList();
		}

		private void GetLogInfo(LicensePlate License)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				enterlogs log = (from l in parsPark.enterlogs where (l.enlicense == License.LicenseNumber || l.exlicense == License.LicenseNumber) && l.exit == null select l).FirstOrDefault();

				// Generate database record and save

				if (log != null)
				{
					txtCardNumber.Text = log.code;
					dtpEnter.Value = log.enter;
					dtpExit.Value = log.exit;
					LicensePlate lp = new LicensePlate();
					lp.LoadLicense(log.enlicense != "" ? log.enlicense : log.exlicense != "" ? log.exlicense : "مجهول");
					MyForms.FillLicenseMaskedTextBox(lp, ref mtxtLPNo1, ref mtxtLPAlpha, ref mtxtLPNo2, ref mtxtLPNo3);
					txtCardType.Text = log.type == LogType.sub.ToString() ? "مشترک" : "عادی";
				}
				else
				{
					MessageBox.Show(@"کارتی با این شماره پلاک وارد پارکینگ نشده است.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در ثبت کارت گم شده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void Calculate()
		{
			if (dtpExit.Value != null)
			{
				if (dtpEnter.Value != null)
				{
					GlobalVariables.AllSettingsObject.LoadGlobalSettings(GlobalVariables.ConnectionString);

					TimeSpan span = (dtpExit.Value.Value - dtpEnter.Value.Value);
					if (span.TotalMinutes > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Free)
					{
						DateTime dayStart = new DateTime(dtpExit.Value.Value.Year, dtpExit.Value.Value.Month, dtpExit.Value.Value.Day, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Start, 0, 0);

						DateTime dayEnd = new DateTime(dtpExit.Value.Value.Year, dtpExit.Value.Value.Month, dtpExit.Value.Value.Day, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.End, 0, 0);

						mtxtCost.Text = Tools.CalculateParkingCost(span, dtpEnter.Value.Value, dtpExit.Value.Value, dayStart, dayEnd).ToString();
					}
					txtDuration.Text = span.ToString();
				}
			}
		}

		private void dgvDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex > -1 && (dgvDetails.Columns[e.ColumnIndex].Name == "Selecting" || dgvDetails.Columns[e.ColumnIndex].Name == "Detailes"))
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				enterlogs log = parsPark.enterlogs.Find(dgvDetails.Rows[e.RowIndex].Tag);
				if (log != null)
				{
					if (dgvDetails.Columns[e.ColumnIndex].Name == "Selecting")
					{
						txtCardNumber.Text = log.code;
						dtpEnter.Value = log.enter;
						dtpExit.Value = log.exit;
						LicensePlate lp = new LicensePlate();
						lp.LoadLicense(log.enlicense != "" ? log.enlicense : log.exlicense != "" ? log.exlicense : "مجهول");
						MyForms.FillLicenseMaskedTextBox(lp, ref mtxtLPNo1, ref mtxtLPAlpha, ref mtxtLPNo2, ref mtxtLPNo3);
						txtCardType.Text = log.type == LogType.sub.ToString() ? "مشترک" : "عادی";
						RowId = log.id;

						Calculate();
					}
					else if (dgvDetails.Columns[e.ColumnIndex].Name == "Detailes")
					{
						try
						{
							FormDetailes formDetails = new FormDetailes()
							{
								LogDetail = new logs()
								{
									code = log.code,
									enuser = log.enuser,
									exuser = log.exuser,
									enter = log.enter,
									exit = log.exit,
									enlicense = log.enlicense,
									exlicense = log.exlicense,
									cost = log.cost,
									enpicture = log.enpicture,
									expicture = log.expicture,
								}
							};

							formDetails.ShowDialog();
						}
						catch
						{
							MessageBox.Show(@"خطا در برقراری ارتباط با پایگاه داده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
				}
			}
		}

		private void dtpExit_ValueChanged(object sender, EventArgs e)
		{
			Calculate();
		}
	}
}
