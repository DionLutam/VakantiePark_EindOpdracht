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
using VakantieParkUI_ParkManagement.Beheerder.ReservatiesOpzoeken.MetParkEnPeriode;

namespace VakantieParkUI_ParkManagement.Beheerder.ProbleemReservaties
{
    /// <summary>
    /// Interaction logic for GeefHuisIDWindow.xaml
    /// </summary>
    public partial class GeefHuisIDWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public GeefHuisIDWindow()
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
        }

        private void Button_Click_Zoek(object sender, RoutedEventArgs e)
        {
            try
            {
                int HuisID = 0;
                IReadOnlyCollection<ReservatieInfo> probleemReservaties = new List<ReservatieInfo>();
                if(!String.IsNullOrWhiteSpace(HuisIdTextBox.Text))
                {
                    bool gelukt = int.TryParse(HuisIdTextBox.Text, out HuisID);
                    if(gelukt)
                    {
                        probleemReservaties = _parkManager.GetProbleemReservaties(HuisID);
                    }
                    else
                    {
                        MessageBox.Show("Geef een geldig ID","ongeldig huisID",MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    probleemReservaties = _parkManager.GetProbleemReservaties();
                }
                ToonProbleemReservatiesWindow tprw = new ToonProbleemReservatiesWindow(probleemReservaties);
                tprw.Show();
            }
            catch(Exception ex) 
            {
                throw new UIException("ButtonClickZoek", ex);
            }
        }
    }
}
