using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieParkBL.Exceptions
{
    public class ParkManagerException : Exception
    {
        public ParkManagerException(string? message) : base(message)
        {
        }

        public ParkManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
