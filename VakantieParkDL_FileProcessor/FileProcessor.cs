using System.IO.Compression;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Model;

namespace VakantieParkDL_FileProcessor;

public class FileProcessor : IFileProcessor
{
    public void ClearFolder(string folderName)
    {
        DirectoryInfo dir = new DirectoryInfo(folderName);
        foreach (FileInfo file in dir.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo di in dir.GetDirectories())
        {
            ClearFolder(di.FullName);
            di.Delete();
        }
    }

    /*ReadParkFiles: faciliteiten, huizen, park-faciliteit, park-huizen,park
        inlezen dan alles toevoegen aan park om park te maken*/

    //ReadKlantenFile: klanten inlezen

    //ReadReservatie(vorige lijsten meegeven): reservatie, huis-reservatie. 
    //probleemreservaties eruit halen
    public List<string> GetFileNamesConfigInfoFromZip(string fileName, string configName)
    {
        using (ZipArchive archive = ZipFile.OpenRead(fileName))
        {
            var entry = archive.GetEntry(configName);
            if (entry != null)
            {
                List<string> data = new();
                using (Stream entryStream = entry.Open())
                using (StreamReader reader = new StreamReader(entryStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        data.Add(line);
                    }
                    return data;
                }
            }
            else throw new FileNotFoundException($"{configName} not found");
        }
    }

    public List<string> GetFileNamesFromZip(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException($"{fileName} not found");

        }
        using (var zipFile = ZipFile.OpenRead(fileName))
        {
            return zipFile.Entries.Select(x => x.FullName).ToList();
        }
    }

    public bool IsFolderEmpty(string folderName)
    {
        DirectoryInfo dir = new DirectoryInfo(folderName);
        return (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0);
    }
    public Dictionary<int, Faciliteit> ReadFaciliteiten (Dictionary<string, string> checkedFiles, string destinationFolder)
    {
        try
        {
            Dictionary<int, Faciliteit> faciliteiten = new();
            //lees faciliteiten
            using (StreamReader ft = new StreamReader(Path.Combine(destinationFolder, checkedFiles["faciliteiten"])))
            {
                string line;
                while ((line = ft.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int faciliteitID = int.Parse(ss[0]);
                    string beschrijving = ss[1];

                    faciliteiten.Add(faciliteitID, new Faciliteit(faciliteitID, beschrijving));
                }
            }
            return faciliteiten;
        }
        catch (Exception ex)
        {
            throw new FileProcessorException("ReadFaciliteiten", ex);
        }
    }

    public List<Park> ReadParkFiles(Dictionary<string, string> checkedFiles, string destinationFolder, Dictionary<int,Faciliteit> faciliteiten)
    {
        try
        {

            Dictionary<int, int> huisParken = new();
            Dictionary<int, List<int>> parkFaciliteiten = new();
            Dictionary<int, Park> parken = new();
            Dictionary<int, Huis> huizen = new();

            //lees tussen tabel: Park_Faciliteit
            using (StreamReader pf = new StreamReader(Path.Combine(destinationFolder, checkedFiles["link_park_faciliteiten"])))
            {
                string line;
                while ((line = pf.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int parkID = int.Parse(ss[0]);
                    int faciliteitID = int.Parse(ss[1]);

                    if (parkFaciliteiten.ContainsKey(parkID))
                    {
                        parkFaciliteiten[parkID].Add(faciliteitID);
                    }
                    else
                    {
                        parkFaciliteiten[parkID] = new List<int> { faciliteitID };
                    }
                }
            }
            //lees tussen tabel: Park_Huis
            using (StreamReader ph = new StreamReader(Path.Combine(destinationFolder, checkedFiles["link_park_huizen"])))
            {
                string line;
                while ((line = ph.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int parkID = int.Parse(ss[0]);
                    int huisID = int.Parse(ss[1]);

                    huisParken.Add(huisID, parkID);
                }
            }


            //lees parken
            using (StreamReader prk = new StreamReader(Path.Combine(destinationFolder, checkedFiles["parken"])))
            {
                string line;
                while ((line = prk.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int parkID = int.Parse(ss[0]);
                    string parkNaam = ss[1];
                    string locatie = ss[2];

                    List<int> parkFaciliteitenIds = parkFaciliteiten[parkID];

                    List<Faciliteit> faciliteitenVoorPark = new List<Faciliteit>();

                    foreach (int faciliteitID in parkFaciliteitenIds)
                    {
                        if (faciliteiten.ContainsKey(faciliteitID))
                        {
                            Faciliteit faciliteit = faciliteiten[faciliteitID];
                            faciliteitenVoorPark.Add(faciliteit);
                        }
                    }

                    if (!parken.ContainsKey(parkID))
                    {
                        parken.Add(parkID, new Park(parkID, parkNaam, locatie, faciliteitenVoorPark));
                    }

                    faciliteiten
                        .Where(f => parkFaciliteitenIds.Contains(f.Key))
                        .Select(f => f.Value)
                        .ToList()
                        .ForEach(f => f.VoegParkToe(parken[parkID]));
                    

                }
            }

            //lees huizen 
            using (StreamReader hzn = new StreamReader(Path.Combine(destinationFolder, checkedFiles["huizen"])))
            {
                string line;
                Park park = null;
                int parkID = 0;
                while ((line = hzn.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int huisID = int.Parse(ss[0]);
                    string straat = ss[1];
                    int nummer = int.Parse(ss[2]);
                    bool IsActief = bool.Parse(ss[3]);
                    int capaciteit = int.Parse(ss[4]);

                    if (huisParken.ContainsKey(huisID))
                    {
                        parkID = huisParken[huisID];
                    }

                    if (parken.ContainsKey(parkID))
                    {
                        park = parken[parkID];
                    }

                    if (!huizen.ContainsKey(huisID))
                    {
                        huizen.Add(huisID, new Huis(huisID, straat, nummer, IsActief, capaciteit, park));
                        park.VoegHuisToe(huizen[huisID]);
                    }
                }
            }

            return parken.Values.ToList();

        }
        catch (Exception ex)
        {
            throw new FileProcessorException("ReadParkFiles", ex);
        }
    }
    public List<Klant> ReadKlantFiles(Dictionary<string, string> checkedFiles, string destinationFolder)
    {
        try
        {
            Dictionary<int, Klant> klanten = new Dictionary<int, Klant>();

            using (StreamReader kl = new StreamReader(Path.Combine(destinationFolder, checkedFiles["klanten"])))
            {
                string line;
                while ((line = kl.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split("|");
                    int klantID = int.Parse(ss[0]);
                    string naam = ss[1];
                    string adres = ss[2];

                    if (!klanten.ContainsKey(klantID))
                    {
                        klanten.Add(klantID, new Klant(klantID, naam, adres));
                    }
                }
            }
            return klanten.Values.ToList();

        }
        catch (Exception ex)
        {
            throw new FileProcessorException("ReadKlantFiles", ex);
        }
    }


    public List<Reservatie> ReadReservatieFiles(Dictionary<string, string> checkedFiles, List<Park> parken, List<Klant> klanten, string destinationFolder)
    {
        try
        {
            Dictionary<int, Reservatie> reservaties = new Dictionary<int, Reservatie>();
            Dictionary<int, int> reservatieHuis = new Dictionary<int, int>();
            //lees tussentabel huisReservatie
            using (StreamReader hr = new StreamReader(Path.Combine(destinationFolder, checkedFiles["link_huis_reservaties"])))
            {
                string line;
                while ((line = hr.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int reservatieID = int.Parse(ss[1]);
                    int huisID = int.Parse(ss[0]);

                    reservatieHuis.Add(reservatieID, huisID);

                }
            }

            //lees reservaties
            using (StreamReader reader = new StreamReader(Path.Combine(destinationFolder, checkedFiles["reservaties"])))
            {
                string line;
                int huisID;
                Huis huis=null;
                Klant klant = null;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] ss = line.Trim().Split(",");
                    int reservatieID = int.Parse(ss[0]);
                    DateTime StartDatum = DateTime.Parse(ss[1]);
                    DateTime EindDatum = DateTime.Parse(ss[2]);
                    int klantID = int.Parse(ss[3]);

                    if (!reservatieHuis.ContainsKey(reservatieID))
                    {
                        throw new FileProcessorException("ReservatieID niet in reservatieHuis");
                    }

                    huisID = reservatieHuis[reservatieID];
                    huis = parken.SelectMany(p => p.Huizen).FirstOrDefault(h => h.ID == huisID);
                    klant = klanten.FirstOrDefault(k => k.ID == klantID);

                    if (huis == null)
                    {
                        throw new FileProcessorException($"Huis with ID {huisID} not found");
                    }

                    if (klant == null)
                    {
                        throw new FileProcessorException($"Klant with ID {klantID} not found");
                    }

                    Reservatie reservatie = new Reservatie(reservatieID, StartDatum, EindDatum, klant, huis);
                    reservaties.Add(reservatieID, reservatie);
                    reservatie.ZetHuis(huis);
                    reservatie.ZetKlant(klant);
                    huis.VoegReservatieToe(reservatie);
                    klant.VoegReservatieToe(reservatie);
                }

            }

            return reservaties.Values.ToList();


        }
        catch (Exception ex)
        {
            throw new FileProcessorException("ReadReservatieFiles",ex);
        }
    }


    public void UnZip(string filename, string dir)
    {
        ZipFile.ExtractToDirectory(filename, dir);
    }
}

