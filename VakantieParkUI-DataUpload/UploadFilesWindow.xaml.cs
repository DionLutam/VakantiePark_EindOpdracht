using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Managers;
using VakantieParkDL_FileProcessor;
using VakantieParkDL_SQL;

namespace VakantieParkUI_DataUpload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UploadFilesWindow : Window
    {
        private OpenFileDialog fileDialog = new OpenFileDialog();
        private OpenFolderDialog folderDialog = new OpenFolderDialog();
        private IFileProcessor _fileProcessor;
        private IParkRepository _parkRepository;
        private FileManager _fileManager;
        private string destinationFolder;
        string conn = @"Data Source=sarmii99\sqlexpress;Initial Catalog=VakantiePark;Integrated Security=True;TrustServerCertificate=True";
        public UploadFilesWindow()
        {
            InitializeComponent();
            fileDialog.DefaultExt = ".zip"; // Default file extension
            fileDialog.Filter = "Zip files (.zip)|*.zip"; // Filter files by extension
            fileDialog.InitialDirectory = @"C:\School\tweede_semester\Programmeren_gevorderd\PG_Tom\Opdrachten\EindOpdracht";
            fileDialog.Multiselect = false;
            destinationFolder = @"C:\School\tweede_semester\Programmeren_gevorderd\PG_Tom\Opdrachten\EindOpdracht\Temp";
            _fileProcessor = new FileProcessor();
            _parkRepository = new ParkRepository(conn);
            _fileManager = new FileManager(_fileProcessor,_parkRepository);
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_fileManager.IsFolderEmpty(destinationFolder))
                {
                    if (MessageBox.Show($"Clean folder {destinationFolder}", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _fileManager.CleanFolder(destinationFolder);

                    }

                }
                _fileManager.ProcessFiles(SourceFileTextBox.Text, destinationFolder);
                MessageBox.Show("DataUpload Gelukt", "Success", MessageBoxButton.OK);
            }
            catch(Exception ex)  
            {
                throw new UIException("ExecuteButton_Click", ex);
            }
        }

        private void SourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            bool? result = fileDialog.ShowDialog();
            if (result == true && !string.IsNullOrEmpty(fileDialog.FileName))
            {
                SourceFileTextBox.Text = fileDialog.FileName;
                try
                {
                    //haalt de files names uit zipfile
                    List<string> fileNames = _fileManager.GetFilesFromZip(fileDialog.FileName);
                    ZipListBox.ItemsSource = fileNames;
                    //controleert of alle files in zipfile zitten mbv fileconfigtxt
                    _fileManager.CheckZipFile(fileDialog.FileName, fileNames);
                }
                catch (ZipFileManagerException ex)
                {
                    List<string> errors = new();
                    foreach (var key in ex.Data.Keys) errors.Add($"'{key}' - error : {ex.Data[key]}");
                    ZipListBox.ItemsSource = errors;
                    MessageBox.Show(ex.Message, "FileManager", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileManagerException ex)
                {
                    ZipListBox.ItemsSource = null;
                    MessageBox.Show(ex.Message, "FileManager", MessageBoxButton.OK, MessageBoxImage.Error);
                    SourceFileTextBox.Text = null;
                }


            }
        }
    }
}