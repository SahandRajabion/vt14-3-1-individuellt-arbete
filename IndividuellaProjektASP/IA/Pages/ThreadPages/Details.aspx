<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Individuella.Pages.ThreadPages.Details" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="Panel" Visible="false" CssClass="success">
    <asp:Literal runat="server" ID="Literal" />
    <asp:ImageButton ID="CloseButton" runat="server" ImageUrl="~/Content/themes/base/images/close.png" CausesValidation="False" CssClass="closebutton" />
    </asp:Panel>
     
       
        
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validation"  />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="validation" ValidationGroup="ValidationGr" ShowModelStateErrors="false"/>
    <asp:ListView ID="ThreadListView" runat="server"
        ItemType="Individuella.Model.Thread"
        SelectMethod="ThreadListView_GetData"
        UpdateMethod="ThreadListView_UpdateItem"
        DeleteMethod="ThreadListView_DeleteItem"
        DataKeyNames="ThreadID">
        <LayoutTemplate>
             <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <h1><%#: Item.Titel %></h1>

            <p><%#: Item.Innehåll %></p>

            <span id="datum"><%#: Item.Datum.ToString("yyyy/MM/dd") %></span>

           
            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" Text="Ta bort" CssClass="Detailsbuttons" OnClientClick='<%# String.Format("return confirm (\"Är du säker att du vill ta bort befintlig tråd?\")") %>' CausesValidation="false" />
            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Edit" Text="Redigera" CssClass="Detailsbuttons" CausesValidation="false" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>
                Trådar saknas!
            </p>
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="editor-field0">
                <asp:TextBox ID="Titel" runat="server" Text='<%# BindItem.Titel %>' MaxLength="30" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="En Titel måste anges." ControlToValidate="Titel" Display="None" ValidationGroup="ValidationGr"></asp:RequiredFieldValidator>
            </div>
            <div class="field2">
                <asp:TextBox ID="Innehåll" runat="server" Text='<%# BindItem.Innehåll %>' height="200" TextMode="MultiLine" CssClass="innehåll" MaxLength="8000" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Innehåll måste anges." ControlToValidate="Innehåll" Display="None" ValidationGroup="ValidationGr"></asp:RequiredFieldValidator>
            </div>
            <div>
                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Update" Text="Spara" CssClass="Detailsbuttons" ValidationGroup="ValidationGr" />
                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Cancel" Text="Avbryt" ValidationGroup="ValidationGr"  CausesValidation="false" CssClass="Detailsbuttons" />
            </div>
        </EditItemTemplate>
    </asp:ListView>




    <asp:ListView ID="Listview" runat="server"
        ItemType="Individuella.Model.Tagtype"
        DataKeyNames="ThreadID, TypeID, TagID"
        SelectMethod="Listview_GetData"
        OnItemDataBound="Listview_ItemDataBound"
        DeleteMethod="TaggListView_DeleteItem">


        <LayoutTemplate>
       
               <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
    
                
            
        </LayoutTemplate>
      
           <ItemTemplate>
               <div id ="tagNames">
               <asp:Label ID="TagLabel" runat="server" Text="{0} " />
                   </div>

               <div id ="TaBortknappTag">
             <asp:LinkButton ID="LinkButton1" runat="server"  CommandName="Delete" Text="Ta bort" CssClass="DeteleTag" OnClientClick='<%# String.Format("return confirm (\"Är du säker att du vill ta bort befintlig tagg?\")") %>' CausesValidation="false" />
              </div>


           </ItemTemplate>

        
               
    </asp:ListView>
</asp:Content>