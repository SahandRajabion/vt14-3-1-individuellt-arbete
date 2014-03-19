using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Individuella
{
    public static class ValidationEctension
    {
        //Validering av affärslogiklagret, ser till att en instans av ett objekt skickas, annars felmeddelande (Modelstate). 
        public static bool Validate<T>(this T instance, out ICollection<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(instance);
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(instance, validationContext, validationResults, true);
        }
    }

}





