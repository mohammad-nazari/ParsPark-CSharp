using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;

namespace SerialPortLib
{
	using ToolsLib;

	public class CityPay
	{
		#region import
		[System.Runtime.InteropServices.DllImport("CityPayLib.dll")]
		private static extern int OpenCOMPort(byte[] Name);
		[System.Runtime.InteropServices.DllImport("CityPayLib.dll")]
		private static extern int SetupCOMPort(byte[] BaudRate);
		[System.Runtime.InteropServices.DllImport("CityPayLib.dll")]
		private static extern int SendText(byte[] Command);
		[System.Runtime.InteropServices.DllImport("CityPayLib.dll")]
		private static extern int send_text1x2_price(byte[] Command, byte[] Command2);
		[System.Runtime.InteropServices.DllImport("CityPayLib.dll")]
		private static extern int CloseCOMPort();
		#endregion

		private string _errors;
		private string _portName;
		private int _baudRate;
		public string Errors
		{
			get
			{
				return _errors;
			}
			set
			{
				_errors = value;
			}
		}
		public string PortName
		{
			get
			{
				return _portName;
			}
			set
			{
				_portName = value;
			}
		}
		public int BaudRate
		{
			get
			{
				return _baudRate;
			}
			set
			{
				_baudRate = value;
			}
		}

		private bool _isInitialized;
		public bool IsInitialized
		{
			get
			{
				return _isInitialized;
			}
			set
			{
				_isInitialized = value;
			}
		}

		~CityPay()
		{
			CloseComPort();
		}

		/// <summary>
		/// Initialize Gates COM ports
		/// </summary>
		public bool InitializeComport(string ModuleName)
		{
			if (Constants.SoftwareMode == Mode.Use)
			{
				try
				{
					if (CloseComPort())
					{
						_isInitialized = false;
						_errors = "";
						if (_portName.Length > 4)
						{
							_portName = @"\\.\" + _portName;
						}
						byte[] command = Encoding.ASCII.GetBytes(_portName);
						try
						{
							int result = OpenCOMPort(command);

							if (result == 0)
							{
								_isInitialized = true;
								command = Encoding.ASCII.GetBytes(_baudRate.ToString());
								result = SetupCOMPort(command);
								if (result != 0)
								{
									_errors = "خطا در تنظیم نرخ ارسال پورت " + ModuleName;
									return false;
								}
							}
							else
							{
								_isInitialized = false;
								_errors = "خطا در باز کردن پورت " + ModuleName;
								return false;
							}
						}
						catch
						{
							_isInitialized = false;
							_errors = "خطا در باز کردن پورت ." + ModuleName;
							return false;
						}
						return true;
					}
				}
				catch
				{
					return false;
					// ignored
				}
			}
			return true;
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}

		private string SetLength(string StringValue, int Length)
		{
			if (Length < StringValue.Length)
			{
				StringValue = StringValue.Substring(Length);
			}
			else
			{
				for (int i = StringValue.Length; i < Length; i++)
				{
					StringValue = "0" + StringValue;
				}
			}
			return StringValue;
		}

		public bool SendPriceToCityPay(int PriceValue)
		{
			if (Constants.SoftwareMode == Mode.Use && _isInitialized)
			{
				try
				{
					string hexValue = "ba076a" + SetLength((PriceValue * 10).ToString("X"), 4) + "00006aaabb";
					byte[] command = StringToByteArray(hexValue);
					int result = SendText(command);
					return (result != 0);
				}
				catch
				{
					_isInitialized = false;
					_errors = "خطا در ارسال پیام به تابلو قیمت.";
					return false;
				}
			}
			return true;
		}

		public bool CloseComPort()
		{
			if (Constants.SoftwareMode == Mode.Use && _isInitialized)
			{
				try
				{
					if (_isInitialized)
					{
						int result = CloseCOMPort();
						_isInitialized = (result != 0);
					}
				}
				catch
				{
					_isInitialized = false;
				}
				return _isInitialized;
			}
			return true;
		}

		public bool CheckVerify(string PortNameStr, string BaudRateStr, string BoardName)
		{
			_baudRate = Convert.ToInt32(BaudRateStr);
			_portName = PortNameStr;
			return InitializeComport(BoardName);
		}
	}
}
