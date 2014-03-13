using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Individuella.Model
{
    public class Thread
    {



        public int ThreadID { get; set; }
        public int KatID { get; set; }
        public int UserID { get; set; }
     

        [Required(ErrorMessage = "Innehåll måste anges.")]
        public string Titel { get; set; }
        [Required(ErrorMessage = "Innehåll måste anges.")]
        public string Innehåll { get; set; }
        public DateTime Datum { get; set; }


    }
}