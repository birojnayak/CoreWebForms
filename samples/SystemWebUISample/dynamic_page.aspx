<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Site.master.cs" Inherits="eShopModernizedWebForms.SiteMaster" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - Catalog manager (Web Forms)</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <!--<webopt:BundleReference runat="server" Path="~/Content/css" />-->
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

</head>
<body>
    <form runat="server">
        <!--<asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
            </Scripts>
        </asp:ScriptManager>
        -->

        <header class="navbar navbar-light navbar-static-top">
            <div class="esh-header-brand">
                <a runat="server" href="~/">
                    <img src="/images/brand.png" />
                </a>
                <asp:LoginView id="Login" runat="server" ViewStateMode="Disabled">
                    <AnonymousTemplate>
                        <ul class="nav navbar-nav navbar-right">
                            <li><asp:LinkButton CssClass="esh-login-link" runat="server" Text="Sign in" OnClick="Login_Click" /></li>
                        </ul>
                    </AnonymousTemplate>
                    <LoggedInTemplate>
                        <ul class="nav navbar-nav navbar-right">
                            <li class="navbar-text" runat="server">Hello, <%: Context.User.Identity.Name %> !</li>
                            <li>
                                <asp:LoginStatus CssClass="esh-login-link" runat="server" LogoutAction="Redirect" LogoutText="Sign out" LogoutPageUrl="~/" OnLoggingOut="Unnamed_LoggingOut" />
                            </li>
                        </ul>
                    </LoggedInTemplate>
                </asp:LoginView>
            </div>
        </header>

        <section class="esh-app-hero">
            <div class="container esh-header">
                <h1 class="esh-header-title">Catalog Manager <span>(WebForms)</span></h1>
            </div>
        </section>

        <div>
            <asp:ContentPlaceHolder ID="MainContent" runat="server">
            </asp:ContentPlaceHolder>
        </div>

        <footer class="esh-app-footer">
            <div class="container">
                <article class="row">
                    <section class="col-sm-6">
                        <img class="esh-app-footer-brand" src="/images/brand_dark.png" />
                    </section>
                    <section class="col-sm-6">
                        <img class="esh-app-footer-text hidden-xs" src="/images/main_footer_text.png" width="335" height="26" alt="footer text image" />
                        <br />
                        <small style="color:white"><asp:Label ID="SessionInfoLabel" runat="server"></asp:Label></small>
                    </section>
                </article>
            </div>
        </footer>

    </form>
</body>
</html>
