using System;
using System.Drawing;

namespace ToolsLib
{
    [Serializable]
    public class RequestData
    {
        private Detailes _detailsObject;
        public Detailes DetailsObject
        {
            get { return _detailsObject; }
            set { _detailsObject = value; }
        }
        private Bitmap _pictureData;
        public Bitmap PictureData
        {
            get { return _pictureData; }
            set { _pictureData = value; }
        }
        private RequestType _reqType;
        public RequestType ReqType
        {
            get { return _reqType; }
            set { _reqType = value; }
        }
        private string _licensePlate;
        public string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }
		private string _testData;
		public string TestData
		{
			get { return _testData; }
			set { _testData = value; }
		}
	}
}
