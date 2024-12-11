using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;

namespace VakantieParkBL.Model
{
    public class Faciliteiten
    {
        private string _beschrijving;
        private int _id;

        public Faciliteiten (int id, string beschrijving)
        {
            id = Id;
            beschrijving = Beschrijving)
        }
        public string Beschrijving
        {
            get { return _beschrijving; }
            set
            {
                _beschrijving = String.IsNullOrWhiteSpace(value) 
                    ? throw new ModelException("Beschrijving is leeg of incorrect") : value;
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value <= 0 
                    ? throw new ModelException("Faciliteiten ID is negatief of 0") : value;
            }
        }   
    }
}
