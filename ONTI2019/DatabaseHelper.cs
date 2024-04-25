using _2019_onti.Models;
using ONTI2019.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ONTI2019
{
    public class DatabaseHelper
    {

        public static UserModel model = new UserModel();
        public static List<UserModel> cititori = new List<UserModel>();
        public static List<ImprumutModel> imprumut = new List<ImprumutModel>();
        public static List<RezervariModel> rezervari = new List<RezervariModel>();
        public static List<CartiModel> carti = new List<CartiModel>();

        public static void InsertIntoDB()
        {
          DeleteAll();
           ResetAllTables();
            InsertAll();
        }
        private static void InsertAll()
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                InsertUsers(con);
                InsertCarti(con);
                InsertRezervari(con);
                InsertImprumuturi(con);
            }
        }
        private static void InsertUsers(SqlConnection con)
        {
            using(StreamReader rdr = new StreamReader(Resources.utilizatoriString))
            {
                while(rdr.Peek()>=1)
                {
                    var line = rdr.ReadLine().Split(';');
                    using(SqlCommand cmd = new SqlCommand("Insert into Utilizatori values(@t,@n,@e,@p)", con))
                    {
                        cmd.Parameters.AddWithValue("t", Int32.Parse(line[0]));
                        cmd.Parameters.AddWithValue("n", line[1]);
                        cmd.Parameters.AddWithValue("e",line[2]);
                        cmd.Parameters.AddWithValue("p", PasswordEncryption.EncryptPassword(line[3].Trim()));
                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }
        public static bool CheckUtilizator(string email,string parola)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Select * from Utilizatori where(EmailUtilizator=@e and Parola = @p)";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("e", email);
                    cmd.Parameters.AddWithValue("p", parola);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try { 
                        rdr.Read();
                            Console.WriteLine(rdr.GetValue(0).ToString());
                            model.Id = rdr.GetInt32(0);
                            model.Tip=rdr.GetInt32(1);
                            model.Name= rdr.GetString(2);
                            model.Email=rdr.GetString(3);
                            model.Password=rdr.GetString(4);
                            return true;

                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }
        private static void InsertCarti(SqlConnection con)
        {
            using (StreamReader rdr = new StreamReader(Resources.cartiString))
            {
                while (rdr.Peek() >= 1)
                {
                    var line = rdr.ReadLine().Split(';');
                    using (SqlCommand cmd = new SqlCommand("Insert into Carti values(@t,@a,@p)", con))
                    {
                        cmd.Parameters.AddWithValue("t", line[0]);
                        cmd.Parameters.AddWithValue("a", line[1]);
                        cmd.Parameters.AddWithValue("p", Int32.Parse(line[2]));
                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }
        private static void InsertRezervari(SqlConnection con)
        {
            using (StreamReader rdr = new StreamReader(Resources.rezervariString))
            {
                while (rdr.Peek() >= 1)
                {
                    var line = rdr.ReadLine().Split(';');
                    using (SqlCommand cmd = new SqlCommand("Insert into Rezervari values(@ici,@ica,@d,@s)", con))
                    {
                        cmd.Parameters.AddWithValue("ici", Int32.Parse(line[0]));
                        cmd.Parameters.AddWithValue("ica", Int32.Parse(line[1]));
                        cmd.Parameters.AddWithValue("d", DateTime.ParseExact(line[2],"MM/dd/yyyy hh/mm/ss tt",CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("s", Int32.Parse(line[3]));
                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }
        private static void InsertImprumuturi(SqlConnection con)
        {
            using (StreamReader rdr = new StreamReader(Resources.imprumuturiString))
            {
                while (rdr.Peek() >= 1)
                {
                    var line = rdr.ReadLine().Split(';');
                    using (SqlCommand cmd = new SqlCommand("Insert into Imprumuturi values(@ici,@ica,@d,@ds)", con))
                    {
                        cmd.Parameters.AddWithValue("ici", Int32.Parse(line[0]));
                        cmd.Parameters.AddWithValue("ica", Int32.Parse(line[1]));
                        cmd.Parameters.AddWithValue("d", DateTime.ParseExact(line[2], "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("ds", DateTime.ParseExact(line[2], "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture).AddDays(30));
                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }
        private static void DeleteAll()
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                DeleteFromTable("Carti",con);
                DeleteFromTable("Utilizatori", con);
                DeleteFromTable("Imprumuturi", con);
                DeleteFromTable("Rezervari", con);
                
            }
        }
        private static void ResetAllTables()
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                ResetFromTable("Carti", con);
                ResetFromTable("Utilizatori", con);
                ResetFromTable("Imprumuturi", con);
                ResetFromTable("Rezervari", con);
            }
        }
        private static void DeleteFromTable(string table,SqlConnection con)
        {
            using(SqlCommand cmd = new SqlCommand("Delete From "+table, con))
            {
                cmd.ExecuteNonQuery();
            }
        }
        private static void ResetFromTable(string table,SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT('" + table + "',RESEED,0)", con))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public static void InsertUser(UserModel user)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString)) 
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Utilizatori values(@t,@n,@e,@p)", con))
                {
                    cmd.Parameters.AddWithValue("t", user.Tip);
                    cmd.Parameters.AddWithValue("n", user.Name);
                    cmd.Parameters.AddWithValue("e", user.Email);
                    cmd.Parameters.AddWithValue("p", user.Password);
                    cmd.ExecuteNonQuery();

                }
            }

        }
        public static void GetReaders()
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Select * from Utilizatori where TipUtilizator=2";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while(rdr.Read())
                        {
                            cititori.Add(new UserModel
                            {
                                Id = rdr.GetInt32(0),
                                Tip = rdr.GetInt32(1),

                                Name = rdr.GetString(2),
                                Email = rdr.GetString(3),
                                Password = rdr.GetString(4)
                            });
                        }
                    }
                }
            }
        }
        public static void GetCarti()
        {
            using (SqlConnection con = new SqlConnection(Properties.Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from Carti", con))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (rdr.Read())
                            {
                                carti.Add(new CartiModel { IdCarte = rdr.GetInt32(0), Titlu = rdr.GetString(1), Autor = rdr.GetString(2), Nrpag = rdr.GetInt32(3) });
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        public static void GetImprumuturi(int ic)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Select * from Imprumuturi where IdCititor=@ic";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("ic", ic);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            imprumut.Add(new ImprumutModel
                            {
                                IdImprumut = rdr.GetInt32(0),
                                IdCititor = rdr.GetInt32(1),

                                IdCarte = rdr.GetInt32(2),
                                DataImprumut = rdr.GetDateTime(3),
                                DataRestituire = rdr.GetDateTime(4)
                            });
                        }
                    }
                }
            }
        }
        public static void InsertImprumuturi(ImprumutModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Insert Into Imprumuturi values(@ic,@ica,@di,@dr)";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("ic", model.IdCititor);
                    cmd.Parameters.AddWithValue("ica", model.IdCarte);
                    cmd.Parameters.AddWithValue("di", model.DataImprumut);
                    cmd.Parameters.AddWithValue("dr", model.DataRestituire);
                    cmd.ExecuteNonQuery();
                }
           
            }
        }
        public static void InsertRezervari(RezervariModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Insert Into Rezervari values(@ic,@ica,@di,@dr)";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("ic", model.IdCititor);
                    cmd.Parameters.AddWithValue("ica", model.IdCarte);
                    cmd.Parameters.AddWithValue("di", model.DataRezervare);
                    cmd.Parameters.AddWithValue("dr", model.StatusRezervare);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void GetRezervari(int ic)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                string cmdText = "Select * from Rezervari where IdCititor=@ic";
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("ic", ic);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            rezervari.Add(new RezervariModel
                            {
                                IdRezervare = rdr.GetInt32(0),
                                IdCititor = rdr.GetInt32(1),

                                IdCarte = rdr.GetInt32(2),
                                DataRezervare = rdr.GetDateTime(3),
                                StatusRezervare = rdr.GetInt32(4)
                            });
                        }
                    }
                }
            }
        }
        public static void DeleteFromImprumuturi(int idimpr)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Delete From Imprumuturi where IdImprumut=@id", con))
                {
                    cmd.Parameters.AddWithValue("id", idimpr);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteFromRezervari(int idrez)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Delete From Rezervari where IdRezervare=@id", con))
                {
                    cmd.Parameters.AddWithValue("id", idrez);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
