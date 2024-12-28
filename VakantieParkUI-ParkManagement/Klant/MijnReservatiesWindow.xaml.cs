using System;
using System.Collections.Generic;
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
using VakantieParkUI_ParkManagement.Model;

namespace VakantieParkUI_ParkManagement.Klant
{
    /// <summary>
    /// Interaction logic for MijnReservatiesWindow.xaml
    /// </summary>
    /// 
    //select p.naam, p.locatie, r.startdatum, r.einddatum, h.nummer from Reservatie r join huis h on r.huisId = h.id join park p on h.parkId = p.id where r.klantId='3282' order by r.startdatum;
    public partial class MijnReservatiesWindow : Window
    {
        public MijnReservatiesWindow(IReadOnlyCollection<ReservatieInfoKlantId> reservaties)
        {
            InitializeComponent();
            KlantReservatiesDataGrid.ItemsSource = reservaties;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {

        }
    }
}
