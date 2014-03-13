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
            // Försöker hämta lista med Taggar från cachen.
            var Tags = HttpContext.Current.Cache["Tagg"] as IEnumerable<Tagg>;

            // Om det inte finns det en lista med taggar...
            if (Tags == null || refresh)
            {
                // ...hämtar då lista med Taggar från databasen...
                Tags = TagDAL.GetTags();

                // ...och cachar dessa. List-objektet, inklusive alla tagg-objek, kommer att cachas 
                // under 10 minuter, varefter de automatiskt avallokeras från webbserverns primärminne.
                HttpContext.Current.Cache.Insert("Tagg", Tags, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }

            // Returnerar listan med taggar.
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

        // Hämtar trådar med ett specifikt id från databasen.
        public Tagg GetTagByID(int tagID)
        {
            return TagDAL.GetTTagById(tagID);
        }



        // Tar bort tråd från databasen.
        public void DeleteThread(int threadID)
        {
            ThreadDAL.DeleteThread(threadID);
        }


        //Spara Tråd efter uppdatering eller Insert.
        public void SaveThread(Thread thread)
        {

            // Validering på affärslogiklagret
            ICollection<ValidationResult> validationResults;
            if (!thread.Validate(out validationResults))
            {
                // kastas ett undantag med ett allmänt felmeddelande samt en referens till samlingen med resultat av valideringen
                var ex = new ValidationException("Objektet klarade inte av valideringen.");
                ex.Data.Add("ValidationResults", validationResults);
                throw ex;
            }

            // Tråd-objektet sparas antingen genom att en ny post eller
            // skapas  genom att en befintlig tråd uppdateras. Ny Tråd skapas om ID: et = 0.
            if (thread.ThreadID == 0)
            {
                ThreadDAL.InsertThread(thread);
            }
            else
            {
                ThreadDAL.UpdateThread(thread);
            }
        }



        // Hämtar alla trådar som finns tillgängliga i databasen.
        public IEnumerable<Tagtype> GetTagtypes()
        {
            return TagtypeDAL.GetTagtypes();
        }

        // Hämtar trådar med ett specifikt id från databasen.
        public Tagtype GetTagtypeByID(int typeId)
        {
            return TagtypeDAL.GetTagtypeByID(typeId);
        }


        // Tar bort tråd från databasen.
        public void DeleteTagtype(int TypeId)
        {
            TagtypeDAL.DeleteTagtype(TypeId);
        }


        //FUNKAR FORTFARANDE EJ

        ////Spara Tråd efter uppdatering eller Insert.
        //public void InsertTagtype(Tagtype tagtype)
        //{
        //    // Tråd-objektet sparas antingen genom att en ny post eller
        //    // skapas  genom att en befintlig tråd uppdateras.
        //    if (tagtype.TypeID == 0)
        //    {
        //        TagtypeDAL.InsertTagtype(tagtype);
        //    }
        //    else
        //    {
        //        TagtypeDAL.UpdateTagtype(tagtype);
        //    }
        //}

    }
}