<%@ Page Title="Política de Privacidad" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PoliticaDePrivacidad.aspx.cs" Inherits="SkillMirror.PoliticaDePrivacidad" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 id="headerTitle" runat="server">Política de Privacidad</h1>
        <p id="headerSubtitle" runat="server" class="lead">Tu confianza y la protección de tus datos son nuestra prioridad.</p>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-md-10 offset-md-1">
                <p><asp:Literal ID="litIntro" runat="server"></asp:Literal></p>

                <h4 class="mt-4"><asp:Literal ID="litTitulo1" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto1" runat="server"></asp:Literal></p>

                <h4 class="mt-4"><asp:Literal ID="litTitulo2" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto2_intro" runat="server"></asp:Literal></p>
                <ul>
                    <li><strong><asp:Literal ID="litTexto2_li1_strong" runat="server"></asp:Literal></strong><asp:Literal ID="litTexto2_li1_text" runat="server"></asp:Literal></li>
                    <li><strong><asp:Literal ID="litTexto2_li2_strong" runat="server"></asp:Literal></strong><asp:Literal ID="litTexto2_li2_text" runat="server"></asp:Literal></li>
                </ul>

                <h4 class="mt-4"><asp:Literal ID="litTitulo3" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto3_intro" runat="server"></asp:Literal></p>
                <ul>
                    <li><asp:Literal ID="litTexto3_li1" runat="server"></asp:Literal></li>
                    <li><asp:Literal ID="litTexto3_li2" runat="server"></asp:Literal></li>
                    <li><asp:Literal ID="litTexto3_li3" runat="server"></asp:Literal></li>
                    <li><asp:Literal ID="litTexto3_li4" runat="server"></asp:Literal></li>
                </ul>

                <h4 class="mt-4"><asp:Literal ID="litTitulo4" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto4" runat="server"></asp:Literal></p>

                <h4 class="mt-4"><asp:Literal ID="litTitulo5" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto5" runat="server"></asp:Literal></p>

                <h4 class="mt-4"><asp:Literal ID="litTitulo6" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto6" runat="server"></asp:Literal></p>

                <h4 class="mt-4"><asp:Literal ID="litTitulo7" runat="server"></asp:Literal></h4>
                <p><asp:Literal ID="litTexto7" runat="server"></asp:Literal></p>
            </div>
        </div>
    </div>
</asp:Content>