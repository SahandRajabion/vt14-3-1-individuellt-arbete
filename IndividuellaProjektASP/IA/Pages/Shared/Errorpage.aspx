<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Errorpage.aspx.cs" Inherits="Individuella.Pages.Shared.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


     <p>
        Vi är beklagar, ett oväntat fel har inträffat. Försök igen senare.
    </p>
    <p>
        <asp:HyperLink ID="HyperLink1" runat="server" Text="Tillbaka Till Startsidan" NavigateUrl='<%$ RouteUrl:routename=Default %>' />
    </p>

</asp:Content>
