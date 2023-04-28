using System;
using System.Text;

namespace SerialPortLib
{
	public class PriceLed
	{
		private string _errors;
		private string _portName;
		private int _baudRate;

		SerialComPort _serialComPort = new SerialComPort();

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

		~PriceLed()
		{
			CloseComPort();
		}

		/// <summary>
		/// Initialize Gates COM ports
		/// </summary>
		public bool InitializeComport(string ModuleName)
		{
			try
			{
				if (CloseComPort())
				{
					_isInitialized = false;
					_errors = "";
					if (_portName.Length > 4)
					{
						//_portName = @"\\.\" + _portName;
					}
					_serialComPort = new SerialComPort
					{
						PortName = _portName,
						BaudRate = _baudRate
					};

					if (!_serialComPort.Open())
					{
						_isInitialized = false;
						_errors = "خطا در باز کردن پورت " + ModuleName;
						return false;
					}
					_isInitialized = true;
					return true;
				}
			}
			catch
			{
				return false;
				// ignored
			}
			return true;
		}

		public bool SendPriceToLedBoard(string PriceValue)
		{
			try
			{
				if (!_serialComPort.Send(Encoding.ASCII.GetBytes(@"P" + PriceValue + @"#")))
				{
					_isInitialized = false;
					_errors = "خطا در ارسال پیام به تابلو قیمت.";
					return false;
				}
			}
			catch (Exception)
			{
				_errors = "خطا در ارسال پیام به تابلو قیمت.";
				return false;
			}
			return true;
		}

		public bool CloseComPort()
		{
			try
			{
				_serialComPort.Close();
				_isInitialized = true;
			}
			catch
			{
				_errors = "خطا در بستن پورت تابلو قیمت.";
				_isInitialized = false;
			}
			return _isInitialized;
		}

		public bool CheckVerify(string PortNameStr, string BaudRateStr, string BoardName)
		{
			_baudRate = Convert.ToInt32(BaudRateStr);
			_portName = PortNameStr;
			return InitializeComport(BoardName);
		}
	}
}
