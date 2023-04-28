using System;

namespace ToolsLib
{
	[Serializable]
	public class LicensePlate
	{
		public string LicenseNumber { get; set; } = "";
		public string LpNumber1 { get; set; } = "";
		public string LpAlpha { get; set; } = "";
		public string LpNumber2 { get; set; } = "";
		public string LpNumber3 { get; set; } = "";

		public LicensePlate()
		{
			Reset();
		}

		public void Reset()
		{
			LicenseNumber = "";
			LpNumber1 = "";
			LpAlpha = "";
			LpNumber2 = "";
			LpNumber3 = "";
		}

		public void InitializeResult(string LpResultFa)
		{
			Reset();
			string temp = !string.IsNullOrEmpty(LpResultFa) ? StringConvert.UnicodToAsciiNumber(LpResultFa) : "";
			if (!string.IsNullOrEmpty(temp))
			{
				int index = 0;
				foreach (char ch in temp)
				{
					if (index < 2)
					{
						if (char.IsNumber(ch))
						{
							LicenseNumber += ch;
							index++;
						}
						else
						{
							if (index == 0)
							{
								LicenseNumber += "  ";
							}
							else
							{
								LicenseNumber += " ";
							}
							LicenseNumber += ch;
							index = 3;
						}
					}
					else
					{
						if (index == 2)
						{
							if (!char.IsNumber(ch))
							{
								LicenseNumber += ch;
							}
							else
							{
								LicenseNumber += " ";
							}
						}
						else
						{
							if (char.IsNumber(ch))
							{
								LicenseNumber += ch;
							}
							else
							{
								LicenseNumber += " ";
							}
						}
						index++;
					}
				}
				for (int i = LicenseNumber.Length; i < 8; i++)
				{
					LicenseNumber += " ";
				}
			}

			if (!string.IsNullOrEmpty(LicenseNumber))
			{
				try
				{
					LpNumber1 = LicenseNumber.Length > 1 ? Convert.ToInt32(LicenseNumber.Substring(0, Math.Min(2, LicenseNumber.Length))).ToString() : "  ";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpAlpha = LicenseNumber.Length > 2 ? LicenseNumber.Substring(2, Math.Min(1, LicenseNumber.Length - 2)) : " ";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpNumber2 = LicenseNumber.Length > 3 ? Convert.ToInt32(LicenseNumber.Substring(3, Math.Min(3, LicenseNumber.Length - 3))).ToString() : "   ";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpNumber3 = LicenseNumber.Length > 6 ? Convert.ToInt32(LicenseNumber.Substring(6, Math.Min(2, LicenseNumber.Length - 6))).ToString() : "  ";
				}
				catch
				{
					//ignored
				}
			}
			LicenseNumber = LpNumber2 + LpNumber3 + LpAlpha + LpNumber1;
		}

		public void LoadLicense(string License)
		{
			LicenseNumber = License;
			if (!string.IsNullOrEmpty(LicenseNumber))
			{
				try
				{
					LpNumber1 = LicenseNumber.Length > 1 ? Convert.ToInt32(LicenseNumber.Substring(6, Math.Min(2, LicenseNumber.Length - 6))).ToString() : "";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpAlpha = LicenseNumber.Length > 2 ? LicenseNumber.Substring(5, Math.Min(1, LicenseNumber.Length - 5)) : "";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpNumber2 = LicenseNumber.Length > 3 ? Convert.ToInt32(LicenseNumber.Substring(0, Math.Min(3, LicenseNumber.Length))).ToString() : "";
				}
				catch
				{
					//ignored
				}
				try
				{
					LpNumber3 = LicenseNumber.Length > 6 ? Convert.ToInt32(LicenseNumber.Substring(3, Math.Min(2, LicenseNumber.Length - 3))).ToString() : "";
				}
				catch
				{
					//ignored
				}
			}
		}

		public string GetStandardString()
		{
			return LpNumber2 + LpNumber3 + LpAlpha + LpNumber1;
		}
	};
}
