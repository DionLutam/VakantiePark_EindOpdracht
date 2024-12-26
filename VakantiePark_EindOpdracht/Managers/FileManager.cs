using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Model;

namespace VakantieParkBL.Managers
{
    public class FileManager
    {
        private IFileProcessor _fileProcessor;
        private IParkRepository _parkRepository;

        public FileManager (IFileProcessor fileProcessor, IParkRepository parkRepository)
        {
            _fileProcessor = fileProcessor;
            _parkRepository = parkRepository;
        }

        public List<string> GetFilesFromZip(string fileName)
        {
            try
            {
                var names = _fileProcessor.GetFileNamesFromZip(fileName);
                return names;
            }
            catch (Exception ex)
            {
                throw new FileManagerException($"GetFilesFromZip - {ex.Message}", ex);
            }
        }
        public Dictionary<string, string> CheckZipFile(string fileName, List<string> fileNames)
        {
            try
            {
                Dictionary<string, string> map = new Dictionary<string, string>();
                List<string> configEntries = new List<string>()
                {
                    "faciliteiten",
                    "link_huis_reservaties",
                    "huizen",
                    "klanten",
                    "link_park_huizen",
                    "parken",
                    "link_park_faciliteiten",
                    "reservaties"

                };
                Dictionary<string, string> errors = new();

                if (!fileNames.Contains("FileNamesConfig.txt"))
                {


                    throw new ZipFileManagerException("FileNamesConfig.txt is missing");
                }
                var data = _fileProcessor.GetFileNamesConfigInfoFromZip(fileName, "FileNamesConfig.txt");
                foreach (var line in data)
                {
                    string[] parts = line.ToString().Split(':');
                    map.Add(parts[0].Trim(), parts[1].Trim().Replace('\"'.ToString(), string.Empty));
                }

                foreach (string entry in configEntries)
                {
                    if (!map.ContainsKey(entry))
                    {
                        errors.Add(entry, "missing in config");
                    }
                }

                foreach (string file in map.Values)
                {
                    if (!fileNames.Contains(file))
                    {
                        errors.Add(file, "missing in zip");
                    }
                }

                if (errors.Count > 0)
                {
                    ZipFileManagerException ex = new ZipFileManagerException("Files Missing");
                    foreach (var e in errors)
                    {
                        ex.Data.Add(e.Key, e.Value);
                    }
                    throw ex;
                }
                return map;

            }
            catch (ZipFileManagerException) { throw; }
            catch (Exception e)
            {
                throw new FileManagerException($"CheckZipFile - {e.Message}", e);
            }


        }

        public void ProcessFiles(string zipFileName, string destinationFolder)
        {
            try
            {
                _fileProcessor.UnZip(zipFileName, destinationFolder);
                List<string> files = GetFilesFromZip(zipFileName);
                Dictionary<string, string> checkedFiles = CheckZipFile(zipFileName, files);
                Dictionary<int,Faciliteit> faciliteiten = _fileProcessor.ReadFaciliteiten(checkedFiles, destinationFolder);
                List<Park> parken = _fileProcessor.ReadParkFiles(checkedFiles, destinationFolder, faciliteiten);
                List<Klant> klanten = _fileProcessor.ReadKlantFiles(checkedFiles, destinationFolder);
                List<Reservatie> reservaties = _fileProcessor.ReadReservatieFiles(checkedFiles, parken, klanten, destinationFolder);


                _parkRepository.SchrijfParken(parken, faciliteiten.Values.ToList());
                _parkRepository.SchrijfKlanten(klanten);
                _parkRepository.SchrijfReservaties(reservaties, parken);
            }
            catch (Exception ex)
            {
                throw new FileManagerException("ProcessFiles", ex);
            }
            //parkfiles inlezen en parken lijst aanmaken
            //klantfiles inlezen en klanten aanmaken
            //reservatie files inlezen en reservaties aanmaken

            //parken/klanten/reservaties wegschrijven
        }

        public bool IsFolderEmpty(string folderName)
        {
            try
            {
                return _fileProcessor.IsFolderEmpty(folderName);
            }
            catch (Exception e) { throw new FileManagerException($"IsFolderEmpty - {e.Message}"); }
        }

        public void CleanFolder(string folderName)
        {
            try
            {
                _fileProcessor.ClearFolder(folderName);
            }
            catch (Exception e)
            {
                throw new FileManagerException($"CleanFolder - {e.Message}");
            }
        }
    }
}
