using Microsoft.Data.SqlClient;
using System.Data;
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

                    foreach(Reservatie reservatie in  reservaties)
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

                    cmd.Parameters.Add(new SqlParameter("@reservatieId",SqlDbType.Int));

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
    }
}

