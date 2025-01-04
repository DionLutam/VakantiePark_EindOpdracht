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
using VakantieParkUI_ParkManagement.Beheerder.HuisOnderhoud;
using VakantieParkUI_ParkManagement.Beheerder.ProbleemReservaties;

namespace VakantieParkUI_ParkManagement.Beheerder
{
    /// <summary>
    /// Interaction logic for BeheerderMenu.xaml
    /// </summary>
    public partial class BeheerderMenu : Window
    {
        public BeheerderMenu()
        {
            InitializeComponent();
        }

        private void Button_Click_ZoekReservatie(object sender, RoutedEventArgs e)
        {
            try
            {
                BeheerderOpzoekMenu bom = new BeheerderOpzoekMenu();
                bom.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_ZoekReservatie", ex);
            }
        }

        private void Button_Click_ProbleemReservatie(object sender, RoutedEventArgs e)
        {
            try
            {
                GeefHuisIDWindow ghw = new GeefHuisIDWindow();
                ghw.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_ProbleemReservatie", ex);
            }
        }

        private void Button_Click_HuisOnderhoud(object sender, RoutedEventArgs e)
        {
            try
            {
                HuisInOnderhoudWindow huisInOnderhoud = new HuisInOnderhoudWindow();
                huisInOnderhoud.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_HuisOnderhoud", ex);
            }
        }
    }
}
