using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Individuella.Model.DAL
{
    public class ThreadDAL:DALBase
    {

        //Får en referens av ett list objekt i return med alla Trådar i databasen
        public static IEnumerable<Thread> GetThreads()
        {
            //Skapar och initierar ett anslutningsobjekt genom basklassen.
            //Använder Using (ist. för conn.Close) för att stänga ner objektet per automatik efter användning.
            using (var conn = CreateConnection())
            {

                try
                {
                    //Skapar ett list-objekt som har plats för 100 referenser(hårdkodat) i Threads- objektet.
                    var Threads = new List<Thread>(100);

                    //Exekverar den lagrade proceduren "appSchema.usp_GetThreads", som har samma anslutningsobjekt (conn).
                    var cmd = new SqlCommand("appSchema.usp_GetThreads", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Öppnar upp en anslutning till databasen.
                    conn.Open();

                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt som gör att vi kan få ut rätt index nedan.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Via "GetOrdinal", får vi tillbaka rätt index som tillhör det angivna fältet. (Bättre än hårdkodat index).
                        var threadIdIndex = reader.GetOrdinal("ThreadID");
                        var titelIndex = reader.GetOrdinal("Titel");
                        var datumIndex = reader.GetOrdinal("Datum");

                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.(While Statement Returns True).
                        while (reader.Read())
                        {

                            //Refererar till klassen "Thread".
                            Threads.Add(new Thread
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                ThreadID = reader.GetInt32(threadIdIndex),
                                Titel = reader.GetString(titelIndex),
                                Datum = reader.GetDateTime(datumIndex)

                            });

                        }

                    }
                    //Ser till att antal data som retunerats till list-objektet stämmer överens med den hårdkodade antal platserna "10" som vi angett. 
                    Threads.TrimExcess();
                    return Threads;
                }

                catch (Exception)
                {
                    throw new ApplicationException("Fel uppstod när trådarna skulle hämtas från databasen.");
                }
            }

        }





        //Hämtar ut en Tråd i taget via ID.
        public Thread GetThreadById(int threadID)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.usp_GetThreadByID", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_GetThreadByID", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    ///Skickar med parametrar den lagrade proceduren kräver. (Mindre jobb för ASP.NET än addWithValue).
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = threadID;
                    

                    //Öppnar en anslutning.
                    conn.Open();


                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.
                        if (reader.Read())
                        {
                            int threadIdIndex = reader.GetOrdinal("ThreadID");
                            int katIdIndex = reader.GetOrdinal("KatID");
                            int userIdIndex = reader.GetOrdinal("UserID");
                            int titelIndex = reader.GetOrdinal("Titel");
                            int innehållIndex = reader.GetOrdinal("Innehåll");
                            int datumIndex = reader.GetOrdinal("Datum");

                            //Går igenom klassen "Thread" och retunerar datat en i taget.
                            return new Thread
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                ThreadID = reader.GetInt32(threadIdIndex),
                                KatID = reader.GetInt32(katIdIndex),
                                UserID = reader.GetInt32(userIdIndex),
                                Titel = reader.GetString(titelIndex),
                                Innehåll = reader.GetString(innehållIndex),
                                Datum = reader.GetDateTime(datumIndex)
                            };
                        }
                    }
                    //Retunerar null ifall det ej finns ngn data att hämta.
                    return null;
                }

                catch
                {
                    throw new ApplicationException("Fel har uppstått i dataåtkomstlagret.");
                }
            }
        }




        //Skapar ny Tråd i databasen.
        public void InsertThread(Thread thread)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.usp_NewThread", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_NewThread", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till de parametrar som behövs för tillägg av ny tråd i proceduren, samt datatyper.
                    cmd.Parameters.Add("@Titel", SqlDbType.VarChar, 30).Value = thread.Titel;
                    cmd.Parameters.Add("@Innehåll", SqlDbType.VarChar, 8000).Value = thread.Innehåll;
                   
                    //Hämtar data från proceduren som har en parameter i sig av typen "Output" genom att skapa ett SqlParameter-Objekt
                    // av samma typ, genom egenskapen "Direction". Hämtar sedan den nya postens PK-värde efter att den lagrade proceduren
                    // exekverats, det nya värdet hamnar då i "@ContactId" för den nya skapade kontakten.  
                    // Hämtar det värdet som databasen tilldelat ThreadID.
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    //Öppnar anslutning till databasen
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att lägga till en ny tråd(INSERT-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                    //Hämtar primärnyckelns nya värde den fått för den nya posten och tilldelar thread-objektet detta värde.
                    thread.ThreadID = (int)cmd.Parameters["@ThreadID"].Value;
                   
                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret. Gick ej skapa ny tråd.");
                }

            }
        }




        //Uppdaterar befintlig tråd.
        public void UpdateThread(Thread thread)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.usp_UpdateThread", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_UpdateThread", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till de parametrar som behövs för Uppdatering av ny tråd i proceduren, samt datatyper.
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = thread.ThreadID;
                    cmd.Parameters.Add("@Titel", SqlDbType.VarChar, 30).Value = thread.Titel;
                    cmd.Parameters.Add("@Innehåll", SqlDbType.VarChar, 8000).Value = thread.Innehåll;

                    //Öppnar anslutning till databasen
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att Uppdatera en ny post(UPDATE-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();
                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret, gick ej uppdatera tråd");
                }
            }
        }




        //Radera en befintlig tråd.
        public void DeleteThread(int threadID)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.usp_DeleteThread", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_DeleteThread", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Skickar med parametrar för att ta bort en tråd från databasen, så man kan ta bort tråden med rätt ID från tabellen.
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = threadID;

                    //Öppnar anslutning till databasen.
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att Radera en tråd(DELETE-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret, gick ej ta bort tråd.");
                }
            }
        }





    }
}