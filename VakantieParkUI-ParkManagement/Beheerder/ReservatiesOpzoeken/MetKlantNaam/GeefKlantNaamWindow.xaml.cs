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
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkBL.Model;
using VakantieParkDL_SQL;

namespace VakantieParkUI_ParkManagement.Beheerder
{
    /// <summary>
    /// Interaction logic for GeefKlantNaamWindow.xaml
    /// </summary>
    public partial class GeefKlantNaamWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public GeefKlantNaamWindow()
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
                if(!String.IsNullOrWhiteSpace(KlantNaamTextBox.Text))
                {
                    string klantNaam = KlantNaamTextBox.Text.ToLower();
                    IReadOnlyCollection<Klant> klanten = _parkManager.GetKlanten(klantNaam);
                    SelecteerKlantWindow skw = new SelecteerKlantWindow(klanten);
                    skw.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Geef een klant naam","ongeldig klantnaam",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                throw new UIException("ButtonClickEnter", ex);
            }
        }
    }
}
