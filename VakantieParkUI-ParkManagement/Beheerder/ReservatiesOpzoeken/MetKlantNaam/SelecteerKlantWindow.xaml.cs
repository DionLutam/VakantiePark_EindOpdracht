using System.Configuration;
using System.Windows;
using System.Windows.Input;
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkBL.Model;
using VakantieParkDL_SQL;
using VakantieParkUI_ParkManagement.KlantWindows;

namespace VakantieParkUI_ParkManagement.Beheerder
{
    /// <summary>
    /// Interaction logic for SelecteerKlantWindow.xaml
    /// </summary>
    public partial class SelecteerKlantWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public SelecteerKlantWindow(IReadOnlyCollection<Klant> klanten)
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
            KlantenDataGrid.ItemsSource = klanten;
        }

        private void Double_Click_Klant(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (KlantenDataGrid.SelectedItem is Klant klant)
                {
                    IReadOnlyCollection<ReservatieInfo> reservatieLijst = _parkManager.GetReservaties(klant.ID);

                    KlantReservatiesWindow rw = new KlantReservatiesWindow(reservatieLijst);
                    rw.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw new UIException("DoubleClickKlant", ex);
            }

        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception ex)
            {
                throw new UIException("ButtonClose", ex);
            }
        }
    }
}
