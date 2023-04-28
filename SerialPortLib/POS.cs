using System;
using System.Text;

namespace SerialPortLib
{
	using ToolsLib;

	public class POS
	{
		#region import
		[System.Runtime.InteropServices.DllImport("PosLib.dll")]
		private static extern int OpenCOMPort(byte[] Name);
		[System.Runtime.InteropServices.DllImport("PosLib.dll")]
		private static extern int SetupCOMPort(byte[] BaudRate);
		[System.Runtime.InteropServices.DllImport("PosLib.dll")]
		private static extern int SendText(byte[] Command);
		[System.Runtime.InteropServices.DllImport("PosLib.dll")]
		private static extern int send_text1x2_price(byte[] Command, byte[] Command2);
		[System.Runtime.InteropServices.DllImport("PosLib.dll")]
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

		~POS()
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

		public bool SendPriceToPos(int PriceValue)
		{
			if (Constants.SoftwareMode == Mode.Use && _isInitialized)
			{
				try
				{
					byte[] command = Encoding.ASCII.GetBytes("P");
					byte[] command2 = Encoding.ASCII.GetBytes(PriceValue < 0 ? "VIP" : PriceValue.ToString());
					int result = send_text1x2_price(command, command2);
					return (result != 0);
				}
				catch
				{
					_isInitialized = false;
					_errors = "خطا در ارسال پیام به دستگاه POS.";
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
