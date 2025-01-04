using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Huis
    {
        private int _id;
        private string _straat;
        private int _nummer;
        private int _capaciteit;
        public Park Park { get; private set; }
        public List<Reservatie> Reservaties { get; private set; } = new List<Reservatie>();
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
                _id = (value < 0) ?
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
                _capaciteit = (value < 1) ? throw new ModelException("Huis moet min. voor 1 Persoon zijn") : value;
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

        public bool IsBeschikbaar()
        {
            return this.IsActief;
        }
        public bool HeeftReservaties()
        {
            return this.Reservaties.Any();
        }
        //public bool HeeftActieveReservatie()
        //{
        //    try
        //    {
        //        if (this.Reservaties.Any())
        //        {
        //            foreach (Reservatie reservatie in this.Reservaties)
        //            {
        //                if (reservatie.StartDatum <= DateTime.Now && reservatie.EndDatum >= DateTime.Now)
        //                {
        //                    return true;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            throw new ModelException("Huis heeft geen reservaties");
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ModelException("HeeftActieveReservatie", ex);
        //    }

        //}
        public void ZetHuisInOnderhoud()
        {
            try
            {
                if (this.IsBeschikbaar() && this.HeeftReservaties())
                {
                    foreach (Reservatie reservatie in this.Reservaties)
                    {

                        if (reservatie.StartDatum > DateTime.Now)
                        {
                            ProbleemReservaties.Add(reservatie);
                        }

                    }
                }
                else
                {
                    throw new ModelException("Huis kan niet in onderhoud, is bezet");
                }
                this.IsActief = false;
            }
            catch (Exception ex)
            {
                throw new ModelException("ZetHuisInOnderhoud", ex);
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

                    if (this.IsBeschikbaar() == false)
                    {
                        if (reservatie.IsInToekomst())
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

        public bool HeeftVoldoendeCapaciteit(int aantalPersonen)
        {
            try
            {
                return (this.Capaciteit >= aantalPersonen);
            }
            catch (Exception ex)
            {
                throw new ModelException("HeeftVoldoendeCapaciteit", ex);
            }
        }

        public bool IsBeschikbaar(int aantalPersonen, DateTime startDatum, DateTime eindDatum)
        {
            try
            {
                if (aantalPersonen > 0 && startDatum != DateTime.MinValue && eindDatum != DateTime.MinValue)
                {
                    if (this.HeeftVoldoendeCapaciteit(aantalPersonen) && this.IsBeschikbaar())
                    {
                        foreach (Reservatie reservatie in this.Reservaties)
                        {
                            if (reservatie.CheckOverlapping(startDatum, eindDatum))
                            {
                                return false;
                            }

                        }
                        return true;
                    }
                    return false;
                }
                else
                {
                    throw new ModelException("Incorrecte Data");
                }


            }
            catch (Exception ex)
            {
                throw new ModelException("IsBeschikbaar()", ex);
            }
        }

        public void VoegReservatiesToe(IReadOnlyCollection<Reservatie> reservaties)
        {
            try
            {
                foreach (Reservatie reservatie in reservaties)
                {
                    this.VoegReservatieToe(reservatie);
                }
            }
            catch (Exception ex)
            {
                throw new ModelException("VoegReservatiesToe", ex);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Huis otherHuis = (Huis)obj;


            return this.ID == otherHuis.ID;
        }
    }
}
