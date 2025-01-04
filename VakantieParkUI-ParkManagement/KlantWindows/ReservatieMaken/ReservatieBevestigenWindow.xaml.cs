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

namespace VakantieParkUI_ParkManagement.KlantWindows.ReservatieMaken
{
    /// <summary>
    /// Interaction logic for ReservatieBevestigenWindow.xaml
    /// </summary>
    public partial class ReservatieBevestigenWindow : Window
    {
        Klant selectedKlant = null;
        Park selectedPark = null;
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        public ReservatieBevestigenWindow(Park park, Klant klant)
        {
            InitializeComponent();
            selectedKlant = klant;
            selectedPark = park;
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
        }

        private void ButtonClick_ReserveringBevestigen(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(BeschikbareHuizenDataGrid.SelectedItems.Count != 1)) 
                {
                    if (BeschikbareHuizenDataGrid.SelectedItem is Huis huis)
                    {
                        DateTime startDatum = StartDatumPicker.SelectedDate.Value;
                        DateTime eindDatum = EindDatumPicker.SelectedDate.Value;
                        _parkManager.MaakReservatie(selectedKlant, huis, startDatum, eindDatum);
                        MessageBox.Show("Reservatie Aanmaken gelukt", "gelukt", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Gelieve 1 huis te selecteren","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_ReserveringBevestigen", ex);
            }
        }

        private void Button_Click_BeschikbareHuizenZoeken(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(AantalPersonenTextBox.Text) && 
                    StartDatumPicker.SelectedDate.HasValue && EindDatumPicker.SelectedDate.HasValue) 
                {
                    int aantalPersonen = 0;
                    DateTime StartDatum = StartDatumPicker.SelectedDate.Value;
                    DateTime EindDatum = EindDatumPicker.SelectedDate.Value;
                    bool gelukt = int.TryParse(AantalPersonenTextBox.Text, out aantalPersonen);
                    if (gelukt)
                    {
                        List <Huis> huizen = _parkManager.GetBeschikbareHuizen(selectedPark, aantalPersonen,StartDatum,EindDatum);
                        if (huizen.Any())
                        {
                            BeschikbareHuizenDataGrid.ItemsSource = huizen;
                        }
                        else
                        {
                            MessageBox.Show("Geen beschikbare huizen gevonden","Not Found",MessageBoxButton.OK, MessageBoxImage.Information);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Gelieve een getal in te vullen", "incorrect data", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Gelieve alle velden correct in te vullen","Incorrect Data", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClick_BeschikbareHuizenZoeken", ex);
            }
        }
    }
}
