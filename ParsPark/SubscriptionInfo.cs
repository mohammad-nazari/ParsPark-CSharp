using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseLib;
using ToolsLib;

namespace ParsPark
{
	class SubscriptionInfo
	{
		public OrganSubInfo OrganInfo { set; get; }

		public bool Result { set; get; }

		public string OrganInformation { set; get; }

		public void ResetData()
		{
			OrganInfo = new OrganSubInfo();
		}

		public long GetSubscriptionInfo(string LastCardSerialNumber, string CarPlateNumber)
		{
			OrganInformation = "عادی";
			DateTime enterTime = DateTime.Now;
			try
			{
				parsparkoEntities parsPark = new parsparkoEntities(GlobalVariables.ConnectionString);
				var subInfo = parsPark.subscription.FirstOrDefault(t => t.code == LastCardSerialNumber && t.startdate <= enterTime && t.enddate >= enterTime);

				if (subInfo != null && subInfo.id > 0)
				{
					var carInfo = parsPark.car.Find(subInfo.drivercarid);

					if (carInfo != null && carInfo.id > 0)
					{
						var driverInfo = parsPark.driver.Find(carInfo.driverid);
						if (driverInfo != null && driverInfo.id > 0)
						{
							OrganInfo.Id = driverInfo.id;
							OrganInfo.Orgname = driverInfo.orgname;
							OrganInfo.Orgval = driverInfo.orgval;
							OrganInfo.Code = subInfo.code;

							OrganInformation = OrganInfo.Orgname + " (" + OrganInfo.Orgval + ")";

							return subInfo.id;
						}
					}
				}
			}
			catch
			{
				return 0;
			}
			return 0;
		}
	}
}
