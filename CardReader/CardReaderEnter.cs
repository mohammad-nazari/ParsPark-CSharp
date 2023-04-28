using System;
using System.Runtime.InteropServices;
using System.Threading;
using ToolsLib;
using CameraLib;
using DataBaseLib;
using SerialPortLib;
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace CardReaderEnterLib
{
	public class CardReaderEnter
	{
		#region RFID
		[DllImport("MF_API.dll")]
		private static extern unsafe int MF_GetDLL_Ver(char* ver);
		[DllImport("MF_API.dll")]
		private static extern int MF_InitComm(IntPtr portname, uint baud);//打开与读写器相联的端口
		[DllImport("MF_API.dll")]
		private static extern int MF_ExitComm();         //关闭与读写器相联的端口

		[DllImport("MF_API.dll")]
		private static extern int MF_SetRecvTimeout(uint timeout);
		[DllImport("MF_API.dll")]
		private static extern int MF_GeneralCMD(int DeviceAddr, byte[] cData, byte cLen, byte[] rData, byte[] rLen);

		[DllImport("MF_API.dll")]
		private static extern unsafe int MF_GetDevice_Ver(int DeviceAddr, char* ver);
		[DllImport("MF_API.dll")]
		private static extern int MF_ControlLED(int DeviceAddr, byte led1, byte led2);
		[DllImport("MF_API.dll")]
		private static extern int MF_ControlBuzzer(int DeviceAddr, byte BeepTime);
		[DllImport("MF_API.dll")]
		private static extern int MF_SetDeviceBaud(int DeviceAddr, byte baud);
		[DllImport("MF_API.dll")]
		private static extern int MF_SetDeviceAddr(int DeviceAddr, byte addr);
		[DllImport("MF_API.dll")]
		private static extern int MF_GetDeviceAddr(int DeviceAddr, byte[] addr);        //!!!
		[DllImport("MF_API.dll")]
		private static extern int MF_SetDeviceSNR(int DeviceAddr, byte[] snr);
		[DllImport("MF_API.dll")]
		private static extern int MF_GetDeviceSNR(int DeviceAddr, byte[] snr);      // !!!
		[DllImport("MF_API.dll")]
		private static extern int MF_SetRF_ON(int DeviceAddr);
		[DllImport("MF_API.dll")]
		private static extern int MF_SetRF_OFF(int DeviceAddr);
		[DllImport("MF_API.dll")]
		private static extern int MF_DeviceReset(int DeviceAddr);
		[DllImport("MF_API.dll")]
		private static extern int MF_SetWiegandMode(int DeviceAddr, byte mode, byte alarm);

		// mode=0: request all, mode=1: request idle   return: cardtype (2bytes);
		[DllImport("MF_API.dll")]
		private static extern int MF_Request(int DeviceAddr, byte mode, byte[] CardType);
		// return: snr (4 bytes)
		[DllImport("MF_API.dll")]
		private static extern int MF_Anticoll(int DeviceAddr, byte[] snr);
		// input: snr(4 bytes)
		[DllImport("MF_API.dll")]
		private static extern int MF_Select(int DeviceAddr, byte[] snr);
		[DllImport("MF_API.dll")]
		private static extern int MF_Halt(int DeviceAddr);
		// input: key (6 bytes)
		[DllImport("MF_API.dll")]
		private static extern int MF_LoadKey(int DeviceAddr, byte[] key);
		// KeyType: 0=KeyA, 1=KeyB 	KeyNum: 0~15
		[DllImport("MF_API.dll")]
		private static extern int MF_LoadKeyFromEE(int DeviceAddr, byte KeyType, byte KeyNum);
		// AuthType: 0=KeyA, 1=KeyB;
		[DllImport("MF_API.dll")]
		private static extern int MF_Authentication(int DeviceAddr, byte AuthType, byte block, byte[] snr);
		[DllImport("MF_API.dll")]
		private static extern int MF_Read(int DeviceAddr, byte block, byte numbers, byte[] databuff);
		[DllImport("MF_API.dll")]
		private static extern int MF_Write(int DeviceAddr, byte block, byte numbers, byte[] databuff);
		// ValOption:0=dec, 1=inc, 2=restore;  block:0~15;  value (4 byte)
		[DllImport("MF_API.dll")]
		private static extern int MF_Value(int DeviceAddr, byte ValOption, byte block, byte[] value);
		// block:0~15;
		[DllImport("MF_API.dll")]
		private static extern int MF_Transfer(int DeviceAddr, byte block);
		// KeyAB: 0=KeyA,1=KeyB; KeyAddr:0~15; key(6 bytes);
		[DllImport("MF_API.dll")]
		private static extern int MF_StoreKeyToEE(int DeviceAddr, byte keyAb, byte KeyAddr, byte[] key);

		public const int PICC_AUTHENT1A = 0x60;         //!< authentication using key A
		public const int PICC_AUTHENT1B = 0x61;     //!< authentication using key B

		// error code define
		public const int MI_OK = 0x00;
		public const int MI_NOTAGERR = 0x01;
		public const int MI_COLLERR = 0x02;
		public const int MI_BITERR = 0x03;
		public const int MI_SAKERR = 0x04;
		public const int MI_AUTHERR = 0x05;
		public const int MI_Value = 0x0D;
		public const int MI_ACCESSERR = 0x0E;
		public const int MI_ACCESSTIMEOUT = 0x0F;

		public const int MI_CommandErr = 0x10;
		public const int MI_OtherErr = 0x11;

		public const int Code_RetFrameErr = 0x20;
		public const int Code_TimeoutErr = 0x21;
		public const int Code_SetCommPortErr = 0x22;
		#endregion RFID

		public Camera CameraObject { get; set; } = new Camera()
		{
			ImageAddress = "ImageIn.jpg"
		};

		public enterlogs LogsObject { get; set; } = new enterlogs();

		public string LastError { get; set; }

		~CardReaderEnter()
		{
			try
			{
				MF_ExitComm();
			}
			catch
			{
				// ignored
			}
		}

		public bool IsInitialized { get; set; }

		public SerialComPort SerialPort { get; set; } = new SerialComPort();

		public string LicensePlate { get; set; }

		public int CardReaderAddress { get; set; } = 0;

		public string OldCardSerialNumber { get; set; } = "";

		public string LastCardSerialNumber { get; set; } = "";

		public long LastCardNumber { get; set; } = 0;

		public void OKAlarm(int DeviceAddress)
		{
			if (IsInitialized)
			{
				MF_ControlBuzzer(DeviceAddress, 10);
				MF_ControlLED(DeviceAddress, 1, 0);
				Thread.Sleep(100);
				MF_ControlBuzzer(DeviceAddress, 10);
				Thread.Sleep(300);
				MF_ControlLED(DeviceAddress, 0, 0);
			}
		}

		public void InfoAlarm(int DeviceAddress)
		{
			if (IsInitialized)
			{
				MF_ControlBuzzer(DeviceAddress, 10);
				MF_ControlLED(DeviceAddress, 1, 0);
				Thread.Sleep(200);
				MF_ControlLED(DeviceAddress, 0, 0);
			}
		}

		public void ErrorAlarm(int DeviceAddress)
		{
			if (IsInitialized)
			{
				MF_ControlBuzzer(DeviceAddress, 80);
				MF_ControlLED(DeviceAddress, 0, 1);
				Thread.Sleep(1000);
				MF_ControlLED(DeviceAddress, 0, 0);
			}
		}

		public void ErrorAlarm2(int DeviceAddress)
		{
			if (IsInitialized)
			{
				//MF_ControlBuzzer(DeviceAddress, 30);
				MF_ControlLED(DeviceAddress, 0, 1);
				Thread.Sleep(300);
				MF_ControlLED(DeviceAddress, 0, 0);
				Thread.Sleep(300);
				//MF_ControlBuzzer(DeviceAddress, 30);
				MF_ControlLED(DeviceAddress, 0, 1);
				Thread.Sleep(300);
				MF_ControlLED(DeviceAddress, 0, 0);
				Thread.Sleep(300);
				//MF_ControlBuzzer(DeviceAddress, 30);
				MF_ControlLED(DeviceAddress, 0, 1);
				Thread.Sleep(300);
				MF_ControlLED(DeviceAddress, 0, 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="CardReaderAddressValue"></param>
		public void TestCardReader(int CardReaderAddressValue)
		{
			if (IsInitialized)
			{
				MF_ControlBuzzer(CardReaderAddressValue, 100);
				//MF_ControlLED(CardReaderAddress, 1, 0);
				//Thread.Sleep(500);
				//MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddressValue, 1, 1);
				Thread.Sleep(500);
				/*MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddress, 0, 0);
				Thread.Sleep(1000);
				MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddress, 1, 1);
				Thread.Sleep(1000);
				MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddress, 0, 0);
				Thread.Sleep(1000);
				MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddress, 1, 1);
				Thread.Sleep(1000);*/
				//MF_ControlBuzzer(CardReaderAddress, 50);
				MF_ControlLED(CardReaderAddressValue, 0, 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void InitializeCardReader()
		{
			IsInitialized = false;
			IntPtr comPort = Marshal.StringToCoTaskMemAnsi(@"\\.\" + SerialPort.PortName);

				{
					int result = MF_InitComm(comPort, (uint)SerialPort.BaudRate);
					if (result == 0)
					{
						IsInitialized = true;
						result = MF_SetDeviceAddr(0, Convert.ToByte(CardReaderAddress));
						if (result == 0)
						{
							TestCardReader(CardReaderAddress);
						}
					}
			}
		}

		/// <summary>
		/// Read card serial number
		/// </summary>
		public void ReadCardData()
		{
			if (IsInitialized)
			{
				byte[] cardType = new byte[2];
				byte[] cardSerialNumber = new byte[4];

				LastCardSerialNumber = "";

				byte mode = 0;
				int result = MF_Request(CardReaderAddress, mode, cardType);
				if (result == 0)
				{
					result = MF_Anticoll(CardReaderAddress, cardSerialNumber);
					if (result == 0)
					{
						LastCardSerialNumber = StringConvert.ByteArrayToString(cardSerialNumber);
					}
					/*do
                        {
                            result = MF_Request(this._cardReaderAddress, 0, cardType);
                        } while (result == 0);*/
				}
			}
		}

		/// <summary>
		/// Close COM port
		/// </summary>
		public int CloseCardReader()
		{
			if (IsInitialized)
			{
				try
				{
					return MF_ExitComm();
				}
				catch
				{
					return 1;
				}
			}
			return 0;
		}

		public void ResetValues()
		{
			CameraObject.License.Reset();
		}
	}
}
