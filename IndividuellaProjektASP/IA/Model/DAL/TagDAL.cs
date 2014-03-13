using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Individuella.Model.DAL
{
    public class TagDAL:DALBase
    {

        public static IEnumerable<Tagg> GetTags()
        {
            //Skapar och initierar ett anslutningsobjekt genom basklassen.
            //Använder Using (ist. för conn.Close) för att stänga ner objektet per automatik efter användning.
            using (var conn = CreateConnection())
            {

                try
                {
                    //Skapar ett list-objekt som har plats för 100 referenser(hårdkodat) i Contact- objektet.
                    var tags = new List<Tagg>(10);

                    //Exekverar den lagrade proceduren "Person.uspGetContacts", som har samma anslutningsobjekt (conn).
                    var cmd = new SqlCommand("appSchema.usp_GetTags", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Öppnar upp en anslutning till databasen.
                    conn.Open();

                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt som gör att vi kan få ut rätt index nedan.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Via "GetOrdinal", får vi tillbaka rätt index som tillhör det angivna fältet. (Bättre än hårdkodat index).
                        var tagIdIndex = reader.GetOrdinal("TagID");
                        var tagIndex = reader.GetOrdinal("Tag");
                     

                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.(While Statement Returns True).
                        while (reader.Read())
                        {

                            //Refererar till klassen "Contact".
                            tags.Add(new Tagg
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TagID = reader.GetInt32(tagIdIndex),
                                Tag = reader.GetString(tagIndex),
                              

                            });

                        }

                    }
                    //Ser till att antal data som retunerats till list-objektet stämmer överens med den hårdkodade antal platserna "100" som vi angett. 
                    tags.TrimExcess();
                    return tags;
                }

                catch (Exception)
                {
                    throw new ApplicationException("Fel uppstod när taggarna skulle hämtas från databasen.");
                }
            }

        }





        //Hämtar ut en kontakt i taget.
        public Tagg GetTTagById(int TagID)
        {
            //Skapar och initierar ett anslutningsobjekt.
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    //Exekverar den lagrade proceduren "Person.uspGetContact", som har samma anslutningsobjekt (conn).
                    SqlCommand cmd = new SqlCommand("appSchema.usp_GetTagByID", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Lägger till en parameter till ett kommando via metoden ".Add" som den lagrade proceduren behöver, så man kan hämta en kontakt,
                    //med specifikt ID.
                    cmd.Parameters.Add("@TagID", SqlDbType.Int, 4).Value = TagID;
                    //cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    //Öppnar en anslutning.
                    conn.Open();


                    //Exekverar SELECT-frågan i den lagrade proceduren och retunerar en Datareader-Objekt
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Loopar igenom det retunerade SqlDataReader-objektet tills det ej finns poster kvar att läsa.
                        if (reader.Read())
                        {
                            int tagIdIndex = reader.GetOrdinal("TagID");
                          
                            int tagIndex = reader.GetOrdinal("Tag");
                          
                            //Går igenom klassen "Contact" och retunerar datat en i taget.
                            return new Tagg
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TagID = reader.GetInt32(tagIdIndex),
                                Tag = reader.GetString(tagIndex),
                               
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











    }
}