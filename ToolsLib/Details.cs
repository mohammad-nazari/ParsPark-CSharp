using System;

namespace ToolsLib
{
    [Serializable]
    public class Detailes
    {
        private string _serialNumber;
        private string _enterLicensePlate;
        private string _exitLicensePlate;
        private string _enterTime;
        private string _exitTime;
        private string _transType;
        private long _rowId;
        private string _cost;
        private string _duration;
        private string _print;
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
            }
        }
        public string EnterLicensePlate
        {
            get
            {
                return _enterLicensePlate;
            }
            set
            {
                _enterLicensePlate = value;
            }
        }
        public string ExitLicensePlate
        {
            get
            {
                return _exitLicensePlate;
            }
            set
            {
                _exitLicensePlate = value;
            }
        }
        public string EnterTime
        {
            get
            {
                return _enterTime;
            }
            set
            {
                _enterTime = value;
            }
        }
        public string TransType
        {
            get
            {
                return _transType;
            }
            set
            {
                _transType = value;
            }
        }
        public long RowId
        {
            get
            {
                return _rowId;
            }
            set
            {
                _rowId = value;
            }
        }
        public string Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }
        public string Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }
        public string ExitTime
        {
            get
            {
                return _exitTime;
            }
            set
            {
                _exitTime = value;
            }
        }
        public string Print
        {
            get
            {
                return _print;
            }
            set
            {
                _print = value;
            }
        }
    }
}
