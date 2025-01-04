using System.Configuration;
using System.Windows;
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkBL.Model;
using VakantieParkDL_SQL;
using VakantieParkUI_ParkManagement.KlantWindows;

namespace VakantieParkUI_ParkManagement.KlantWindows
{
    /// <summary>
    /// Interaction logic for KlantWindow.xaml
    /// </summary>
    public partial class KlantWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        private Klant Selectedklant = null;
        public KlantWindow(Klant klant)
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
            KlantNaamTextBlock.Text = klant.Naam;
            Selectedklant = klant;
        }
        public KlantWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_MaakReservatie(object sender, RoutedEventArgs e)
        {
            try
            {
                ParkenInfo parkenInfo = _parkManager.GetParkenInfo();
                SelecteerParkWindow selecteerParkWindow = new SelecteerParkWindow(parkenInfo,Selectedklant);
                selecteerParkWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_MaakReservatie", ex);
            }
        }

        private void Button_Click_MijnReservaties(object sender, RoutedEventArgs e)
        {
            try
            {
                IReadOnlyCollection<ReservatieInfo> reservatieLijst = _parkManager.GetReservaties(Selectedklant.ID);

                MijnReservatiesWindow rw = new MijnReservatiesWindow(reservatieLijst);
                rw.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_MijnReservaties", ex);
            }
        }
    }
}
