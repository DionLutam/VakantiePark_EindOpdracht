using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Park
    {
        private int _id;
        private string _naam;
        private string _locatie;

        public Park(int id, string naam, string locatie)
        {
            id = Id;
            naam = Naam;
            locatie = Locatie;
        }
        public List<Faciliteit> Faciliteiten { get; private set; } = new List<Faciliteit>();
        public List<Huis> Huizen { get; private set; } = new List<Huis>();
        public int Id
        {
            get { return _id; }
            private set
            {
                if (value <= 0)
                {
                    throw new ModelException("Park_Id is kleiner of 0");
                }
                _id = value;
            }
        }

        public string Naam
        {
            get { return _naam; }
            private set
            {
                _naam = String.IsNullOrWhiteSpace(value) ?
                     throw new ModelException("Park naam is leeg") : value;
            }
        }

        public string Locatie
        {
            get { return _locatie; }
            private set
            {
                _locatie = String.IsNullOrWhiteSpace(value) ?
                    throw new ModelException("Park locatie is leeg") : value;
            }
        }

        public void VoegFaciliteitToe(Faciliteit faciliteit)
        {
            this.Faciliteiten.Add(faciliteit);
        }

    }
}
