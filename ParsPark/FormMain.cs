using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ToolsLib;
using System.Linq;
using PrinterLib;
using CardReaderExitLib;
using CardReaderEnterLib;
using DataBaseLib;
using SerialPortLib;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.ServiceModel;
using System.Timers;
using Detailes = ToolsLib.Detailes;
using Image = System.Drawing.Image;
using Message = System.Windows.Forms.Message;
using Timer = System.Timers.Timer;
using System.Threading;
using MetroFramework.Forms;
// ReSharper disable RedundantLogicalConditionalExpressionOperand
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
#pragma warning disable 162

namespace ParsPark
{
	public partial class FormMain : MetroForm
	{
		private readonly Timer _timerEnter = new Timer(100);
		private readonly Timer _timerExit = new Timer(100);
		private readonly Timer _timerCloseExitGate = new Timer(100);
		private readonly Timer _timerCloseEnterGate = new Timer(100);
		private readonly Timer _timerUpdateGui = new Timer(5000);
		private readonly Timer _timerSendCapacity = new Timer(60000);
		private readonly Timer _timerShowClock = new Timer(1000);
		private readonly Timer _timerListiner = new Timer(1000);

		private ServiceHost _serviceListiner = new ServiceHost(typeof(ParsParkWebService), new Uri("http://localhost:50005"));

		private readonly ActivationClass _actviationObj = new ActivationClass();

		private bool _isActivated;
		private users _user = new users();
		private bool _isLogedin;

		public CardReaderEnter EnterCardReader { get; } = new CardReaderEnter();
		public CardReaderExit ExitCardReader { get; } = new CardReaderExit();

		private readonly SubscriptionInfo _enterSubInfo = new SubscriptionInfo();
		private readonly SubscriptionInfo _exitSubInfo = new SubscriptionInfo();

		private readonly GateEnter _serialComPortEnter = new GateEnter();
		private readonly GateExit _serialComPortExit = new GateExit();
		private readonly PriceLed _serialComPortPrice = new PriceLed();
		private readonly CapacityLED _serialComPortCapacity = new CapacityLED();
		private readonly CityPay _serialComPortCityPay = new CityPay();
		private readonly POS _serialComPortPos = new POS();

		// Lock when get device list and when search in device list
		private readonly object _pictureBoxEnterLock = new object();
		private readonly object _pictureBoxExitLock = new object();
		private readonly object _pictureBoxExitEnterLock = new object();

		private bool _editEnterLicensePlate;
		private bool _editExitLicensePlate;

		private bool _submitEnterRecord;
		private bool _submitExitRecord;

		private bool _initializeEnterCardReader = true;
		private bool _initializeExitCardReader = true;

		private bool _acceptInput = true;

		private static readonly object DataGridLock = new object();
		private static readonly object CapacityLock = new object();
		private static readonly object FillCapacityLock = new object();

		private readonly string _enterImageName = "ImageIn.jpg";
		private readonly string _exitImageName = "ImageOut.jpg";

		private DateTime _startDateTime = DateTime.Now;

		readonly Dictionary<int, string> _editOk = new Dictionary<int, string>() { { 0, "ویرایش" }, { 1, "ثبت" } };

		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			InitializeTimersThread(_timerEnter, timerEnter_Tick, false, false);
			InitializeTimersThread(_timerExit, timerExit_Tick, false, false);
			InitializeTimersThread(_timerCloseExitGate, timerCloseExitGate_Tick, false, false);
			InitializeTimersThread(_timerCloseEnterGate, timerCloseEnterGate_Tick, false, false);
			InitializeTimersThread(_timerUpdateGui, timerUpdateGUI_Tick, false, false);
			InitializeTimersThread(_timerSendCapacity, timerSendCapacity_Tick, false, false);
			InitializeTimersThread(_timerShowClock, timerShowClock_Tick, false, false);
			InitializeTimersThread(_timerListiner, timerListiner_Tick, false, false);

			// Check is software activated
			_isActivated = _actviationObj.IsSoftwareActivated();
			if (_isActivated == false)
			{
				// Load activation form
				FormActive formActive = new FormActive()
				{
					ActivationObject = _actviationObj
				};

				formActive.ShowDialog();
				_isActivated = formActive.IsActivated;
			}

			if (_isActivated)
			{
				_isLogedin = false;

				Load_Settings();
				InitializeSettingObject();

				FormLogin login = new FormLogin()
				{
					FormMainObject = this
				};

				login.ShowDialog();
				if (login.DialogResult == DialogResult.OK)
				{
					_isLogedin = true;
					_user = login.UserInfo;

					// Just in Enterprise edition ANPR is enabled
					if (Tools.AllowUsingAnpr() && Tools.IsServer())
					{
						EnterCardReader.CameraObject.InitializeCamera();
						ExitCardReader.CameraObject.InitializeCamera();
					}

					// Send data to other ParsPark software system on TCP
					//Task.Factory.StartNew(StartTcpListener);

					FillDetailsList();

					EnableDisableEnterFormToolbox(false);
					EnableDisableExitFormToolbox(false);

					btnEnSubmit.Enabled = false;
					btnExSubmit.Enabled = false;

					ChangeSettings();
					_acceptInput = true;

					EnableDisableTimersThread(_timerEnter, true);
					StartStopTimersThread(_timerEnter, true);

					EnableDisableTimersThread(_timerExit, true);
					StartStopTimersThread(_timerExit, true);

					EnableDisableTimersThread(_timerCloseExitGate, true);
					StartStopTimersThread(_timerCloseExitGate, true);

					EnableDisableTimersThread(_timerCloseEnterGate, true);
					StartStopTimersThread(_timerCloseEnterGate, true);

					EnableDisableTimersThread(_timerUpdateGui, true);
					StartStopTimersThread(_timerUpdateGui, true);

					EnableDisableTimersThread(_timerSendCapacity, true);
					StartStopTimersThread(_timerSendCapacity, true);

					EnableDisableTimersThread(_timerShowClock, true);
					StartStopTimersThread(_timerShowClock, true);

					EnableDisableTimersThread(_timerListiner, true);
					StartStopTimersThread(_timerListiner, true);
				}
				else
				{
					Close();
				}
			}
			else
			{
				Close();
			}
		}

		private void lblSettings_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblSettings, @"تنظیمات");
		}

		private void lblUsers_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblUsers, @"کاربران");
		}

		private void lblSubs_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblSubs, @"مشترکین");
		}

		private void lblBlackList_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblBlackList, @"لیست سیاه");
		}

		private void lblReports_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblReports, @"جستجو و گزارش گیری");
		}

		private void lblExit_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblExit, @"خروج");
		}

		private void lblLostCards_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblLostCards, @"کارت های گم شده");
		}

		private void lblAbout_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(lblAbout, @"درباره نرم افزار");
		}

		private void ChangeSettings(bool IsInitialize = true)
		{
			if (_serviceListiner.State == CommunicationState.Opened)
			{
				try
				{
					_serviceListiner.Close();
				}
				catch
				{
					statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در دریافت اطلاعات از پورت نرم افزار ");
				}
			}
			ChangeEntityFrameworkConnectionString();
			if (!GlobalVariables.AllSettingsObject.SaveGlobalSettings(GlobalVariables.ConnectionString))
			{
				MessageBox.Show(@"خطا در ثبت تنظیمات در پایگاه داده", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			SetSkin();
			if (_isLogedin)
			{
				ChangeCameraSettings();
				ChangeBarrierSettings();
				ChangeBoardSettings();
				ChangePaymentSettings();
				if (IsInitialize)
				{
					ChangeCardReaderSettings();
				}
			}
		}

		public void ChangeEntityFrameworkConnectionString()
		{
			GlobalVariables.ConnectionString = "server=" + GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Server + ";port=" + GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Port + ";database=" + GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Database + ";user id=" + GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Username + ";password=" + GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Password + ";persistsecurityinfo = True;Connection Timeout=3";
		}

		private void ChangeCameraSettings()
		{
			if (Tools.AllowCamera())
			{
				if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow == PermissionAllow.Yes)
				{
					EnterCardReader.CameraObject.SourceUrl = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL;
				}

				if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow == PermissionAllow.Yes)
				{
					ExitCardReader.CameraObject.SourceUrl = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.URL;
				}
			}
		}

		private void ChangeCardReaderSettings()
		{
			InitializeCardReaders();
		}

		private void ChangeBarrierSettings()
		{
			InitializeBarrierComPort();
		}

		private void ChangeBoardSettings()
		{
			InitializeBoardComPort();
		}

		private void ChangePaymentSettings()
		{
			InitializePaymentComPort();
		}

		/// <summary>
		/// Initialize Setting object data
		/// </summary>
		/// <returns></returns>
		private void InitializeSettingObject()
		{
			GlobalVariables.AllSettingsObject.InitializeSettingObject();
			ChangeEntityFrameworkConnectionString();
		}

		/// <summary>
		/// Load setting from Setting.xml file
		/// </summary>
		/// <returns></returns>
		private void Load_Settings()
		{
			if (GlobalVariables.AllSettingsObject.LoadSettings() == false)
			{
				MessageBox.Show(@"خطا در خواندن تنظیمات از فایل", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void InitializeTimersThread(Timer TimerName, ElapsedEventHandler MethodName, bool EnableDisable, bool StartStop)
		{
			TimerName.Elapsed += MethodName;
			TimerName.AutoReset = true;
			EnableDisableTimersThread(TimerName, EnableDisable);
			StartStopTimersThread(TimerName, StartStop);
		}

		private void EnableDisableTimersThread(Timer TimerName, bool EnableDisable)
		{
			TimerName.Enabled = EnableDisable;
		}

		private void StartStopTimersThread(Timer TimerName, bool StartStop)
		{
			if (StartStop)
			{
				TimerName.Start();
			}
			else
			{
				TimerName.Stop();
			}
		}

		private void SetSkin()
		{
			//this.skeMain.SkinFile = Environment.CurrentDirectory + @"\Skins\" + GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Skin;
			//this.skeMain.Active = true;
		}

		// Retrieve all rows from enterlogs table
		private void FillDetailsList()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var logList = from l in parsPark.enterlogs
					select
					new { l.exit, l.enter, l.id, l.code, l.enlicense, l.exlicense, l.cost, l.type };
				UpdateParkingCapacity((from l in parsPark.enterlogs where l.exit == null select l).Count());
				foreach (var log in logList)
				{
					SubscriptionInfo subInfo = new SubscriptionInfo();
					subInfo.GetSubscriptionInfo(log.code, log.enlicense);

					lock (DataGridLock)
					{
						dgvDetailsAll.InvokeIfRequired(d =>
						{
							int rowIndex = d.Rows.Add();
							d.Rows[rowIndex].Cells["Code"].Value = log.code;
							d.Rows[rowIndex].Cells["EnLicense"].Value = log.enlicense;
							d.Rows[rowIndex].Cells["ExLicense"].Value = log.exlicense ?? "";
							d.Rows[rowIndex].Cells["EnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
							d.Rows[rowIndex].Cells["ExitTime"].Value = log.exit != null ? DateTimeClass.ToPersianFormat(log.exit.Value) : "";
							d.Rows[rowIndex].Cells["Duration"].Value = log.exit != null ? (log.exit - log.enter).Value.ToString() : null;
							d.Rows[rowIndex].Cells["Cost"].Value = log.cost;
							d.Rows[rowIndex].Cells["Type"].Value = subInfo.OrganInformation;
							d.Rows[rowIndex].Cells["Detailes"].Value = "جزئیات";
							d.Rows[rowIndex].Cells["Print"].Value = log.exit != null ? "پرینت" : "";
							d.Rows[rowIndex].Tag = log.id;
						});
						if (log.exit == null)
						{
							dgvEnDetails.InvokeIfRequired(d =>
								{
									int rowIndex = d.Rows.Add();
									d.Rows[rowIndex].Cells["EnCode"].Value = log.code;
									d.Rows[rowIndex].Cells["EnEnLicense"].Value = log.enlicense;
									d.Rows[rowIndex].Cells["EnExLicense"].Value = log.exlicense ?? "";
									d.Rows[rowIndex].Cells["EnEnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									d.Rows[rowIndex].Cells["EnExitTime"].Value = log.exit != null ? DateTimeClass.ToPersianFormat(log.exit.Value) : "";
									d.Rows[rowIndex].Cells["EnDuration"].Value = log.exit != null ? (log.exit - log.enter).Value.ToString() : null;
									d.Rows[rowIndex].Cells["EnCost"].Value = log.cost;
									d.Rows[rowIndex].Cells["EnType"].Value = subInfo.OrganInformation;
									d.Rows[rowIndex].Cells["EnDetailes"].Value = "جزئیات";
									d.Rows[rowIndex].Cells["EnPrint"].Value = log.exit != null ? "پرینت" : "";
									d.Rows[rowIndex].Tag = log.id;
								}
							);
						}
						else
						{
							dgvExDetails.InvokeIfRequired(d =>
							{
								int rowIndex = d.Rows.Add();
								d.Rows[rowIndex].Cells["ExCode"].Value = log.code;
								d.Rows[rowIndex].Cells["ExEnLicense"].Value = log.enlicense;
								d.Rows[rowIndex].Cells["ExExLicense"].Value = log.exlicense ?? "";
								d.Rows[rowIndex].Cells["ExEnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
								d.Rows[rowIndex].Cells["ExExitTime"].Value = log.exit != null ? DateTimeClass.ToPersianFormat(log.exit.Value) : "";
								d.Rows[rowIndex].Cells["ExDuration"].Value = log.exit != null ? (log.exit - log.enter).Value.ToString() : null;
								d.Rows[rowIndex].Cells["ExCost"].Value = log.cost;
								d.Rows[rowIndex].Cells["ExType"].Value = subInfo.OrganInformation;
								d.Rows[rowIndex].Cells["ExDetailes"].Value = "جزئیات";
								d.Rows[rowIndex].Cells["ExPrint"].Value = log.exit != null ? "پرینت" : "";
								d.Rows[rowIndex].Tag = log.id;
							});
						}
					}
				}
			}
			catch
			{
				// ignored
			}
		}

		// ReSharper disable once InconsistentNaming
		private void UpdateDetailsListLicense(int ID, string LicensePlate, string ColumnName)
		{
			lock (DataGridLock)
			{
				dgvDetailsAll.InvokeIfRequired(d =>
				{
					foreach (DataGridViewRow row in d.Rows)
					{
						if ((int)row.Tag == ID)
						{
							lock (DataGridLock)
							{
								row.Cells[ColumnName].Value = LicensePlate;
								break;
							}
						}
					}
				});
			}
		}

		/// <summary>
		/// As same as pbExitApp_Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_isLogedin)
			{
				DoExitOperation(e);
			}
		}

		private void DoExitOperation(FormClosingEventArgs e = null)
		{
			_acceptInput = false;
			DialogResult result = MessageBox.Show(@"آیا می خواهید خارج شوید؟", @"اخطار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			if (result == DialogResult.Yes)
			{
				StopAllThreads();
			}
			else
			{
				if (e != null) e.Cancel = true;
			}
			_acceptInput = true;
		}

		private void StopAllThreads()
		{
			_timerEnter.Stop();
			_timerEnter.Close();

			_timerExit.Stop();
			_timerExit.Close();

			_timerCloseExitGate.Stop();
			_timerCloseExitGate.Close();

			_timerCloseEnterGate.Stop();
			_timerCloseEnterGate.Close();

			_timerUpdateGui.Stop();
			_timerUpdateGui.Close();

			_timerSendCapacity.Stop();
			_timerSendCapacity.Close();

			_timerShowClock.Stop();
			_timerShowClock.Close();

			_timerListiner.Stop();
			_timerListiner.Close();
		}

		private void SetErrorLable(Label LabelObject, string TextData, LogStatus Status)
		{
			switch (Status)
			{
				case LogStatus.Normal:
				{
					LabelObject.InvokeIfRequired(lb => { lb.BackColor = Color.Green; });
					break;
				}
				case LogStatus.Warning:
				{
					LabelObject.InvokeIfRequired(lb => { lb.BackColor = Color.Yellow; });
					break;
				}
				case LogStatus.Error:
				{
					LabelObject.InvokeIfRequired(lb => { lb.BackColor = Color.Red; });
					break;
				}
			}
			LabelObject.InvokeIfRequired(c =>
				c.Text = TextData);
		}

		/// <summary>
		/// Every millisecond check input from entrance card reader
		/// 1- Check card is read
		/// 2- Check card is not repeated again
		/// 3- Check no body enter in parking with this card and dose not exit still 
		/// 4- Get a picture from car license plate and process this number
		/// 5- Check this license plate is not in Black List
		/// 6- Check this license plate is not in parking and does not exit still
		/// 7- Create a record in database
		/// 8- Update GUI
		/// 9- Open the gate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerEnter_Tick(object sender, EventArgs e)
		{
			if (_acceptInput)
			{
				_timerEnter.Stop();
				if (
					SerialPort.GetPortNames().Any(x => x == GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.Name))
				{
					if (_initializeEnterCardReader)
					{
						SetErrorLable(lblEnStatus, "کارت دوباره راه اندازی شد", LogStatus.Warning);
						InitializeEnterCardReaders();
					}
					// wait until a card be read
					EnterCardReader.ReadCardData();
					if (_acceptInput)
					{
						// Is there a card
						if (EnterCardReader.LastCardSerialNumber != "")
						{
							SetErrorLable(lblEnStatus, "کارت خوانده شد", LogStatus.Warning);
							if (_actviationObj.IsDateTimeBetween())
							{
								ResetEnterFormData();
								SetErrorLable(lblEnStatus, "نرم افزار فعال است", LogStatus.Warning);

								// Check cart is submitted
								EnterCardReader.LastCardNumber = CheckCard(EnterCardReader.LastCardSerialNumber);
								if (EnterCardReader.LastCardNumber > 0)
								{
									SetErrorLable(lblEnStatus, "کارت معتبر است", LogStatus.Warning);
									// Check it is not duplicate in database
									if (CheckLog(EnterCardReader.LastCardSerialNumber) <= 0)
									{
										SetErrorLable(lblEnStatus, "کارت جدید است", LogStatus.Warning);
										EnterCardReader.InfoAlarm(EnterCardReader.CardReaderAddress);

										// Get a picture from car license plate
										if (Tools.AllowEnterCamera())
										{
											EnterCardReader.CameraObject.GetPicture();

											if (EnterCardReader.CameraObject.PictureObject != null)
											{
												// Update picture box image
												UpdatePicturBoxImage(_pictureBoxEnterLock, ref pbEn, EnterCardReader.CameraObject.PictureObject);
												if (Tools.AllowUsingAnpr())
												{
													if (Tools.IsServer())
													{
														// Start ANPR
														Stream stream = File.Open(_enterImageName, FileMode.Open);
														EnterCardReader.CameraObject.PictureObject = (Bitmap)Image.FromStream(stream);
														EnterCardReader.CameraObject.ProgressPictureFromCamera();
														stream.Close();
													}
													else
													{
														try
														{
															Stream stream = File.Open(_enterImageName, FileMode.Open);
															Image pictureObject = Image.FromStream(stream);

															EnterCardReader.CameraObject.License = GetLpFromServer(pictureObject);
															stream.Close();
														}
														catch
														{
															//ignored
														}
													}
												}
											}
											else
											{
												SetErrorLable(lblEnStatus, "خطا در ثبت تصویر پلاک", LogStatus.Error);
												EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
											}
										}
										// Input license plate number is enabled
										if (Tools.AllowSubmitLicense())
										{
											// Input license number manually
											if (Tools.AllowSubmitLicenseManual())
											{
												// Add License manually and after that save record
												_submitEnterRecord = false;
												EnableDisableEnterFormToolbox(_submitEnterRecord);
												PrepareEnterLicense();
											}
											else
											{
												if (Tools.AllowSubmitEnter() || !string.IsNullOrEmpty(EnterCardReader.CameraObject.License.LicenseNumber))
												{
													//if (this.CheckLicenseLog(EnterCardReader.CameraObject.License.LicenseNumber) > 0)
													{
														MyForms.FillLicenseMaskedTextBox(EnterCardReader.CameraObject.License, ref mtxtEnLPNo1, ref mtxtEnLPAlpha, ref mtxtEnLPNo2, ref mtxtEnLPNo3);

														AddNewRecord();
													}
													/*else
													{
														this.SetErrorLable(this.lblEnterError, "ماشین هم اکنون در پارکینگ است.", LogStatus.Error);
														EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
													}*/
												}
												else
												{
													SetErrorLable(lblEnStatus, "پلاک شناسایی نشد.", LogStatus.Error);
													EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
												}
											}
										}
										else
										{
											AddNewRecord();
										}
									}
									else
									{
										btnEnSubmit.InvokeIfRequired(b => { b.Enabled = false; });
										SetErrorLable(lblEnStatus, "این کارت قبلا ثبت شده است.", LogStatus.Error);
										EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
									}
									EnterCardReader.LastCardSerialNumber = "";
								}
								else
								{
									btnEnSubmit.InvokeIfRequired(b => { b.Enabled = false; });
									SetErrorLable(lblEnStatus, "کارت نامعتبر می باشد.", LogStatus.Error);
									EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
								}
							}
							else
							{
								MessageBox.Show(@"دوره استفاده از نرم افزار به پایان رسیده است." + Environment.NewLine + @"لطفا نسخه فعال را تهیه فرمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							}
						}
					}
				}
				else
				{
					_initializeEnterCardReader = true;
					SetErrorLable(lblEnStatus, "کارتخوان قطع شده است.", LogStatus.Error);
				}
				_timerEnter.Start();
			}
		}

		/// <summary>
		/// Every millisecond check input from entrance card reader
		/// 1- Check card is read
		/// 2- Check card is not repeated again
		/// 3- Check a enter record is in database 
		/// 4- Get a picture from car license plate and process this number
		/// 5- Check this license plate is not in Black List
		/// 6- Update record in database
		/// 7- Update GUI
		/// 8- Open the gate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerExit_Tick(object sender, EventArgs e)
		{
			if (_acceptInput)
			{
				_timerExit.Stop();
				if (SerialPort.GetPortNames().Any(x => x == GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.Name))
				{
					if (_initializeExitCardReader)
					{
						SetErrorLable(lblExStatus, "کارت دوباره راه اندازی شد", LogStatus.Warning);
						InitializeExitCardReaders();
					}
					// wait until a card be read
					ExitCardReader.ReadCardData();
					if (_acceptInput)
					{
						// Is there a card
						if (ExitCardReader.LastCardSerialNumber != "")
						{
							SetErrorLable(lblExStatus, "کارت خوانده شد", LogStatus.Warning);
							if (_actviationObj.IsDateTimeBetween())
							{
								ResetExitFormData();
								SetErrorLable(lblExStatus, "نرم افزار فعال است", LogStatus.Warning);

								// Check cart is submitted
								ExitCardReader.LastCardNumber = CheckCard(ExitCardReader.LastCardSerialNumber);
								if (ExitCardReader.LastCardNumber > 0)
								{
									SetErrorLable(lblExStatus, "کارت معتبر است", LogStatus.Warning);
									// Check it is in database
									if (CheckExitLog() > 0)
									{
										SetErrorLable(lblExStatus, "کارت جدید است", LogStatus.Warning);
										ExitCardReader.InfoAlarm(ExitCardReader.CardReaderAddress);

										// Get a picture from car license plate
										if (Tools.AllowExitCamera())
										{
											ExitCardReader.CameraObject.GetPicture();

											if (ExitCardReader.CameraObject.PictureObject != null)
											{
												// Update picture box image
												UpdatePicturBoxImage(_pictureBoxExitLock, ref pbEx, ExitCardReader.CameraObject.PictureObject);
												// Load enter picture from database
												try
												{
													UpdatePicturBoxImage(_pictureBoxExitEnterLock, ref pbExEn, StringConvert.ByteArrayToImage(ExitCardReader.LogsObject.enpicture));
												}
												catch
												{
													// ignored
												}
												if (Tools.AllowUsingAnpr())
												{
													// Start ANPR
													if (Tools.IsServer())
													{
														// Start ANPR
														Stream stream = File.Open(_exitImageName, FileMode.Open);
														ExitCardReader.CameraObject.PictureObject = (Bitmap)Image.FromStream(stream);
														ExitCardReader.CameraObject.ProgressPictureFromCamera();
														stream.Close();
													}
													else
													{
														try
														{
															Stream stream = File.Open(_exitImageName, FileMode.Open);
															Image pictureObject = Image.FromStream(stream);

															ExitCardReader.CameraObject.License = GetLpFromServer(pictureObject);
															stream.Close();
														}
														catch
														{
															// ignored
														}
													}
												}
											}
											else
											{
												SetErrorLable(lblExStatus, "خطا در ثبت تصویر", LogStatus.Error);
												ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
											}
										}
										if (Tools.AllowSubmitLicense())
										{
											if (Tools.AllowSubmitLicenseManual())
											{
												// Add License manually and after that save record
												_submitExitRecord = false;
												EnableDisableExitFormToolbox(_submitExitRecord);
												PrepareExitLicense();
											}
											else
											{
												if (Tools.AllowSubmitExit() || !string.IsNullOrEmpty(ExitCardReader.CameraObject.License.LicenseNumber))
												{
													// Update License Plate labels
													UpdateExitEnterLicensePlate(ExitCardReader.LogsObject.enlicense ?? "");
													MyForms.FillLicenseMaskedTextBox(ExitCardReader.CameraObject.License, ref mtxtExLPNo1, ref mtxtExLPAlpha, ref mtxtExLPNo2, ref mtxtExLPNo3);
													if (Tools.AllowSubmitExit() || ExitCardReader.CameraObject.License.LicenseNumber == ExitCardReader.LogsObject.enlicense)
													{
														UpdateRecord();
													}
													else
													{
														SetErrorLable(lblExStatus, "پلاک ورودی و خروجی متفاوت.", LogStatus.Error);
														ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
														// Add License manually and after that save record
														_submitExitRecord = false;
														EnableDisableExitFormToolbox(_submitExitRecord);
														PrepareExitLicense();
													}
												}
												else
												{
													SetErrorLable(lblExStatus, "پلاک شناسایی نشد.", LogStatus.Error);
													ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
													// Add License manually and after that save record
													_submitExitRecord = false;
													EnableDisableExitFormToolbox(_submitExitRecord);
													PrepareExitLicense();
												}
											}
										}
										else
										{
											UpdateRecord();
										}
										// Load enter time from database
										lblExEnTime.InvokeIfRequired(c => { c.Text = DateTimeClass.ToPersianFormat(ExitCardReader.LogsObject.enter, "yyyy/MM/dd  ساعت HH:mm"); });
									}
									else
									{
										btnExSubmit.InvokeIfRequired(b => { b.Enabled = false; });
										SetErrorLable(lblExStatus, "این کارت ثبت نشده است.", LogStatus.Error);
										ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
									}
									ExitCardReader.LastCardSerialNumber = "";
								}
								else
								{
									btnExSubmit.InvokeIfRequired(b => { b.Enabled = false; });
									SetErrorLable(lblExStatus, "کارت نامعتبر می باشد.", LogStatus.Error);
									ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
								}
							}
							else
							{
								MessageBox.Show(@"دوره استفاده از نرم افزار به پایان رسیده است." + Environment.NewLine + @"لطفا نسخه فعال را تهیه فرمائید.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
							}
						}
					}
				}
				else
				{
					_initializeExitCardReader = true;
					SetErrorLable(lblExStatus, "کارتخوان قطع شده است.", LogStatus.Error);
				}
				_timerExit.Start();
			}
		}

		private void AddNewRecord()
		{
			// Update enter date and time
			if (AddNewLog() > 0)
			{
				SetErrorLable(lblEnStatus, "رکورد ثبت شد", LogStatus.Warning);
				lblEnTime.InvokeIfRequired(c => { c.Text = DateTimeClass.ToPersianFormat(EnterCardReader.LogsObject.enter, "yyyy/MM/dd  ساعت HH:mm"); });
				Detailes details = new Detailes()
				{
					SerialNumber = EnterCardReader.LastCardSerialNumber,
					EnterLicensePlate = EnterCardReader.CameraObject.License.LicenseNumber,
					EnterTime = DateTimeClass.ToPersianFormat(EnterCardReader.LogsObject.enter),
					TransType = _enterSubInfo.OrganInformation,
					RowId = EnterCardReader.LogsObject.id
				};
				AddNewDetailsRow(details);
				SetErrorLable(lblEnStatus, "رکورد نمایش داده شد", LogStatus.Warning);

				if (Tools.AllowSubmitLicense() && CheckBlackList(EnterCardReader.CameraObject.License.LicenseNumber) > 0)
				{
					SetErrorLable(lblEnStatus, "ماشین لیست سیاه", LogStatus.Error);
					EnterCardReader.ErrorAlarm2(EnterCardReader.CardReaderAddress);
				}
				else
				{
					SetErrorLable(lblEnStatus, "وضعیت بدون خطا", LogStatus.Normal);
					EnterCardReader.OKAlarm(EnterCardReader.CardReaderAddress);
				}
				_submitEnterRecord = true;
				EnableDisableEnterFormToolbox(_submitEnterRecord);
				_editEnterLicensePlate = true;
				PrepareEnterLicense();

				if (Tools.AllowOpenEnterBarrierAuto())
				{
					OpenEnterGate();
				}
			}
			else
			{
				_submitEnterRecord = false;
				EnableDisableEnterFormToolbox(_submitEnterRecord);
				SetErrorLable(lblEnStatus, "خطا در ثبت در پایگاه داده", LogStatus.Error);
				EnterCardReader.ErrorAlarm(EnterCardReader.CardReaderAddress);
			}
		}

		private long AddNewLog()
		{
			if (EnterCardReader.LogsObject == null)
			{
				EnterCardReader.LogsObject = new enterlogs();
			}

			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				// Generate database record and save
				EnterCardReader.LogsObject.code = EnterCardReader.LastCardSerialNumber;
				EnterCardReader.LogsObject.enlicense = EnterCardReader.CameraObject.License.LicenseNumber;
				EnterCardReader.LogsObject.type = _enterSubInfo.GetSubscriptionInfo(EnterCardReader.LastCardSerialNumber, EnterCardReader.LogsObject.enlicense) > 0 ? LogType.sub.ToString() : LogType.pub.ToString();

				/*if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Save)
				{
					EnterCardReader.LogsObject.enpicture = EnterCardReader.CameraObject.PictureObject != null ? StringConvert.ImageToByteArray(EnterCardReader.CameraObject.PictureObject) : EnterCardReader.LogsObject.enpicture;
				}*/

				if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Save)
				{
					Stream stream = File.Open(_enterImageName, FileMode.Open);
					Image pictureObject = Image.FromStream(stream);
					EnterCardReader.LogsObject.enpicture = StringConvert.ImageToByteArray(pictureObject);
					stream.Close();
				}

				EnterCardReader.LogsObject.enuser = _user.id;

				EnterCardReader.LogsObject = parsPark.enterlogs.Add(EnterCardReader.LogsObject);
				if (parsPark.SaveChanges() > 0)
				{
					return EnterCardReader.LogsObject.id;
				}
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private void UpdateRecord()
		{
			// Update exit date and time
			if (UpdateLog() > 0)
			{
				SetErrorLable(lblExStatus, "رکورد به روز رسانی شد", LogStatus.Warning);
				lblExTime.InvokeIfRequired(c =>
				{
					if (ExitCardReader.LogsObject.exit != null)
						c.Text = DateTimeClass.ToPersianFormat(ExitCardReader.LogsObject.exit.Value, "yyyy/MM/dd  ساعت HH:mm");
				});

				ExitCardReader.Cost = ExitCardReader.LogsObject.type == LogType.pub.ToString() ? ExitCardReader.Cost : 0;

				// Add new record in details DataGridView
				Detailes details = new Detailes()
				{
					RowId = ExitCardReader.LogsObject.id,
					ExitLicensePlate = ExitCardReader.CameraObject.License.LicenseNumber,
					Cost = ExitCardReader.Cost.ToString(),
					ExitTime = ExitCardReader.LogsObject.exit != null ? DateTimeClass.ToPersianFormat(ExitCardReader.LogsObject.exit.Value) : "",
					Duration = ExitCardReader.Span.ToString(),
					Print = "پرینت"
				};
				UpdateDetailsRow(details);
				SetErrorLable(lblExStatus, "رکورد نمایش داده شد", LogStatus.Warning);

				lblCost.InvokeIfRequired(c => { c.Text = ExitCardReader.LogsObject.type == LogType.pub.ToString() ? (ExitCardReader.Cost > 0 ? ExitCardReader.Cost.ToString(@"0,0 تومان") : @"0") : _exitSubInfo.OrganInfo.Orgval; });
				lblDuration.InvokeIfRequired(c => { c.Text = ExitCardReader.Span.ToString(); });

				if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Allow == PermissionAllow.Yes)
				{
					string data = ExitCardReader.Cost > 0
						? (ExitCardReader.Cost * 10).ToString()
						: _exitSubInfo.OrganInfo != null && _exitSubInfo.OrganInfo.Id > 0 ? _exitSubInfo.OrganInfo.Orgval : "VIP";
					_serialComPortPrice.SendPriceToLedBoard(ImageProcess.GetImageHexString(data, new Font("B Titr", 27), Color.Black, Color.Transparent, 128, 32));
				}
				if (ExitCardReader.LogsObject.type == LogType.pub.ToString() && Tools.AllowCityPayAuto())
				{
					_serialComPortCityPay.SendPriceToCityPay(ExitCardReader.Cost);
				}
				if (ExitCardReader.LogsObject.type == LogType.pub.ToString() && Tools.AllowPos())
				{
					_serialComPortPos.SendPriceToPos(ExitCardReader.Cost);
				}

				if (Tools.AllowSubmitLicense() && CheckBlackList(ExitCardReader.CameraObject.License.LicenseNumber) > 0)
				{
					SetErrorLable(lblExStatus, "ماشین لیست سیاه", LogStatus.Error);
					ExitCardReader.ErrorAlarm2(ExitCardReader.CardReaderAddress);
				}
				else
				{
					SetErrorLable(lblExStatus, "وضعیت بدون خطا", LogStatus.Normal);
					ExitCardReader.OKAlarm(ExitCardReader.CardReaderAddress);
				}
				_submitExitRecord = true;
				EnableDisableExitFormToolbox(_submitExitRecord);

				if (Tools.AllowOpenExitBarrierAuto())
				{
					OpenExitGate();
				}
				if (GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Allow == PermissionAllow.Yes)
				{
					int rowIndex = (int)ExitCardReader.LogsObject.id;
					PrintReceipt(dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value != null && dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value.ToString() != "" ? dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value.ToString() : dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value != null && dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value.ToString() != "" ? dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value.ToString() : "مجهول", dgvDetailsAll.Rows[rowIndex].Cells["Cost"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Cost"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name, dgvDetailsAll.Rows[rowIndex].Cells["EnterTime"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["EnterTime"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["Type"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Type"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["Duration"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Duration"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric, @"پشتیبانی 09177382160");
				}
			}
			else
			{
				SetErrorLable(lblExStatus, ExitCardReader.LastError, LogStatus.Error);
				ExitCardReader.ErrorAlarm(ExitCardReader.CardReaderAddress);
				_submitExitRecord = false;
				EnableDisableExitFormToolbox(_submitExitRecord);
			}
		}

		private long UpdateLog()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				ExitCardReader.LogsObject = parsPark.enterlogs.Find(ExitCardReader.LogsObject.id);

				var dateQuery = parsPark.Database.SqlQuery<DateTime>("SELECT NOW()");

				DateTime exitTime = dateQuery.AsEnumerable().FirstOrDefault();
				if (ExitCardReader.LogsObject != null && ExitCardReader.LogsObject.enter < exitTime)
				{
					long data = _exitSubInfo.GetSubscriptionInfo(ExitCardReader.LastCardSerialNumber, ExitCardReader.LogsObject.enlicense);

					ExitCardReader.Cost = data > 0 ? 0 : CalculateParkingCost(ExitCardReader.LogsObject.enter, exitTime);

					// Generate database record and save
					ExitCardReader.LogsObject.cost = ExitCardReader.Cost;
					ExitCardReader.LogsObject.exlicense = ExitCardReader.CameraObject.License.LicenseNumber;
					ExitCardReader.LogsObject.exit = exitTime;
					ExitCardReader.LogsObject.exuser = _user.id;
					ExitCardReader.LogsObject.type = data > 0 ? LogType.sub.ToString() : LogType.pub.ToString();

					/*if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Save)
					{
						ExitCardReader.LogsObject.expicture = ExitCardReader.CameraObject.PictureObject != null
							? StringConvert.ImageToByteArray(ExitCardReader.CameraObject.PictureObject)
							: ExitCardReader.LogsObject.expicture;
					}*/

					if (GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Save)
					{
						Stream stream = File.Open(_exitImageName, FileMode.Open);
						Image pictureObject = Image.FromStream(stream);
						ExitCardReader.LogsObject.expicture = StringConvert.ImageToByteArray(pictureObject);
						stream.Close();
					}

					parsPark.Entry(ExitCardReader.LogsObject).CurrentValues.SetValues(ExitCardReader.LogsObject);

					if (parsPark.SaveChanges() > 0)
					{
						// Delete row from table
						/*parsPark.enterlogs.Remove(ExitCardReader.LogsObject);
						if (parsPark.SaveChanges() > 0)
						{*/
						return ExitCardReader.LogsObject.id;
						/*}
						else
						{
							this._exitCardReader.LastError = "خطا در ثبت در پایگاه داده";
							return 0;
						}*/
					}
					ExitCardReader.LastError = "خطا در ثبت در پایگاه داده";
					return 0;
				}
				ExitCardReader.LastError = "زمان ورود بزرگتر از زمان خروج";
				return 0;
			}
			catch
			{
				ExitCardReader.LastError = "خطا در ثبت در پایگاه داده";
				return 0;
			}
		}

		private void UpdateParkingCapacity(int Capacity)
		{
			int emptyCapacity = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity - Capacity;
			lock (CapacityLock)
			{
				lblCapacity.InvokeIfRequired(c => c.Text = emptyCapacity.ToString());
			}
			lock (FillCapacityLock)
			{
				lblEnteredCount.InvokeIfRequired(c => c.Text = Capacity.ToString());
			}
		}

		/// <summary>
		/// Add details data grid view row with new entered car info
		/// It is enter gate software so update her details data grid view
		/// Or is exit gate software and should send data to enter gate software
		/// </summary>
		/// <param name="DetailsObject"></param>
		private void AddNewDetailsRow(Detailes DetailsObject)
		{
			// It is enter gate software
			// Add new record in details DataGridView
			lock (DataGridLock)
			{
				// If number of rows is more than parking capacity
				// Delete oldest row and insert new row
				if (dgvDetailsAll.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
				{
					RemoveFromAllRecords();
				}
				dgvDetailsAll.InvokeIfRequired(c =>
				{
					int newRowIndex = c.Rows.Add();
					c.Rows[newRowIndex].Cells["Code"].Value = DetailsObject.SerialNumber;
					c.Rows[newRowIndex].Cells["EnLicense"].Value = DetailsObject.EnterLicensePlate;
					c.Rows[newRowIndex].Cells["EnterTime"].Value = DetailsObject.EnterTime;
					c.Rows[newRowIndex].Cells["Type"].Value = DetailsObject.TransType;
					c.Rows[newRowIndex].Cells["Detailes"].Value = @"جزئیات";
					c.Rows[newRowIndex].Tag = DetailsObject.RowId;
					c.FirstDisplayedScrollingRowIndex = newRowIndex;
				});

				// If number of rows is more than parking capacity
				// Delete oldest row and insert new row
				dgvEnDetails.InvokeIfRequired(c =>
				{
					int newRowIndex = c.Rows.Add();
					c.Rows[newRowIndex].Cells["EnCode"].Value = DetailsObject.SerialNumber;
					c.Rows[newRowIndex].Cells["EnEnLicense"].Value = DetailsObject.EnterLicensePlate;
					c.Rows[newRowIndex].Cells["EnEnterTime"].Value = DetailsObject.EnterTime;
					c.Rows[newRowIndex].Cells["EnType"].Value = DetailsObject.TransType;
					c.Rows[newRowIndex].Cells["EnDetailes"].Value = @"جزئیات";
					c.Rows[newRowIndex].Tag = DetailsObject.RowId;
					c.FirstDisplayedScrollingRowIndex = newRowIndex;
				});
			}
		}

		/// <summary>
		/// Update details data grid view row with new exit car
		/// It is exit gate software so update her details data grid view
		/// Or is enter gate software and should send data to exit gate software
		/// </summary>
		/// <param name="DetailsObject"></param>
		private void UpdateDetailsRow(Detailes DetailsObject)
		{
			lock (DataGridLock)
			{
				// It is enter gate software
				int newRowIndex = FindRowInDetailsDataGridView(DetailsObject.RowId);
				if (newRowIndex > -1 && (dgvDetailsAll.Rows[newRowIndex].Cells["ExitTime"].Value == null || dgvDetailsAll.Rows[newRowIndex].Cells["ExitTime"].Value.ToString() == ""))
				{
					if (dgvDetailsAll.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
					{
						RemoveFromAllRecords();
					}
					dgvDetailsAll.InvokeIfRequired(c =>
					{
						c.Rows[newRowIndex].Cells["ExLicense"].Value = DetailsObject.ExitLicensePlate;
						c.Rows[newRowIndex].Cells["Cost"].Value = DetailsObject.Cost;
						c.Rows[newRowIndex].Cells["Duration"].Value = DetailsObject.Duration;
						c.Rows[newRowIndex].Cells["ExitTime"].Value = DetailsObject.ExitTime;
						c.Rows[newRowIndex].Cells["Print"].Value = DetailsObject.Print;
					});
					if (dgvExDetails.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
					{
						RemoveFromExitRecords();
					}
					dgvExDetails.InvokeIfRequired(c =>
					{
						int newRowIndexEn = c.Rows.Add();
						c.Rows[newRowIndexEn].Cells["ExCode"].Value = dgvDetailsAll.Rows[newRowIndex].Cells["Code"].Value;
						c.Rows[newRowIndexEn].Cells["ExEnLicense"].Value = dgvDetailsAll.Rows[newRowIndex].Cells["EnLicense"].Value;
						c.Rows[newRowIndexEn].Cells["ExExLicense"].Value = DetailsObject.ExitLicensePlate;
						c.Rows[newRowIndexEn].Cells["ExEnterTime"].Value = dgvDetailsAll.Rows[newRowIndex].Cells["EnterTime"].Value;
						c.Rows[newRowIndexEn].Cells["ExExitTime"].Value = DetailsObject.ExitTime;
						c.Rows[newRowIndexEn].Cells["ExCost"].Value = DetailsObject.Cost;
						c.Rows[newRowIndexEn].Cells["ExDuration"].Value = DetailsObject.Duration;
						c.Rows[newRowIndexEn].Cells["ExType"].Value = dgvDetailsAll.Rows[newRowIndex].Cells["Type"].Value;
						c.Rows[newRowIndexEn].Cells["ExDetailes"].Value = dgvDetailsAll.Rows[newRowIndex].Cells["Detailes"].Value;
						c.Rows[newRowIndexEn].Cells["ExPrint"].Value = DetailsObject.Print;
						c.Rows[newRowIndexEn].Tag = DetailsObject.RowId;
						c.FirstDisplayedScrollingRowIndex = newRowIndexEn;
					});

					RemoveFromEnterRecords(dgvDetailsAll.Rows[newRowIndex].Tag);
				}
			}
		}

		private int RemoveFromAllRecords()
		{
			for (int i = 0; i < dgvDetailsAll.Rows.Count; i++)
			{
				if (dgvDetailsAll.Rows[i].Cells["ExitTime"].Value != null && dgvDetailsAll.Rows[i].Cells["ExitTime"].Value.ToString() != "")
				{
					lock (DataGridLock)
					{
						dgvDetailsAll.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private int RemoveFromAllRecords(object TagId)
		{
			for (int i = 0; i < dgvDetailsAll.Rows.Count; i++)
			{
				if (dgvDetailsAll.Rows[i].Tag != null && dgvDetailsAll.Rows[i].Tag != TagId)
				{
					lock (DataGridLock)
					{
						dgvDetailsAll.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private int RemoveFromEnterRecords()
		{
			for (int i = 0; i < dgvEnDetails.Rows.Count; i++)
			{
				if (dgvEnDetails.Rows[i].Cells["EnExitTime"].Value != null && dgvEnDetails.Rows[i].Cells["EnExitTime"].Value.ToString() != "")
				{
					lock (DataGridLock)
					{
						dgvEnDetails.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private int RemoveFromEnterRecords(object TagId)
		{
			for (int i = 0; i < dgvEnDetails.Rows.Count; i++)
			{
				if (dgvEnDetails.Rows[i].Tag != null && dgvEnDetails.Rows[i].Tag != TagId)
				{
					lock (DataGridLock)
					{
						dgvEnDetails.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private int RemoveFromExitRecords()
		{
			for (int i = 0; i < dgvExDetails.Rows.Count; i++)
			{
				if (dgvExDetails.Rows[i].Cells["ExExitTime"].Value != null && dgvExDetails.Rows[i].Cells["ExExitTime"].Value.ToString() != "")
				{
					lock (DataGridLock)
					{
						dgvExDetails.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private int RemoveFromExitRecords(object TagId)
		{
			for (int i = 0; i < dgvExDetails.Rows.Count; i++)
			{
				if (dgvExDetails.Rows[i].Tag != null && dgvExDetails.Rows[i].Tag != TagId)
				{
					lock (DataGridLock)
					{
						dgvExDetails.InvokeIfRequired(d => { d.Rows.RemoveAt(i); });
					}
					return i;
				}
			}
			return -1;
		}

		private long CheckLog(string CardSerialNumber)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var firstOrDefault = parsPark.enterlogs.FirstOrDefault(t => t.code == CardSerialNumber && t.exit == null);
				if (firstOrDefault != null)
					return firstOrDefault.id;
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private long CheckLicenseLog(string CarLicense)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var firstOrDefault = parsPark.enterlogs.FirstOrDefault(t => t.enlicense == CarLicense && t.exit == null);
				if (firstOrDefault != null)
					return firstOrDefault.id;
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private bool CheckLicenseSyntax(string CarLicense)
		{
			//CarLicense = "55ب12563";
			if (CarLicense == "55ب12563")
			{
				return true;
			}
			return false;
		}

		private long CheckExitLog()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				ExitCardReader.LogsObject = parsPark.enterlogs.FirstOrDefault(t => t.code == ExitCardReader.LastCardSerialNumber && t.exit == null);
				if (ExitCardReader.LogsObject != null)
					return ExitCardReader.LogsObject.id;
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private void ResetEnterFormData()
		{
			SetErrorLable(lblEnStatus, "", LogStatus.Normal);

			mtxtEnLPNo1.InvokeIfRequired(b => { b.Text = ""; });
			mtxtEnLPAlpha.InvokeIfRequired(b => { b.Text = ""; });
			mtxtEnLPNo2.InvokeIfRequired(b => { b.Text = ""; });
			mtxtEnLPNo3.InvokeIfRequired(b => { b.Text = ""; });

			_submitEnterRecord = false;
			EnableDisableEnterFormToolbox(_submitEnterRecord);

			pbEn.InvokeIfRequired(b =>
			{
				b.Image = null;
			});

			EnterCardReader.ResetValues();

			_enterSubInfo.ResetData();
		}

		private void EnableDisableEnterFormToolbox(bool Status)
		{
			btnEnOpenGate.InvokeIfRequired(b => { b.Enabled = Tools.AllowOpenEnterBarrier() && Status; });
			btnEnCloseGate.InvokeIfRequired(b => { b.Enabled = Tools.AllowCloseEnterBarrier() && Status; });
			btnEnSubmit.InvokeIfRequired(b => { b.Enabled = !Status; });
			btnEnBL.InvokeIfRequired(b => { b.Enabled = Status; });
			btnEnLpEdit.InvokeIfRequired(b => { b.Enabled = Status; });
		}

		private void ResetExitFormData()
		{
			SetErrorLable(lblExStatus, "", LogStatus.Normal);

			mtxtExEnLPNo1.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExEnLPAlpha.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExEnLPNo2.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExEnLPNo3.InvokeIfRequired(mt => { mt.Text = ""; });

			mtxtExLPNo1.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExLPAlpha.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExLPNo2.InvokeIfRequired(mt => { mt.Text = ""; });
			mtxtExLPNo3.InvokeIfRequired(mt => { mt.Text = ""; });

			lblExEnTime.InvokeIfRequired(b => { b.Text = ""; });
			lblExTime.InvokeIfRequired(b => { b.Text = ""; });

			_submitExitRecord = false;
			EnableDisableExitFormToolbox(_submitExitRecord);

			pbExEn.InvokeIfRequired(b =>
			{
				b.Image = null;
			});
			pbEx.InvokeIfRequired(b =>
			{
				b.Image = null;
			});

			lblCost.InvokeIfRequired(b => { b.Text = @"0"; });
			lblDuration.InvokeIfRequired(b => { b.Text = @"00:00:00:00"; });

			ExitCardReader.ResetValues();

			_exitSubInfo.ResetData();
		}

		private void EnableDisableExitFormToolbox(bool Status)
		{
			btnExOpenGate.InvokeIfRequired(b => { b.Enabled = Tools.AllowOpenExitBarrier() && Status; });
			btnExCloseGate.InvokeIfRequired(b => { b.Enabled = Tools.AllowCloseExitBarrier() && Status; });
			btnExSubmit.InvokeIfRequired(b => { b.Enabled = !Status; });
			btnExBL.InvokeIfRequired(b => { b.Enabled = Status; });
			btnExLpEdit.InvokeIfRequired(b => { b.Enabled = Status; });

			btnPrintReceipt.InvokeIfRequired(b =>
			{
				b.Enabled = Status;
			});

			btnCityPay.InvokeIfRequired(b => b.Enabled = Tools.AllowCityPay() && Status);
			btnPOSPay.InvokeIfRequired(b => b.Enabled = Tools.AllowPos() && Status);
		}

		private SubscriptionInfo CheckSubscription(string LastCardSerialNumber, string LicensePlateNimber)
		{
			try
			{
				SubscriptionInfo subInfo = new SubscriptionInfo();
				if (subInfo.GetSubscriptionInfo(LastCardSerialNumber, LicensePlateNimber) > 0)
				{
					return subInfo;
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		private long CheckBlackList(string LicensePlate)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				var firstOrDefault = (from s in parsPark.blacklist where s.license == LicensePlate select s).FirstOrDefault();
				if (firstOrDefault != null)
					return firstOrDefault.id;
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private void UpdatePicturBoxImage(object PictureBoxLocker, ref PictureBox PicturePoxObject, Image ImageObject)
		{
			lock (PictureBoxLocker)
			{
				PicturePoxObject.InvokeIfRequired(c =>
				{
					c.Image = (Image)ImageObject.Clone();
				});
			}
		}

		private void GetEnterLicensePlatePicture()
		{
			EnterCardReader.CameraObject.SourceUrl = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL;
			EnterCardReader.CameraObject.GetPicture();
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeEnterCardReaders()
		{
			if (_initializeEnterCardReader)
			{
				SoftwareType softType = Tools.CheckSoftwareMode();
				// This software control enter gate
				if (softType == SoftwareType.Both || softType == SoftwareType.Enter)
				{
					if (EnterCardReader.CloseCardReader() == 0)
					{
						//Enter card reader
						EnterCardReader.CardReaderAddress = 1;
						EnterCardReader.SerialPort.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.BaudRate;
						EnterCardReader.SerialPort.PortName = GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.Name;
						EnterCardReader.InitializeCardReader();
					}

					_timerEnter.Enabled = true;
				}
				else
				{
					_timerEnter.Enabled = false;
				}
				_initializeEnterCardReader = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeExitCardReaders()
		{
			if (_initializeExitCardReader)
			{
				SoftwareType softType = Tools.CheckSoftwareMode();
				// This software control exit gate
				if (softType == SoftwareType.Both || softType == SoftwareType.Exit)
				{
					if (ExitCardReader.CloseCardReader() == 0)
					{
						// Exit card reader
						ExitCardReader.CardReaderAddress = 2;
						ExitCardReader.SerialPort.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.BaudRate;
						ExitCardReader.SerialPort.PortName = GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.Name;
						ExitCardReader.InitializeCardReader();
					}

					_timerExit.Enabled = true;
				}
				else
				{
					_timerExit.Enabled = false;
				}
				_initializeExitCardReader = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeCardReaders()
		{
			InitializeEnterCardReaders();
			InitializeExitCardReaders();
		}

		/// <summary>
		/// Initialize Gates COM ports
		/// </summary>
		private void InitializeBarrierComPort()
		{
			btnEnOpenGate.InvokeIfRequired(c =>
			{
				c.Enabled = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes;
			});
			btnEnCloseGate.InvokeIfRequired(c =>
			{
				c.Enabled = btnEnOpenGate.Enabled;
			});
			btnExOpenGate.InvokeIfRequired(c =>
			{
				c.Enabled = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes;
			});
			btnExCloseGate.InvokeIfRequired(c =>
			{
				c.Enabled = btnExOpenGate.Enabled;
			});

			if (GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes)
			{
				_serialComPortEnter.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.BaudRate;
				_serialComPortEnter.PortName = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.Name;
				if (_serialComPortEnter.InitializeComport("راهبند ورودی") == false)
				{
					MessageBox.Show(_serialComPortEnter.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}

			if (GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes)
			{
				_serialComPortExit.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.BaudRate;
				_serialComPortExit.PortName = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.Name;
				if (_serialComPortExit.InitializeComport("راهبند خروجی") == false)
				{
					MessageBox.Show(_serialComPortExit.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
		}

		/// <summary>
		/// Initialize Gates COM ports
		/// </summary>
		private void InitializeBoardComPort()
		{
			if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Allow == PermissionAllow.Yes)
			{
				_serialComPortPrice.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.BaudRate;
				_serialComPortPrice.PortName = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.Name;
				if (_serialComPortPrice.InitializeComport("قیمت") == false)
				{
					MessageBox.Show(_serialComPortPrice.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}

			if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Allow == PermissionAllow.Yes)
			{
				if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Board)
				{
					_serialComPortCapacity.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.BaudRate;
					_serialComPortCapacity.PortName = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.Name;
					if (_serialComPortCapacity.InitializeComport("ظرفیت") == false)
					{
						MessageBox.Show(_serialComPortCapacity.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
				else if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Board)
				{
					if (!Tools.ExistUrl(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Address))
					{
						MessageBox.Show(@"خطا در ارتباط با سرور ظرفیت ", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
			}
		}

		/// <summary>
		/// Initialize Gates COM ports
		/// </summary>
		private void InitializePaymentComPort()
		{
			if (Tools.AllowCityPay())
			{
				_serialComPortCityPay.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.BaudRate;
				_serialComPortCityPay.PortName = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.Name;
				if (_serialComPortCityPay.InitializeComport("کارت شهروندی") == false)
				{
					MessageBox.Show(_serialComPortCityPay.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}

			if (Tools.AllowPos())
			{
				_serialComPortPos.BaudRate = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.BaudRate;
				_serialComPortPos.PortName = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.Name;
				if (_serialComPortPos.InitializeComport("دستگاه POS") == false)
				{
					MessageBox.Show(_serialComPortPos.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
		}

		/*public void UpdateEnterLicensePlate(LicensePlate License)
		{
			mtxtEnLPNo1.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber1;
			});
			mtxtEnLPAlpha.InvokeIfRequired(c =>
			{
				c.Text = License.LpAlpha;
			});
			mtxtEnLPNo2.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber2;
			});
			mtxtEnLPNo3.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber3;
			});
		}*/

		public void UpdateExitEnterLicensePlate(string License)
		{
			LicensePlate lp = new LicensePlate();
			lp.LoadLicense(License);
			MyForms.FillLicenseMaskedTextBox(lp, ref mtxtExEnLPNo1, ref mtxtExEnLPAlpha, ref mtxtExEnLPNo2, ref mtxtExEnLPNo3);
		}

		public void UpdateExitLicensePlate(LicensePlate License)
		{
			mtxtExLPNo1.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber1;
			});
			mtxtExLPAlpha.InvokeIfRequired(c =>
			{
				c.Text = License.LpAlpha;
			});
			mtxtExLPNo2.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber2;
			});
			mtxtExLPNo3.InvokeIfRequired(c =>
			{
				c.Text = License.LpNumber3;
			});
		}

		private int FindRowInDetailsDataGridView(long Id)
		{
			int rowIndex = -1;
			foreach (DataGridViewRow row in dgvDetailsAll.Rows)
			{
				if (row.Tag != null && (long)row.Tag == Id)
				{
					rowIndex = row.Index;
					break;
				}
			}
			return rowIndex;
		}

		private int CalculateParkingCost(DateTime EnterTimeVal, DateTime ExitTimeVal)
		{
			GlobalVariables.AllSettingsObject.LoadGlobalSettings(GlobalVariables.ConnectionString);

			ExitCardReader.Span = ExitTimeVal - EnterTimeVal;
			if (ExitCardReader.Span.TotalMinutes > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Free)
			{
				DateTime dayStart = new DateTime(ExitTimeVal.Year, ExitTimeVal.Month, ExitTimeVal.Day, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Start, 0, 0);

				DateTime dayEnd = new DateTime(ExitTimeVal.Year, ExitTimeVal.Month, ExitTimeVal.Day, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.End, 0, 0);

				return Tools.CalculateParkingCost(ExitCardReader.Span, EnterTimeVal, ExitTimeVal, dayStart, dayEnd);
			}
			return 0;
		}

		private void btnEnLPEdit_Click(object sender, EventArgs e)
		{
			// Add new record manually
			//if (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes && _submitEnterRecord)
			{
				PrepareEnterLicense();
				// Record created and now just update license plate
				if (UpdateEnterRecordLicensePlate() > 0)
				{
					UpdateDetailsListLicense(Convert.ToInt32(EnterCardReader.LogsObject.id), EnterCardReader.LogsObject.enlicense, "EnLicense");
					SetErrorLable(lblEnStatus, "شماره پلاک ویرایش شد", LogStatus.Normal);
				}
				else
				{
					SetErrorLable(lblEnStatus, "خطا در ویرایش شماره پلاک", LogStatus.Error);
				}
			}
		}

		private long UpdateEnterRecordLicensePlate()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				EnterCardReader.LogsObject = parsPark.enterlogs.Find(EnterCardReader.LogsObject.id);

				//EnterCardReader.LicensePlate = mtxtEnLPNo2.Text + mtxtEnLPNo3.Text + mtxtEnLPAlpha.Text + mtxtEnLPNo1.Text;

				parsPark.Entry(EnterCardReader.LogsObject).CurrentValues.SetValues(EnterCardReader.LogsObject);

				if (parsPark.SaveChanges() > 0)
				{
					if (EnterCardReader.LogsObject != null) return EnterCardReader.LogsObject.id;
				}
				else
				{
					return 0;
				}
			}
			catch
			{
				return 0;
			}

			return 0;
		}

		private void PrepareEnterLicense()
		{
			mtxtEnLPNo1.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editEnterLicensePlate;
			});
			mtxtEnLPAlpha.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editEnterLicensePlate;
			});
			mtxtEnLPNo2.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editEnterLicensePlate;
			});
			mtxtEnLPNo3.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editEnterLicensePlate;
			});

			mtxtEnLPNo1.InvokeIfRequired(c =>
			{
				c.Enabled = !_editEnterLicensePlate;
			});
			mtxtEnLPAlpha.InvokeIfRequired(c =>
			{
				c.Enabled = !_editEnterLicensePlate;
			});
			mtxtEnLPNo2.InvokeIfRequired(c =>
			{
				c.Enabled = !_editEnterLicensePlate;
			});
			mtxtEnLPNo3.InvokeIfRequired(c =>
			{
				c.Enabled = !_editEnterLicensePlate;
			});

			_editEnterLicensePlate = !_editEnterLicensePlate;
			btnEnLpEdit.InvokeIfRequired(c =>
			{
				c.Text = _editOk[Convert.ToInt32(_editEnterLicensePlate)];
			});
		}

		private void btnExLPEdit_Click(object sender, EventArgs e)
		{
			// Add new record manually
			if (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes && _submitExitRecord)
			{
				PrepareExitLicense();
				// Record created and now just update license plate
				if (UpdateExitRecordLicensePlate() > 0)
				{
					UpdateDetailsListLicense(Convert.ToInt32(ExitCardReader.LogsObject.id), ExitCardReader.LogsObject.exlicense, "ExLicense");
					SetErrorLable(lblExStatus, "شماره پلاک ویرایش شد", LogStatus.Normal);
				}
				else
				{
					SetErrorLable(lblExStatus, "خطا در ویرایش شماره پلاک", LogStatus.Error);
				}
			}
		}

		private long UpdateExitRecordLicensePlate()
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				//ExitCardReader.LicensePlate = mtxtExLPAlpha.Text + mtxtExLPNo3.Text + mtxtExLPAlpha.Text + mtxtExLPNo1.Text;

				parsPark.Entry(ExitCardReader.LogsObject).CurrentValues.SetValues(ExitCardReader.LogsObject);

				if (parsPark.SaveChanges() > 0)
				{
					return ExitCardReader.LogsObject.id;
				}
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private void PrepareExitLicense()
		{
			mtxtExLPNo1.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editExitLicensePlate;
			});
			mtxtExLPAlpha.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editExitLicensePlate;
			});
			mtxtExLPNo2.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editExitLicensePlate;
			});
			mtxtExLPNo3.InvokeIfRequired(c =>
			{
				c.ReadOnly = _editExitLicensePlate;
			});

			mtxtExLPNo1.InvokeIfRequired(c =>
			{
				c.Enabled = !_editExitLicensePlate;
			});
			mtxtExLPAlpha.InvokeIfRequired(c =>
			{
				c.Enabled = !_editExitLicensePlate;
			});
			mtxtExLPNo2.InvokeIfRequired(c =>
			{
				c.Enabled = !_editExitLicensePlate;
			});
			mtxtExLPNo3.InvokeIfRequired(c =>
			{
				c.Enabled = !_editExitLicensePlate;
			});

			_editExitLicensePlate = !_editExitLicensePlate;
			btnExLpEdit.InvokeIfRequired(c =>
			{
				c.Text = _editOk[Convert.ToInt32(_editExitLicensePlate)];
			});
		}

		private void btnPrintReceipt_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(btnPrintReceipt, @"پرینت قبض");
		}

		private void dgvDetailsAll_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			int columnIndex = e.ColumnIndex;
			int rowIndex = e.RowIndex;
			if (columnIndex >= 0 && rowIndex > -1)
			{
				DoListAction(rowIndex, columnIndex, dgvDetailsAll.Columns[columnIndex].Name, "Print", "Detailes", dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value, dgvDetailsAll.Rows[rowIndex].Tag, dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value != null && dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value.ToString() != "" ? dgvDetailsAll.Rows[rowIndex].Cells["EnLicense"].Value.ToString() : dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value != null && dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value.ToString() != "" ? dgvDetailsAll.Rows[rowIndex].Cells["ExLicense"].Value.ToString() : "مجهول", dgvDetailsAll.Rows[rowIndex].Cells["Cost"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Cost"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name, dgvDetailsAll.Rows[rowIndex].Cells["EnterTime"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["EnterTime"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["Type"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Type"].Value.ToString() : "", dgvDetailsAll.Rows[rowIndex].Cells["Duration"].Value != null ? dgvDetailsAll.Rows[rowIndex].Cells["Duration"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric, @"پشتیبانی 09177382160");
			}
		}

		private void DoListAction(int RowIndex, int ColumnIndex, string ColumnName, string ColumnNamePrint, string ColumnNameDetailes, object ColumnValue, object RowTag, string LicensePlateVal, string CostVal, string NameVal, string EnterTimeVal, string ExitTimeVal, string TypeVal, string DurationVal, string LyricVal, string SupportVal)
		{
			if (ColumnIndex >= 0 && RowIndex > -1)
			{
				if (ColumnName == ColumnNamePrint)
				{
					if (ColumnValue != null)
					{
						try
						{
							PrintReceipt(LicensePlateVal, CostVal, NameVal, EnterTimeVal, ExitTimeVal, TypeVal, DurationVal, LyricVal, SupportVal);
						}
						catch
						{
							// ignored
						}
					}
				}
				else if (ColumnName == ColumnNameDetailes)
				{
					try
					{
						parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
						enterlogs enterLogObject = parsPark.enterlogs.Find(RowTag);
						if (enterLogObject != null)
						{
							logs logObject = new logs()
							{
								code = enterLogObject.code,
								enuser = enterLogObject.enuser,
								exuser = enterLogObject.exuser,
								enter = enterLogObject.enter,
								exit = enterLogObject.exit,
								enlicense = enterLogObject.enlicense,
								exlicense = enterLogObject.exlicense,
								cost = enterLogObject.cost,
								enpicture = enterLogObject.enpicture,
								expicture = enterLogObject.expicture,
							};

							FormDetailes formDetails = new FormDetailes() { LogDetail = logObject };

							formDetails.ShowDialog();
						}
					}
					catch
					{
						MessageBox.Show(@"خطا در برقراری ارتباط با پایگاه داده.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
					}
				}
			}
		}

		private void PrintReceipt(string LicensePlateVal, string CostVal, string NameVal, string EnterTimeVal, string ExitTimeVal, string TypeVal, string DurationVal, string LyricVal, string SupportVal)
		{
			try
			{
				Printer printerObject = new Printer
				{
					LicensePlate = LicensePlateVal,
					Cost = CostVal,
					Name = NameVal,
					EnterTime = EnterTimeVal,
					ExitTime = ExitTimeVal,
					Type = TypeVal,
					Duration = DurationVal,
					Lyric = LyricVal,
					Support = SupportVal
				};

				printerObject.Run();
			}
			catch
			{
				MessageBox.Show(@"خطا در پرینت قبض.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			KeyEventArgs e = new KeyEventArgs(keyData);
			switch (e.KeyCode)
			{
				case Keys.F3:
				{
					if (btnEnOpenGate.Enabled)
					{
						btnEnOpenGate_Click(msg, e);
					}
					return true;
				}
				case Keys.F4:
				{
					if (btnEnCloseGate.Enabled)
					{
						btnEnCloseGate_Click(msg, e);
					}
					return true;
				}
				case Keys.F5:
				{
					if (btnExOpenGate.Enabled)
					{
						btnExOpenGate_Click(msg, e);
					}
					return true;
				}
				case Keys.F6:
				{
					if (btnExCloseGate.Enabled)
					{
						btnExCloseGate_Click(msg, e);
					}
					return true;
				}
				case Keys.F7:
				{
					if (btnEnLpEdit.Enabled)
					{
						btnEnLPEdit_Click(msg, e);
					}
					return true;
				}
				case Keys.F8:
				{
					if (btnExLpEdit.Enabled)
					{
						btnExLPEdit_Click(msg, e);
					}
					return true;
				}
				case Keys.F9:
				{
					if (btnCityPay.Enabled)
					{
						btnCityPay_Click(msg, e);
					}
					return true;
				}
				case Keys.F10:
				{
					if (btnPOSPay.Enabled)
					{
						btnPOSPay_Click(msg, e);
					}
					return true;
				}
				case Keys.F11:
				{
					if (btnEnSubmit.Enabled)
					{
						btnEnSubmit_Click(msg, e);
					}
					return true;
				}
				case Keys.F12:
				{
					if (btnExSubmit.Enabled)
					{
						btnExSubmit_Click(msg, e);
					}
					return true;
				}
				case Keys.Control | Keys.P:
				{
					if (e.Control)
					{
						btnPrintReceipt_Click(msg, e);
						return true;
					}
					break;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void btnPrintReceipt_Click(object sender, EventArgs e)
		{
			if (ExitCardReader.LogsObject.exit != null)
			{
				Printer printerObject = new Printer
				{
					LicensePlate = ExitCardReader.LogsObject.exlicense,
					Cost = ExitCardReader.LogsObject.cost.ToString(),
					Name = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name,
					EnterTime = DateTimeClass.ToPersianFormat(ExitCardReader.LogsObject.enter),
					ExitTime =
						ExitCardReader.LogsObject.exit != null
							? DateTimeClass.ToPersianFormat(ExitCardReader.LogsObject.exit.Value)
							: "",
					Duration = (ExitCardReader.LogsObject.exit - ExitCardReader.LogsObject.enter).Value.ToString(),
					Lyric = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric,
					Type = ExitCardReader.LogsObject.type == LogType.pub.ToString() ? "عادی" : "مشترک",
					Support = "پشتیبانی 09177382160"
				};
				printerObject.Run();
			}
		}

		private void btnExBL_Click(object sender, EventArgs e)
		{
			_acceptInput = false;
			if (!string.IsNullOrEmpty(ExitCardReader.LogsObject.exlicense))
			{
				FormEditBlackList formEditBlackList = new FormEditBlackList()
				{
					CarLicense = ExitCardReader.LogsObject.exlicense,
					IsNew = true
				};

				formEditBlackList.ShowDialog();
			}
			else
			{
				MessageBox.Show(@"شماره پلاکی برای ثبت وجود ندارد.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			_acceptInput = true;
		}

		private void btnEnOpenGate_Click(object sender, EventArgs e)
		{
			OpenEnterGate();
		}

		public void OpenEnterGate()
		{
			_serialComPortEnter.SendCommandToGate("IN", "UP");
			SetErrorLable(lblEnStatus, "راهبند باز شد", LogStatus.Warning);
			if (Tools.AllowCloseEnterBarrierAuto())
			{
				_timerCloseExitGate.Interval = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Open.ActTime;
				_timerCloseExitGate.Enabled = true;
			}
		}

		private void btnEnCloseGate_Click(object sender, EventArgs e)
		{
			CloseEnterGate();
		}

		public void CloseEnterGate()
		{
			_serialComPortEnter.SendCommandToGate("IN", "DN");
			SetErrorLable(lblEnStatus, "راهبند بسته شد", LogStatus.Warning);
		}

		private void btnExOpenGate_Click(object sender, EventArgs e)
		{
			OpenExitGate();
		}

		public void OpenExitGate()
		{
			_serialComPortExit.SendCommandToGate("OUT", "UP");
			SetErrorLable(lblExStatus, "راهبند بسته شد", LogStatus.Warning);
			if (Tools.AllowCloseExitBarrierAuto())
			{
				_timerCloseExitGate.Interval = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Open.ActTime;
				_timerCloseExitGate.Enabled = true;
			}
		}

		private void btnExCloseGate_Click(object sender, EventArgs e)
		{
			CloseExitGate();
		}

		public void CloseExitGate()
		{
			_serialComPortExit.SendCommandToGate("OUT", "DN");
			SetErrorLable(lblExStatus, "راهبند باز شد", LogStatus.Warning);

		}

		private void btnEnSubmit_Click(object sender, EventArgs e)
		{
			if (_submitEnterRecord == false)
			{
				AddNewRecord();
			}
		}

		private void btnExSubmit_Click(object sender, EventArgs e)
		{
			if (_submitExitRecord == false)
			{
				UpdateRecord();
			}
		}

		private void btnEnBL_Click(object sender, EventArgs e)
		{
			_acceptInput = false;
			if (!string.IsNullOrEmpty(EnterCardReader.LogsObject.enlicense))
			{
				FormEditBlackList formEditBlackList = new FormEditBlackList()
				{
					CarLicense = EnterCardReader.LogsObject.enlicense,
					IsNew = true
				};

				formEditBlackList.ShowDialog();
			}
			else
			{
				MessageBox.Show(@"شماره پلاکی برای ثبت وجود ندارد.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			_acceptInput = true;
		}

		public LicensePlate GetLpFromServer(Image RequestObject)
		{
			if (Tools.ExistUrl(GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Address))
			{
				try
				{
					string url = "http://" + GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Address + ":" + GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Port;
					ParsParkServiceClient.ParsParkWebService clientWebService = new ParsParkServiceClient.ParsParkWebService
					{
						Url = url,
						Timeout = 5000
					};

					return (LicensePlate)MySerialize.DeserializeObject(clientWebService.DetectLp(ImageProcess.ImageToBase64(RequestObject, ImageFormat.Jpeg)));
				}
				catch
				{
					statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در ارسال اطلاعات به پورت نرم افزار ");
					return null;
				}
			}
			statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در ارسال اطلاعات به پورت نرم افزار ");
			return null;
		}

		public void SendCapacityToServer(int CapacityValue)
		{
			if (Tools.ExistUrl(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Address))
			{
				try
				{
					// Create a TcpClient.
					// Note, for this client to work you need to have a TcpServer 
					// connected to the same address as specified by the server, port
					// combination.
					TcpClient tcpClient = new TcpClient(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Address, GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Port)
					{
						SendTimeout = 5000
					};
					if (tcpClient.Connected)
					{
						// Translate the passed message into ASCII and store it as a Byte array.
						Byte[] data =
							System.Text.Encoding.ASCII.GetBytes(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Name +
							                                    ";CAP;" + CapacityValue + ";\r\n");

						// Get a client stream for reading and writing.
						//  Stream stream = client.GetStream();

						NetworkStream stream = tcpClient.GetStream();

						// Send the message to the connected TcpServer. 
						stream.Write(data, 0, data.Length);
						Thread.Sleep(1000);

						// Close everything.
						stream.Close();
						tcpClient.Close();
						return;
					}
				}
				catch
				{
					statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در ارسال ظرفیت به سرور ");
					return;
				}
			}
			statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در ارسال ظرفیت به سرور ");
		}

		/// <summary>
		/// After sum time send close gate command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerCloseEnterGate_Tick(object sender, EventArgs e)
		{
			_serialComPortEnter.SendCommandToGate("IN", "DN");
			_timerCloseEnterGate.Enabled = false;
		}

		/// <summary>
		/// After sum time send close gate command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerCloseExitGate_Tick(object sender, EventArgs e)
		{
			_serialComPortExit.SendCommandToGate("OUT", "DN");
			_timerCloseExitGate.Enabled = false;
		}

		/// <summary>
		/// Update list and capacity every time (1 second)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateGUI_Tick(object sender, EventArgs e)
		{
			_timerUpdateGui.Stop();

			try
			{
				GlobalVariables.AllSettingsObject.LoadGlobalSettings(GlobalVariables.ConnectionString);
				// Get count of enterlogs table rows for capacity
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				UpdateParkingCapacity((from l in parsPark.enterlogs where l.exit == null select l.id).Count());
			}
			catch
			{
				// ignored
			}

			// Add new entered cars rows from another gates to list
			// Select all entered cars from this time to now
			DateTime startDateTime = _startDateTime;

			try
			{
				parsparkoEntities parsPark1 = new parsparkoEntities(GlobalVariables.ConnectionString);
				var newEnterCarsList = (from el in parsPark1.enterlogs where el.enter >= startDateTime || el.exit >= startDateTime select new { el.id, el.exit, el.enter, el.cost, el.type, el.code, el.enlicense, el.exlicense });
				foreach (var log in newEnterCarsList)
				{
					int rowIndex = FindRowInDetailsDataGridView(log.id);
					// exist
					if (rowIndex > -1)
					{
						// It exit but does not exit from this gate
						// Update grids
						if ((dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value == null ||
						     dgvDetailsAll.Rows[rowIndex].Cells["ExitTime"].Value.ToString() == "") && (log.exit != null))
						{
							lock (DataGridLock)
							{
								dgvDetailsAll.InvokeIfRequired(c =>
								{
									c.Rows[rowIndex].Cells["ExLicense"].Value = log.exlicense;
									c.Rows[rowIndex].Cells["Cost"].Value = log.cost;
									c.Rows[rowIndex].Cells["Duration"].Value = log.exit != null
										? (log.exit - log.enter).Value.ToString()
										: null;
									c.Rows[rowIndex].Cells["ExitTime"].Value = log.exit != null
										? DateTimeClass.ToPersianFormat(log.exit.Value)
										: "";
									c.Rows[rowIndex].Cells["Print"].Value = log.exit != null ? "پرینت" : "";
								});
							}
							SubscriptionInfo subInfo = new SubscriptionInfo();
							subInfo.GetSubscriptionInfo(log.code, log.enlicense);

							if (dgvExDetails.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
							{
								RemoveFromExitRecords();
							}

							lock (DataGridLock)
							{
								dgvExDetails.InvokeIfRequired(c =>
								{
									rowIndex = c.Rows.Add();
									c.Rows[rowIndex].Cells["ExCode"].Value = log.code;
									c.Rows[rowIndex].Cells["ExEnLicense"].Value = log.enlicense;
									c.Rows[rowIndex].Cells["ExExLicense"].Value = log.exlicense;
									c.Rows[rowIndex].Cells["ExEnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									c.Rows[rowIndex].Cells["ExExitTime"].Value = log.exit != null
										? DateTimeClass.ToPersianFormat(log.exit.Value)
										: "";
									c.Rows[rowIndex].Cells["ExDuration"].Value = log.exit != null
										? (log.exit - log.enter).Value.ToString()
										: null;
									c.Rows[rowIndex].Cells["ExCost"].Value = log.cost;
									c.Rows[rowIndex].Cells["ExType"].Value = subInfo.OrganInformation;
									c.Rows[rowIndex].Cells["ExDetailes"].Value = "جزئیات";
									c.Rows[rowIndex].Cells["ExPrint"].Value = log.exit != null ? "پرینت" : "";
									c.Rows[rowIndex].Tag = log.id;
									c.FirstDisplayedScrollingRowIndex = rowIndex;
								});
							}

							RemoveFromEnterRecords(log.id);
						}
					}
					else
					{
						// It is exit from another exit gate
						if (log.exit != null)
						{
							SubscriptionInfo subInfo = new SubscriptionInfo();
							subInfo.GetSubscriptionInfo(log.code, log.enlicense);

							if (dgvDetailsAll.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
							{
								RemoveFromAllRecords();
							}

							lock (DataGridLock)
							{
								dgvDetailsAll.InvokeIfRequired(c =>
								{
									rowIndex = c.Rows.Add();
									c.Rows[rowIndex].Cells["Code"].Value = log.code;
									c.Rows[rowIndex].Cells["EnLicense"].Value = log.enlicense;
									c.Rows[rowIndex].Cells["ExLicense"].Value = log.exlicense;
									c.Rows[rowIndex].Cells["EnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									c.Rows[rowIndex].Cells["ExitTime"].Value = log.exit != null
										? DateTimeClass.ToPersianFormat(log.exit.Value)
										: "";
									c.Rows[rowIndex].Cells["Duration"].Value = log.exit != null
										? (log.exit - log.enter).Value.ToString()
										: null;
									c.Rows[rowIndex].Cells["Cost"].Value = log.cost;
									c.Rows[rowIndex].Cells["Type"].Value = subInfo.OrganInformation;
									c.Rows[rowIndex].Cells["Detailes"].Value = "جزئیات";
									c.Rows[rowIndex].Cells["Print"].Value = log.exit != null ? "پرینت" : "";
									c.Rows[rowIndex].Tag = log.id;
									c.FirstDisplayedScrollingRowIndex = rowIndex;
								});
							}
							if (dgvExDetails.Rows.Count > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity)
							{
								RemoveFromExitRecords();
							}

							lock (DataGridLock)
							{
								dgvExDetails.InvokeIfRequired(c =>
								{
									rowIndex = c.Rows.Add();
									c.Rows[rowIndex].Cells["ExCode"].Value = log.code;
									c.Rows[rowIndex].Cells["ExEnLicense"].Value = log.enlicense;
									c.Rows[rowIndex].Cells["ExExLicense"].Value = log.exlicense;
									c.Rows[rowIndex].Cells["ExEnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									c.Rows[rowIndex].Cells["ExExitTime"].Value = log.exit != null
										? DateTimeClass.ToPersianFormat(log.exit.Value)
										: "";
									c.Rows[rowIndex].Cells["ExDuration"].Value = log.exit != null
										? (log.exit - log.enter).Value.ToString()
										: null;
									c.Rows[rowIndex].Cells["ExCost"].Value = log.cost;
									c.Rows[rowIndex].Cells["ExType"].Value = subInfo.OrganInformation;
									c.Rows[rowIndex].Cells["ExDetailes"].Value = "جزئیات";
									c.Rows[rowIndex].Cells["ExPrint"].Value = log.exit != null ? "پرینت" : "";
									c.Rows[rowIndex].Tag = log.id;
									c.FirstDisplayedScrollingRowIndex = rowIndex;
								});
							}
						}
						else // It is entered from another gate
						{
							SubscriptionInfo subInfo = new SubscriptionInfo();
							subInfo.GetSubscriptionInfo(log.code, log.enlicense);

							lock (DataGridLock)
							{
								dgvDetailsAll.InvokeIfRequired(c =>
								{
									rowIndex = c.Rows.Add();
									c.Rows[rowIndex].Cells["Code"].Value = log.code;
									c.Rows[rowIndex].Cells["EnLicense"].Value = log.enlicense;
									c.Rows[rowIndex].Cells["EnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									c.Rows[rowIndex].Cells["Type"].Value = subInfo.OrganInformation;
									c.Rows[rowIndex].Cells["Detailes"].Value = "جزئیات";
									c.Rows[rowIndex].Tag = log.id;
									c.FirstDisplayedScrollingRowIndex = rowIndex;
								});
							}

							lock (DataGridLock)
							{
								dgvEnDetails.InvokeIfRequired(c =>
								{
									rowIndex = c.Rows.Add();
									c.Rows[rowIndex].Cells["EnCode"].Value = log.code;
									c.Rows[rowIndex].Cells["EnEnLicense"].Value = log.enlicense;
									c.Rows[rowIndex].Cells["EnEnterTime"].Value = DateTimeClass.ToPersianFormat(log.enter);
									c.Rows[rowIndex].Cells["EnType"].Value = subInfo.OrganInformation;
									c.Rows[rowIndex].Cells["EnDetailes"].Value = "جزئیات";
									c.Rows[rowIndex].Tag = log.id;
									c.FirstDisplayedScrollingRowIndex = rowIndex;
								});
							}
						}
					}
					_startDateTime = ((log.exit != null && _startDateTime < log.exit.Value) ? log.exit.Value : ((_startDateTime
					                                                                                             < log.enter) ? log.enter : _startDateTime));
				}
			}
			catch
			{
				// ignored
			}

			//timerUpdateGUI.Enabled = true;
			_timerUpdateGui.Start();
		}

		/// <summary>
		/// Update list and capacity every time (1 second)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerShowClock_Tick(object sender, EventArgs e)
		{
			_timerShowClock.Stop();

			statusStripMain.InvokeIfRequired(c =>
			{
				c.Items["tsslClock"].Text = DateTimeClass.ToPersianFormat(DateTime.Now, "yyyy/MM/dd   HH:mm:ss");
			});

			//timerUpdateGUI.Enabled = true;
			_timerShowClock.Start();
		}

		/// <summary>
		/// Update list and capacity every time (1 second)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerListiner_Tick(object sender, EventArgs e)
		{
			_timerListiner.Stop();

			if (Tools.AllowUsingAnpr() && Tools.IsServer())
			{
				if (_serviceListiner.State != CommunicationState.Opened)
				{
					try
					{
						Uri baseAddress = new Uri("http://localhost:" + GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Port);

						_serviceListiner = new ServiceHost(typeof(ParsParkWebService), baseAddress);

						_serviceListiner.Open();
					}
					catch
					{
						statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در دریافت اطلاعات از پورت نرم افزار ");
					}
				}
			}
			else if (_serviceListiner.State == CommunicationState.Opened)
			{
				try
				{
					_serviceListiner.Close();
				}
				catch
				{
					statusStripMain.InvokeIfRequired(c => c.Items["tsslStatus"].Text = @"خطا در دریافت اطلاعات از پورت نرم افزار ");
				}
			}

			_timerListiner.Start();
		}

		// ReSharper disable once UnusedParameter.Local
		// ReSharper disable once UnusedParameter.Local
		// ReSharper disable once UnusedParameter.Local
		private void timerSendCapacity_Tick(object sender, EventArgs e)
		{
			_timerSendCapacity.Stop();
			if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Allow == PermissionAllow.Yes)
			{
				if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Board)
				{
					_serialComPortCapacity.SendCapacityToLedBoard(Convert.ToInt32(lblCapacity.Text));
				}
				else if (GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Tcp)
				{
					SendCapacityToServer(Convert.ToInt32(lblCapacity.Text));
				}
			}
			_timerSendCapacity.Start();
		}

		private void lblSettings_Click(object sender, EventArgs e)
		{
			//if (_user.type == UserType.admin.ToString() || _user.type == UserType.manager.ToString())
			{
				// Save old settings
				_acceptInput = false;
				GlobalVariables.AllOldSettingsObject = MyClone.DeepClone(GlobalVariables.AllSettingsObject);
				FormSettings formSettings = new FormSettings()
				{
					/*EnterCardReader = _enterCardReader,
					ExitCardReader = _exitCardReader,*/
					IsAdmin = _user.type == UserType.admin.ToString(),
					SerialComPortEnter = _serialComPortEnter,
					SerialComPortExit = _serialComPortExit,
					SerialComPortPrice = _serialComPortPrice,
					SerialComPortCapacity = _serialComPortCapacity,
					SerialComPortCityPay = _serialComPortCityPay,
					SerialComPortPos = _serialComPortPos,
					FormMainObject = this
				};

				if (formSettings.ShowDialog() == DialogResult.OK)
				{
					// If changed
					ChangeSettings(false);
				}
				//formSettings.ShowDialog();

				_acceptInput = true;
			}
			/*else
			{
				MessageBox.Show(@"شما مجوز دسترسی به تنظیمات را ندارید", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}*/
		}

		private void lblUsers_Click(object sender, EventArgs e)
		{
			if (_user.type == UserType.admin.ToString() || _user.type == UserType.manager.ToString())
			{
				_acceptInput = false;
				FormUsers formUsers = new FormUsers();

				formUsers.ShowDialog();
				_acceptInput = true;
			}
			else
			{
				MessageBox.Show(@"شما مجوز دسترسی به کاربران را ندارید", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void lblSubs_Click(object sender, EventArgs e)
		{
			_acceptInput = false;
			FormSubscriptions formSubscription = new FormSubscriptions()
			{
				/*EnterCardReader = _enterCardReader,
				ExitCardReader = _exitCardReader*/
				FormMainObject = this
			};

			formSubscription.ShowDialog();
			_acceptInput = true;
		}

		private void lblBlackList_Click(object sender, EventArgs e)
		{
			_acceptInput = false;
			FormBlackList formBlackList = new FormBlackList();

			formBlackList.ShowDialog();
			_acceptInput = true;
		}

		private void lblReports_Click(object sender, EventArgs e)
		{
			if (_user.type == UserType.admin.ToString() || _user.type == UserType.manager.ToString())
			{
				_acceptInput = false;
				FormReport formReport = new FormReport();
				formReport.ShowDialog();
				_acceptInput = true;
			}
			else
			{
				MessageBox.Show(@"شما مجوز دسترسی به گزارشات را ندارید", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void lblLostCards_Click(object sender, EventArgs e)
		{
			if (_user.type == UserType.admin.ToString() || _user.type == UserType.manager.ToString())
			{
				_acceptInput = false;
				FormLostList formLostList = new FormLostList()
				{
					User = _user,
					ExitImageName = _exitImageName
				};
				formLostList.ShowDialog();
				_acceptInput = true;
			}
			else
			{
				MessageBox.Show(@"شما مجوز دسترسی به فهرست کارتهای گم شده را ندارید", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void lblExit_Click(object sender, EventArgs e)
		{
			//MessageBox.Show(CheckCard("kdrfle33") > 0 ? @"OKKKKKK" : @"Errorrrrrr");
			Close();
		}

		private void btnCityPay_Click(object sender, EventArgs e)
		{
			if (ExitCardReader.LogsObject.type == LogType.pub.ToString() && Tools.AllowCityPay())
			{
				_serialComPortCityPay.SendPriceToCityPay(ExitCardReader.Cost);
			}
		}

		private void btnPOSPay_Click(object sender, EventArgs e)
		{
			if (ExitCardReader.LogsObject.type == LogType.pub.ToString() && Tools.AllowPos())
			{
				_serialComPortPos.SendPriceToPos(ExitCardReader.Cost);
			}
		}

		private void mtxtEnLPNo1_TextChanged(object sender, EventArgs e)
		{
			if (mtxtEnLPNo1.TextLength >= 2)
			{
				mtxtEnLPAlpha.Focus();
			}
		}

		private void mtxtEnLPAlpha_TextChanged(object sender, EventArgs e)
		{
			if (mtxtEnLPAlpha.TextLength >= 1)
			{
				mtxtEnLPNo2.Focus();
			}
		}

		private void mtxtEnLPNo2_TextChanged(object sender, EventArgs e)
		{
			if (mtxtEnLPNo2.TextLength >= 3)
			{
				mtxtEnLPNo3.Focus();
			}
		}

		private void mtxtEnLPNo3_TextChanged(object sender, EventArgs e)
		{
			if (mtxtEnLPNo3.TextLength >= 2)
			{
				btnEnLpEdit.Focus();
			}
		}

		private void mtxtExLPNo1_TextChanged(object sender, EventArgs e)
		{
			if (mtxtExLPNo1.TextLength >= 2)
			{
				mtxtExLPAlpha.Focus();
			}
		}

		private void mtxtExLPAlpha_TextChanged(object sender, EventArgs e)
		{
			if (mtxtExLPAlpha.TextLength >= 1)
			{
				mtxtExLPNo2.Focus();
			}
		}

		private void mtxtExLPNo2_TextChanged(object sender, EventArgs e)
		{
			if (mtxtExLPNo2.TextLength >= 3)
			{
				mtxtExLPNo3.Focus();
			}
		}

		private void mtxtExLPNo3_TextChanged(object sender, EventArgs e)
		{
			if (mtxtExLPNo3.TextLength >= 2)
			{
				btnExLpEdit.Focus();
			}
		}

		private void dgvEnDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			int columnIndex = e.ColumnIndex;
			int rowIndex = e.RowIndex;
			if (columnIndex >= 0 && rowIndex > -1)
			{
				DoListAction(rowIndex, columnIndex, dgvEnDetails.Columns[columnIndex].Name, "EnPrint", "EnDetailes", dgvEnDetails.Rows[rowIndex].Cells["EnExitTime"].Value, dgvEnDetails.Rows[rowIndex].Tag, dgvEnDetails.Rows[rowIndex].Cells["EnEnLicense"].Value != null && dgvEnDetails.Rows[rowIndex].Cells["EnEnLicense"].Value.ToString() != "" ? dgvEnDetails.Rows[rowIndex].Cells["EnEnLicense"].Value.ToString() : dgvEnDetails.Rows[rowIndex].Cells["EnExLicense"].Value != null && dgvEnDetails.Rows[rowIndex].Cells["EnExLicense"].Value.ToString() != "" ? dgvEnDetails.Rows[rowIndex].Cells["EnExLicense"].Value.ToString() : "مجهول", dgvEnDetails.Rows[rowIndex].Cells["EnCost"].Value != null ? dgvEnDetails.Rows[rowIndex].Cells["EnCost"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name, dgvEnDetails.Rows[rowIndex].Cells["EnEnterTime"].Value != null ? dgvEnDetails.Rows[rowIndex].Cells["EnEnterTime"].Value.ToString() : "", dgvEnDetails.Rows[rowIndex].Cells["EnExitTime"].Value != null ? dgvEnDetails.Rows[rowIndex].Cells["EnExitTime"].Value.ToString() : "", dgvEnDetails.Rows[rowIndex].Cells["EnType"].Value != null ? dgvEnDetails.Rows[rowIndex].Cells["EnType"].Value.ToString() : "", dgvEnDetails.Rows[rowIndex].Cells["EnDuration"].Value != null ? dgvEnDetails.Rows[rowIndex].Cells["EnDuration"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric, @"پشتیبانی 09177382160");
			}
		}

		private void dgvExDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			int columnIndex = e.ColumnIndex;
			int rowIndex = e.RowIndex;
			if (columnIndex >= 0 && rowIndex > -1)
			{
				DoListAction(rowIndex, columnIndex, dgvExDetails.Columns[columnIndex].Name, "ExPrint", "ExDetailes", dgvExDetails.Rows[rowIndex].Cells["ExExitTime"].Value, dgvExDetails.Rows[rowIndex].Tag, dgvExDetails.Rows[rowIndex].Cells["ExEnLicense"].Value != null && dgvExDetails.Rows[rowIndex].Cells["ExEnLicense"].Value.ToString() != "" ? dgvExDetails.Rows[rowIndex].Cells["ExEnLicense"].Value.ToString() : dgvExDetails.Rows[rowIndex].Cells["ExExLicense"].Value != null && dgvExDetails.Rows[rowIndex].Cells["ExExLicense"].Value.ToString() != "" ? dgvExDetails.Rows[rowIndex].Cells["ExExLicense"].Value.ToString() : "مجهول", dgvExDetails.Rows[rowIndex].Cells["ExCost"].Value != null ? dgvExDetails.Rows[rowIndex].Cells["ExCost"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name, dgvExDetails.Rows[rowIndex].Cells["ExEnterTime"].Value != null ? dgvExDetails.Rows[rowIndex].Cells["ExEnterTime"].Value.ToString() : "", dgvExDetails.Rows[rowIndex].Cells["ExExitTime"].Value != null ? dgvExDetails.Rows[rowIndex].Cells["ExExitTime"].Value.ToString() : "", dgvExDetails.Rows[rowIndex].Cells["ExType"].Value != null ? dgvExDetails.Rows[rowIndex].Cells["ExType"].Value.ToString() : "", dgvExDetails.Rows[rowIndex].Cells["ExDuration"].Value != null ? dgvExDetails.Rows[rowIndex].Cells["ExDuration"].Value.ToString() : "", GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric, @"پشتیبانی 09177382160");
			}
		}


		public long CheckCard(string CardSerialNumber)
		{
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse

			ActivationClass activationObj = new ActivationClass { CardCode = CardSerialNumber };
			activationObj.GenerateCardKey();
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				cards card = parsPark.cards.FirstOrDefault(t => t.code == CardSerialNumber);
				if (card != null)
				{
					if (card.key == activationObj.CardKey)
					{
						return card.id;
					}
					return 0;
				}

				// just for old parkings
				//*******************************************
				//** it should be removed for new parkings **
				//*******************************************
				// in the first create card info
				// ReSharper disable once ConditionIsAlwaysTrueOrFalse
				/*if (Constants.CardValidationCheck == CheckCardValidation.No)
				{
					return SaveNewCard(activationObj.CardCode, activationObj.CardKey);
				}*/
				//return 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Error : " + ex.Message);
				return 0;
			}
			return 0;
		}


		private long SaveNewCard(string CardCode, string CardKey)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				cards cardObj = new cards()
				{
					code = CardCode,
					key = CardKey,
				};

				cardObj = parsPark.cards.Add(cardObj);
				if (parsPark.SaveChanges() > 0)
				{
					return cardObj.id;
				}
				return 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return 0;
			}
		}

		private void lblAbout_Click(object sender, EventArgs e)
		{
			_acceptInput = false;

			FormAbout formAbout = new FormAbout();
			formAbout.ShowDialog();

			_acceptInput = true;
		}
	}
}
