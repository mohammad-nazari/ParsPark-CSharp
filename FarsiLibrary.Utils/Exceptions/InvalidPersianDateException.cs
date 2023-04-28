using System;

namespace FarsiLibrary.Utils.Exceptions
{
    public class InvalidPersianDateException : Exception
    {
        public InvalidPersianDateException()
            : base()
        {
        }

        public InvalidPersianDateException(string message)
            : base(message)
        {
        }
    }
}
