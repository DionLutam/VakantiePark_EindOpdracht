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
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;

namespace VakantieParkUI_ParkManagement.Beheerder.ReservatiesOpzoeken.MetParkEnPeriode
{
    /// <summary>
    /// Interaction logic for ToonReservatiesPeriodeWindow.xaml
    /// </summary>
    public partial class ToonReservatiesPeriodeWindow : Window
    {
        public ToonReservatiesPeriodeWindow(IReadOnlyCollection<ReservatieInfo> reservaties)
        {
            InitializeComponent();
            ParkReservatiesDataGrid.ItemsSource = reservaties;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClose", ex);
            }
        }
    }
}
