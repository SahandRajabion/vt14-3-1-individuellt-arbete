<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Individuella.Pages.ThreadPages.Details" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="Panel" Visible="false" CssClass="successsaved">
    <asp:Literal runat="server" ID="Literal" />
    </asp:Panel>


    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validationsummary" />
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

            <span id="datum"><%#: Item.Datum %></span>

            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" Text="Ta bort" OnClientClick='<%# String.Format("return confirm (\"Är du säker att du vill ta bort befintlig tråd?\")") %>' CausesValidation="false" />
            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Edit" Text="Redigera" CausesValidation="false" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>
                Trådar saknas!
            </p>
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="editor-field0">
                <asp:TextBox ID="Titel" runat="server" Text='<%# BindItem.Titel %>' MaxLength="30" />
            </div>
            <div class="field2">
                <asp:TextBox ID="Innehåll" runat="server" Text='<%# BindItem.Innehåll %>' TextMode="MultiLine" CssClass="innehåll" MaxLength="8000" />
            </div>
            <div>
                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Update" Text="Spara" />
                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Cancel" Text="Avbryt" CausesValidation="false" />
            </div>
        </EditItemTemplate>
    </asp:ListView>

</asp:Content>