using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Individuella.Model.DAL
{
    public class TagtypeDAL:DALBase
    {
        
        public List<Tagtype> GetTagForThread(int threadId)
        {
            // Skapar ett anslutningsobjekt.
            using (var conn = CreateConnection())
            {
                try
                {
                    
                    
                    // Skapar och initierar ett SqlCommand-objekt som används till att exekveras lagrad procedur. (Hämtar TypeID från Tagtypetabell)
                    var cmd = new SqlCommand("appSchema.usp_GetTagTypeForThreads", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    //Skickar med parametrar den lagrade proceduren kräver. (Mindre jobb för ASP.NET än addWithValue).
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = threadId;
                    

                    // Skapar det List-objekt som initialt har plats för 10 referenser till objekt.
                    var tags = new List<Tagtype>(10);

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en SELECT-sats som kan returnera flera poster varför
                    // ett SqlDataReader-objekt måste ta hand om alla poster. 

                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt som gör att vi kan få ut rätt index nedan.(Ett SqlDataReader-objekt tar hand om alla poster).
                    //Metoden ExecuteReader skapar ett SqlDataReader-objekt och returnerar en referens till objektet.
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        

                        //Via "GetOrdinal", får vi tillbaka rätt index som tillhör det angivna fältet. (Bättre än hårdkodat index).
                        var typeIdIndex = reader.GetOrdinal("TypeID");
                        var threadIdIndex = reader.GetOrdinal("ThreadID");
                        var tagIdIndex = reader.GetOrdinal("TagID");             
                        
                        

                        
                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.(While Statement Returns True).
                        while (reader.Read())
                        {
                             //Refererar till klassen "Tagg".
                            tags.Add(new Tagtype
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TypeID = reader.GetInt32(typeIdIndex),
                                ThreadID = reader.GetInt32(threadIdIndex),
                                TagID = reader.GetInt32(tagIdIndex)
                                



                            });
                        }
                       
                    }

                    //Ser till att antal data som retunerats till list-objektet stämmer överens med den hårdkodade antal platserna "10" som vi angett eller till X-antal data som retuneras(ej mer än 10). 
                    //Avallokerar oanvänt minne.
                    tags.TrimExcess();

                    // Returnerar referensen till List-objektet med referenser med Contact-objekt.
                    return tags;
                }
                catch
                {
                    
                    throw new ApplicationException("Fel! Det gick inte att hämta tag via tagtype id");
                }
            }
        }







        //Radera en befintlig TagtypeID genom att vi hämtat ut info(ThreadID) i annan metod och matchat med rätt "rad" i tagtype tabellen.
        public void DeleteTagtype(int TypeId)
        {
            //Skapar och initierar ett anslutningsobjekt genom basklassen (DALBase).
            //Använder Using (ist. för conn.Close) för att stänga ner objektet per automatik efter användning.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.usp_DeleteTagType", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_DeleteTagType", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till en parameter till ett kommando via metoden ".Add" som den lagrade proceduren kräver för att exekveras, så man kan radera en tagtyp 
                    //med specifikt ID.
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Value = TypeId;

                    //Öppnar anslutning till databasen.
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att Radera en TypeID (DELETE-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret, gick ej ta bort taggtyp.");
                }
            }
        }





        // Skapar ny Tagtyperad i databasen.
        public void InsertTagtype(int threadid, int tagid)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "appSchema.uspNewTagType", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.uspNewTagType", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till de parametrar som behövs för tillägg av ny Tagtype i proceduren, samt datatyper.
                  
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = threadid;
                    cmd.Parameters.Add("@TagID", SqlDbType.Int, 4).Value = tagid;

                    //Hämtar data från proceduren som har en parameter i sig av typen "Output" genom att skapa ett SqlParameter-Objekt
                    // av samma typ, genom egenskapen "Direction". Hämtar sedan den nya postens PK-värde efter att den lagrade proceduren
                    // exekverats, det nya värdet hamnar då i "@TypeID" för den nya skapade tagtypen.
                    // Hämtar det värdet som databasen tilldelat ThreadID.
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    //Öppnar anslutning till databasen
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att lägga till en ny tagtype(INSERT-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                    
                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret. Gick ej skapa taggtyp.");
                }

            }
        }
        
    }
}