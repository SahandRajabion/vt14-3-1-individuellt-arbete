using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Individuella.Model;
using System.Web.ModelBinding;
using System.Collections;

namespace Individuella.Pages.ThreadPages
{
    public partial class Details : System.Web.UI.Page
    {

        private Service _service;

        private Service Service
        {
            get { return _service ?? (_service = new Service()); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {



            Literal.Text = Page.GetTempData("Msg") as string;
            Literal.Visible = !String.IsNullOrWhiteSpace(Literal.Text);


        }



        public Thread ThreadListView_GetData([RouteData] int ID)
        {
            try
            {
                return Service.GetThreadByID(ID);
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Fel inträffade då Tråden skulle Läsas.");
                return null;
            }
        }


        public void ThreadListView_DeleteItem([RouteData] int ID)
        {
            try
            {
                Service.DeleteThread(ID);

                // Lägger till meddelande i Page extension-metoden
                Page.SetTempData("Msg", "Tråden är borttagen.");
                Response.RedirectToRoute("Default");
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel har inträffat när tråden skulle tas bort.");
            }
        }


        public void ThreadListView_UpdateItem([RouteData] int ID)
        {
            try
            {
                var thread = Service.GetThreadByID(ID);

                if (thread == null)
                {
                    ModelState.AddModelError(String.Empty,
                    String.Format("Tråden kunde ej hittas."));
                    return;
                }

                if (TryUpdateModel(thread))
                {
                    Service.SaveThread(thread);

                    // Lägger till ett meddelande i Page extension-metoden
                    Page.SetTempData("Msg", "Tråden har uppdaterats.");
                    Response.RedirectToRoute("ThreadDetails", new { id = thread.ThreadID });
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel har inträffat när tråden skulle uppdateras.");
            }
        }



        public IEnumerable<Tagg> CheckBoxListReadOnly_GetData()
        {

            Service service = new Service();
            return service.GetTags();
        }

        protected void CheckBoxListReadOnly_DataBinding(object sender, EventArgs e)
        {

            //FUNGERAR EJ 

            //ArrayList TagCheckId = new ArrayList();
          
            //CheckBoxList checkboxlist = (CheckBoxList)ThreadListView.FindControl("CheckBoxListReadOnly");
            //foreach (ListItem fields in checkboxlist.Items)
            //{
            //    if (fields.Selected)
            //    {

            //        TagCheckId.Add((string)fields.Value);
                   
            //    }
            //}


            








        }
    }
}