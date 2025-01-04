using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieParkBL.DTO
{
    public class ReservatieInfo
    {
        public ReservatieInfo(string parkName, int klantId, string klantNaam, int huisNummer)
        {
            ParkNaam = parkName;
            KlantId = klantId;
            KlantNaam = klantNaam;
            HuisNummer = huisNummer;
        }

        public ReservatieInfo(int klantId, string klantNaam, DateTime checkIn, DateTime checkOut, int huisNummer)
        {
            KlantId = klantId;
            KlantNaam = klantNaam;
            CheckIn = checkIn;
            CheckOut = checkOut;
            HuisNummer = huisNummer;
        }
        public ReservatieInfo(int reservatieId, int klantId, string klantNaam, DateTime checkIn, DateTime checkOut)
        {
            KlantId = klantId;
            KlantNaam = klantNaam;
            CheckIn = checkIn;
            CheckOut = checkOut;
            ReservatieId = reservatieId;
        }
        public ReservatieInfo(string parkName, string parkLocation, DateTime checkIn, DateTime checkOut, int huisnummer)
        {
            ParkNaam = parkName;
            ParkLocation = parkLocation;
            CheckIn = checkIn;
            CheckOut = checkOut;
            HuisNummer = huisnummer;
        }

        public ReservatieInfo(int reservatieId, string parkNaam, int klantId, DateTime checkIn, DateTime checkOut, int huisNummer)
        {
            ParkNaam = parkNaam;
            KlantId = klantId;
            CheckIn = checkIn;
            CheckOut = checkOut;
            HuisNummer = huisNummer;
            ReservatieId = reservatieId;
        }

        public string ParkNaam { get; private set; }
        public string ParkLocation { get; private set; }
        public int KlantId { get; private set; }
        public string KlantNaam {  get; private set; }
        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public int HuisNummer { get; private set; }
        public int ReservatieId { get; private set; }
    }
}
