using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using WebServiceLib;
// ReSharper disable UnusedMember.Local

namespace ParsPark
{
	public class ActivationClass
	{
		private static readonly string AlphaNum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static readonly List<int> RandomGenerationCodeValues = new List<int> { 3, 5, 7, 9, 5, 1, 2, 6, 8, 4, 1, 9, 3, 7, 6, 4 }; // 16
		private static readonly List<int> RandomActivationCodeValues = new List<int> { 5, 4, 1, 7, 8, 5, 6, 6, 4, 8, 2, 3, 9, 5, 2, 6, 1, 3, 4, 7, 5, 3, 4, 7 }; // 24
		private static readonly List<int> RandomCardKeyValues = new List<int> { 5, 4, 1, 7, 8, 5, 6, 6, 4, 8, 2, 3, 9, 5, 2, 6, 1, 3, 4, 7, 5, 3, 4, 7, 4, 5, 7, 1, 1, 6, 9, 5 }; // 32
		private static readonly string AESPassword = "DgrPK2fqs5iuCbvEjHRe7bjyiHNFSYjv";
		private static readonly string AESIV = "L6PknN7WPzvDTGuOIEx3429Banx9ytYk";

		public string HardSerialNumber { get; set; } = "";

		public string ProcessorId { get; set; } = "";

		public List<int> HardSerialNumberValue { get; set; }

		public List<int> ProcessorIdValue { get; set; }

		public string SerialNumber { get; set; } = "";

		public string CardCode { get; set; } = "";

		public string GenerationCode { get; set; } = "";

		public string ActivationCode { get; set; } = "";

		public string CardKey { get; set; } = "";

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string RegistrySerialNumber { get; set; } = "";

		public string RegistryGenerationCode { get; set; } = "";

		public string RegistryActivationCode { get; set; } = "";

		public bool IsActivated { get; set; }

		public ActivationRequest ActivationData { get; set; } = new ActivationRequest();

		public ActivationResponse ActivationResult { get; set; } = new ActivationResponse();

		private String AES_encrypt(String Input)
		{
			var aes = new RijndaelManaged
			{
				KeySize = 256,
				BlockSize = 256,
				Padding = PaddingMode.PKCS7,
				Key = Encoding.ASCII.GetBytes(AESPassword),
				IV = Encoding.ASCII.GetBytes(AESIV)
			};

			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			String output = string.Empty;
			try
			{
				byte[] xBuff;
				using (var ms = new MemoryStream())
				{
					using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
					{
						byte[] xXml = Encoding.UTF8.GetBytes(Input);
						cs.Write(xXml, 0, xXml.Length);
					}

					xBuff = ms.ToArray();
				}

				output = Convert.ToBase64String(xBuff);
			}
			catch (Exception e)
			{
				ActivationResult.ActivationResult = false;
				ActivationResult.ActivationMessage = "خطا در رمزگذاری اطلاعات" + Environment.NewLine + e.Message;
			}
			return output;
		}

		private String AES_decrypt(String Input)
		{
			RijndaelManaged aes = new RijndaelManaged
			{
				KeySize = 256,
				BlockSize = 256,
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7,
				Key = Encoding.ASCII.GetBytes(AESPassword),
				IV = Encoding.ASCII.GetBytes(AESIV)
			};

			var decrypt = aes.CreateDecryptor();
			String output = string.Empty;
			try
			{
				byte[] xBuff;
				using (var ms = new MemoryStream())
				{
					using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
					{
						byte[] xXml = Convert.FromBase64String(Input);
						cs.Write(xXml, 0, xXml.Length);
					}

					xBuff = ms.ToArray();
				}

				output = Encoding.UTF8.GetString(xBuff);
			}
			catch (Exception e)
			{
				ActivationResult.ActivationResult = false;
				ActivationResult.ActivationMessage = "خطا در رمزگشایی اطلاعات" + Environment.NewLine + e.Message;
			}
			return output;
		}

		/// <summary>
		/// Generate generation code from hard serial number and processor id
		/// </summary>
		/// <returns></returns>
		public string GenerateGenerationCode()
		{
			GetProcessorId();
			GetHardSerialNumber();

			// Generation code with 16 character length
			GenerationCode = CalculateString(ProcessorId, HardSerialNumber, RandomGenerationCodeValues);
			ActivationData.GenerationCode = GenerationCode;

			return GenerationCode;
		}

		/// <summary>
		/// Generate activation code from generated code and serial number
		/// </summary>
		/// <returns></returns>
		public string GenerateActivationCode()
		{
			// Generation code with 16 character length
			ActivationCode = CalculateString(SerialNumber, GenerationCode, RandomActivationCodeValues);

			return ActivationCode;
		}

		/// <summary>
		/// Generate activation code from generated code and serial number
		/// </summary>
		/// <returns></returns>
		public string GenerateCardKey()
		{
			// Generation code with 16 character length
			CardKey = CalculateString(CardCode, CardKey, RandomCardKeyValues);

			return CardKey;
		}

		/// <summary>
		/// Get hard serial number from system
		/// </summary>
		/// <returns></returns>
		private void GetHardSerialNumber()
		{
			PhysicalDisk hardDisk = GetPhysicalDiskFromCurrentDrive();

			HardSerialNumber = hardDisk.SerialNumber.Trim();
		}

		/// <summary>
		/// Convert hard serial number string to list of int values
		/// </summary>
		private void GenerateHardSerialNumber()
		{
			HardSerialNumberValue = GenerateAsciiCodeList(HardSerialNumber, 16);
		}

		/// <summary>
		/// Get process id info from system
		/// </summary>
		/// <returns></returns>
		private void GetProcessorId()
		{
			ManagementClass mc = new ManagementClass("win32_processor");
			ManagementObjectCollection moc = mc.GetInstances();

			foreach (var o in moc)
			{
				var mo = (ManagementObject)o;
				ProcessorId = mo.Properties["processorID"].Value.ToString();
				break;
			}
			/*string sQuery = "SELECT ProcessorId FROM Win32_Processor";
			ManagementObjectSearcher oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);
			ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();
			foreach (var o in oCollection)
			{
				var oManagementObject = (ManagementObject) o;
				ProcessorId = (string)oManagementObject["ProcessorId"];
			}*/

			ProcessorId = ProcessorId.Trim();
		}

		/// <summary>
		/// Convert processor id string to list of int values
		/// </summary>
		private void GenerateProcessorId()
		{
			ProcessorIdValue = GenerateAsciiCodeList(ProcessorId, 16);
		}

		/// <summary>
		/// Read serial number from registry
		/// </summary>
		/// <returns></returns>
		public string GetRegistrySerialNumber()
		{
			RegistrySerialNumber = GetWindowsRegistryValue("SerialNumber");
			if (RegistrySerialNumber.Length != 20)
			{
				RegistrySerialNumber = string.Empty;
				SerialNumber = string.Empty;
			}
			SerialNumber = RegistrySerialNumber;
			return RegistrySerialNumber;
		}

		/// <summary>
		/// Read generated code from registry
		/// </summary>
		/// <returns></returns>
		public string GetRegistryGenerationCode()
		{
			RegistryGenerationCode = GetWindowsRegistryValue("GenerationCode");
			if (RegistryGenerationCode.Length != 16)
			{
				RegistryGenerationCode = string.Empty;
			}
			return RegistryGenerationCode;
		}

		/// <summary>
		/// Read activation code from registry
		/// </summary>
		/// <returns></returns>
		public string GetRegistryActivationCode()
		{
			string tmpStr = GetWindowsRegistryValue("ActivationCode");
			tmpStr = AES_decrypt(tmpStr.Trim());
			if (tmpStr.Length != 62)
			{
				RegistryActivationCode = string.Empty;
			}
			else
			{
				RegistryActivationCode = tmpStr.Substring(0, 24);
				StartDate = Convert.ToDateTime(tmpStr.Substring(24, 19));
				EndDate = Convert.ToDateTime(tmpStr.Substring(43, 19));
			}
			return RegistryActivationCode;
		}

		public string GetActivationCodeFromString(string Input)
		{
			string tmpStr = AES_decrypt(Input.Trim());
			if (tmpStr.Length != 62)
			{
				return string.Empty;
			}
			StartDate = Convert.ToDateTime(tmpStr.Substring(24, 19));
			EndDate = Convert.ToDateTime(tmpStr.Substring(43, 19));
			return tmpStr.Substring(0, 24);
		}

		/// <summary>
		/// Generate an ASCII code(int) list from characters in string
		/// with specific length
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="Length"></param>
		/// <returns></returns>
		private List<int> GenerateAsciiCodeList(string Input, int Length)
		{
			// Convert the string into a int list.
			List<int> asciiCodes = new List<int>();
			foreach (char c in Input)
			{
				asciiCodes.Add(Convert.ToInt32(c));
			}
			if (asciiCodes.Count < Length)
			{
				for (int i = asciiCodes.Count; i < Length; i++)
				{
					int charsSum = 0;
					foreach (int t in asciiCodes)
					{
						charsSum += (t % 1000);
					}
					asciiCodes.Add(charsSum);
				}
			}
			else if (asciiCodes.Count > Length)
			{
				asciiCodes = asciiCodes.GetRange(0, Length);
			}

			for (int i = 0; i < asciiCodes.Count; i++)
			{
				int charsSum = 0;
				for (int j = 0; j <= i; j++)
				{
					charsSum += (asciiCodes[j] % 1000);
				}
				asciiCodes[i] = charsSum;
			}
			return asciiCodes;
		}

		/// <summary>
		/// Generate a key from to string and list of values
		/// with a specific length
		/// </summary>
		/// <param name="StringStream1"></param>
		/// <param name="StringStream2"></param>
		/// <param name="RandomValues"></param>
		/// <returns></returns>
		private string CalculateString(string StringStream1, string StringStream2, List<int> RandomValues)
		{
			int length = RandomValues.Count;
			string tmpStr = string.Empty;
			List<int> tmpList1 = GenerateAsciiCodeList(StringStream1, length);
			List<int> tmpList2 = GenerateAsciiCodeList(StringStream2, length);

			for (int i = 0; i < length; i++)
			{
				var randomIndex = ((tmpList1[i] + tmpList2[i]) + ((tmpList1[i] + tmpList2[i]) * RandomValues[i])) % AlphaNum.Length;
				tmpStr += AlphaNum[randomIndex];
			}
			return tmpStr;
		}

		/// <summary>
		/// Read registered values from windows register key
		/// </summary>
		/// <param name="ValueName"></param>
		/// <returns></returns>
		private string GetWindowsRegistryValue(string ValueName)
		{
			string tempKeyValue = string.Empty;
			var tempKey = Registry.LocalMachine.OpenSubKey(ToolsLib.Constants.RegistryKeyAddress);
			if (tempKey != null)
			{
				RegistryValueKind rvk = tempKey.GetValueKind(ValueName);
				if (rvk == RegistryValueKind.String)
				{
					tempKeyValue = (string)tempKey.GetValue(ValueName);
				}
				tempKey.Close();
			}
			return tempKeyValue;
		}

		/// <summary>
		/// Set value and check it is OK
		/// </summary>
		/// <param name="ValueName"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		public bool SetWindowsRegistryValue(string ValueName, string Value)
		{
			RegistryKey tempKey = Registry.LocalMachine.CreateSubKey(ToolsLib.Constants.RegistryKeyAddress);
			if (tempKey != null)
			{
				tempKey.SetValue(ValueName, Value, RegistryValueKind.String);
				RegistryValueKind rvk = tempKey.GetValueKind(ValueName);
				if (rvk == RegistryValueKind.String)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get all informations from registry
		/// And generate needed info
		/// and compare them
		/// </summary>
		/// <returns></returns>
		public bool IsSoftwareActivated()
		{
			if (GetRegistrySerialNumber() != "")
			{
				if (GetRegistryGenerationCode() == GenerateGenerationCode() && RegistryGenerationCode != "" && GenerationCode != "")
				{
					if (GetRegistryActivationCode() == GenerateActivationCode() && RegistryActivationCode != "" && ActivationCode != "")
					{
						return IsDateTimeBetween();
					}
				}
			}

			return false;
		}

		public bool IsDateTimeBetween()
		{
			DateTime nowDateTime = DateTime.Now;
			var currentCalendar = CultureInfo.CurrentCulture.Calendar;
			try
			{
				nowDateTime = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, nowDateTime.Second, currentCalendar);
			}
			catch
			{
				// For hijri
				nowDateTime = nowDateTime.AddDays(-2);
				nowDateTime = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, nowDateTime.Second, currentCalendar);
			}

			if (StartDate <= nowDateTime && EndDate >= nowDateTime)
			{
				return true;
			}
			return false;
		}

		public void DoOnlineActivation()
		{
			ActivationWebService activationService = new ActivationWebService { Timeout = 10000 };
			// Assign web service server address
			//activationService.Url = "http://www.anshanrayanesh.ir/products/parspark/activation.php";
			//activationService.Url = "http://localhost:88/web/Anshan%20Rayanesh/Website/products/activation/activation.php";
			// Web service timeout
			IsActivated = false;
			try
			{
				ActivationResult = activationService.Activate(ActivationData);
			}
			catch (Exception e)
			{
				ActivationResult.ActivationResult = false;
				ActivationResult.ActivationMessage = "خطا در برقراری ارتباط با سرور" + Environment.NewLine + e.Message;
			}
		}

		public void WriteInfoToWindowsRegistry()
		{
			SetWindowsRegistryValue("SerialNumber", ActivationResult.ActivationInfo.SerialNumber);
			SetWindowsRegistryValue("GenerationCode", ActivationResult.ActivationInfo.GenerationCode);
			SetWindowsRegistryValue("ActivationCode", ActivationResult.ActivationInfo.ActivationCode);
			SetWindowsRegistryValue("User", ActivationResult.ActivationInfo.User);
			SetWindowsRegistryValue("Company", ActivationResult.ActivationInfo.Company);
			SetWindowsRegistryValue("Email", ActivationResult.ActivationInfo.Email);
		}

		public void ReadInfoFromWindowsRegistry()
		{
			ActivationData.SerialNumber = GetWindowsRegistryValue("SerialNumber");
			ActivationData.GenerationCode = GetWindowsRegistryValue("GenerationCode");
			ActivationData.ActivationCode = GetWindowsRegistryValue("ActivationCode");
			ActivationData.User = GetWindowsRegistryValue("User");
			ActivationData.Company = GetWindowsRegistryValue("Company");
			ActivationData.Email = GetWindowsRegistryValue("Email");
		}

		public void EncryptionData()
		{
			ActivationData.SerialNumber = AES_encrypt(ActivationData.SerialNumber);
			ActivationData.GenerationCode = AES_encrypt(ActivationData.GenerationCode);
			ActivationData.ActivationCode = AES_encrypt(ActivationData.ActivationCode);
			ActivationData.User = AES_encrypt(ActivationData.User);
			ActivationData.Company = AES_encrypt(ActivationData.Company);
			ActivationData.Email = AES_encrypt(ActivationData.Email);
			ActivationData.Version = AES_encrypt(ActivationData.Version);
			ActivationData.Software = AES_encrypt(ActivationData.Software);
		}

		public void DecryptionData()
		{
			ActivationResult.ActivationInfo.SerialNumber = AES_decrypt(ActivationResult.ActivationInfo.SerialNumber);
			ActivationResult.ActivationInfo.GenerationCode = AES_decrypt(ActivationResult.ActivationInfo.GenerationCode);
			//this._activationResult.ActivationInfo.ActivationCode = this.AES_decrypt(this._activationResult.ActivationInfo.ActivationCode);
			ActivationResult.ActivationInfo.User = AES_decrypt(ActivationResult.ActivationInfo.User);
			ActivationResult.ActivationInfo.Company = AES_decrypt(ActivationResult.ActivationInfo.Company);
			ActivationResult.ActivationInfo.Email = AES_decrypt(ActivationResult.ActivationInfo.Email);
			ActivationResult.ActivationInfo.Version = AES_decrypt(ActivationResult.ActivationInfo.Version);
			ActivationResult.ActivationInfo.Software = AES_decrypt(ActivationResult.ActivationInfo.Software);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="mountPoint"></param>
		/// <param name="name"></param>
		/// <param name="bufferLength"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean GetVolumeNameForVolumeMountPoint(String mountPoint, StringBuilder name, UInt32 bufferLength);

		private enum FileAccess : uint
		{
			GenericReadWrite = 0x80000000 | 0x40000000 // GENERIC_READ | GENERIC_WRITE
		}

		private enum FileShare : uint
		{
			ReadWriteDelete = 0x00000001 | 0x00000002 | 0x00000004 // FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE
		}

		private enum FileCreation : uint
		{
			OpenExisting = 3 // OPEN_EXISTING
		}

		private enum FileFlags : uint
		{
			None = 0
		}

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CreateFile(String fileName, FileAccess access, FileShare share, IntPtr secAttr,
			FileCreation creation, FileFlags flags, IntPtr templateFile);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean CloseHandle(IntPtr handle);

		private enum IoControlCode
		{
			GetVolumeDiskExtents = 0x00560000 // IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct VolumeDiskExtents
		{
			[FieldOffset(0)]
			private readonly uint numberOfDiskExtents;
			[FieldOffset(8)]
			public readonly uint diskNumber;
			[FieldOffset(16)]
			private readonly long startingOffset;
			[FieldOffset(24)]
			private readonly long extentLength;
		}

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean DeviceIoControl(IntPtr device, IoControlCode controlCode, IntPtr inBuffer, UInt32 inBufferSize,
			ref VolumeDiskExtents extents, UInt32 outBufferSize, ref UInt32 bytesReturned, IntPtr overlapped);

		public class PhysicalDisk
		{
			public PhysicalDisk(String physicalName, String model, String interfaceType, String serialNumber)
			{
				PhysicalName = physicalName;
				Model = model;
				InterfaceType = interfaceType;
				SerialNumber = serialNumber;
			}
			public String PhysicalName { get; private set; }
			public String Model { get; private set; }
			public String InterfaceType { get; private set; }
			public String SerialNumber { get; set; }
		}
		public PhysicalDisk GetPhysicalDiskFromCurrentDrive()
		{
			//
			// Get the drive letter of the drive the executable was loaded from.
			//
			var basePath = Path.GetPathRoot(Environment.SystemDirectory); //System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
			var driveLetter = Path.GetPathRoot(basePath);

			//
			// Get the volume name of the drive letter.
			//
			var volumeNameBuffer = new StringBuilder(65536);
			if (!GetVolumeNameForVolumeMountPoint(driveLetter, volumeNameBuffer, (UInt32)volumeNameBuffer.Capacity))
				throw new Win32Exception();
			var volumeName = volumeNameBuffer.ToString().TrimEnd('\\'); // Remove trailing backslash

			//
			// Open the volume and retrieve the disk number.
			//
			var volume = CreateFile(volumeName, FileAccess.GenericReadWrite, FileShare.ReadWriteDelete, IntPtr.Zero,
				FileCreation.OpenExisting, FileFlags.None, IntPtr.Zero);
			if (volume == (IntPtr)(-1)) // INVALID_HANDLE_VALUE
			{
				throw new Win32Exception();
			}

			VolumeDiskExtents extents = new VolumeDiskExtents();
			UInt32 bytesReturned = 0;
			if (!DeviceIoControl(volume, IoControlCode.GetVolumeDiskExtents, IntPtr.Zero, 0,
				ref extents, (UInt32)Marshal.SizeOf(extents), ref bytesReturned, IntPtr.Zero))
			{
				// Partitions can span more than one disk, we will ignore this case for now.
				// See http://msdn.microsoft.com/en-us/library/windows/desktop/aa365727(v=vs.85).aspx
				if (Marshal.GetLastWin32Error() != 234 /*ERROR_MORE_DATA*/)
					throw new Win32Exception();
			}

			var diskNumber = extents.diskNumber;

			//
			// Build the physical disk name from the disk number.
			//
			String physicalName = ("\\\\.\\PHYSICALDRIVE" + diskNumber).Replace("\\", "\\\\");

			//
			// Find information about the physical disk using WMI.
			//
			var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE DeviceID = \"" + physicalName + "\"");
			foreach (var o in searcher.Get())
			{
				var obj = (ManagementObject)o;
				return new PhysicalDisk(
					obj["DeviceID"].ToString(),
					obj["Model"].ToString(),
					obj["InterfaceType"].ToString(),
					obj["SerialNumber"].ToString()
					);
			}

			throw new InvalidOperationException();
		}

	}
}
