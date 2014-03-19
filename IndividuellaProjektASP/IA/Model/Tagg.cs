using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Individuella.Model
{
    public class Tagg
    {


        public int TagID { get; set; }

        [Required(ErrorMessage = "Minst ett val måste anges.")]
        public string Tag { get; set; }


    }
}