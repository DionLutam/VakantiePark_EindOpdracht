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
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkBL.Model;
using VakantieParkDL_SQL;
using VakantieParkUI_ParkManagement.KlantWindows.ReservatieMaken;

namespace VakantieParkUI_ParkManagement.KlantWindows
{
    /// <summary>
    /// Interaction logic for SelecteerParkWindow.xaml
    /// </summary>
    public partial class SelecteerParkWindow : Window
    {
        ParkenInfo parkenInformatie;
        private ParkManager _parkManager;
        private IParkRepository _parkRepository;
        private string user;
        Klant selectedKlant = null;
        public SelecteerParkWindow(ParkenInfo parkenInfo,Klant klant)
        {
            InitializeComponent();
            user = ConfigurationManager.AppSettings["user"];
            _parkRepository = new ParkRepository(ConfigurationManager.ConnectionStrings[user].ConnectionString);
            _parkManager = new ParkManager(_parkRepository);
            LocatieComboBox.ItemsSource = parkenInfo.Locaties;
            selectedKlant = klant;
            foreach (string faciliteit in parkenInfo.Faciliteiten)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = faciliteit;
                FaciliteitenCheckbox.Items.Add(checkBox);
            }
            parkenInformatie = parkenInfo; 
        }

        public List<string> GetSelectedFaciliteitNamen()
        {
            try
            {
                List<string> selectedFaciliteitNamen = new List<string>();
                foreach (CheckBox checkBox in FaciliteitenCheckbox.Items)
                {
                    if (checkBox.IsChecked == true)
                    {
                        selectedFaciliteitNamen.Add(checkBox.Content.ToString());
                    }
                }
                return selectedFaciliteitNamen;
            }
            catch (Exception ex) 
            {
                throw new UIException("GetSelectedFaciliteitNamen",ex);
            }
        }
        private void Button_Click_Zoeken(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> faciliteitNamen = GetSelectedFaciliteitNamen();
                IReadOnlyCollection<Park> parken = new List<Park>();
                if ((LocatieComboBox.SelectedItem.ToString() != "Alle Parken")) 
                {
                    string locatie = (string)LocatieComboBox.SelectedItem;

                     parken = _parkManager.ZoekParken(faciliteitNamen, locatie);
                }
                else
                {
                     parken = _parkManager.ZoekParken(faciliteitNamen, null);
                }
                if (parken.Any())
                {
                    ParkenDataGrid.ItemsSource = parken;
                }
                else
                {
                    ParkenDataGrid.ItemsSource = parken;
                    MessageBox.Show("Geen parken gevonden", "Park not found", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                throw new UIException("ButtonClickZoeken", ex);
            }
         
        }

        private void DoubleClick_SelectedPark(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (ParkenDataGrid.SelectedItem is Park park)
                {
                    ReservatieBevestigenWindow rbw = new ReservatieBevestigenWindow(park,selectedKlant);
                    rbw.Show();
                }
            }
            catch(Exception ex) 
            {
                throw new UIException("DoubleClick_SelectedPark", ex);
            }
        }
    }
}
