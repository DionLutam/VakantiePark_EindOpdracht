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

namespace VakantieParkUI_ParkManagement.Klant
{
    /// <summary>
    /// Interaction logic for KlantIdWindow.xaml
    /// </summary>
    public partial class KlantIdWindow : Window
    {
        public KlantIdWindow()
        {
            InitializeComponent();
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
                        MessageBox.Show("Gelieve een geldig ID in te geven", "Ongeldig ID", MessageBoxButton.OK);
                    }
                    else
                    {
                        KlantWindow kl = new KlantWindow(klantId);
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
