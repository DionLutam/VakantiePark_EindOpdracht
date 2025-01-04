using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkDL_SQL;

namespace VakantieParkUI_ParkManagement.Beheerder.ReservatiesOpzoeken.MetParkEnPeriode
{
    /// <summary>
    /// Interaction logic for GeefParkEnPariodeWindow.xaml
    /// </summary>
    public partial class GeefParkEnPariodeWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public GeefParkEnPariodeWindow()
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
        }

        private void Button_Click_Enter(object sender, RoutedEventArgs e)
        {
            try
            {
                string parknaam = null;
                DateTime? startDatum = null;
                DateTime? eindDatum = null;
                if (!String.IsNullOrWhiteSpace(ParkNaamTextBox.Text))
                {
                    parknaam = ParkNaamTextBox.Text;
                }
                if (startdatumDatePicker.SelectedDate.HasValue)
                {
                    startDatum = startdatumDatePicker.SelectedDate;
                }
                if (einddatumDatePicker.SelectedDate.HasValue)
                {
                    eindDatum = einddatumDatePicker.SelectedDate;
                }
                IReadOnlyCollection<ReservatieInfo> reservaties = _parkManager.GetReservaties(parknaam, startDatum, eindDatum);
                if(parknaam != null)
                {
                    ToonParkReservatiesWindow tpw = new ToonParkReservatiesWindow(reservaties);
                    tpw.Show();
                }
                else
                {
                    ToonReservatiesPeriodeWindow trpw = new ToonReservatiesPeriodeWindow(reservaties);
                    trpw.Show();

                }



            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick", ex);
            }
        }
    }
}
