using System.Configuration;
using System.Windows;
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkBL.Model;
using VakantieParkDL_SQL;

namespace VakantieParkUI_ParkManagement.Beheerder.HuisOnderhoud
{
    /// <summary>
    /// Interaction logic for HuisInOnderhoudWindow.xaml
    /// </summary>
    public partial class HuisInOnderhoudWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        Huis huis = null;
        public HuisInOnderhoudWindow()
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
        }

        private void Button_Click_BekijkReservaties(object sender, RoutedEventArgs e)
        {
            try
            {
                int HuisID = 0;
                IReadOnlyCollection<ReservatieInfo> probleemReservaties = new List<ReservatieInfo>();
                if (!String.IsNullOrWhiteSpace(HuisIDTextBox.Text))
                {
                    bool gelukt = int.TryParse(HuisIDTextBox.Text, out HuisID);
                    if (gelukt)
                    {
                        huis = _parkManager.GetHuis(HuisID);
                        probleemReservaties = _parkManager.GetPotentielePRs(huis);
                        ProbleemReservatiesDataGrid.ItemsSource = probleemReservaties;
                    }
                    else
                    {
                        MessageBox.Show("Geef een geldig HuisID", "Ongeldig HuisID", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("HuisID mag niet leeg zijn", "Geen HuisID ingevuld", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_BekijkReservaties", ex);
            }
        }

        private void Button_Click_BevestigOnderhoud(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_parkManager.IsHuisBeschikbaar(huis))
                {
                    _parkManager.ZetHuisInOnderhoud(huis);
                    MessageBox.Show("Huis is in onderhoud", "Gelukt", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Huis is al in onderhoud", "Huis kan niet in onderhoud", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                throw new UIException("BevestigOnderhoud", ex);
            }
        }
    }
}
