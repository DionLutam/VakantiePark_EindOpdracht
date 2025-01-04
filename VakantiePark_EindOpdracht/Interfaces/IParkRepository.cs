using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.DTO;
using VakantieParkBL.Model;

namespace VakantieParkBL.Interfaces
{
    public interface IParkRepository
    {
        IReadOnlyCollection<Park> LeesParken(Dictionary<int, Faciliteit> faciliteiten);
        Dictionary<int, Faciliteit> LeesFaciliteiten();
        List<Huis> LeesHuizen(Park park);
        Klant LeesKlant(int klantId);
        IReadOnlyCollection<Klant> LeesKlanten(string klantNaam);
        Park LeesPark(int huisID, Dictionary<int, Faciliteit> faciliteiten);
        IReadOnlyCollection<ReservatieInfo> LeesProbleemReservaties(int? huisID);
        IReadOnlyCollection<ReservatieInfo> LeesReservaties(int klantId);
        IReadOnlyCollection<ReservatieInfo> LeesReservaties(string? parkNaam, DateTime? startDatum, DateTime? eindDatum);
        List<Reservatie> LeesReservaties(Huis huis);
        void SchrijfKlanten(List<Klant> klanten);
        void SchrijfParken(List<Park> parken, List<Faciliteit> faciliteiten);
        void SchrijfProbleemReservaties(Huis huis);
        void SchrijfReservaties(List<Reservatie> reservaties,List<Park> parken);
        List<string> LeesParkLocaties();
        void SchrijfReservatie(Reservatie reservatie);
    }
}
