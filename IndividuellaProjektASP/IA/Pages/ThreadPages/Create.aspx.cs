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

                    ArrayList TagCheckId = new ArrayList();
                    CheckBoxList checkboxlist = (CheckBoxList)ThreadFormView.FindControl("CheckBoxList1");
                    foreach (ListItem fields in checkboxlist.Items)
                    {
                        if (fields.Selected)
                        {
                            TagCheckId.Add(int.Parse(fields.Value));
                        }
                    }


                         Service service = new Service();
                         service.SaveThread(thread);

                         for (int i = 0; i < TagCheckId.Count; i++)
                         {

                             service.InsertTagType(thread.ThreadID, (int)TagCheckId[i]);
                         
                         }


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
