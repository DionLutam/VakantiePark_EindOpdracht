using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Huis
    {
        private int _id;
        private string _straat;
        private int _nummer;
        private int _aantalPersonen;
        public Park Park {  get; private set; }
        public List<Reservatie> Reservaties { get; private set; } = new List<Reservatie> ();

        public Huis(int iD, string straat, int nummer, bool isActief, int aantalPersonen, Park park)
        {
            ID = iD;
            Straat = straat;
            Nummer = nummer;
            IsActief = isActief;
            AantalPersonen = aantalPersonen;
            Park = park;
        }

        public int ID 
        {   
            get { return _id; } 
            private set 
            { 
                _id = (value <= 0) ? 
                    throw new ModelException("HuisID is negatief") : value; 
            }
        }

        public string Straat
        {
            get { return _straat; }
            private set
            {
                _straat = String.IsNullOrWhiteSpace(value) ? 
                    throw new ModelException("Straat is leeg") : value;
            }
        }

        public int AantalPersonen
        {
            get { return _aantalPersonen; }
            private set
            {
                _aantalPersonen = (value <= 0) ? throw new ModelException("Huis moet min. voor 1 Persoon zijn") : value;
            }
        }

        public int Nummer
        {
            get { return _nummer; }
            private set
            {
                _nummer = (value <= 0) ? throw new ModelException("Huisnummer is negatief") : value;
            }
        }

        public bool IsActief { get; private set; } = true;

        public void ZetHuisInOnderhoud()
        {
            try
            {
                this.IsActief = false;
            }
            catch(Exception ex)
            {
                throw new ModelException("ZetHuisInOnderhoud", ex);
            }
        }

        public void ZetHuisBeschikbaar()
        {
            try
            {
                this.IsActief = true;
            }
            catch (Exception ex)
            {
                throw new ModelException("ZetHuisBeschikbaar", ex);
            }
        }

        public void ZetPark(Park park)
        {
            try
            {
                this.Park = park;
            }
            catch (Exception ex)
            {
                throw new ModelException("ZetPark", ex);
            }

        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            try
            {
                if (!this.Reservaties.Contains(reservatie))
                {
                    this.Reservaties.Add(reservatie);
                }
                else
                {
                    throw new ModelException("Reservatie voor huis al toegevoegd");
                }
            }
            catch (Exception ex)
            {
                throw new ModelException("VoegReservatieToe", ex);
            }

        }
    }
}
