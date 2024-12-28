using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieParkUI_ParkManagement.Model
{
    public class ReservatieInfoKlantId
    {
        public ReservatieInfoKlantId(string parkName, string parkLocation, DateTime checkIn, DateTime checkOut, int huisnummer)
        {
            ParkName = parkName;
            ParkLocation = parkLocation;
            CheckIn = checkIn;
            CheckOut = checkOut;
            HuisNummer = huisnummer;
        }

        public string ParkName { get; private set; }
        public string ParkLocation { get; private set; }
        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public int HuisNummer { get; private set; }
    }
}
