<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Individuella.Pages.ThreadPages.Create" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
      <h1>
        Skapa Ny Tråd
    </h1>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validation"  />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="validation" ValidationGroup="ValidationG" ShowModelStateErrors="false"/>

    <asp:FormView ID="ThreadFormView" runat="server"
        ItemType="Individuella.Model.Thread"
        DefaultMode="Insert"
        RenderOuterTable="false"
        InsertMethod="ThreadFormView_InsertItem1">
        <InsertItemTemplate>
           
            
             <div class="edit">
                <label for="Titel">Rubrik:</label>
            </div>
            <div class="field">
                <asp:TextBox ID="Titel" runat="server" Text='<%# BindItem.Titel %>' MaxLength="30" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="En Titel måste anges." ControlToValidate="Titel" Display="None" ValidationGroup="ValidationG"></asp:RequiredFieldValidator>
            </div>
            
           
            
             <div class="edit1">
                <label for="Innehåll">Innehåll:</label>
            </div>
            <div class="field1">
                <asp:TextBox ID="Innehåll" runat="server" Height="200" Text='<%# BindItem.Innehåll %>' TextMode="MultiLine" MaxLength="8000" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Innehåll måste anges." ControlToValidate="Innehåll" Display="None" ValidationGroup="ValidationG"></asp:RequiredFieldValidator>

            </div>
            
            <div class="edit2">
                <label for="Tag">Välj Taggar:</label>
            </div>
           
            
           <asp:CheckBoxList ID="CheckBoxList1" 
              SelectMethod="CheckBoxes_GetTags" 
              TextAlign="Left" runat="server" 
              OnDataBinding="CheckBoxList1_DataBinding"
              DataValueField="TagID" 
              DataTextField="Tag">
          </asp:CheckBoxList>

            <div>
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Spara" CommandName="Insert" CssClass="createknappar" />
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="createknappar" NavigateUrl='<%$ RouteUrl:routename=Default %>'>Tillbaka</asp:HyperLink>
               
            </div>


        </InsertItemTemplate>
    </asp:FormView>
</asp:Content>
