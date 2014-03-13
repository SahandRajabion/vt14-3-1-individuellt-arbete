using Individuella.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Individuella.Pages.ThreadPages
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
        
        }


             public void ThreadFormView_InsertItem1(Thread thread)
             {
                 if (ModelState.IsValid)
                 {
                     try
                     {
                         Service service = new Service();
                         service.SaveThread(thread);

                         Page.SetTempData("Msg", "Tråden har lagts till.");
                         Response.RedirectToRoute("Default");
                         Context.ApplicationInstance.CompleteRequest();

                     }
                     catch (Exception)
                     {
                         ModelState.AddModelError(String.Empty, "Ett fel inträffade när Tråden skulle skapas.");
                     }
                 }
             }

             public IEnumerable<Tagg> CheckBoxes_GetTags()
             {
                 Service service = new Service();
                 return service.GetTags();
             }

             protected void CheckBoxList1_DataBinding(object sender, EventArgs e)
             {

               
             }

        }
    }
