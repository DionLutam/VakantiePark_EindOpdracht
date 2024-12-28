using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Model;
using VakantieParkUI_ParkManagement.Model;

namespace VakantieParkBL.Managers
{
    public class ParkManager
    {
        private IParkRepository _parkRepository;

        public ParkManager(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public Huis GetHuis(int iD)
        {
            throw new NotImplementedException();
        }

        public Park GetPark(int iD)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<ReservatieInfoKlantId> GetReservaties(int klantId)
        {
            try
            {
                return _parkRepository.LeesReservatiesViaKlantId(klantId);
            }
            catch (Exception ex)
            {
                throw new ParkManagerException("GetReservaties", ex);
            }
        }
    }
}
