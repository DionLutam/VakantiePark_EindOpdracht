using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Park
    {
        private int _id;
        private string _naam;
        private string _locatie;

        public Park(int id, string naam, string locatie, List<Faciliteit> faciliteiten)
        {
            Id = id;
            Naam = naam;
            Locatie = locatie;
            if (!faciliteiten.Any()) { throw new ModelException("Park moet min. 1 faciliteit hebben"); }
            Faciliteiten = faciliteiten;
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

        public bool HasHuis(int huisID)
        {
            try
            {
                int index = this.Huizen.FindIndex(match: x => x.ID.Equals(huisID));
                if (index != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ModelException("HasHuis", ex);
            }
        }

        public Huis GetHuis(int huisID)
        {
            try
            {
                int index = this.Huizen.FindIndex(match: x => x.ID.Equals(huisID));
                bool result = this.HasHuis(huisID);

                if (result == true) 
                {
                    return this.Huizen[index];
                }
                else
                {
                    throw new ModelException("Huis niet in Park");
                }   
            }
            catch(Exception ex) 
            {
                throw new ModelException("GetHuis", ex);
            }
        }

        public void VoegFaciliteitToe(Faciliteit faciliteit)
        {
            try
            {
                if(!this.Faciliteiten.Contains(faciliteit))
                {
                    this.Faciliteiten.Add(faciliteit);
                }
                else
                {
                    throw new ModelException("Park bevat al faciliteit");
                }

            }
            catch (Exception ex) 
            {
                throw new ModelException("VoegFaciliteitToe",ex);
            }

        }
        public void VoegHuisToe(Huis huis)
        {
            try
            {
                if(!this.Huizen.Contains(huis))
                {
                    this.Huizen.Add(huis);
                }
                else
                {
                    throw new ModelException("Park bevat al huis");
                }

            }
            catch (Exception ex) 
            {
                throw new ModelException("VoegHuisToe",ex);
            }

        }

    }
}
