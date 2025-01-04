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

namespace VakantieParkUI_ParkManagement.KlantWindows
{
    /// <summary>
    /// Interaction logic for KlantIdWindow.xaml
    /// </summary>
    public partial class KlantIdWindow : Window
    {
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public KlantIdWindow()
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
                int klantId = 0;
                if(KlantIdTextBox.Text != null)
                {
                    bool gelukt = int.TryParse(KlantIdTextBox.Text, out klantId);
                    if(!gelukt)
                    {
                        MessageBox.Show("Gelieve een geldig ID in te geven", "Ongeldig ID", MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                    else
                    {
                        Klant klant = _parkManager.GetKlant(klantId);
                        KlantWindow kl = new KlantWindow(klant);
                        kl.ShowDialog();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new UIException("ButtonClick_Enter", ex);
            }
        }
    }
}
