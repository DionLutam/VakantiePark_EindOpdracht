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
using VakantieParkBL.Exceptions;
using VakantieParkUI_ParkManagement.Beheerder.ReservatiesOpzoeken.MetParkEnPeriode;

namespace VakantieParkUI_ParkManagement.Beheerder
{
    /// <summary>
    /// Interaction logic for BeheerderOpzoekMenu.xaml
    /// </summary>
    public partial class BeheerderOpzoekMenu : Window
    {
        public BeheerderOpzoekMenu()
        {
            InitializeComponent();
        }

        private void Button_Click_ZoekMetKlantNaam(object sender, RoutedEventArgs e)
        {
            try
            {
                GeefKlantNaamWindow gkw = new GeefKlantNaamWindow();
                gkw.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_ZoekMetKlantNaam", ex);
            }
        }

        private void Button_Click_ZoekMetParkNaam(object sender, RoutedEventArgs e)
        {
            try
            {
                GeefParkEnPariodeWindow g = new GeefParkEnPariodeWindow();
                g.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_ZoekMetParkNaam", ex);
            }
        }
    }
}
