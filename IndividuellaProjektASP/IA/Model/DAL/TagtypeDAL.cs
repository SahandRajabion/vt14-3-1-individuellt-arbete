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

        //Får en referens av ett list objekt i return med alla kontakter i databasen
        public static IEnumerable<Tagtype> GetTagtypes()
        {
            //Skapar och initierar ett anslutningsobjekt genom basklassen.
            //Använder Using (ist. för conn.Close) för att stänga ner objektet per automatik efter användning.
            using (var conn = CreateConnection())
            {

                try
                {
                    //Skapar ett list-objekt som har plats för 100 referenser(hårdkodat) i Contact- objektet.
                    var tagtypes = new List<Tagtype>(100);

                    //Exekverar den lagrade proceduren "Person.uspGetContacts", som har samma anslutningsobjekt (conn).
                    var cmd = new SqlCommand("appSchema.usp_GetTagtype", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Öppnar upp en anslutning till databasen.
                    conn.Open();

                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt som gör att vi kan få ut rätt index nedan.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Via "GetOrdinal", får vi tillbaka rätt index som tillhör det angivna fältet. (Bättre än hårdkodat index).
                        var typeIdIndex = reader.GetOrdinal("TypeID");
                        var ThreadIdIndex = reader.GetOrdinal("ThreadID");
                        var TagIdIndex = reader.GetOrdinal("TagID");
                       

                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.(While Statement Returns True).
                        while (reader.Read())
                        {

                            //Refererar till klassen "Contact".
                            tagtypes.Add(new Tagtype
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TypeID = reader.GetInt32(typeIdIndex),
                                ThreadID = reader.GetInt32(ThreadIdIndex),
                                TagID = reader.GetInt32(TagIdIndex),
                               

                            });

                        }

                    }
                    //Ser till att antal data som retunerats till list-objektet stämmer överens med den hårdkodade antal platserna "100" som vi angett. 
                    tagtypes.TrimExcess();
                    return tagtypes;
                }

                catch (Exception)
                {
                    throw new ApplicationException("Fel uppstod när tagg typerna skulle hämtas från databasen.");
                }
            }

        }







        //Hämtar ut en kontakt i taget.
        public Tagtype GetTagtypeByID(int typeId)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "Person.uspGetContact", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_GetTagtypesByID", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till en parameter till ett kommando via metoden ".Add" som den lagrade proceduren behöver, så man kan hämta en kontakt,
                    //med specifikt ID.
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Value = typeId;

                    //Öppnar en anslutning.
                    conn.Open();


                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.
                        if (reader.Read())
                        {
                            var typeIdIndex = reader.GetOrdinal("TypeID");
                            var ThreadIdIndex = reader.GetOrdinal("ThreadID");
                            var TagIdIndex = reader.GetOrdinal("TagID");

                            //Går igenom klassen "Contact" och retunerar datat en i taget.
                            return new Tagtype
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TypeID = reader.GetInt32(typeIdIndex),
                                ThreadID = reader.GetInt32(ThreadIdIndex),
                                TagID = reader.GetInt32(TagIdIndex),
                               

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




        //Radera en befintlig kontakt
        public void DeleteTagtype(int TypeId)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "Person.uspRemoveContact", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_DeleteTagtype", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Skickar med parametrar för att ta bort en kontakt från databasen, så man kan ta bort kontakt med rätt ID från tabellen.
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Value = TypeId;

                    //Öppnar anslutning till databasen.
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att Radera en kontakt(DELETE-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret, gick ej ta bort taggtyp.");
                }
            }
        }





        //"Insert New Contact" Skapar ny kontakt i databasen.
        public void InsertTagtype(int threadid, int tagid)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "Person.uspAddContact", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.uspNewTagType", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till de parametrar som behövs för tillägg av ny kontakt i proceduren, samt datatyper.
                    //cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Value = tagtype.TypeID;
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = threadid;
                    cmd.Parameters.Add("@TagID", SqlDbType.Int, 4).Value = tagid;

                    //Hämtar data från proceduren som har en parameter i sig av typen "Output" genom att skapa ett SqlParameter-Objekt
                    // av samma typ, genom egenskapen "Direction". Hämtar sedan den nya postens PK-värde efter att den lagrade proceduren
                    // exekverats, det nya värdet hamnar då i "@ContactId" för den nya skapade kontakten.  
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    //Öppnar anslutning till databasen
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att lägga till en ny post(INSERT-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();

                    //Hämtar primärnyckelns nya värde den fått för den nya posten och tilldelar Customer-objektet detta värde.
                    //tagtype.TypeID = (int)cmd.Parameters["@TypeID"].Value;
                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret. Gick ej skapa taggtyp.");
                }

            }
        }



        //Uppdaterar befintlig kontakt.
        public void UpdateTagtype(Tagtype tagtype)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "Person.uspUpdateContact", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.sup_UpdateTagtpe", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till de parametrar som behövs för Uppdatering av ny kontakt i proceduren, samt datatyper.
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int, 4).Value = tagtype.TypeID;
                    cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Value = tagtype.ThreadID;
                    cmd.Parameters.Add("@TagID", SqlDbType.Int, 4).Value = tagtype.TagID;

                    //Öppnar anslutning till databasen
                    conn.Open();

                    //Exekverar den del av den lagrade proceduren (ej SELECT) som används till att Uppdatera en ny post(UPDATE-sats).
                    //Antalet påverkade poster retuneras.
                    cmd.ExecuteNonQuery();
                }

                catch
                {
                    throw new ApplicationException("Ett fel har uppstått i dataåtkomst lagret, gick ej uppdatera taggtyp");
                }
            }
        }






        
    }
}