using System;
using System.Text;
using System.IO.Ports;

namespace SerialPortLib
{
	public delegate void DataReceived(object sender, SerialPortEventArgs arg);

	public class SerialPortEventArgs : EventArgs
	{
		public string ReceivedData
		{
			get; private set;
		}
		public SerialPortEventArgs(string data)
		{
			ReceivedData = data;
		}
	}

	public class SerialComPort
	{
		/// <summary>
		/// Serial port class
		/// </summary>
		private readonly SerialPort _serialPort = new SerialPort();

		/// <summary>
		/// BaudRate set to default for Serial Port Class
		/// </summary>
		private int _baudRate = 9600;

		/// <summary>
		/// DataBits set to default for Serial Port Class
		/// </summary>
		private int _dataBits = 8;

		/// <summary>
		/// Handshake set to default for Serial Port Class
		/// </summary>
		private Handshake _handshake = Handshake.None;

		/// <summary>
		/// Parity set to default for Serial Port Class
		/// </summary>
		private Parity _parity = Parity.None;

		/// <summary>
		/// Communication Port name, not default in SerialPort. Defaulted to COM1
		/// </summary>
		private string _portName = "COM1";

		/// <summary>
		/// StopBits set to default for Serial Port Class
		/// </summary>
		private StopBits _stopBits = StopBits.One;

		/// <summary>
		/// Holds data received until we get a terminator.
		/// </summary>
		private string _tString = "";
		public string TString
		{
			get
			{
				return _tString;
			}
			set
			{
				_tString = value;
			}
		}

		/// <summary>
		/// Gets or sets BaudRate (Default: 9600)
		/// </summary>
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

		/// <summary>
		/// Gets or sets DataBits (Default: 8)
		/// </summary>
		public int DataBits
		{
			get
			{
				return _dataBits;
			}
			set
			{
				_dataBits = value;
			}
		}

		/// <summary>
		/// Gets or sets Handshake (Default: None)
		/// </summary>
		public Handshake Handshake
		{
			get
			{
				return _handshake;
			}
			set
			{
				_handshake = value;
			}
		}

		/// <summary>
		/// Gets or sets Parity (Default: None)
		/// </summary>
		public Parity Parity
		{
			get
			{
				return _parity;
			}
			set
			{
				_parity = value;
			}
		}

		/// <summary>
		/// Gets or sets PortName (Default: COM1)
		/// </summary>
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

		/// <summary>
		/// Gets or sets StopBits (Default: One}
		/// </summary>
		public StopBits StopBits
		{
			get
			{
				return _stopBits;
			}
			set
			{
				_stopBits = value;
			}
		}

		/// <summary>
		/// Sets the current settings for the Comport and tries to open it.
		/// </summary>
		/// <returns>True if successful, false otherwise</returns>
		public bool Open()
		{
			try
			{
				_serialPort.BaudRate = _baudRate;
				_serialPort.DataBits = _dataBits;
				_serialPort.Handshake = _handshake;
				_serialPort.Parity = _parity;
				_serialPort.PortName = _portName;
				_serialPort.StopBits = _stopBits;
				_serialPort.DataReceived += _serialPort_DataReceived;
			}
			catch
			{
				return false;
			}
			try
			{
				_serialPort.DtrEnable = true;
			}
			catch
			{
				// ignored
			}
			try
			{
				_serialPort.RtsEnable = true;
			}
			catch
			{
				// ignored
			}
			try
			{
				_serialPort.Open();
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool Send(byte[] data)
		{
			try
			{
				_serialPort.Write(data, 0, data.Length);
			}
			catch { return false; }
			return true;
		}

		public bool Send(string data)
		{
			try
			{
				_serialPort.Write(data);
			}
			catch { return false; }
			return true;
		}

		/// <summary>
		/// Handles DataReceived Event from SerialPort
		/// </summary>
		/// <param name="sender">Serial Port</param>
		/// <param name="e">Event arguments</param>
		private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			// Initialize a buffer to hold the received data
			byte[] buffer = new byte[_serialPort.ReadBufferSize];

			// There is no accurate method for checking how many bytes are read
			// unless you check the return from the Read method
			int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

			// For the example assume the data we are received is ASCII data.
			_tString += Encoding.ASCII.GetString(buffer, 0, bytesRead);

			// Check if string contains the terminator
		}

		public void Close()
		{
			_serialPort.DataReceived -= _serialPort_DataReceived;
			_serialPort.Close();
		}
	}
}
