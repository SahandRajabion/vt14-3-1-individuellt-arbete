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
     

        [Required(ErrorMessage = "Titel måste anges.")]
        [StringLength(30, ErrorMessage = "Titeln får endast innehålla max 30 tecken.")]
        public string Titel { get; set; }
        
        [StringLength(8000, ErrorMessage = "Trådens innehåll får endast innehålla max 8000 tecken.")]
        [Required(ErrorMessage = "Innehåll måste anges.")]
        public string Innehåll { get; set; }
        public DateTime Datum { get; set; }


    }
}