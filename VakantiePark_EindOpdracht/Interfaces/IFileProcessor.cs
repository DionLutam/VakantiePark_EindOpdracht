using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Model;

namespace VakantieParkBL.Interfaces
{
    public interface IFileProcessor
    {
        List<string> GetFileNamesFromZip(string fileName);
        List<string> GetFileNamesConfigInfoFromZip(string zipfile, string configfile);
        void UnZip(string zipFileName, string destinationFolder);
        List<Park> ReadParkFiles(Dictionary<string, string> checkedFiles, string destinationFolder, Dictionary<int,Faciliteit> faciliteiten);
        List<Klant> ReadKlantFiles(Dictionary<string, string> checkedFiles, string destinationFolder);
        List<Reservatie> ReadReservatieFiles(Dictionary<string, string> checkedFiles, List<Park> parken, List<Klant> klanten, string destinationFolder);
        bool IsFolderEmpty(string folderName);
        void ClearFolder(string folderName);
        Dictionary<int, Faciliteit> ReadFaciliteiten(Dictionary<string, string> checkedFiles, string destinationFolder);
    }
}
