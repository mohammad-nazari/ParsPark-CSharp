using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DataBaseLib;

namespace SettingsLib
{
	[Serializable]
	public class AllSettings
	{
		private string _settingFileAddress;
		private string _error;
		private Settings _settingsObject = new Settings();

		public GlobalSettings GlobalSettingsObject { set; get; } = new GlobalSettings();

		public string SettingFileAddress
		{
			get
			{
				return _settingFileAddress;
			}
			set
			{
				_settingFileAddress = value;
			}
		}
		public string Error
		{
			get
			{
				return _error;
			}
			set
			{
				_error = value;
			}
		}

		public Settings SettingsObject
		{
			get
			{
				return _settingsObject;
			}
			set
			{
				_settingsObject = value;
			}
		}

		public static Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static string GenerateStringFromStream(GlobalSettings s)
		{
			XmlSerializer xsSubmit = new XmlSerializer(typeof(GlobalSettings));

			using (var sww = new StringWriter())
			{
				using (XmlWriter writer = XmlWriter.Create(sww))
				{
					xsSubmit.Serialize(writer, s);
					string tmp = sww.ToString(); // Your XML
					tmp = tmp.Replace("encoding=\"utf-16\"", "");
					return tmp;
				}
			}
		}

		public bool LoadGlobalSettings(string ConnectionString)
		{
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(ConnectionString);
				settings globalSettings = parsPark.settings.FirstOrDefault();
				if (globalSettings != null)
				{
					string globalSettingsValue = globalSettings.settings1;

					using (Stream stream = GenerateStreamFromString(globalSettingsValue))
					{
						XmlSerializer xSerializer = new XmlSerializer(typeof(GlobalSettings));
						GlobalSettingsObject = (GlobalSettings)xSerializer.Deserialize(stream);
					}

					InitializeGlobalSettingObject();

					SettingsObject.ParkingSetting.Name = GlobalSettingsObject.ParkingSetting.Name;
					SettingsObject.ParkingSetting.Capacity = GlobalSettingsObject.ParkingSetting.Capacity;

					SettingsObject.ParkingSetting.Tariff.Day.Start = GlobalSettingsObject.ParkingSetting.Tariff.Day.Start;
					SettingsObject.ParkingSetting.Tariff.Day.End = GlobalSettingsObject.ParkingSetting.Tariff.Day.End;
					SettingsObject.ParkingSetting.Tariff.Day.First = GlobalSettingsObject.ParkingSetting.Tariff.Day.First;
					SettingsObject.ParkingSetting.Tariff.Day.Next = GlobalSettingsObject.ParkingSetting.Tariff.Day.Next;

					SettingsObject.ParkingSetting.Tariff.Night.Start = GlobalSettingsObject.ParkingSetting.Tariff.Night.Start;
					SettingsObject.ParkingSetting.Tariff.Night.End = GlobalSettingsObject.ParkingSetting.Tariff.Night.End;
					SettingsObject.ParkingSetting.Tariff.Night.First = GlobalSettingsObject.ParkingSetting.Tariff.Night.First;
					SettingsObject.ParkingSetting.Tariff.Night.Next = GlobalSettingsObject.ParkingSetting.Tariff.Night.Next;

					SettingsObject.ParkingSetting.Tariff.ADay = GlobalSettingsObject.ParkingSetting.Tariff.ADay;

					SettingsObject.ParkingSetting.Tariff.Free = GlobalSettingsObject.ParkingSetting.Tariff.Free;
					SettingsObject.ParkingSetting.Tariff.Last = GlobalSettingsObject.ParkingSetting.Tariff.Last;

					return true;
				}
				InitializeGlobalSettingObject();
				return false;
			}
			catch
			{
				InitializeGlobalSettingObject();
				return false;
			}
		}

		public bool SaveGlobalSettings(string ConnectionString)
		{
			InitializeGlobalSettingObject();
			InitializeSettingObject();
			GlobalSettingsObject.ParkingSetting.Name = SettingsObject.ParkingSetting.Name;
			GlobalSettingsObject.ParkingSetting.Capacity = SettingsObject.ParkingSetting.Capacity;

			GlobalSettingsObject.ParkingSetting.Tariff.Day.Start = SettingsObject.ParkingSetting.Tariff.Day.Start;
			GlobalSettingsObject.ParkingSetting.Tariff.Day.End = SettingsObject.ParkingSetting.Tariff.Day.End;
			GlobalSettingsObject.ParkingSetting.Tariff.Day.First = SettingsObject.ParkingSetting.Tariff.Day.First;
			GlobalSettingsObject.ParkingSetting.Tariff.Day.Next = SettingsObject.ParkingSetting.Tariff.Day.Next;

			GlobalSettingsObject.ParkingSetting.Tariff.Night.Start = SettingsObject.ParkingSetting.Tariff.Night.Start;
			GlobalSettingsObject.ParkingSetting.Tariff.Night.End = SettingsObject.ParkingSetting.Tariff.Night.End;
			GlobalSettingsObject.ParkingSetting.Tariff.Night.First = SettingsObject.ParkingSetting.Tariff.Night.First;
			GlobalSettingsObject.ParkingSetting.Tariff.Night.Next = SettingsObject.ParkingSetting.Tariff.Night.Next;

			GlobalSettingsObject.ParkingSetting.Tariff.ADay = SettingsObject.ParkingSetting.Tariff.ADay;

			GlobalSettingsObject.ParkingSetting.Tariff.Free = SettingsObject.ParkingSetting.Tariff.Free;
			GlobalSettingsObject.ParkingSetting.Tariff.Last = SettingsObject.ParkingSetting.Tariff.Last;

			try
			{
				string globalSettingsValue = GenerateStringFromStream(GlobalSettingsObject);

				parsparkoEntities parsPark = new parsparkoEntities(ConnectionString);

				settings settingsObject = parsPark.settings.FirstOrDefault();
				if (settingsObject != null)
				{

					if (settingsObject.settings1 != globalSettingsValue)
					{
						settingsObject.settings1 = globalSettingsValue;
						parsPark.Entry(settingsObject).CurrentValues.SetValues(settingsObject);

						return parsPark.SaveChanges() > 0;
					}
					return true;
				}

				return false;
			}
			catch
			{
				InitializeGlobalSettingObject();
				return false;
			}
		}

		public bool LoadSettings()
		{
			try
			{
				Stream str = File.Open("Settings.xml", FileMode.Open);
				XmlSerializer xSerializer = new XmlSerializer(typeof(Settings));
				_settingsObject = (Settings)xSerializer.Deserialize(str);
				str.Close();
				InitializeSettingObject();
				return true;
			}
			catch
			{
				InitializeSettingObject();
				return false;
			}
		}

		public bool SaveSettings()
		{
			try
			{
				Stream str = File.Open("Settings.xml", FileMode.Create);
				XmlSerializer xSerializer = new XmlSerializer(typeof(Settings));
				xSerializer.Serialize(str, _settingsObject);
				str.Flush();
				str.Close();
				return true;
			}
			catch
			{
				InitializeSettingObject();
				return false;
			}
		}

		public void InitializeSettingObject()
		{
			// All
			if (_settingsObject.ParkingSetting == null)
			{
				_settingsObject.ParkingSetting = new SettingParking();
			}

			// Parking
			if (_settingsObject.ParkingSetting.Tariff == null)
			{
				_settingsObject.ParkingSetting.Tariff = new SettingTariff();
			}
			if (_settingsObject.ParkingSetting.Tariff.Day == null)
			{
				_settingsObject.ParkingSetting.Tariff.Day = new TariffInfo();
			}
			if (_settingsObject.ParkingSetting.Tariff.Night == null)
			{
				_settingsObject.ParkingSetting.Tariff.Night = new TariffInfo();
			}

			// ANPR
			if (_settingsObject.ANPRSetting == null)
			{
				_settingsObject.ANPRSetting = new SettingANPR();
			}
			if (_settingsObject.ANPRSetting.Enter == null)
			{
				_settingsObject.ANPRSetting.Enter = new Permission();
			}
			if (_settingsObject.ANPRSetting.Exit == null)
			{
				_settingsObject.ANPRSetting.Exit = new Permission();
			}

			// DataBase
			if (_settingsObject.DatabaseSetting == null)
			{
				_settingsObject.DatabaseSetting = new SettingDatabase();
			}

			// Camera
			if (_settingsObject.CameraSetting == null)
			{
				_settingsObject.CameraSetting = new SettingCamera();
			}
			if (_settingsObject.CameraSetting.Enter == null)
			{
				_settingsObject.CameraSetting.Enter = new CameraInfo();
			}
			if (_settingsObject.CameraSetting.Exit == null)
			{
				_settingsObject.CameraSetting.Exit = new CameraInfo();
			}

			// Printer
			if (_settingsObject.PrinterSetting == null)
			{
				_settingsObject.PrinterSetting = new SettingPrinter();
			}

			// Software
			if (_settingsObject.SoftwareSetting == null)
			{
				_settingsObject.SoftwareSetting = new SettingSoftware();
			}
			if (_settingsObject.SoftwareSetting.Server == null)
			{
				_settingsObject.SoftwareSetting.Server = new ServerInfo();
			}
			if (_settingsObject.SoftwareSetting.Enter == null)
			{
				_settingsObject.SoftwareSetting.Enter = new Permission();
			}
			if (_settingsObject.SoftwareSetting.Exit == null)
			{
				_settingsObject.SoftwareSetting.Exit = new Permission();
			}

			// Card reader
			if (_settingsObject.CardReaderSetting == null)
			{
				_settingsObject.CardReaderSetting = new SettingCardReader();
			}
			if (_settingsObject.CardReaderSetting.Enter == null)
			{
				_settingsObject.CardReaderSetting.Enter = new ComPortInfo();
			}
			if (_settingsObject.CardReaderSetting.Exit == null)
			{
				_settingsObject.CardReaderSetting.Exit = new ComPortInfo();
			}

			// Barrier
			if (_settingsObject.BarrierSetting == null)
			{
				_settingsObject.BarrierSetting = new SettingBarrier();
			}
			if (_settingsObject.BarrierSetting.Enter == null)
			{
				_settingsObject.BarrierSetting.Enter = new BarrierInfo();
			}
			if (_settingsObject.BarrierSetting.Enter.Open == null)
			{
				_settingsObject.BarrierSetting.Enter.Open = new BarrierTypeInfo();
			}
			if (_settingsObject.BarrierSetting.Enter.Close == null)
			{
				_settingsObject.BarrierSetting.Enter.Close = new BarrierTypeInfo();
			}
			if (_settingsObject.BarrierSetting.Enter.Port == null)
			{
				_settingsObject.BarrierSetting.Enter.Port = new ComPortInfo();
			}
			if (_settingsObject.BarrierSetting.Exit == null)
			{
				_settingsObject.BarrierSetting.Exit = new BarrierInfo();
			}
			if (_settingsObject.BarrierSetting.Exit.Open == null)
			{
				_settingsObject.BarrierSetting.Exit.Open = new BarrierTypeInfo();
			}
			if (_settingsObject.BarrierSetting.Exit.Close == null)
			{
				_settingsObject.BarrierSetting.Exit.Close = new BarrierTypeInfo();
			}
			if (_settingsObject.BarrierSetting.Exit.Port == null)
			{
				_settingsObject.BarrierSetting.Exit.Port = new ComPortInfo();
			}

			// Board
			if (_settingsObject.BoardSetting == null)
			{
				_settingsObject.BoardSetting = new SettingBoard();
			}
			if (_settingsObject.BoardSetting.Capacity == null)
			{
				_settingsObject.BoardSetting.Capacity = new SettingCapacity();
			}
			if (_settingsObject.BoardSetting.Capacity.ComPort == null)
			{
				_settingsObject.BoardSetting.Capacity.ComPort = new ComPortInfo();
			}
			if (_settingsObject.BoardSetting.Price == null)
			{
				_settingsObject.BoardSetting.Price = new BoardInfo();
			}
			if (_settingsObject.BoardSetting.Price.Port == null)
			{
				_settingsObject.BoardSetting.Price.Port = new ComPortInfo();
			}
			if (_settingsObject.PaymentSetting == null)
			{
				_settingsObject.PaymentSetting = new SettingPayment();
			}
			if (_settingsObject.PaymentSetting.CityPay == null)
			{
				_settingsObject.PaymentSetting.CityPay = new PaymentInfo();
			}
			if (_settingsObject.PaymentSetting.CityPay.Port == null)
			{
				_settingsObject.PaymentSetting.CityPay.Port = new ComPortInfo();
			}
			if (_settingsObject.PaymentSetting.POS == null)
			{
				_settingsObject.PaymentSetting.POS = new PaymentInfo();
			}
			if (_settingsObject.PaymentSetting.POS.Port == null)
			{
				_settingsObject.PaymentSetting.POS.Port = new ComPortInfo();
			}
		}

		public void InitializeGlobalSettingObject()
		{
			// All
			if (GlobalSettingsObject.ParkingSetting == null)
			{
				GlobalSettingsObject.ParkingSetting = new GlobalSettingParking();
			}

			// Parking
			if (GlobalSettingsObject.ParkingSetting.Tariff == null)
			{
				GlobalSettingsObject.ParkingSetting.Tariff = new GlobalSettingTariff();
			}
			if (GlobalSettingsObject.ParkingSetting.Tariff.Day == null)
			{
				GlobalSettingsObject.ParkingSetting.Tariff.Day = new GlobalTariffInfo();
			}
			if (GlobalSettingsObject.ParkingSetting.Tariff.Night == null)
			{
				GlobalSettingsObject.ParkingSetting.Tariff.Night = new GlobalTariffInfo();
			}
		}
	}
}