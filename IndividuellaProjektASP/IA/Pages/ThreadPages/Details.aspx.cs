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


            //Finns det något meddelande användaren bör se, hämtas det genom "Page.GetTempData" i PageExctencion klassen.
            Literal.Text = Page.GetTempData("Msg") as string;
            Panel.Visible = !String.IsNullOrWhiteSpace(Literal.Text);


        }

        //Visar specifik tråd genom hämtat ID. Rätt tråd hämtat med "[RoutData]".
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

        //Tar bort en tråd med specifikt ID. Rätt tråd hämtat med "[RoutData]".
        public void ThreadListView_DeleteItem([RouteData] int ID)
        {
            try
            {
                Service.DeleteThread(ID);

                // Lägger till nytt meddelande i Page extension-metoden för användaren.
                Page.SetTempData("Msg", "Tråden är borttagen.");
                Response.RedirectToRoute("Default");
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel har inträffat när tråden skulle tas bort.");
            }
        }

        //Uppdaterar en tråd med specifikt ID. Rätt tråd hämtat med "[RoutData]".
        public void ThreadListView_UpdateItem([RouteData] int ID)
        {
            try
            {
                var thread = Service.GetThreadByID(ID);

                //Retuneras ett nullvärde när ID ska hämtas visas felmeddelande.
                if (thread == null)
                {
                    ModelState.AddModelError(String.Empty,
                    String.Format("Tråden kunde ej hittas."));
                    return;
                }

                if (TryUpdateModel(thread))
                {
                    Service.SaveThread(thread);

                    // Lägger till ett nytt meddelande i Page extension-metoden för användaren.
                    Page.SetTempData("Msg", "Tråden har uppdaterats.");
                    //Skickar tillbaka användaren från redigeringsläge till Details, med hjälp av rätt tråd ID.
                    Response.RedirectToRoute("ThreadDetails", new { id = thread.ThreadID });
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel har inträffat när tråden skulle uppdateras.");
            }
        }



        //Hämtar Taggtype. Retunerar den information som hämtats.
        public List<Tagtype> Listview_GetData([RouteData] int id)
        {
            try
            {
                return Service.GetTagForThread(id);
            }


            catch (Exception)
            {

                ModelState.AddModelError(String.Empty, "Ett fel inträffade då Tagtype skulle hämtas.");
                return null;
            }
        }


        //Hämtar Taggarna kopplade till tråden.
        protected void Listview_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //Hittar kontroll
            var label = e.Item.FindControl("TagLabel") as Label;

            //Om label inte är null går den in i IF-satsen... 
            if (label != null)
            {

                //... och typ omvandlar datat så vi kan använda oss utav nyckeln ...
                var type = (Tagtype)e.Item.DataItem;

                //... därefter(efter att taggarna hämtats) loopar vi igenom och matchar valda taggar(Id:n med retunerad data). 
                var tags = Service.GetTags().Single(ct => ct.TagID == type.TagID);

                //Skriver ut de taggar som valts.
                label.Text = String.Format(label.Text, tags.Tag);
            }




        }





        // Hämtar RouteData id från tidigare hämtad data, och lägger in den i egenskapen för vidare användning av "id".
        protected int Id
        {
            get { return int.Parse(RouteData.Values["id"].ToString()); }
        }


        //Tar bort Tagtype.
        public void TaggListView_DeleteItem(Tagtype tagtype)
        {
            try
            {
                // Hämtar först Tagtype.
                Service service = new Service();
                var tagtypes = service.GetTagForThread(Id);

                // Kontrollerar om minst 1 tagg finns kvar =(minst en  tagtype rad i tabellen).
                if (tagtypes.Count > 1)
                {
                    
                    //Skickas här vidare till service klassen för bortagning av rätt rad i Tagtype tabellen =(rätt tagg för användaren).
                    service.DeleteTagtype(tagtype.TypeID);

                    // Lägger till meddelande i Page extension-metoden för användaren.
                    Page.SetTempData("Msg", "Taggen är borttagen.");
                    Response.RedirectToRoute("ThreadDetails");
                    Context.ApplicationInstance.CompleteRequest();
                }

                else
                {
                    ModelState.AddModelError(String.Empty, "Går ej ta bort, tråden måste minst ha en tagg.");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett fel har inträffat när taggen skulle tas bort.");
            }
        }

    }

}


