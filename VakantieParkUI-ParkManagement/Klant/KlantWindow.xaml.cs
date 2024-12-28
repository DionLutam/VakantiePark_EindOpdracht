using System.Configuration;
using System.Windows;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkDL_SQL;
using VakantieParkUI_ParkManagement.Klant;
using VakantieParkUI_ParkManagement.Model;

namespace VakantieParkUI_ParkManagement
{
    /// <summary>
    /// Interaction logic for KlantWindow.xaml
    /// </summary>
    public partial class KlantWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        private int klantId;
        public KlantWindow(int klantID)
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
            KlantIdTextBlock.Text = klantID.ToString();
            klantId = klantID;
        }
        public KlantWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_MaakReservatie(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_MijnReservaties(object sender, RoutedEventArgs e)
        {
            try
            {
                IReadOnlyCollection<ReservatieInfoKlantId> reservatieLijst = _parkManager.GetReservaties(klantId);

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
