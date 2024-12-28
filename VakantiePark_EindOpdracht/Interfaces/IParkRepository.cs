using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Model;
using VakantieParkUI_ParkManagement.Model;

namespace VakantieParkBL.Interfaces
{
    public interface IParkRepository
    {
        IReadOnlyCollection<ReservatieInfoKlantId> LeesReservatiesViaKlantId(int klantId);
        void SchrijfKlanten(List<Klant> klanten);
        void SchrijfParken(List<Park> parken, List<Faciliteit> faciliteiten);
        void SchrijfReservaties(List<Reservatie> reservaties,List<Park> parken);
    }
}
