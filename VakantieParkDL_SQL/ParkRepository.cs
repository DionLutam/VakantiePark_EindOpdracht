using Microsoft.Data.SqlClient;
using System.Data;
using VakantieParkBL.DTO;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Interfaces;
using VakantieParkBL.Model;

namespace VakantieParkDL_SQL
{
    public class ParkRepository : IParkRepository
    {
        private string connString;

        public ParkRepository(string connString)
        {
            this.connString = connString;
        }

        public Dictionary<int, Faciliteit> LeesFaciliteiten()
        {
            try
            {
                string query = "select * from Faciliteit;";
                Dictionary<int, Faciliteit> faciliteiten = new Dictionary<int, Faciliteit>();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!faciliteiten.ContainsKey((int)reader["id"]))
                        {
                            faciliteiten.Add((int)reader["id"], new Faciliteit((int)reader["id"], (string)reader["beschrijving"]));
                        }
                    }
                }
                return faciliteiten;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesFaciliteiten", ex);
            }

        }
        public Dictionary<int, List<int>> LeesParkFaciliteiten()
        {
            try
            {
                string query = "select * from Park_Faciliteit;";
                Dictionary<int, List<int>> parkfaciliteiten = new Dictionary<int, List<int>>();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (parkfaciliteiten.ContainsKey((int)reader["park_id"]))
                        {
                            parkfaciliteiten[(int)reader["park_id"]].Add((int)reader["faciliteit_id"]);
                        }
                        else
                        {
                            parkfaciliteiten.Add((int)reader["park_id"], new List<int> { (int)reader["faciliteit_id"] });
                        }
                    }
                }
                return parkfaciliteiten;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesParkFaciliteit", ex);
            }
        }
        public List<Huis> LeesHuizen(Park park)
        {
            try
            {
                string query = "select * from Huis where parkId = @parkId;";
                Dictionary<int, Huis> huizen = new Dictionary<int, Huis>();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@parkId", park.Id);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!huizen.ContainsKey((int)reader["id"]))
                        {
                            huizen.Add((int)reader["id"], new Huis((int)reader["id"], (string)reader["straat"],
                                (int)reader["nummer"], (bool)reader["IsActief"], (int)reader["capaciteit"], park));
                        }
                    }
                }
                return huizen.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesHuizen", ex);
            }
        }

        public IReadOnlyCollection<Klant> LeesKlanten(string klantNaam)
        {
            try
            {
                string query = "select * from Klant where naam=@klantNaam;";
                Dictionary<int, Klant> klanten = new Dictionary<int, Klant>();

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantNaam", klantNaam);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!klanten.ContainsKey((int)reader["id"]))
                        {
                            klanten.Add((int)reader["id"], new Klant((int)reader["id"], (string)reader["naam"], (string)reader["adres"]));
                        }
                    }

                }
                return klanten.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesKlanten", ex);
            }

        }

        public Park LeesPark(int huisID, Dictionary<int, Faciliteit> faciliteiten)
        {
            try
            {
                string query = "select p.id,p.naam,p.locatie,f.faciliteit_id from Park p join Park_faciliteit f on p.id = f.park_id join Huis h on h.parkId = p.id where h.id = @huisID;";
                List<int> parkFaciliteitenIds = new();
                List<Faciliteit> parkFaciliteiten = new List<Faciliteit>();
                string parkNaam = string.Empty;
                string parkLocatie = string.Empty;
                int parkID = 0;
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@huisID", huisID);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        parkFaciliteitenIds.Add((int)reader["faciliteit_id"]);
                        parkNaam = (string)reader["naam"];
                        parkLocatie = (string)reader["locatie"];
                        parkID = (int)reader["id"];
                    }

                }
                foreach (int id in parkFaciliteitenIds)
                {
                    if (faciliteiten.ContainsKey(id))
                    {
                        parkFaciliteiten.Add(faciliteiten[id]);
                    }
                }
                Park park = new Park(parkID, parkNaam, parkLocatie, parkFaciliteiten);
                return park;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesPark", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> LeesProbleemReservaties(int? huisID)
        {
            try
            {
                string query = "select r.id reservatieid, p.naam parknaam, k.id,r.startdatum, r.einddatum, h.nummer from Reservatie r " +
                    "join Klant k on r.klantId = k.id join Huis h on r.huisId = h.id " +
                    "join Park p on p.id = h.parkId join ProbleemReservatie pr on r.id = pr.reservatieId";
                if (huisID != null)
                {
                    query += " where h.id = @huisID;";
                }
                Dictionary<int, ReservatieInfo> probleemReservaties = new Dictionary<int, ReservatieInfo>();

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    if (huisID != null)
                    {
                        cmd.Parameters.AddWithValue("@huisID", huisID);
                    }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!probleemReservaties.ContainsKey((int)reader["reservatieid"]))
                        {
                            probleemReservaties.Add((int)reader["reservatieid"], new ReservatieInfo((int)reader["reservatieid"],
                                (string)reader["parknaam"], (int)reader["id"], (DateTime)reader["startdatum"], (DateTime)reader["einddatum"], (int)reader["nummer"]));
                        }
                        else
                        {
                            throw new RepositoryException("probleem reservatie dubbel");
                        }
                    }
                }
                return probleemReservaties.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Lees Probleem Reservaties", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> LeesReservaties(int klantId)
        {
            try
            {
                string query = "select p.naam, p.locatie, r.startdatum, r.einddatum, h.nummer from Reservatie r " +
                    "join huis h on r.huisId = h.id " +
                    "join park p on h.parkId = p.id where r.klantId=@klantId order by r.startdatum;";
                List<ReservatieInfo> reservatieLijst = new List<ReservatieInfo>();

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantId", klantId);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reservatieLijst.Add(new ReservatieInfo((string)reader["naam"], (string)reader["locatie"],
                            (DateTime)reader["startdatum"], (DateTime)reader["einddatum"], (int)reader["nummer"]));
                    }
                }
                return reservatieLijst;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesReservatiesVieKlantId", ex);
            }
        }

        public IReadOnlyCollection<ReservatieInfo> LeesReservaties(string? parkNaam, DateTime? startDatum, DateTime? eindDatum)
        {
            try
            {
                string query = "select p.naam parknaam, k.id, k.naam, r.startdatum, r.einddatum, h.nummer from Reservatie r" +
                    " join Klant k on r.klantId = k.id join Huis h on r.huisId = h.id join Park p on p.id =" +
                    " h.parkId";
                if (parkNaam != null)
                {
                    query += " where p.naam=@parkNaam";
                }
                else if (eindDatum == null)
                {
                    query += " where r.startdatum=@startDatum";
                }
                else if (startDatum == null)
                {
                    query += " where r.einddatum=@eindDatum";
                }
                else
                {
                    query += " where r.startdatum=@startDatum and einddatum=@eindDatum";
                }
                if (startDatum != null && parkNaam != null)
                {
                    query += " and r.startdatum=@startDatum";
                }
                if (eindDatum != null && parkNaam != null)
                {
                    query += " and r.einddatum= @eindDatum";
                }
                List<ReservatieInfo> reservatieLijst = new();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    if (parkNaam != null)
                    {
                        cmd.Parameters.AddWithValue("@parkNaam", parkNaam);
                    }
                    if (startDatum != null)
                    {
                        cmd.Parameters.AddWithValue("@startDatum", startDatum);
                    }
                    if (eindDatum != null)
                    {
                        cmd.Parameters.AddWithValue("@eindDatum", eindDatum);
                    }

                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (parkNaam != null)
                        {
                            reservatieLijst.Add(new ReservatieInfo((int)reader["id"], (string)reader["naam"],
                                (DateTime)reader["startdatum"], (DateTime)reader["einddatum"], (int)reader["nummer"]));
                        }
                        else
                        {
                            reservatieLijst.Add(new ReservatieInfo((string)reader["parknaam"], (int)reader["id"], (string)reader["naam"], (int)reader["nummer"]));
                        }

                    }
                }
                return reservatieLijst;

            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesReservaties", ex);
            }
        }
        public Klant LeesKlant(int klantId)
        {
            try
            {
                string query = "select * from Klant where id = @klantId;";
                Klant klant = null;
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantId", klantId);
                    conn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        klant = new Klant((int)reader["id"], (string)reader["naam"], (string)reader["adres"]);
                    }
                    return klant;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesKlant", ex);
            }

        }

        public List<Reservatie> LeesReservaties(Huis huis)
        {
            try
            {
                string query = "select * from Reservatie where huisId = @huisId;";
                Dictionary<int, Reservatie> reservaties = new Dictionary<int, Reservatie>();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@huisId", huis.ID);
                    conn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reservaties.ContainsKey((int)reader["id"]))
                        {
                            reservaties.Add((int)reader["id"], new Reservatie((int)reader["id"], (DateTime)reader["startdatum"], (DateTime)reader["einddatum"], LeesKlant((int)reader["KlantId"]), huis));
                        }
                    }
                    return reservaties.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesReservaties(Huis)", ex);
            }
        }

        public void SchrijfKlanten(List<Klant> klanten)
        {
            //klant query


            string query = "INSERT INTO Klant(id,naam,adres) VALUES (@id,@naam,@adres);";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.CommandText = query;

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.NVarChar));

                    foreach (Klant klant in klanten)
                    {
                        cmd.Parameters["@id"].Value = klant.ID;
                        cmd.Parameters["@naam"].Value = klant.Naam;
                        cmd.Parameters["@adres"].Value = klant.Adres;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Transaction.Commit();
                }

                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new RepositoryException("SchrijfKlanten", ex);
                }



            }
        }

        public void SchrijfParken(List<Park> parken, List<Faciliteit> faciliteiten)
        {


            string parkQuery = "INSERT INTO Park(id,naam,locatie) VALUES (@id,@naam,@locatie);";
            string faciliteitQuery = "INSERT INTO Faciliteit(id,beschrijving) VALUES (@id,@beschrijving);";
            string parkFaciliteitQuery = "INSERT INTO Park_faciliteit(park_Id, faciliteit_Id) VALUES (@parkId, @faciliteitId);";
            string huizenQuery = "INSERT INTO Huis(id,straat,nummer,IsActief,capaciteit,parkId) VALUES " +
                "(@id,@straat,@nummer,@IsActief,@capaciteit,@parkId);";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {


                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.CommandText = parkQuery;
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@locatie", SqlDbType.NVarChar));

                    foreach (Park park in parken)
                    {
                        cmd.Parameters["@id"].Value = park.Id;
                        cmd.Parameters["@naam"].Value = park.Naam;
                        cmd.Parameters["@locatie"].Value = park.Locatie;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = faciliteitQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@beschrijving", SqlDbType.NVarChar));

                    foreach (Faciliteit faciliteit in faciliteiten)
                    {
                        cmd.Parameters["@id"].Value = faciliteit.Id;
                        cmd.Parameters["@beschrijving"].Value = faciliteit.Beschrijving;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = parkFaciliteitQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@parkId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@faciliteitId", SqlDbType.Int));

                    foreach (Park park in parken)
                    {
                        foreach (Faciliteit faciliteit in park.Faciliteiten)
                        {
                            cmd.Parameters["@parkId"].Value = park.Id;
                            cmd.Parameters["@faciliteitId"].Value = faciliteit.Id;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cmd.CommandText = huizenQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@nummer", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@IsActief", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@capaciteit", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@parkId", SqlDbType.Int));

                    foreach (Park park in parken)
                    {
                        foreach (Huis huis in park.Huizen)
                        {
                            cmd.Parameters["@id"].Value = huis.ID;
                            cmd.Parameters["@straat"].Value = huis.Straat;
                            cmd.Parameters["@nummer"].Value = huis.Nummer;
                            cmd.Parameters["@IsActief"].Value = huis.IsActief;
                            cmd.Parameters["@capaciteit"].Value = huis.Capaciteit;
                            cmd.Parameters["@parkId"].Value = park.Id;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    cmd.Transaction.Commit();




                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new RepositoryException("SchrijfParken", ex);
                }

            }

            //park query
            //huis query
            //faciliteit query
            //park_faciliteit_query
        }

        public void SchrijfReservaties(List<Reservatie> reservaties, List<Park> parken)
        {
            string reservatieQuery = "INSERT INTO Reservatie(startdatum,einddatum,klantId,huisId) output inserted.id VALUES " +
                "(@startdatum,@einddatum,@klantId,@huisId);";
            string probleemReservatieQuery = "INSERT INTO ProbleemReservatie(reservatieId) VALUES (@reservatieId);";

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.CommandText = reservatieQuery;
                    cmd.Parameters.Add(new SqlParameter("@startdatum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@einddatum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@klantId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@huisId", SqlDbType.Int));

                    foreach (Reservatie reservatie in reservaties)
                    {
                        cmd.Parameters["@startdatum"].Value = reservatie.StartDatum;
                        cmd.Parameters["@einddatum"].Value = reservatie.EndDatum;
                        cmd.Parameters["@klantId"].Value = reservatie.Klant.ID;
                        cmd.Parameters["@huisId"].Value = reservatie.Huis.ID;
                        int newID = (int)cmd.ExecuteScalar();
                        reservatie.ZetID(newID);
                    }

                    cmd.Parameters.Clear();
                    cmd.CommandText = probleemReservatieQuery;

                    cmd.Parameters.Add(new SqlParameter("@reservatieId", SqlDbType.Int));

                    IEnumerable<Reservatie> probleemReservaties = parken
                        .SelectMany(park => park.Huizen)
                        .SelectMany(huis => huis.ProbleemReservaties);

                    foreach (Reservatie reservatie in probleemReservaties)
                    {
                        cmd.Parameters["@reservatieId"].Value = reservatie.Id;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new RepositoryException("SchrijfReservaties", ex);
                }
            }
            //reservatie query
            //probleemreservatie query
        }

        public void SchrijfProbleemReservaties(Huis huis)
        {
            string PRquery = "INSERT INTO ProbleemReservatie (reservatieId) VALUES (@reservatieId);";
            string IsActiefQuery = "update Huis Set IsActief='0' where Huis.id= @huisId;";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.CommandText = PRquery;

                    cmd.Parameters.Add(new SqlParameter("@reservatieId", SqlDbType.Int));

                    foreach (Reservatie reservatie in huis.ProbleemReservaties)
                    {
                        cmd.Parameters["@reservatieId"].Value = reservatie.Id;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = IsActiefQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@huisId", huis.ID);
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new RepositoryException("SchrijfProbleemReservaties", ex);
                }
            }

        }

        public IReadOnlyCollection<Park> LeesParken(Dictionary<int, Faciliteit> faciliteiten)
        {
            try
            {
                Dictionary<int, List<int>> parkFaciliteitenIds = LeesParkFaciliteiten();
                string query = "select * from park;";
                Dictionary<int, Park> parken = new Dictionary<int, Park>();
                int parkId = 0;
                string parkNaam = string.Empty;
                string parkLocatie = string.Empty;

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    conn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        parkId = (int)reader["id"];
                        parkNaam = (string)reader["naam"];
                        parkLocatie = (string)reader["locatie"];
                        List<Faciliteit> parkFaciliteiten = new List<Faciliteit>();
                        foreach (int faciliteitId in parkFaciliteitenIds[parkId])
                        {
                            if (faciliteiten.ContainsKey(faciliteitId))
                            {
                                parkFaciliteiten.Add(faciliteiten[faciliteitId]);
                            }
                        }
                        if (!parken.ContainsKey(parkId))
                        {
                            parken.Add(parkId, new Park(parkId, parkNaam, parkLocatie, parkFaciliteiten));
                        }
                    }
                }
                return parken.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesParken", ex);
            }
        }

        public List<string> LeesParkLocaties()
        {
            try
            {
                string query = "select locatie from Park;";
                List<string> locaties = new List<string>();
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        locaties.Add((string)reader["locatie"]);
                    }
                }
                return locaties;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("LeesParkLocaties", ex);
            }
        }

        public void SchrijfReservatie(Reservatie reservatie)
        {
            string reservatieQuery = "INSERT INTO Reservatie(startdatum,einddatum,klantId,huisId) output inserted.id VALUES " +
            "(@startdatum,@einddatum,@klantId,@huisId);";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = reservatieQuery;
                    cmd.Parameters.AddWithValue("@startdatum", reservatie.StartDatum);
                    cmd.Parameters.AddWithValue("@einddatum",reservatie.EndDatum);
                    cmd.Parameters.AddWithValue("@klantId",reservatie.Klant.ID);
                    cmd.Parameters.AddWithValue("@huisId",reservatie.Huis.ID);
                    int newID = (int)cmd.ExecuteScalar();
                    reservatie.ZetID(newID);
                }
                catch (Exception ex) 
                {
                    throw new RepositoryException("SchrijfReservatie", ex);
                }
            }

        }
    }
}

