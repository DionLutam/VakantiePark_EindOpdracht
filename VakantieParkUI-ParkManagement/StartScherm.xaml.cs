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
using VakantieParkUI_ParkManagement.Beheerder;
using VakantieParkUI_ParkManagement.KlantWindows;

namespace VakantieParkUI_ParkManagement
{
    /// <summary>
    /// Interaction logic for StartScherm.xaml
    /// </summary>
    public partial class StartScherm : Window
    {
        public StartScherm()
        {
            InitializeComponent();
        }

        private void Button_Click_Klant(object sender, RoutedEventArgs e)
        {
            try
            {
                KlantIdWindow k = new KlantIdWindow();
                k.ShowDialog();
                
            }
            catch (Exception ex)
            {
                throw new UIException("Button_Click_Klant", ex);
            }
        }

        private void Button_Click_Beheerder(object sender, RoutedEventArgs e)
        {
            try
            {
                BeheerderMenu bm = new BeheerderMenu();
                bm.ShowDialog();

            }
            catch (Exception ex)
            {
                throw new UIException("Button_Click_Beheerder", ex);
            }
        }
    }
}
