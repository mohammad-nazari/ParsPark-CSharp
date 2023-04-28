using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CameraLib;
using DataBaseLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormEditSubscription : MetroForm
	{
		/*public CardReaderEnter EnterCardReader { get; set; } = new CardReaderEnter();

		public CardReaderExit ExitCardReader { get; set; } = new CardReaderExit();*/

		public FormMain FormMainObject { get; set; }

		public string CarLicense { get; set; }

		public string SubId { get; set; }

		public bool IsNew { get; set; }

		public FormEditSubscription()
		{
			InitializeComponent();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			if (ofdPicture.ShowDialog() == DialogResult.OK)
			{
				pbUser.Image = Image.FromFile(ofdPicture.FileName);
			}
		}

		private void pbUser_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(pbUser, "برای بارگذاری عکس کلیک کنید");
		}

		private void cbSelectLicense_SelectedIndexChanged(object sender, EventArgs e)
		{
			List<string> selectedItem = Tools.Splitter(" (", cbLP.SelectedItem.ToString());
			if (selectedItem.Count > 0)
			{
				string strLicense = selectedItem.FirstOrDefault();

				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					var driverCarObject = (from d in parsPark.driver
										   join c in parsPark.car on d.id equals c.driverid
										   where c.license == strLicense
										   select new
										   {
											   c.license,
											   d.fname,
											   d.lname,
											   d.phone,
											   dpicture = d.picture,
											   cpicture = c.picture
										   }).FirstOrDefault();

					if (driverCarObject != null)
					{
						FillFormData(driverCarObject.fname, driverCarObject.lname, driverCarObject.license, driverCarObject.phone, txtOrgName.Text, txtOrgValue.Text, mtxtCost.Text, rtxtAddress.Text, dtsStart.Value ?? DateTime.Now, dtsEnd.Value ?? DateTime.Now, StringConvert.ByteArrayToImage(driverCarObject.dpicture), StringConvert.ByteArrayToImage(driverCarObject.cpicture));
					}
				}
				catch
				{
					// ignored
				}
			}
		}

		private void FormEditSubscription_Load(object sender, EventArgs e)
		{
			FillLicensePlatesList();
			FillOrgansList();

			if (IsNew)
			{
				// Fill data with empty string
				FillFormData(@"", @"", @"", @"", @"", @"", @"", @"", DateTime.Now, DateTime.Now.AddMonths(1), null, null, true);
				//FillFormData("احمد", "حسینی", "58463ب96", "9177354882", "دانشگاه علوم پزشکی شیراز", "SUMS", "5000000", "شیراز", DateTime.Now, DateTime.Now.AddYears(1), null, null, true);

				MessageBox.Show(@"لطفا کارت مورد نظر را به کارتخوان نزدیک کنید.", @"توجه", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

				timerCardReader.Enabled = true;
				timerCardReader.Start();
			}
			else
			{
				Text = @"ویرایش اشتراک";
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

					subscription subObject = parsPark.subscription.Find(Convert.ToInt32(SubId));

					if (subObject != null)
					{
						txtCode.Text = subObject.code;

						parsparkoEntities parsPark2 = new parsparkoEntities(GlobalVariables.ConnectionString);

						var driverCarObject = (from d in parsPark2.driver
											   join c in parsPark2.car on d.id equals c.driverid
											   where c.id == subObject.drivercarid
											   select new
											   {
												   c.license,
												   d.fname,
												   d.lname,
												   d.phone,
												   d.orgname,
												   d.orgval,
												   dpicture = d.picture,
												   cpicture = c.picture
											   }).FirstOrDefault();

						if (driverCarObject != null)
						{
							FillFormData(driverCarObject.fname, driverCarObject.lname, driverCarObject.license, driverCarObject.phone, driverCarObject.orgname, driverCarObject.orgval, subObject.cost.ToString(CultureInfo.InvariantCulture), rtxtAddress.Text, subObject.startdate, subObject.enddate, StringConvert.ByteArrayToImage(driverCarObject.dpicture), StringConvert.ByteArrayToImage(driverCarObject.cpicture), IsNew);
						}
					}
				}
				catch
				{
					// ignored
				}
			}


		}

		private void FillLicensePlatesList()
		{
			cbLP.Items.Clear();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var driverCarList = (parsPark.driver.Join(parsPark.car, d => d.id, c => c.driverid, (d, c) => new
				{
					c.license,
					d.fname,
					d.lname
				}));

				foreach (var driverObject in driverCarList)
				{
					cbLP.Items.Add(driverObject.license + " (" + driverObject.fname + " " + driverObject.lname + ")");
				}
			}
			catch
			{
				// ignored
			}
		}

		private void FillOrgansList()
		{
			cbOrgans.Items.Clear();
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
					cbOrgans.Items.Add(organsObject.orgname + " (" + organsObject.orgval + ")");
				}
			}
			catch
			{
				// ignored
			}
		}

		private void FillFormData(string FirstName, string LastName, string LicenseNumber, string PhoneNumber, string OrganName, string OrganValue, string Cost, string Address, DateTime StartDateTime, DateTime EndDateTime, Image UserPicture = null, Image CarPicture = null, bool ReadOnlyFormat = false)
		{
			btnSubmit.Enabled = !ReadOnlyFormat;

			txtFname.Text = FirstName;
			txtFname.ReadOnly = ReadOnlyFormat;
			txtFname.Enabled = !ReadOnlyFormat;

			txtLname.Text = LastName;
			txtLname.ReadOnly = ReadOnlyFormat;
			txtLname.Enabled = !ReadOnlyFormat;

			mtxtPhone.Text = PhoneNumber;
			mtxtPhone.ReadOnly = ReadOnlyFormat;
			mtxtPhone.Enabled = !ReadOnlyFormat;

			LicensePlate license = new LicensePlate { LicenseNumber = LicenseNumber };
			license.LoadLicense(LicenseNumber);

			MyForms.FillLicenseMaskedTextBox(license, ref mtxtLPNo1, ref mtxtLPAlpha, ref mtxtLPNo2, ref mtxtLPNo3);

			mtxtLPNo1.ReadOnly = ReadOnlyFormat;
			mtxtLPNo1.Enabled = !ReadOnlyFormat;
			mtxtLPAlpha.ReadOnly = ReadOnlyFormat;
			mtxtLPAlpha.Enabled = !ReadOnlyFormat;
			mtxtLPNo2.ReadOnly = ReadOnlyFormat;
			mtxtLPNo2.Enabled = !ReadOnlyFormat;
			mtxtLPNo3.ReadOnly = ReadOnlyFormat;
			mtxtLPNo3.Enabled = !ReadOnlyFormat;

			cbLP.Enabled = !ReadOnlyFormat;

			txtOrgName.Text = OrganName;
			txtOrgName.ReadOnly = ReadOnlyFormat;
			txtOrgName.Enabled = !ReadOnlyFormat;

			cbOrgans.Enabled = !ReadOnlyFormat;

			txtOrgValue.Text = OrganValue;
			txtOrgValue.ReadOnly = ReadOnlyFormat;
			txtOrgValue.Enabled = !ReadOnlyFormat;

			dtsStart.Enabled = !ReadOnlyFormat;
			dtsStart.Value = StartDateTime;
			dtsEnd.Enabled = !ReadOnlyFormat;
			dtsEnd.Value = EndDateTime;


			mtxtCost.Text = Cost;
			mtxtCost.ReadOnly = ReadOnlyFormat;
			mtxtCost.Enabled = !ReadOnlyFormat;

			rtxtAddress.Text = Address;
			rtxtAddress.ReadOnly = ReadOnlyFormat;
			rtxtAddress.Enabled = !ReadOnlyFormat;

			if (UserPicture != null)
			{
				pbUser.Image = UserPicture;
			}
			pbUser.Enabled = !ReadOnlyFormat;

			if (CarPicture != null)
			{
				pbCLP.Image = CarPicture;
			}
			pbCLP.Enabled = !ReadOnlyFormat;
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			// Check Card Serial Number
			if (CheckCardNumber(txtCode.Text) == false)
			{
				LicensePlate license = new LicensePlate()
				{
					LpNumber1 = mtxtLPNo1.Text,
					LpAlpha = mtxtLPAlpha.Text,
					LpNumber2 = mtxtLPNo2.Text,
					LpNumber3 = mtxtLPNo3.Text
				};
				license.LicenseNumber = license.GetStandardString();

				// Check user and car number plate
				long driverId = CheckDriverInfo();
				driverId = driverId > 0 ? UpdateDriverInfo(driverId) : AddNewDriver();

				if (driverId > 0)
				{
					long carId = CheckLicensePlate(license.LicenseNumber, driverId);
					// Check license plate
					if (carId > 0)
					{
						// submit new subscription
						if (AddNewSubscription(carId) > 0)
						{
							MessageBox.Show(@"اشتراک جدید ثبت گردید. ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							DialogResult = DialogResult.OK;
							Close();
						}
						else
						{
							MessageBox.Show(@"خطا در ثبت اشتراک جدید ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						}
					}
					else
					{
						carId = CheckLicensePlate(license.LicenseNumber);
						if (carId <= 0)
						{
							// Add license to driver car list
							carId = AddCarToDriverList(license.LicenseNumber, driverId);
							if (carId > 0)
							{
								// Submit new subscription
								if (AddNewSubscription(carId) > 0)
								{
									MessageBox.Show(@"اشتراک جدید ثبت گردید. ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
									DialogResult = DialogResult.OK;
									Close();
								}
								else
								{
									MessageBox.Show(@"خطا در ثبت اشتراک جدید ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
							}
							else
							{
								MessageBox.Show(@"خطا در ثبت ماشین " + license.LicenseNumber + @" برای راننده : " + txtFname.Text + @" " + txtLname.Text, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							}
						}
						else // carId > 0
						{
							// Car is for another driver
							DialogResult dResult = MessageBox.Show(@"ماشین با پلاک " + license.LicenseNumber + @" متعلق به راننده " + txtFname + @" " + txtLname + @"می باشد." + Environment.NewLine + @"(Yes)ثبت اشتراک برای راننده قدیمی" + Environment.NewLine + @"(No)ثبت اشتراک برای راننده جدید(انتقال ماشین به پروفایل راننده جدید)", @"هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							if (dResult == DialogResult.Yes)
							{
								// Add new subscription to old driver
								if (AddNewSubscription(carId) > 0)
								{
									MessageBox.Show(@"اشتراک جدید ثبت گردید. ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
									DialogResult = DialogResult.OK;
									Close();
								}
								else
								{
									MessageBox.Show(@"خطا در ثبت اشتراک جدید ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
								}
							}
							else if (dResult == DialogResult.No)
							{
								// Transfer car info to new driver profile
								// Add new subscription to new driver
								if (TransformCarInformation(carId, driverId) > 0)
								{
									carId = AddCarToDriverList(license.LicenseNumber, driverId);
									if (carId > 0)
									{
										// Submit new subscription
										if (AddNewSubscription(carId) > 0)
										{
											MessageBox.Show(@"اشتراک جدید ثبت گردید. ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
											DialogResult = DialogResult.OK;
											Close();
										}
										else
										{
											MessageBox.Show(@"خطا در ثبت اشتراک جدید ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
										}
									}
									else
									{
										MessageBox.Show(@"خطا در ثبت ماشین " + license.LicenseNumber + @" برای راننده : " + txtFname.Text + @" " + txtLname.Text, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
									}
								}
							}
						}
					}
				}
				else
				{
					MessageBox.Show(@"خطا در ویرایش مشخصات راننده: " + txtFname.Text + @" " + txtLname.Text, @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
		}

		private long AddCarToDriverList(string LicensePlate, long DriverId)
		{
			// Add license to driver car list
			try
			{
				try
				{
					parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
					car carObj = new car
					{
						license = LicensePlate,
						name = "",
						picture = pbCLP.Image != null ? StringConvert.ImageToByteArray(pbCLP.Image) : null,
						driverid = DriverId
					};


					carObj = parsPark.car.Add(carObj);
					if (parsPark.SaveChanges() > 0)
					{
						return carObj.id;
					}
				}
				catch
				{
					// ignored
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private decimal TransformCarInformation(long CarId, long NewDriverId)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				car carObject = parsPark.car.Find(CarId);
				if (carObject != null)
				{
					carObject.id = CarId;
					carObject.driverid = NewDriverId;

					parsPark.Entry(carObject).CurrentValues.SetValues(carObject);
					if (parsPark.SaveChanges() > 0)
					{
						return carObject.id;
					}
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private long AddNewDriver()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				driver driverObject = new driver
				{
					fname = txtFname.Text,
					lname = txtLname.Text,
					address = rtxtAddress.Text,
					phone = mtxtPhone.Text,
					orgname = txtOrgName.Text,
					orgval = txtOrgValue.Text,
					picture = pbUser.Image != null ? StringConvert.ImageToByteArray(pbUser.Image) : null
				};


				driverObject = parsPark.driver.Add(driverObject);
				if (parsPark.SaveChanges() > 0)
				{
					return driverObject.id;
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private long CheckLicensePlate(string LicensePlate, long DriverId = 0)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				IQueryable<car> carObject = (from c in parsPark.car
											 where c.license == LicensePlate
											 select c);
				if (DriverId > 0)
				{
					carObject = carObject.Where(c => c.driverid == DriverId);
				}
				if (carObject.Any())
				{
					return carObject.First().id;
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private long UpdateDriverInfo(long DriverId)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				driver drv = parsPark.driver.Find(DriverId);
				if (drv != null)
				{
					drv.phone = mtxtPhone.Text;
					drv.address = rtxtAddress.Text;
					drv.orgname = txtOrgName.Text;
					drv.orgval = txtOrgValue.Text;
					drv.picture = pbUser.Image != null ? ImageProcess.ImageToByteArray(pbUser.Image) : null;

					parsPark.Entry(drv).CurrentValues.SetValues(drv);
					if (parsPark.SaveChanges() > 0)
					{
						return drv.id;
					}
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private long CheckDriverInfo()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				driver driverObject = (from d in parsPark.driver
									   where d.fname == txtFname.Text
									   && d.lname == txtLname.Text
									   select d).FirstOrDefault();
				if (driverObject != null)
				{
					return driverObject.id;
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private decimal AddNewSubscription(long driverCarId)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				if (IsNew)
				{
					if (dtsStart.Value != null)
					{
						if (dtsEnd.Value != null)
						{
							subscription subObject = new subscription
							{
								drivercarid = driverCarId,
								startdate = dtsStart.Value.Value,
								enddate = dtsEnd.Value.Value,
								cost = Convert.ToInt32(mtxtCost.Text),
								code = txtCode.Text
							};

							subObject = parsPark.subscription.Add(subObject);
							if (parsPark.SaveChanges() > 0)
							{
								return subObject.id;
							}
						}
					}
				}
				else
				{
					if (dtsStart.Value != null)
					{
						if (dtsEnd.Value != null)
						{
							subscription subObject = parsPark.subscription.Find(Convert.ToInt32(SubId));
							if (subObject != null)
							{
								subObject.drivercarid = driverCarId;
								subObject.startdate = dtsStart.Value.Value;
								subObject.enddate = dtsEnd.Value.Value;
								subObject.cost = Convert.ToInt32(mtxtCost.Text);
								subObject.code = txtCode.Text;

								parsPark.Entry(subObject).CurrentValues.SetValues(subObject);
								if (parsPark.SaveChanges() > 0)
								{
									return subObject.id;
								}
							}
						}
					}
				}
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		private void timerCardReader_Tick(object sender, EventArgs e)
		{
			FormMainObject.EnterCardReader.ReadCardData();

			FormMainObject.ExitCardReader.ReadCardData();

			string strCardSerialNumber = FormMainObject.EnterCardReader.LastCardSerialNumber != "" ? FormMainObject.EnterCardReader.LastCardSerialNumber : FormMainObject.ExitCardReader.LastCardSerialNumber;

			if ((FormMainObject.CheckCard(strCardSerialNumber)) > 0)
			{

				var error = strCardSerialNumber == "" || CheckCardNumber(strCardSerialNumber);
				if (error == false)
				{
					txtCode.Text = strCardSerialNumber;

					// Fill data with empty string
					FillFormData(@"", @"", @"", @"", @"", @"", @"", @"", DateTime.Now, DateTime.Now.AddMonths(1));

					// exit from timer
					timerCardReader.Stop();
					timerCardReader.Enabled = false;
				}
			}
		}

		/// <summary>
		/// 
		/// Check card number
		/// There is not a car in parking with this card
		/// This card is not registered before and have time
		/// </summary>
		/// <param name="CardSerialNumber"></param>
		/// <returns></returns>
		private bool CheckCardNumber(string CardSerialNumber)
		{
			bool error = false;
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				logs logObj = parsPark.logs.FirstOrDefault(l => l.code == CardSerialNumber && l.exit == null);

				if (logObj != null)
				{
					MessageBox.Show(@"اتومبیلی با این کارت در پارکینگ وجود دارد و هنوز خارج نشده است." + Environment.NewLine + @"لطفا از کارت دیگری استفاده نمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					error = true;
				}
			}
			catch
			{
				error = false;
			}

			if (error == false && IsNew)
			{
				try
				{
					parsparkoEntities parsPark2 = new parsparkoEntities(GlobalVariables.ConnectionString);

					subscription subObj = parsPark2.subscription.FirstOrDefault(s => s.code == CardSerialNumber);

					if (subObj != null)
					{
						MessageBox.Show(@"این کارت دارای اعتبار می باشد." + Environment.NewLine + @"لطفا از کارت دیگری استفاده نمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
						error = true;
					}
				}
				catch
				{
					error = false;
				}
			}

			return error;
		}

		private void pbCLP_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show(@"انتخاب عکس از فایل (Yes)" + Environment.NewLine + @"ثبت عکس با دوربین (No)", @"ثبت عکس", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			if (dialogResult == DialogResult.Yes)
			{
				if (ofdPicture.ShowDialog() == DialogResult.OK)
				{
					pbCLP.Image = Image.FromFile(ofdPicture.FileName);
				}
			}
			else if (dialogResult == DialogResult.No)
			{
				string camAddressEn = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow ==
									PermissionAllow.Yes
					? GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL : "";
				string camAddressEx = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow ==
									PermissionAllow.Yes
					? GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.URL : "";

				if (camAddressEn != "" && camAddressEx != "")
				{
					dialogResult = MessageBox.Show(@"از دوربین ورودی (Yes)" + Environment.NewLine + @"از دوربین خروجی (No)", @"ثبت عکس", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					if (dialogResult == DialogResult.No)
					{
						camAddressEn = camAddressEx;
					}
				}
				else if (camAddressEx != "")
				{
					camAddressEn = camAddressEx;
				}

				if (dialogResult != DialogResult.Cancel)
				{
					Camera cameraObject = new Camera { SourceUrl = camAddressEn };
					cameraObject.GetPicture();
					pbCLP.Image = cameraObject.PictureObject;
				}
			}
			if (dialogResult != DialogResult.Cancel && pbCLP.Image != null)
			{
				dialogResult = MessageBox.Show(@"آیا پلاک از روی عکس بازیابی شود؟", @"ثبت پلاک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				if (dialogResult == DialogResult.Yes)
				{
					Camera cameraObject = new Camera { PictureObject = (Bitmap)pbCLP.Image };
					if (Tools.AllowUsingAnpr())
					{
						if (Tools.IsServer())
						{
							cameraObject.InitializeCamera();
							// Start ANPR
							cameraObject.ProgressPictureFromCamera();
						}
						else
						{
							//ignored
							cameraObject.License = FormMainObject.GetLpFromServer(pbCLP.Image);
						}
					}

					mtxtLPNo1.InvokeIfRequired(c =>
					{
						c.Text = cameraObject.License.LpNumber1;
					});
					mtxtLPAlpha.InvokeIfRequired(c =>
					{
						c.Text = cameraObject.License.LpAlpha;
					});
					mtxtLPNo2.InvokeIfRequired(c =>
					{
						c.Text = cameraObject.License.LpNumber2;
					});
					mtxtLPNo3.InvokeIfRequired(c =>
					{
						c.Text = cameraObject.License.LpNumber3;
					});
				}
			}
		}

		private void pbCLP_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(pbCLP, @"برای بارگذاری عکس کلیک کنید");
		}

		private void FormEditSubscription_FormClosed(object sender, FormClosedEventArgs e)
		{
			timerCardReader.Dispose();
			timerCardReader = null;
		}

		private void FormEditSubscription_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerCardReader.Stop();
		}

		private void cbOrgans_SelectedIndexChanged(object sender, EventArgs e)
		{
			List<string> selectedItem = Tools.Splitter(" (", cbOrgans.SelectedItem.ToString());
			if (selectedItem.Count > 0)
			{
				txtOrgName.Text = selectedItem.FirstOrDefault();
				txtOrgValue.Text = selectedItem[1].Replace(" (", "").Replace(")", "");
			}
		}
	}
}
