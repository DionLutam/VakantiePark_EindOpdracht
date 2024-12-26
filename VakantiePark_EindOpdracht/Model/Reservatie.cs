using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Reservatie
    {
        private int _id;
        private DateTime _startDatum;
        private DateTime _endDatum;

        public DateTime StartDatum { get; private set; }
        public DateTime EndDatum { get; private set; }
        public Klant Klant { get; private set; }
        public Huis Huis { get; private set; }

        public int Id
        {
            get { return _id; }
            private set
            {
                _id = (value <= 0) ? throw new ModelException("ReservatieID is negatief"):value;
            }
        }

        public Reservatie(int id, DateTime startDatum, DateTime endDatum, Klant klant, Huis huis) : this(startDatum, endDatum, klant,huis)
        {
            Id = id;
        }
        public Reservatie(DateTime startDatum, DateTime endDatum, Klant klant, Huis huis)
        {
            if (startDatum > endDatum) { throw new ModelException("StartDatum moet voor EindDatum zijn"); }
            StartDatum = startDatum;
            EndDatum = endDatum;
            Klant = klant;
            Huis = huis;
        }

        public void ZetID(int id)
        {
            try
            {
                this.Id = id;
            }
            catch (Exception ex) 
            {
                throw new ModelException("ZetReservatieID", ex);
            }

        }

        public void ZetKlant(Klant klant)
        {
            try
            {
                this.Klant = klant;
            }
            catch (Exception ex)
            {
                throw new ModelException("ZetKlant", ex);
            }
        }
        public void ZetHuis(Huis huis)
        {
            try
            {
                this.Huis = huis;
            }
            catch (Exception ex)
            {
                throw new ModelException("ZetHuis", ex);
            }
        }
    }
}
