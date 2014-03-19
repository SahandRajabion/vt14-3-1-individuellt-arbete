using Individuella.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Individuella.Pages.ThreadPages
{
    public partial class Listing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
              // Om det finns något meddelande från annan sida, i Pageextension-metoden så hämtas det ut och visas för användaren.
            Literal.Text = Page.GetTempData("Msg") as string;
            Panel.Visible = !String.IsNullOrWhiteSpace(Literal.Text);
        }

             //Hämtar ut alla trådar i en lista för användaren (Bokstavs ordning).
             public IEnumerable<Thread> ThreadListView_GetData()
       
             {

             try
             {
            
                Service service = new Service();
                return service.GetThreads();
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel inträffade då Titlarna skulle läsas in.");
                return null;
            }
        }



        }
    }





  