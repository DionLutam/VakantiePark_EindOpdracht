using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Model;

namespace VakantieParkBL.Managers
{
    public class ParkManager
    {
        private IParkRepository _parkRepository;

        public ParkManager(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public List<Reservatie> GetReservaties(Huis huis)
        {
            try
            {
                return _parkRepository.LeesReservaties(huis);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetReservaties(Huis)", ex);
            }
        }

        public Huis GetHuis(int HuisID)
        {
            try
            {
                Park park = GetPark(HuisID);
                Huis huis = park.GetHuis(HuisID);
                return huis;
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetHuis", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> GetPotentielePRs(Huis huis)
        {
            try
            {
                List<ReservatieInfo> potentielePRs = new List<ReservatieInfo>();
                foreach (Reservatie reservation in huis.Reservaties)
                {
                    if (reservation.StartDatum >= DateTime.Now)
                    {
                        potentielePRs.Add(new ReservatieInfo
                            (reservation.Id, reservation.Klant.ID, reservation.Klant.Naam, reservation.StartDatum, reservation.EndDatum));
                    }
                }
                return potentielePRs;
            }
            catch (Exception ex) { throw new ParkManagerException("GetPotentielePRs", ex); }

        }

        public IReadOnlyCollection<Klant> GetKlanten(string klantNaam)
        {
            try
            {
                return _parkRepository.LeesKlanten(klantNaam);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetKlanten", ex);
            }
        }

        public Dictionary<int, Faciliteit> GetFaciliteiten()
        {
            try
            {
                return _parkRepository.LeesFaciliteiten();
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetFaciliteiten", ex);
            }
        }
        public IReadOnlyCollection<Huis> GetHuizen(Park park)
        {
            try
            {
                List<Huis> huizen = _parkRepository.LeesHuizen(park);
                foreach (Huis hu in huizen)
                {
                    IReadOnlyCollection<Reservatie> reservaties = GetReservaties(hu);
                    hu.VoegReservatiesToe(reservaties);
                }
                return huizen;

            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetHuizen", ex);
            }
        }

        public Park GetPark(int HuisID)
        {
            try
            {
                Dictionary<int, Faciliteit> faciliteiten = GetFaciliteiten();
                Park park = _parkRepository.LeesPark(HuisID, faciliteiten);
                IReadOnlyCollection<Huis> huizen = GetHuizen(park);
                park.VoegHuizenToe(huizen); 

                return park;
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetPark()", ex);
            }
        }

        public IReadOnlyCollection<Park> GetParkenWithFaciliteitOnly()
        {
            try
            {
                Dictionary<int, Faciliteit> faciliteiten = GetFaciliteiten();
                IReadOnlyCollection<Park> parken = _parkRepository.LeesParken(faciliteiten);

                return parken;
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetParkenWithFaciliteitOnly", ex);
            }
        }
        public IReadOnlyCollection<Park> GetParken()
        {
            try
            {
                Dictionary<int, Faciliteit> faciliteiten = GetFaciliteiten();
                IReadOnlyCollection<Park> parken = _parkRepository.LeesParken(faciliteiten);
                foreach (Park park in parken)
                {
                    IReadOnlyCollection<Huis> huizen = GetHuizen(park);
                    foreach (Huis huis in huizen)
                    {
                        park.VoegHuisToe(huis);
                    }
                }

                return parken;
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetParken", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> GetProbleemReservaties(int? huisID = null)
        {
            try
            {
                return _parkRepository.LeesProbleemReservaties(huisID);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetProbleemReservaties", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> GetReservaties(int klantId)
        {
            try
            {
                return _parkRepository.LeesReservaties(klantId);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetReservaties", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> GetReservaties(string? parkNaam, DateTime? startDatum, DateTime? eindDatum)
        {
            try
            {
                return _parkRepository.LeesReservaties(parkNaam, startDatum, eindDatum);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetReservaties", ex);
            }
        }
        public bool IsHuisBeschikbaar(Huis huis)
        {
            return huis.IsBeschikbaar();
        }

        public void ZetHuisInOnderhoud(Huis huis)
        {
            try
            {
                huis.ZetHuisInOnderhoud();
                _parkRepository.SchrijfProbleemReservaties(huis);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("ZetHuisInOnderhoud", ex);
            }
        }

        public Klant GetKlant(int klantId)
        {
            try
            {
                return _parkRepository.LeesKlant(klantId);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("LeesKlant", ex);
            }
        }

        public List<string> GetParkLocaties()
        {
            try
            {
                return _parkRepository.LeesParkLocaties();
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetParkLocaties", ex);
            }
        }

        public ParkenInfo GetParkenInfo()
        {
            try
            {
                List<string> locaties = GetParkLocaties();
                List<Faciliteit> faciliteiten = GetFaciliteiten().Values.ToList();
                List<string> faciliteitnamen = new List<string>();
                foreach (Faciliteit faciliteit in faciliteiten)
                {
                    faciliteitnamen.Add(faciliteit.Beschrijving);
                }
                return new ParkenInfo(locaties, faciliteitnamen);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetParkenInfo", ex);
            }
        }

        public IReadOnlyCollection<Park> ZoekParken(List<string> faciliteitNamen, string? locatie)
        {
            try
            {
                List<Park> SelectedParken = new List<Park>();
                IReadOnlyCollection<Park> parkenWithFaciliteiten = GetParkenWithFaciliteitOnly();
                if (locatie != null)
                {
                    SelectedParken = parkenWithFaciliteiten
                                     .Where(park => park.Locatie == locatie && faciliteitNamen.All(naam => park.HasFaciliteit(naam)))
                                     .ToList();
                }
                else
                {
                    SelectedParken = parkenWithFaciliteiten
                                     .Where(park => faciliteitNamen.All(naam => park.HasFaciliteit(naam)))
                                     .ToList();
                }
                return SelectedParken;

            }
            catch (Exception ex)
            {
                throw new ParkManagerException("ZoekParken", ex);
            }
        }

        public List<Huis> GetBeschikbareHuizen(Park park, int aantalPersonen, DateTime startDatum, DateTime eindDatum)
        {
            try
            {
                IReadOnlyCollection<Huis> huizen = GetHuizen(park);
                park.VoegHuizenToe(huizen);
                List<Huis> beschikbareHuizen = park.GetBeschikbareHuizen(aantalPersonen,startDatum, eindDatum);
                return beschikbareHuizen;
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetBeschikbareHuizen", ex);
            }
        }

        public void MaakReservatie(Klant selectedKlant, Huis huis, DateTime startDatum, DateTime eindDatum)
        {
            try
            {
                Reservatie reservatie = new Reservatie(startDatum, eindDatum,selectedKlant,huis);
                huis.VoegReservatieToe(reservatie);
                selectedKlant.VoegReservatieToe(reservatie);
                _parkRepository.SchrijfReservatie(reservatie);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("MaakReservatie", ex);
            }
        }
    }
}
