using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private int _capaciteit;
        public Park Park {  get; private set; }
        public List<Reservatie> Reservaties { get; private set; } = new List<Reservatie> ();
        public List<Reservatie> ProbleemReservaties { get; private set; } = new List<Reservatie>();

        public Huis(int iD, string straat, int nummer, bool isActief, int capaciteit, Park park)
        {
            ID = iD;
            Straat = straat;
            Nummer = nummer;
            IsActief = isActief;
            Capaciteit = capaciteit;
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

        public int Capaciteit
        {
            get { return _capaciteit; }
            private set
            {
                _capaciteit = (value <= 0) ? throw new ModelException("Huis moet min. voor 1 Persoon zijn") : value;
            }
        }

        public int Nummer
        {
            get { return _nummer; }
            private set
            {
                _nummer = (value < 0) ? throw new ModelException("Huisnummer is negatief") : value;
            }
        }

        public bool IsActief { get; private set; } = true;


        public void ZetHuisInOnderhoud()
        {
            try
            {

                if(this.Reservaties.Any())
                {
                    foreach (Reservatie reservatie in this.Reservaties)
                    {
                        if (reservatie.StartDatum <= DateTime.Now && reservatie.EndDatum >= DateTime.Now)
                        {
                            throw new ModelException("Huis kan niet in onderhoud, is niet leeg");
                        }
                        else if(reservatie.StartDatum > DateTime.Now)
                        {
                            ProbleemReservaties.Add(reservatie);
                        }
                        
                    }
                }
                this.IsActief = false;
            }
            catch(Exception ex)
            {
                throw new ModelException("ZetHuisInOnderhoud", ex);
            }
        }

        //public void ZetProbleemReservaties()
        //{
        //    try
        //    {
        //        if (this.IsActief == false && this.Reservaties.Any())
        //        {
        //            foreach (Reservatie reservatie in this.Reservaties)
        //            {
        //                if(reservatie.StartDatum > DateTime.Now)
        //                {
        //                    ProbleemReservatie.Add(reservatie);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ModelException("ZetProbleemReservaties", ex);
        //    }
        //}

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

                    if(this.IsActief==false)
                    {
                        if (reservatie.StartDatum > DateTime.Now)
                        {
                            this.ProbleemReservaties.Add(reservatie);
                        }
                    }
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
