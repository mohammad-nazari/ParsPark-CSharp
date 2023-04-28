using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;
using PrinterLib;
using SerialPortLib;
using CameraLib;
using MetroFramework;
using MetroFramework.Forms;

// ReSharper disable CoVariantArrayConversion

namespace ParsPark
{
	public partial class FormSettings : MetroForm
	{
		[System.Runtime.InteropServices.DllImport("rtspcameratest.dll")]
		// ReSharper disable once UnusedMember.Local
		private static extern int GetPicture(string CameraUrl, string FileAddress);

		public bool IsAdmin { get; set; } = false;

		public FormMain FormMainObject { get; set; }

		public string[] PortNames { get; set; }

		public GateEnter SerialComPortEnter { get; set; } = new GateEnter();
		public GateExit SerialComPortExit { get; set; } = new GateExit();
		public PriceLed SerialComPortPrice { get; set; } = new PriceLed();
		public CapacityLED SerialComPortCapacity { get; set; } = new CapacityLED();
		public CityPay SerialComPortCityPay { get; set; } = new CityPay();
		public POS SerialComPortPos { get; set; } = new POS();

		/*public CardReaderEnter EnterCardReader { get; set; } = new CardReaderEnter();

		public CardReaderExit ExitCardReader { get; set; } = new CardReaderExit();*/

		/// <summary>
		/// Edit and apply setting to project
		/// Include:
		/// 1. License Plate Recognition options (use or no, and input license plate from picture(ANPR) or manual
		/// 2. Database setting (server, username, password, database name)
		/// 3. Camera (Enter URL, Exit URL)
		/// 4. Printer (use or no, Lyric)
		/// </summary>
		/// <returns></returns>
		public FormSettings()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initialize settings from file
		/// and fill form data
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		private void FormSettings_Load(object sender, EventArgs e)
		{
			PopulateInstalledPrintersCombo();
			LoadSettings();
			InitializeSettingObject();
			PortNames = SerialPort.GetPortNames();
			List<string> termsList = new List<string> { "" };
			termsList.AddRange(PortNames);
			// You can convert it back to an array if you would like to
			PortNames = termsList.ToArray();

			FillForm();
		}

		/// <summary>
		/// Get list of printers and fill combobox
		/// </summary>
		/// <returns></returns>
		private void PopulateInstalledPrintersCombo()
		{
			// Add list of installed printers found to the combo box.
			// The pkInstalledPrinters string will be used to provide the display string.
			for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
			{
				var pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
				cbPrinters.Items.Add(pkInstalledPrinters);
			}
		}

		/// <summary>
		/// Initialize Setting object data
		/// </summary>
		/// <returns></returns>
		private void InitializeSettingObject()
		{
			GlobalVariables.AllSettingsObject.InitializeSettingObject();
		}

		/// <summary>
		/// Load setting from Setting.xml file
		/// </summary>
		/// <returns></returns>
		private void LoadSettings()
		{
			if (!GlobalVariables.AllSettingsObject.LoadGlobalSettings(GlobalVariables.ConnectionString))
			{
				MessageBox.Show(@"خطا در خواندن تنظیمات از پایگاه داده", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			/*if (GlobalVariables.AllSettingsObject.LoadSettings() == false)
			{
				MessageBox.Show(@"خطا در خواندن تنظیمات از فایل", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}*/
		}

		/// <summary>
		/// Write Setting.xml data that loaded into setting class object into form objects
		/// </summary>
		/// <returns></returns>
		private void FillForm()
		{
			//Parking
			txtPrkingName.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name;
			rtbCapacity.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity.ToString();

			cbDayStartTime.SelectedIndex = cbDayStartTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Start.ToString());
			cbDayEndTime.SelectedIndex = cbDayEndTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.End.ToString());
			mtbDayCostFirst.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.First.ToString();
			mtbDayCostNext.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next.ToString();

			cbNightStartTime.SelectedIndex = cbNightStartTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Start.ToString());
			cbNightEndTime.SelectedIndex = cbNightEndTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.End.ToString());
			mtbNightCostFirst.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.First.ToString();
			mtbNightCostNext.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next.ToString();

			mtbCost24.Text = GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.ADay.ToString();

			cbFreeTime.SelectedIndex = cbFreeTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Free.ToString());
			cbLastHourFreeTime.SelectedIndex = cbLastHourFreeTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Last.ToString());

			tpParking.Enabled = IsAdmin;
			tpParking.Visible = IsAdmin;

			// License Plate
			var enableDisable = Tools.AllowAnpr();
			chbSubmitLicensePlate.Enabled = enableDisable;
			chbSubmitLicensePlate.Checked = GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes;
			rbANPR.Enabled = enableDisable && chbSubmitLicensePlate.Checked;
			rbANPR.Checked = enableDisable && (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.StartType == SettingANPRStartType.Auto);
			chbAllowEnter.Enabled = rbANPR.Enabled && rbANPR.Checked;
			chbAllowExit.Enabled = rbANPR.Enabled && rbANPR.Checked;
			chbAllowEnter.Checked = enableDisable && (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Enter.Allow == PermissionAllow.Yes);
			chbAllowExit.Checked = enableDisable && (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Exit.Allow == PermissionAllow.Yes);
			rbInputLicenseManual.Enabled = chbSubmitLicensePlate.Checked;
			rbInputLicenseManual.Checked = !rbANPR.Checked;

			// Database
			txtServerAddress.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Server;
			txtUsername.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Username;
			txtPassword.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Password;
			txtPort.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Port.ToString();
			txtDatabase.Text = GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Database;

			// Camera
			enableDisable = Tools.AllowCamera();
			chbEnterCameraEnable.Enabled = enableDisable;
			chbEnterCameraEnable.Checked = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow == PermissionAllow.Yes;
			btnEnterTest.Enabled = enableDisable && chbEnterCameraEnable.Checked;
			chbEnterCameraSave.Enabled = enableDisable && chbEnterCameraEnable.Checked;
			chbEnterCameraSave.Checked = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Save;
			txtEnterCamera.Enabled = enableDisable && chbEnterCameraEnable.Checked;
			txtEnterCamera.Text = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL;

			chbExitCameraEnable.Enabled = enableDisable;
			chbExitCameraEnable.Checked = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow == PermissionAllow.Yes;
			btnExitTest.Enabled = enableDisable && chbExitCameraEnable.Checked;
			chbExitCameraSave.Enabled = enableDisable && chbExitCameraEnable.Checked;
			chbExitCameraSave.Checked = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Save;
			txtExitCamera.Enabled = enableDisable && chbExitCameraEnable.Checked;
			txtExitCamera.Text = GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.URL;

			// Printer
			chbPrintRecipt.Checked = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Allow == PermissionAllow.Yes;
			rtxtLyric.Text = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric;
			cbPrinters.SelectedIndex = cbPrinters.FindString(GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Name);

			// Card reader
			cbEnterCarReader.Items.Clear();
			// Set combobox items to exist com ports
			cbEnterCarReader.Items.AddRange(PortNames);
			cbEnterCarReader.SelectedIndex = cbEnterCarReader.FindString(GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.Name);
			cbEnterBaudRate.SelectedIndex = cbEnterBaudRate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.BaudRate.ToString());

			cbExitCarReader.Items.Clear();
			// Set combobox items to exist com ports
			cbExitCarReader.Items.AddRange(PortNames);
			cbExitCarReader.SelectedIndex = cbExitCarReader.FindString(GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.Name);
			cbExitBaudRate.SelectedIndex = cbExitBaudRate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.BaudRate.ToString());

			// Software settings
			// Enabled or disabled Enter and Exit side of software
			rbServer.Checked = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Allow == PermissionAllow.Yes;
			mtxtPort.Text = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Port.ToString();
			mtxtPort.Enabled = rbServer.Checked;

			rbClient.Checked = !rbServer.Checked;
			txtServerIP.Text = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Address;
			txtServerIP.Enabled = !rbServer.Checked;
			mtxtServerPort.Text = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Port.ToString();
			mtxtServerPort.Enabled = !rbServer.Checked;
			btnTestServerAddress.Enabled = !rbServer.Checked;

			rbActiveEnterSide.Checked = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Enter.Allow == PermissionAllow.Yes;
			rbActiveExitSide.Checked = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Exit.Allow == PermissionAllow.Yes;
			rbActiveEnterExitSide.Checked = GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Enter.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Exit.Allow == PermissionAllow.Yes;

			// Barrier
			enableDisable = Tools.AllowBarrier();

			// Enter barrier
			// Gates com port
			cbEnterBarrierPortName.Items.Clear();
			cbEnterBarrierPortName.Items.AddRange(PortNames);
			chbEnterBarrier.Enabled = enableDisable;
			cbEnterBarrierPortName.SelectedIndex = cbEnterBarrierPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.Name);
			cbEnterBarrierBaudrate.SelectedIndex = cbEnterBarrierBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.BaudRate.ToString());
			chbEnterBarrier.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes;
			rbEnterBarrierOpenAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Open.StartType == SettingANPRStartType.Auto;
			rbEnterBarrierOpenManual.Checked = !rbEnterBarrierOpenAuto.Checked;
			rbEnterBarrierCloseAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.StartType == SettingANPRStartType.Auto;
			rbEnterBarrierCloseManual.Checked = !rbEnterBarrierCloseAuto.Checked;
			cbEnterBarrierCloseTime.SelectedIndex = cbEnterBarrierCloseTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.ActTime.ToString());
			EnableDisableEnterBarrier(enableDisable && chbEnterBarrier.Checked);

			// Exit barrier
			// Gates com port
			cbExitBarrierPortName.Items.Clear();
			cbExitBarrierPortName.Items.AddRange(PortNames);
			chbExitBarrier.Enabled = enableDisable;
			cbExitBarrierPortName.SelectedIndex = cbExitBarrierPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.Name);
			cbExitBarrierBaudrate.SelectedIndex = cbExitBarrierBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.BaudRate.ToString());
			chbExitBarrier.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes;
			rbExitBarrierOpenAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Open.StartType == SettingANPRStartType.Auto;
			rbExitBarrierOpenManual.Checked = !rbExitBarrierOpenAuto.Checked;
			rbExitBarrierCloseAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.StartType == SettingANPRStartType.Auto;
			rbExitBarrierCloseManual.Checked = !rbExitBarrierCloseAuto.Checked;
			cbExitBarrierCloseTime.SelectedIndex = cbExitBarrierCloseTime.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.ActTime.ToString());
			EnableDisableExitBarrier(enableDisable && chbExitBarrier.Checked);

			// Boards
			enableDisable = Tools.AllowBoard();
			// Price LED board
			chbSendCost.Enabled = enableDisable;
			cbPriceBoardPortName.Items.Clear();
			cbPriceBoardPortName.Items.AddRange(PortNames);
			cbPriceBoardPortName.SelectedIndex = cbEnterBarrierPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.Name);
			cbPriceBoardBaudrate.SelectedIndex = cbEnterBarrierBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.BaudRate.ToString());
			chbSendCost.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Allow == PermissionAllow.Yes;
			EnableDisablePriceBoard(enableDisable && chbSendCost.Checked);

			// Capacity LED board
			chbSendCapacity.Enabled = enableDisable;
			cbCapacityBoardPortName.Items.Clear();
			cbCapacityBoardPortName.Items.AddRange(PortNames);
			rbCapacityBoard.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Board;
			cbCapacityBoardPortName.SelectedIndex = cbEnterBarrierPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.Name);
			cbCapacityBoardBaudrate.SelectedIndex = cbEnterBarrierBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.BaudRate.ToString());
			rbCapacityTCP.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType == SettingCapacitySendType.Tcp;
			txtCapacityIp.Text = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Address;
			rtxtCapacityPort.Text = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Port.ToString();
			txtCapacityName.Text = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Name;
			chbSendCapacity.Checked = GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Allow == PermissionAllow.Yes;
			EnableDisableCapacityBoard(enableDisable && chbSendCapacity.Checked);

			// Payments
			enableDisable = Tools.AllowPayment();
			// CityPay
			chbCityPay.Enabled = enableDisable;
			cbCityPayPortName.Items.Clear();
			cbCityPayPortName.Items.AddRange(PortNames);
			cbCityPayPortName.SelectedIndex = cbCityPayPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.Name);
			cbCityPayBaudrate.SelectedIndex = cbCityPayBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.BaudRate.ToString());
			chbCityPay.Checked = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Allow;
			chbCityPayAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.AutoSend;
			EnableDisableCityPay(enableDisable && chbCityPay.Checked);

			// POS
			chbPOS.Enabled = enableDisable;
			cbPOSPortName.Items.Clear();
			cbPOSPortName.Items.AddRange(PortNames);
			cbPOSPortName.SelectedIndex = cbPOSPortName.FindString(GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.Name);
			cbPOSBaudrate.SelectedIndex = cbPOSBaudrate.FindString(GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.BaudRate.ToString());
			chbPOS.Checked = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Allow;
			chbPOSAuto.Checked = GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.AutoSend;
			EnableDisablePos(enableDisable && chbPOS.Checked);
		}

		/// <summary>
		/// Apply new setting into objects
		/// (License Plate variable, Database info into Entity Framework object and DataSet object, Camera URLs into Camera variables, and Printer settings object)
		/// </summary>
		/// <returns></returns>
		private void btnOk_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		/// <summary>
		/// Save setting into Setting.xml file
		/// </summary>
		/// <returns></returns>
		private void SaveSettings()
		{
			//Parking 
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Name = txtPrkingName.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Capacity = Convert.ToInt16(rtbCapacity.Text);

			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Start = Convert.ToInt32(cbDayStartTime.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.End = Convert.ToInt32(cbDayEndTime.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.First = Convert.ToInt32(mtbDayCostFirst.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next = Convert.ToInt32(mtbDayCostNext.Text);

			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Start = Convert.ToInt32(cbNightStartTime.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.End = Convert.ToInt32(cbNightEndTime.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.First = Convert.ToInt32(mtbNightCostFirst.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next = Convert.ToInt32(mtbNightCostNext.Text);

			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.ADay = Convert.ToInt32(mtbCost24.Text);

			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Free = Convert.ToInt32(cbFreeTime.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Last = Convert.ToInt32(cbLastHourFreeTime.Text);

			// License Plate
			GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow = chbSubmitLicensePlate.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Enter.Allow = chbAllowEnter.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Exit.Allow = chbAllowExit.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.StartType = rbANPR.Checked ? SettingANPRStartType.Auto : SettingANPRStartType.Manual;

			// Database
			GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Server = txtServerAddress.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Username = txtUsername.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Password = txtPassword.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Port = Convert.ToInt32(txtPort.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.DatabaseSetting.Database = txtDatabase.Text;

			// Camera
			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow = chbEnterCameraEnable.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.URL = txtEnterCamera.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Save = chbEnterCameraSave.Checked;

			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow = chbExitCameraEnable.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.URL = txtExitCamera.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Save = chbExitCameraSave.Checked;

			// Printer
			GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Allow = chbPrintRecipt.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric = rtxtLyric.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Name = cbPrinters.Text;

			// Card Printer
			GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.Name = cbEnterCarReader.SelectedIndex >= 0 ? cbEnterCarReader.Text : GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.BaudRate = cbEnterBaudRate.SelectedIndex >= 0 ? Convert.ToInt32(cbEnterBaudRate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Enter.BaudRate;
			//
			GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.Name = cbExitCarReader.SelectedIndex >= 0 ? cbExitCarReader.Text : GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.BaudRate = cbExitBaudRate.SelectedIndex >= 0 ? Convert.ToInt32(cbExitBaudRate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.CardReaderSetting.Exit.BaudRate;

			// Software
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Allow = rbServer.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Address = txtServerIP.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Port = Convert.ToInt32(mtxtServerPort.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Enter.Allow = rbActiveEnterSide.Checked || rbActiveEnterExitSide.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Exit.Allow = rbActiveExitSide.Checked || rbActiveEnterExitSide.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Port = Convert.ToInt32(mtxtPort.Text);

			// Barrier
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.Name = cbEnterBarrierPortName.SelectedIndex >= 0 ? cbEnterBarrierPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.BaudRate = cbEnterBarrierBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbEnterBarrierBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Port.BaudRate;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow = chbEnterBarrier.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Open.StartType = rbEnterBarrierOpenAuto.Checked ? SettingANPRStartType.Auto : SettingANPRStartType.Manual;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.StartType = rbEnterBarrierCloseAuto.Checked ? SettingANPRStartType.Auto : SettingANPRStartType.Manual;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.ActTime = cbEnterBarrierCloseTime.SelectedIndex >= 0 ? Convert.ToInt32(cbEnterBarrierCloseTime.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.ActTime;

			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.Name = cbExitBarrierPortName.SelectedIndex >= 0 ? cbExitBarrierPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.BaudRate = cbExitBarrierBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbExitBarrierBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Port.BaudRate;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow = chbExitBarrier.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Open.StartType = rbExitBarrierOpenAuto.Checked ? SettingANPRStartType.Auto : SettingANPRStartType.Manual;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.StartType = rbExitBarrierCloseAuto.Checked ? SettingANPRStartType.Auto : SettingANPRStartType.Manual;
			GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.ActTime = cbExitBarrierCloseTime.SelectedIndex >= 0 ? Convert.ToInt32(cbExitBarrierCloseTime.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.ActTime;

			// Board
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Allow = chbSendCost.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.Name = cbPriceBoardPortName.SelectedIndex >= 0 ? cbPriceBoardPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.BaudRate = cbPriceBoardBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbPriceBoardBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Price.Port.BaudRate;

			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Allow = chbSendCapacity.Checked ? PermissionAllow.Yes : PermissionAllow.No;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.Name = cbCapacityBoardPortName.SelectedIndex >= 0 ? cbCapacityBoardPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.BaudRate = cbCapacityBoardBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbCapacityBoardBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.ComPort.BaudRate;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Address = txtCapacityIp.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Name = txtCapacityName.Text;
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.Port = Convert.ToInt32(rtxtCapacityPort.Text);
			GlobalVariables.AllSettingsObject.SettingsObject.BoardSetting.Capacity.SendType = rbCapacityBoard.Checked ? SettingCapacitySendType.Board : SettingCapacitySendType.Tcp;

			// Payment
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Allow = chbCityPay.Checked;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.Name = cbCityPayPortName.SelectedIndex >= 0 ? cbCityPayPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.BaudRate = cbCityPayBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbCityPayBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Port.BaudRate;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.AutoSend = chbCityPayAuto.Checked;

			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Allow = chbPOS.Checked;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.Name = cbPOSPortName.SelectedIndex >= 0 ? cbPOSPortName.Text : GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.Name;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.BaudRate = cbPOSBaudrate.SelectedIndex >= 0 ? Convert.ToInt32(cbPOSBaudrate.Text) : GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Port.BaudRate;
			GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.AutoSend = chbPOSAuto.Checked;

			if (GlobalVariables.AllSettingsObject.SaveSettings())
			{
				MessageBox.Show(@"تنظیمات با موفقیت ثبت گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			else
			{
				MessageBox.Show(@"خطا در نوشتن تنظیمات در فایل", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnDatabaseTest_Click(object sender, EventArgs e)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);

				string connInfo = "server=" + txtServerAddress.Text + ";port=" + txtPort.Text + ";database=" + txtDatabase.Text + ";user id=" + txtUsername.Text + ";password=" + txtPassword.Text + ";persistsecurityinfo = True";

				parsPark.Database.Connection.ConnectionString = connInfo;

				if (parsPark.Database.Exists())
				{
					MessageBox.Show(@"ارتباط  با پایگاه داده برقرار گردید.", @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
				else
				{
					MessageBox.Show(@"خطا در برقراری ارتباط  با پایگاه داده", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(@"خطا در برقراری ارتباط  با پایگاه داده", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnEnterTest_Click(object sender, EventArgs e)
		{
			try
			{
				Camera cameraObject = new Camera()
				{
					SourceUrl = txtEnterCamera.Text,
					ImageAddress = "ImageTestIn.jpg"
				};
				cameraObject.GetPicture();
				if (cameraObject.PictureObject != null)
				{
					FormTestPicture pictureTest = new FormTestPicture { pbCamare = { Image = cameraObject.PictureObject } };
					pictureTest.ShowDialog();
				}
				else
				{
					MessageBox.Show(@"خطا در برقراری رتباط با دوربین ورودی ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در برقراری رتباط با دوربین ورودی ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnExitTest_Click(object sender, EventArgs e)
		{
			try
			{
				Camera cameraObject = new Camera()
				{
					SourceUrl = txtExitCamera.Text,
					ImageAddress = "ImageTestOut.jpg"
				};
				cameraObject.GetPicture();
				if (cameraObject.PictureObject != null)
				{
					FormTestPicture pictureTest = new FormTestPicture { pbCamare = { Image = cameraObject.PictureObject } };
					pictureTest.ShowDialog();
				}
				else
				{
					MessageBox.Show(@"خطا در برقراری رتباط با دوربین خروجی ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			catch
			{
				MessageBox.Show(@"خطا در برقراری رتباط با دوربین خروجی ", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void chbLicensePlate_CheckedChanged(object sender, EventArgs e)
		{
			rbANPR.Enabled = chbSubmitLicensePlate.Checked;
			rbInputLicenseManual.Enabled = chbSubmitLicensePlate.Checked;
			chbAllowEnter.Enabled = chbSubmitLicensePlate.Checked;
			chbAllowExit.Enabled = chbSubmitLicensePlate.Checked;
		}

		private void btnPreview_Click(object sender, EventArgs e)
		{
			if (cbPrinters.SelectedIndex >= 0)
			{
				Printer printerObject = new Printer
				{
					LicensePlate = "12ب34567",
					Cost = "12345",
					Name = txtPrkingName.Text != "" ? txtPrkingName.Text : "پارکینگ آزمایشی",
					EnterTime = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture),
					ExitTime = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture),
					Duration = "0",
					Lyric = GlobalVariables.AllSettingsObject.SettingsObject.PrinterSetting.Lyric,
					Type = "عمومی",
					Support = "پشتیبانی 09123456789"
				};
				printerObject.Run();
			}
			else
			{
				MessageBox.Show(@"لطفا نام پرینتر را انتخاب گردند.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnEnterCardReader_Click(object sender, EventArgs e)
		{
			if (cbEnterBaudRate.SelectedIndex >= 0 && cbEnterCarReader.SelectedIndex >= 0)
			{
				/*EnterCardReader.CloseCardReader();
				//Enter card reader
				EnterCardReader.CardReaderAddress = 1;
				EnterCardReader.SerialPort.BaudRate = Convert.ToInt32(cbEnterBaudRate.Text);
				EnterCardReader.SerialPort.PortName = cbEnterCarReader.Text;
				EnterCardReader.InitializeCardReader();

				FormTestCardReader formTest = new FormTestCardReader()
				{
					EnterCardReader = EnterCardReader,
					ExitCardReader = ExitCardReader,
					EnterCard = true
				};
				formTest.ShowDialog();*/
			}
			else
			{
				MessageBox.Show(@"لطفا نام پورت و نرخ ارسال هردو انتخاب گردند.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnExitCardReader_Click(object sender, EventArgs e)
		{
			if (cbExitBaudRate.SelectedIndex >= 0 && cbExitCarReader.SelectedIndex >= 0)
			{
				/*ExitCardReader.CloseCardReader();
				// Exit card reader
				ExitCardReader.CardReaderAddress = 2;
				ExitCardReader.SerialPort.BaudRate = Convert.ToInt32(cbExitBaudRate.Text);
				ExitCardReader.SerialPort.PortName = cbExitCarReader.Text;
				ExitCardReader.InitializeCardReader();

				FormTestCardReader formTest = new FormTestCardReader()
				{
					EnterCardReader = EnterCardReader,
					ExitCardReader = ExitCardReader,
					EnterCard = false
				};
				formTest.ShowDialog();*/
			}
			else
			{
				MessageBox.Show(@"لطفا نام پورت و نرخ ارسال هردو انتخاب گردند.", @"خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void cbDayStartTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			cbNightEndTime.SelectedIndex = cbDayStartTime.SelectedIndex;
		}

		private void cbNightStartTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			cbDayEndTime.SelectedIndex = cbNightStartTime.SelectedIndex;
		}

		private void cbEnterBarrier_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisableEnterBarrier(chbEnterBarrier.Checked);
		}

		private void EnableDisableEnterBarrier(bool EnableDisable)
		{
			rbEnterBarrierOpenAuto.Enabled = EnableDisable;
			rbEnterBarrierOpenManual.Enabled = EnableDisable;
			rbEnterBarrierCloseAuto.Enabled = EnableDisable;
			rbEnterBarrierCloseManual.Enabled = EnableDisable;
			cbEnterBarrierCloseTime.Enabled = EnableDisable;
			btnoEnterOpenTest.Enabled = EnableDisable;
			btnoEnterCloseTest.Enabled = EnableDisable;
			cbEnterBarrierPortName.Enabled = EnableDisable;
			cbEnterBarrierBaudrate.Enabled = EnableDisable;
		}

		private void EnableDisablePriceBoard(bool EnableDisable)
		{
			btnPriceTest.Enabled = EnableDisable;
			cbPriceBoardPortName.Enabled = EnableDisable;
			cbPriceBoardBaudrate.Enabled = EnableDisable;
		}

		private void EnableDisableCapacityBoard(bool EnableDisable)
		{
			rbCapacityBoard.Enabled = EnableDisable;
			cbCapacityBoardPortName.Enabled = EnableDisable && rbCapacityBoard.Checked;
			cbCapacityBoardBaudrate.Enabled = EnableDisable && rbCapacityBoard.Checked;
			btnCapacityTest.Enabled = EnableDisable && rbCapacityBoard.Checked;
			rbCapacityTCP.Enabled = EnableDisable;
			txtCapacityIp.Enabled = EnableDisable && rbCapacityTCP.Checked;
			rtxtCapacityPort.Enabled = EnableDisable && rbCapacityTCP.Checked;
			btnCapacityTCPTest.Enabled = EnableDisable && rbCapacityTCP.Checked;
			txtCapacityName.Enabled = EnableDisable && rbCapacityTCP.Checked;
		}

		private void cbExitBarrier_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisableExitBarrier(chbExitBarrier.Checked);
		}

		private void EnableDisableExitBarrier(bool EnableDisable)
		{
			rbExitBarrierOpenAuto.Enabled = EnableDisable;
			rbExitBarrierOpenManual.Enabled = EnableDisable;
			rbExitBarrierCloseAuto.Enabled = EnableDisable;
			rbExitBarrierCloseManual.Enabled = EnableDisable;
			cbExitBarrierCloseTime.Enabled = EnableDisable;
			btnoExitOpenTest.Enabled = EnableDisable;
			btnoExitCloseTest.Enabled = EnableDisable;
			cbExitBarrierPortName.Enabled = EnableDisable;
			cbExitBarrierBaudrate.Enabled = EnableDisable;
		}

		private void ChangeStatus(bool ServerStatus, bool ClientStatus)
		{
			mtxtPort.Enabled = ServerStatus;

			txtServerIP.Enabled = ClientStatus;
			mtxtServerPort.Enabled = ClientStatus;
			btnTestServerAddress.Enabled = ClientStatus;
		}

		private void btnPriceTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbPriceBoardPortName.SelectedIndex, cbPriceBoardBaudrate.SelectedIndex))
			{
				if (SerialComPortPrice.CheckVerify(cbPriceBoardPortName.Text, cbPriceBoardBaudrate.Text, "قیمت"))
				{
					FormTestBoard formTestBoard = new FormTestBoard();
					formTestBoard.ShowDialog();
					int n;
					int.TryParse(formTestBoard.mtxtCost.Text, out n);
					string data = int.TryParse(formTestBoard.mtxtCost.Text, out n) ? n.ToString() : "VIP";
					SerialComPortPrice.SendPriceToLedBoard(ImageProcess.GetImageHexString(data, new Font("B Titr", 27), Color.Black, Color.Transparent, 128, 32));
				}
				else
				{
					MessageBox.Show(SerialComPortPrice.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnCapacityTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbCapacityBoardPortName.SelectedIndex, cbCapacityBoardBaudrate.SelectedIndex))
			{
				if (SerialComPortCapacity.CheckVerify(cbCapacityBoardPortName.Text, cbCapacityBoardBaudrate.Text, @"ظرفیت"))
				{
					FormTestBoard formTestBoard = new FormTestBoard();
					formTestBoard.ShowDialog();
					SerialComPortCapacity.SendCapacityToLedBoard(Convert.ToInt32(formTestBoard.mtxtCost.Text));
				}
				else
				{
					MessageBox.Show(SerialComPortCapacity.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnoEnterOpenTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbEnterBarrierPortName.SelectedIndex, cbEnterBarrierBaudrate.SelectedIndex))
			{
				if (SerialComPortEnter.CheckVerify(cbEnterBarrierPortName.Text, cbEnterBarrierBaudrate.Text, @"راهبند ورودی"))
				{
					FormMainObject.OpenEnterGate();
				}
				else
				{
					MessageBox.Show(SerialComPortEnter.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnoEnterCloseTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbEnterBarrierPortName.SelectedIndex, cbEnterBarrierBaudrate.SelectedIndex))
			{
				if (SerialComPortEnter.CheckVerify(cbEnterBarrierPortName.Text, cbEnterBarrierBaudrate.Text, @"راهبند ورودی"))
				{
					FormMainObject.CloseEnterGate();
				}
				else
				{
					MessageBox.Show(SerialComPortEnter.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnoExitOpenTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbExitBarrierPortName.SelectedIndex, cbExitBarrierBaudrate.SelectedIndex))
			{
				if (SerialComPortExit.CheckVerify(cbExitBarrierPortName.Text, cbExitBarrierBaudrate.Text, @"راهبند خروجی"))
				{
					FormMainObject.OpenExitGate();
				}
				else
				{
					MessageBox.Show(SerialComPortExit.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnoExitCloseTest_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbExitBarrierPortName.SelectedIndex, cbExitBarrierBaudrate.SelectedIndex))
			{
				if (SerialComPortExit.CheckVerify(cbExitBarrierPortName.Text, cbExitBarrierBaudrate.Text, @"راهبند خروجی"))
				{
					FormMainObject.CloseExitGate();
				}
				else
				{
					MessageBox.Show(SerialComPortExit.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private bool IsSelected(int PortNameIndex, int BaudRateIndex)
		{
			return PortNameIndex > 0 && BaudRateIndex > 0;
		}

		private void rbServer_CheckedChanged(object sender, EventArgs e)
		{
			ChangeStatus(rbServer.Checked, !rbServer.Checked);
		}

		private void rbClient_CheckedChanged(object sender, EventArgs e)
		{
			ChangeStatus(!rbClient.Checked, rbClient.Checked);
		}

		private void btnTestServerAddress_Click(object sender, EventArgs e)
		{
			try
			{
				ParsParkServiceClient.ParsParkWebService clientWebService = new ParsParkServiceClient.ParsParkWebService()
				{
					Url = "http://" + txtServerIP.Text + ":" + mtxtServerPort.Text
				};

				MessageBox.Show(clientWebService.TestServer("Hello"));
			}
			catch
			{
				MessageBox.Show(@"خطا در ارسال اطلاعات به پورت نرم افزار ");
			}
		}

		private void rbANPR_CheckedChanged(object sender, EventArgs e)
		{
			chbAllowEnter.Enabled = rbANPR.Checked;
			chbAllowExit.Enabled = rbANPR.Checked;
		}

		private void chbEnterCameraEnable_CheckedChanged(object sender, EventArgs e)
		{
			txtEnterCamera.Enabled = chbEnterCameraEnable.Checked;
			btnEnterTest.Enabled = chbEnterCameraEnable.Checked;
			chbEnterCameraSave.Enabled = chbEnterCameraEnable.Checked;
		}

		private void chbExitCameraEnable_CheckedChanged(object sender, EventArgs e)
		{
			txtExitCamera.Enabled = chbExitCameraEnable.Checked;
			btnExitTest.Enabled = chbExitCameraEnable.Checked;
			chbExitCameraSave.Enabled = chbExitCameraEnable.Checked;
		}

		private void chbSendCost_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisablePriceBoard(chbSendCost.Checked);
		}

		private void chbSendCapacity_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisableCapacityBoard(chbSendCapacity.Checked);
		}

		private void btnTestCityPay_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbCityPayPortName.SelectedIndex, cbCityPayBaudrate.SelectedIndex))
			{
				if (SerialComPortCityPay.CheckVerify(cbCityPayPortName.Text, cbCityPayBaudrate.Text, "قیمت"))
				{
					FormTestBoard formTestBoard = new FormTestBoard();
					formTestBoard.ShowDialog();
					SerialComPortCityPay.SendPriceToCityPay(Convert.ToInt32(formTestBoard.mtxtCost.Text));
				}
				else
				{
					MessageBox.Show(SerialComPortCityPay.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void btnTestPOS_Click(object sender, EventArgs e)
		{
			if (IsSelected(cbPOSPortName.SelectedIndex, cbPOSBaudrate.SelectedIndex))
			{
				if (SerialComPortPos.CheckVerify(cbPOSPortName.Text, cbPOSBaudrate.Text, "قیمت"))
				{
					FormTestBoard formTestBoard = new FormTestBoard();
					formTestBoard.ShowDialog();
					SerialComPortPos.SendPriceToPos(Convert.ToInt32(formTestBoard.mtxtCost.Text));
				}
				else
				{
					MessageBox.Show(SerialComPortPos.Errors, @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
			}
			else
			{
				MessageBox.Show(@"نام پورت و نرخ ارسال را انتخاب نمائید.", @"اخطار", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
		}

		private void EnableDisableCityPay(bool EnableDisable)
		{
			btnTestCityPay.Enabled = EnableDisable;
			cbCityPayPortName.Enabled = EnableDisable;
			cbCityPayBaudrate.Enabled = EnableDisable;
			chbCityPayAuto.Enabled = EnableDisable;
		}

		private void chbCityPay_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisableCityPay(chbCityPay.Checked);
		}

		private void EnableDisablePos(bool EnableDisable)
		{
			btnTestPOS.Enabled = EnableDisable;
			cbPOSPortName.Enabled = EnableDisable;
			cbPOSBaudrate.Enabled = EnableDisable;
			chbPOSAuto.Enabled = EnableDisable;
		}

		private void chbPOS_CheckedChanged(object sender, EventArgs e)
		{
			EnableDisablePos(chbPOS.Checked);
		}

		private void rbCapacityBoard_CheckedChanged(object sender, EventArgs e)
		{
			bool enableDisable = rbCapacityBoard.Checked;
			cbCapacityBoardPortName.Enabled = enableDisable;
			cbCapacityBoardBaudrate.Enabled = enableDisable;
			btnCapacityTest.Enabled = enableDisable;
		}

		private void rbCapacityTCP_CheckedChanged(object sender, EventArgs e)
		{
			bool enableDisable = rbCapacityTCP.Checked;
			txtCapacityIp.Enabled = enableDisable;
			rtxtCapacityPort.Enabled = enableDisable;
			btnCapacityTCPTest.Enabled = enableDisable;
			txtCapacityName.Enabled = enableDisable;
		}

		private void btnCapacityTCPTest_Click(object sender, EventArgs e)
		{
			if (Tools.ExistUrl(txtCapacityIp.Text))
			{
				FormTestBoard formTestBoard = new FormTestBoard();
				formTestBoard.ShowDialog();
				try
				{
					// Create a TcpClient.
					// Note, for this client to work you need to have a TcpServer 
					// connected to the same address as specified by the server, port
					// combination.
					TcpClient tcpClient = new TcpClient(txtCapacityIp.Text, Convert.ToInt32(rtxtCapacityPort.Text))
					{
						SendTimeout = 5000
					};
					if (tcpClient.Connected)
					{

						// Translate the passed message into ASCII and store it as a Byte array.
						Byte[] data = System.Text.Encoding.ASCII.GetBytes(txtCapacityName.Text + ";CAP;" + Convert.ToInt32(formTestBoard.mtxtCost.Text) + ";\r\n");

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
					MessageBox.Show(@"خطا در ارسال ظرفیت به سرور ");
					return;
				}
			}
			MessageBox.Show(@"خطا در ارسال ظرفیت به سرور ");
		}
	}
}
