using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Faciliteiten
    {
        private string _beschrijving;
        private int _id;
        List<Park> parken = new List<Park>();
        public Faciliteiten(int id, string beschrijving)
        {
            id = Id;
            beschrijving = Beschrijving;
        }
        public string Beschrijving
        {
            get { return _beschrijving; }
            private set
            {
                _beschrijving = String.IsNullOrWhiteSpace(value)
                    ? throw new ModelException("Beschrijving is leeg of incorrect") : value;
            }
        }

        public int Id
        {
            get { return _id; }
            private set
            {
                _id = value <= 0
                    ? throw new ModelException("Faciliteiten ID is negatief of 0") : value;
            }
        }
    }
}
