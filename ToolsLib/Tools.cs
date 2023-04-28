using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

// ReSharper disable RedundantLogicalConditionalExpressionOperand

namespace ToolsLib
{
	public class Tools : WebClient
	{
		/*
		 public parsparkoEntities(string ConnectionString)
			 : base("name=parsparkoEntities")
		{
			this.Database.Connection.ConnectionString = ConnectionString;
			this.Database.CommandTimeout = 5;
		}
		*/

		public static List<string> Splitter(string Delimiter, string Input)
		{
			string[] delimiterChars = { Delimiter };
			StringSplitOptions sso = StringSplitOptions.None;

			string[] words = Input.Split(delimiterChars, sso);

			List<string> list = new List<string>(words);

			return list;
		}

		public static int RandomInRange(int Front, int Rear)
		{
			Random r = new Random();
			if (Front > Rear)
			{
				int tmp = Rear;
				Rear = Front;
				Front = tmp;
			}
			int rInt = r.Next(Front, Rear);

			return rInt;
		}

		public static int CalculateParkingCost(TimeSpan Duration, DateTime EnterTime, DateTime ExitTime, DateTime DayStart, DateTime DayEnd)
		{
			int tempDayDuration = CalcHours(DayStart.Hour, DayEnd.Hour);
			int tempNightDuration = CalcHours(DayEnd.Hour, DayStart.Hour);

			int cost = Duration.Days * GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.ADay;

			if (CheckHourIsBetween(EnterTime.Hour, DayStart.Hour, DayEnd.Hour))
			{
				int tempHour = CalcHours(EnterTime.Hour, DayEnd.Hour);
				if (CheckHourIsBetween(ExitTime.Hour, DayEnd.Hour, DayStart.Hour))
				{
					// Hours is in day night
					cost += CostInPeriodTime(tempHour, 0, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
					cost += CostInPeriodTime(Duration.Hours - tempHour, Duration.Minutes, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
				}
				else
				{
					if (tempHour > Duration.Hours)
					{
						// Hours is in day day
						cost += CostInPeriodTime(Duration.Hours, Duration.Minutes, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
					}
					else
					{
						// Hours is in day night day
						cost += CostInPeriodTime(tempHour, Duration.Minutes, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
						cost += CostInPeriodTime(tempNightDuration, 0, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
						cost += CostInPeriodTime(Duration.Hours - tempHour - tempNightDuration, 0, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
					}
				}
			}
			else
			{
				int tempHour = CalcHours(EnterTime.Hour, DayStart.Hour);
				if (CheckHourIsBetween(ExitTime.Hour, DayStart.Hour, DayEnd.Hour))
				{
					// Hours is in night day
					cost += CostInPeriodTime(tempHour, 0, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
					cost += CostInPeriodTime(Duration.Hours - tempHour, Duration.Minutes, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
				}
				else
				{
					if (tempHour > Duration.Hours)
					{
						// Hours is in night night
						cost += CostInPeriodTime(Duration.Hours, Duration.Minutes, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
					}
					else
					{
						// Hours is in night day night
						cost += CostInPeriodTime(tempHour, Duration.Minutes, cost <= 0 ? GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.First : GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
						cost += CostInPeriodTime(tempDayDuration, 0, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Day.Next);
						cost += CostInPeriodTime(Duration.Hours - tempDayDuration - tempHour, 0, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next, GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Night.Next);
					}
				}
			}
			return cost;
		}

		private static int CostInPeriodTime(int Hours, int Minutes, int FirstHourCost, int NextHourCost)
		{
			if (Minutes > GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Last)
			{
				Minutes -= GlobalVariables.AllSettingsObject.SettingsObject.ParkingSetting.Tariff.Last;
			}
			else
			{
				Minutes = 0;
			}

			// Hours is in day day
			if (Minutes > 0)
			{
				return Hours * NextHourCost + FirstHourCost;
			}
			if (Hours > 0)
			{
				return (Hours - 1) * NextHourCost + FirstHourCost;
			}
			return 0;
		}

		private static int CalcHours(int StartHour, int EndHour)
		{
			return EndHour > StartHour ? EndHour - StartHour : 24 + EndHour - StartHour;
		}

		private static bool CheckHourIsBetween(int InputHour, int StartHour, int EndHour)
		{
			TimeSpan now = new TimeSpan(InputHour, 0, 0);
			TimeSpan start = new TimeSpan(StartHour, 0, 0);
			TimeSpan end = new TimeSpan(EndHour, 0, 0);

			// see if start comes before end
			if (start < end)
			{
				if (start <= now && now < end)
				{
					return true;
				}
			}
			else
			{
				// start is after end, so do the inverse comparison
				if (!(end <= now && now < start))
				{
					return true;
				}
			}
			return false;
		}

		public static bool AllowAnpr()
		{
			return (Constants.Softwareedition == SoftwareEdition.Ultimate);
		}

		public static bool AllowUsingAnpr()
		{
			return (Constants.Softwareedition == SoftwareEdition.Ultimate && GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.StartType == SettingANPRStartType.Auto);
		}

		public static bool IsServer()
		{
			return GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Allow == PermissionAllow.Yes;
		}

		/// <summary>
		/// Enabled just in Ultimate, Enterprise and Professional 
		/// </summary>
		/// <returns></returns>
		public static bool AllowCamera()
		{
			return (Constants.Softwareedition >= SoftwareEdition.Professional);
		}

		public static bool AllowEnterCamera()
		{
			return (Constants.Softwareedition >= SoftwareEdition.Professional && GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Enter.Allow == PermissionAllow.Yes);
		}

		public static bool AllowExitCamera()
		{
			return (Constants.Softwareedition >= SoftwareEdition.Professional && GlobalVariables.AllSettingsObject.SettingsObject.CameraSetting.Exit.Allow == PermissionAllow.Yes);
		}

		public static bool AllowBarrier()
		{
			return (Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise);
		}

		public static bool AllowBoard()
		{
			return (Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise);
		}

		public static bool AllowPayment()
		{
			return (Constants.Softwareedition >= SoftwareEdition.Enterprise);
		}

		public static bool AllowOpenEnterBarrier()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes);
		}

		public static bool AllowOpenEnterBarrierAuto()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Open.StartType == SettingANPRStartType.Auto);
		}

		public static bool AllowCloseEnterBarrier()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes);
		}

		public static bool AllowCloseEnterBarrierAuto()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Enter.Close.StartType == SettingANPRStartType.Auto);
		}

		public static bool AllowOpenExitBarrier()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes);
		}

		public static bool AllowOpenExitBarrierAuto()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Open.StartType == SettingANPRStartType.Auto);
		}

		public static bool AllowCloseExitBarrier()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes);
		}

		public static bool AllowCloseExitBarrierAuto()
		{
			return ((Constants.Softwareedition == SoftwareEdition.Standard || Constants.Softwareedition >= SoftwareEdition.Enterprise) && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.BarrierSetting.Exit.Close.StartType == SettingANPRStartType.Auto);
		}

		public static bool AllowSubmitLicense()
		{
			return GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes;
		}

		public static bool AllowSubmitLicenseManual()
		{
			return (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.StartType == SettingANPRStartType.Manual);
		}

		public static bool AllowCityPay()
		{
			return (AllowPayment() && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Allow);
		}

		public static bool AllowCityPayAuto()
		{
			return (AllowPayment() && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.Allow && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.CityPay.AutoSend);
		}

		public static bool AllowPos()
		{
			return (AllowPayment() && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Allow);
		}

		public static bool AllowPosAuto()
		{
			return (AllowPayment() && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.Allow && GlobalVariables.AllSettingsObject.SettingsObject.PaymentSetting.POS.AutoSend);
		}

		/// <summary>
		/// Is server or client
		/// </summary>
		/// <returns></returns>
		public static SoftwareType CheckSoftwareType()
		{
			return GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Server.Allow == PermissionAllow.No ? SoftwareType.Client : SoftwareType.Server;
		}

		/// <summary>
		/// Is in Enter or exit or both mode
		/// </summary>
		/// <returns></returns>
		public static SoftwareType CheckSoftwareMode()
		{
			if (GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Enter.Allow == PermissionAllow.Yes && GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Exit.Allow == PermissionAllow.Yes)
			{
				return SoftwareType.Both;
			}
			if (GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Enter.Allow == PermissionAllow.Yes)
			{
				return SoftwareType.Enter;
			}
			return GlobalVariables.AllSettingsObject.SettingsObject.SoftwareSetting.Exit.Allow == PermissionAllow.Yes ? SoftwareType.Exit : SoftwareType.Nothing;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest wr = base.GetWebRequest(address);
			if (wr != null)
			{
				wr.Timeout = 5000; // timeout in milliseconds (ms)
				return wr;
			}
			return null;
		}

		public static bool ExistUrl(string InputUrl)
		{
			//InputUrl = "www.google.com";
			// Ping's the local machine.
			Ping pingSender = new Ping();
			PingReply reply = pingSender.Send(InputUrl);

			return (reply != null && reply.Status == IPStatus.Success);

			// If the url does not contain Http. Add it.
			// if i want to also check for https how can i do.this code is only for http not https
			/*if (!InputUrl.Contains("http://"))
			{
				InputUrl = "http://" + InputUrl;
			}
			try
			{
				var request = WebRequest.Create(InputUrl) as HttpWebRequest;
				if (request != null)
				{
					request.Method = "HEAD";
					using (var response = (HttpWebResponse)request.GetResponse())
					{
						return response.StatusCode == HttpStatusCode.OK;
					}
				}
			}
			catch
			{
				return false;
			}
			return false;*/
		}

		public static bool AllowSubmitEnter()
		{
			return (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Enter.Allow == PermissionAllow.Yes);
		}
		public static bool AllowSubmitExit()
		{
			return (GlobalVariables.AllSettingsObject.SettingsObject.ANPRSetting.Exit.Allow == PermissionAllow.Yes);
		}
	}
}
