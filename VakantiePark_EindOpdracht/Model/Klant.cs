using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Klant
    {
        private int _id;
        private string _naam;
        private string _adres;

        public Klant(int iD, string naam, string adres)
        {
            ID = iD;
            Naam = naam;
            Adres = adres;
        }

        public List<Reservatie> Reservaties { get; private set; } = new List<Reservatie>();
        public int ID
        {
            get { return _id; }
            private set
            {
                _id = (value < 0) ? throw new ModelException("KlantID is negatief") : value;
            }
        }

        public string Naam
        {
            get { return _naam; }
            private set
            {
                _naam = String.IsNullOrWhiteSpace(value) ? throw new ModelException("Naam Klant is leeg") : value;
            }

        }

        public string Adres
        {
            get { return _adres; }
            private set
            {
                _adres = String.IsNullOrWhiteSpace(value) ? throw new ModelException("Adres Klant is leeg") : value;
            }
        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            try
            {
                if(!this.Reservaties.Contains(reservatie))
                {
                    Reservaties.Add(reservatie);
                }
                else
                {
                    throw new ModelException("Reservatie reeds toegevoegd");
                }
            }
            catch (Exception ex)
            {
                throw new ModelException("VoegReservatieToe", ex);
            }

        }
    }
}
