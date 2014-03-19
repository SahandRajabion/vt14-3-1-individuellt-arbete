using Individuella.Model;
using System;
using System.Collections;
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
                         // Skapar en array-list objekt där vi kommer retunera alla olika alternativ i checkboxen
                         ArrayList TagCheckId = new ArrayList();
                         //Läser in "checkboxlist1" kontrollen.
                         CheckBoxList checkboxlist = (CheckBoxList)ThreadFormView.FindControl("CheckBoxList1");
                        
                         //För varje ikryssat alternativ, läggs alternativet in i arayen.
                         foreach (ListItem fields in checkboxlist.Items)
                         {
                             if (fields.Selected)
                             {
                                 //Typomvandlar alternativet till en Int innan den läggs till.
                                 TagCheckId.Add(int.Parse(fields.Value));
                             }
                         }

                         // Sert till att minst ett alternativ är ikryssat.
                         if (TagCheckId.Count == 0)
                         {

                             ModelState.AddModelError(string.Empty, "Minst en Tagg måste väljas.");
                         }

                         else
                         {  
                             
                             Service service = new Service();
                             service.SaveThread(thread);

                             //Loopar igeom och kollar om flera taggar har valts.
                             for (int i = 0; i < TagCheckId.Count; i++)
                             {
                                 //Skapar nya rader i relationsobjektet(Minst en ny rad) genom att skicka in...
                                 //... ThreadID samt arrayen bestående av Tag IDn.
                                 service.InsertTagType(thread.ThreadID, (int)TagCheckId[i]);

                             }

                             //Sätter nytt meddelande i "PAgeExtencion metoden"-
                             Page.SetTempData("Msg", "Tråden har lagts till.");
                             Response.RedirectToRoute("Default");
                             Context.ApplicationInstance.CompleteRequest();

                         }
                     }

                     catch (Exception)
                     {
                         ModelState.AddModelError(String.Empty, "Ett fel inträffade när Tråden skulle skapas.");
                     }
                 }
             }
             
             // Retunerar alla olika alternativ (Checkboxes) i en lista, så vi får dem synliga för användaren. 
             public IEnumerable<Tagg> CheckBoxes_GetTags()
             {
               
                 //Skapar ett service objekt och anropar ".GetTags" i serviceklassen.
                 Service service = new Service();
                 return service.GetTags();
             }

             protected void CheckBoxList1_DataBinding(object sender, EventArgs e)
             {
                
               
             }

        }
    }
