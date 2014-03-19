using Individuella.Model.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Individuella.Model
{
    public class Service
    {

        //Skapar Fält.
        private TagDAL _tagDAL;

        //Skapar Fält.
        private TagtypeDAL _tagtypeDAL;

        //Skapar Fält.
        private ThreadDAL _threadDAL;



        //Skapar Egenskap.
        private TagDAL TagDAL
        {

            //Ett TagDAL-objekt skapas först då det behövs för första gången, 
            //det skapas då  ej ngt nytt objekt för varje gång tex. en ändring ska ske av taggen. 
            get { return _tagDAL ?? (_tagDAL = new TagDAL()); }

        }

        //Skapar Egenskap.
        private TagtypeDAL TagtypeDAL
        {

            //Ett TagtypeDAL-objekt skapas först då det behövs för första gången, 
            //det skapas då  ej ngt nytt objekt för varje gång tex. en ändring ska ske av taggtypen. 
            get { return _tagtypeDAL ?? (_tagtypeDAL = new TagtypeDAL()); }

        }

        //Skapar Egenskap.
        private ThreadDAL ThreadDAL
        {

            //Ett ThreadDAL-objekt skapas först då det behövs för första gången, 
            //det skapas då  ej ngt nytt objekt för varje gång tex. en ändring ska ske av tråden. 
            get { return _threadDAL ?? (_threadDAL = new ThreadDAL()); }

        }






        // Hämtar alla Taggar som finns tillgängliga i databasen.
        public IEnumerable<Tagg> GetTags(bool refresh = false)
        {
            // Försöker hämta lista med Taggar från cache minnet.
            var Tags = HttpContext.Current.Cache["Tagg"] as IEnumerable<Tagg>;

            // Om det inte finns en befintlig lista med taggar redan cashat i minnet så...
            if (Tags == null || refresh)
            {
                // ...hämtar den  en lista med Taggar från databasen igen...
                Tags = TagDAL.GetTags();

                // ...och cachar dessa. List-objektet med alla tagg-objekt, kommer att cachas 
                // under 10 minuters tid, därefter kommer de att automatiskt avallokeras från webbserverns primärminne.
                HttpContext.Current.Cache.Insert("Tagg", Tags, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }

            // Returnerar listan med taggarna.
            return Tags;
        }




        // Hämtar alla trådar som finns tillgängliga i databasen.
        public IEnumerable<Thread> GetThreads()
        {
            return ThreadDAL.GetThreads();
        }

        // Hämtar trådar med ett specifikt id från databasen.
        public Thread GetThreadByID(int threadID)
        {
            return ThreadDAL.GetThreadById(threadID);
        }

        // Tar bort tråd från databasen med specifikt ID.
        public void DeleteThread(int threadID)
        {
            ThreadDAL.DeleteThread(threadID);
        }


      

        //Spara Tråd efter uppdatering eller Insert.
        public void SaveThread(Thread thread)
        {

            // Validering på affärslogiklagret sker först. Om valideringen går igenom ...
            ICollection<ValidationResult> validationResults;
            if (!thread.Validate(out validationResults))
            {
                // ... kastas ett undantag med ett allmänt felmeddelande samt en referens till samlingen med resultat av valideringen
                var ex = new ValidationException("Objektet klarade inte av valideringen.");
                ex.Data.Add("ValidationResults", validationResults);
                throw ex;
            }

            // Tråd-objektet sparas antingen genom att en ny post skapats eller 
            // genom att en befintlig tråd uppdateras. Ny Tråd skapas om ID: et = 0 ( alltså ej har fått ngt ID än).
            if (thread.ThreadID == 0)
            {
                ThreadDAL.InsertThread(thread);
                
            }
            else
            {
                ThreadDAL.UpdateThread(thread);
               
            }
        }



      

        //Sparar en ny tagtype genom att skicka med behövande parametrar till metoden för Insert i tagtypeDAL.
        public void InsertTagType(int threadId, int tagTypeId)
        {
            TagtypeDAL.InsertTagtype(threadId, tagTypeId);
        }


        // Tar bort tagtype från databasen.
        public void DeleteTagtype(int TypeId)
        {
            TagtypeDAL.DeleteTagtype(TypeId);
        }


        //Hämtar ut information(data,typeID) från relationsobjektet i databasen via skicka med ThreadID som matchas mot befintlig typerad(ID) i tabellen.  
        public List<Tagtype> GetTagForThread(int threadId)
        {
            return TagtypeDAL.GetTagForThread(threadId);
        }

        }

    }
