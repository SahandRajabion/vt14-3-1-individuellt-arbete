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

        //Listobjekt, ej null får retuneras (IEnumerable).
        public static IEnumerable<Tagg> GetTags()
        {
            //Skapar och initierar ett anslutningsobjekt genom basklassen (DALBase).
            //Använder Using (ist. för conn.Close) för att stänga ner objektet per automatik efter användning.
            using (var conn = CreateConnection())
            {

                try
                {


                    //Exekverar den lagrade proceduren "appSchema.usp_GetTags", som har samma anslutningsobjekt (conn).
                    var cmd = new SqlCommand("appSchema.usp_GetTags", conn);
                    //Sätter om typen till Stored procedure då den av standard är av typen "Text".
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Skapar ett list-objekt som har plats för 10 referenser(hårdkodat) i Tagg- objektet.
                    var tags = new List<Tagg>(10);

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

                            //Refererar till klassen "Tagg".
                            tags.Add(new Tagg
                            {
                                //Tolkar posterna från databasen till C#, datatyper.
                                TagID = reader.GetInt32(tagIdIndex),
                                Tag = reader.GetString(tagIndex),
                              

                            });

                        }

                    }
                    //Ser till att antal data som retunerats till list-objektet stämmer överens med den hårdkodade antal platserna "10" som vi angett eller till X-antal data som retuneras(ej mer än 10). 
                    tags.TrimExcess();
                    return tags;
                }

                catch (Exception)
                {
                    throw new ApplicationException("Fel uppstod när taggarna skulle hämtas från databasen.");
                }
            }

        }


    }
}