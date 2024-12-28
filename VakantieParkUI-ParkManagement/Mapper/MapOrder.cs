using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Model;
using VakantieParkUI_ParkManagement.Model;

namespace VakantieParkUI_ParkManagement.Mapper
{
    public class MapOrder
    {
        public static ReservatieInfoKlantId MapFromDomain (Reservatie reservatie, Park park, Huis huis)
        {
            return new ReservatieInfoKlantId(park.Naam,park.Locatie,reservatie.StartDatum,reservatie.EndDatum,huis.Nummer);
        }


    }
}
