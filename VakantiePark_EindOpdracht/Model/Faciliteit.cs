using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Faciliteit
    {
        private string _beschrijving;
        private int _id;
        public List<Park> Parken { get; private set; } = new List<Park>();
        public Faciliteit(int id, string beschrijving)
        {
            Id = id;
            Beschrijving = beschrijving;
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

        public void VoegParkToe(Park park)
        {
            try
            {
                if(!this.Parken.Contains(park))
                {
                    this.Parken.Add(park);
                }
                else
                {
                    throw new ModelException("Faciliteit bevat park");
                }

            }
            catch (Exception ex) 
            {
                throw new ModelException("VoegParkToe", ex);
            }

        }
    }
}
