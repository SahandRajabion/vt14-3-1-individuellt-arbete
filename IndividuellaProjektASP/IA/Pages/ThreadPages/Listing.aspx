<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listing.aspx.cs" Inherits="Individuella.Pages.ThreadPages.Listing" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Trådar</h1>

     <asp:Panel runat="server" ID="Panel" Visible="false" CssClass="success">
        <asp:Literal runat="server" ID="Literal" />
    </asp:Panel>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validation" />
    <asp:ListView ID="ThreadListView" runat="server"
        ItemType="Individuella.Model.Thread"
        SelectMethod="ThreadListView_GetData"
        DataKeyNames="ThreadID">
        
        <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <dl>
                <dt>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# GetRouteUrl("ThreadDetails", new { id = Item.ThreadID })  %>' Text='<%# Item.Titel %>' />
                </dt>
            </dl>
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>
                Trådar saknas!
            </p>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>