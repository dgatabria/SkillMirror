<%@ Page Title="Contáctenos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contactenos.aspx.cs" Inherits="SkillMirror.Contactenos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1 id="headerTitle" runat="server">Contacto</h1>
        <p id="headerSubtitle" runat="server" class="lead">Estamos aquí para ayudarte. Envíanos tu consulta.</p>
    </div>

    <%-- Literal para mostrar mensajes de éxito o error --%>
    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

    <div class="row">
        <%-- Formulario de Contacto --%>
        <div class="col-lg-7 mb-4 mb-lg-0">
            <h3 id="formTitle" runat="server">Envíanos un Mensaje</h3>
            <div class="mb-3">
                <asp:Label ID="lblNombre" runat="server" CssClass="form-label" AssociatedControlID="txtName">Nombre Completo</asp:Label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" ValidationGroup="Contacto" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblEmail" runat="server" CssClass="form-label" AssociatedControlID="txtEmail">Correo Electrónico</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" ValidationGroup="Contacto" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblAsunto" runat="server" CssClass="form-label" AssociatedControlID="txtSubject">Asunto</asp:Label>
                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valSubject" runat="server" ControlToValidate="txtSubject" ForeColor="Red" Display="Dynamic" ValidationGroup="Contacto" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblMensaje" runat="server" CssClass="form-label" AssociatedControlID="txtMessage">Mensaje</asp:Label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valMessage" runat="server" ControlToValidate="txtMessage" ForeColor="Red" Display="Dynamic" ValidationGroup="Contacto" />
            </div>
            <asp:Button ID="btnSend" runat="server" Text="Enviar Mensaje" CssClass="btn btn-primary" OnClick="BtnSend_Click" ValidationGroup="Contacto" />
        </div>

        <%-- Información de Contacto --%>
        <div class="col-lg-5">
            <h3 id="infoTitle" runat="server">Información de la Empresa</h3>
            <p id="infoText" runat="server">
                Ponte en contacto con nosotros a través de los siguientes canales o visítanos en nuestra oficina.
            </p>
            <ul class="list-unstyled">
                <li class="mb-3">
                    <strong><asp:Literal ID="litDireccion" runat="server">Dirección:</asp:Literal></strong> Av. de Mayo 1234, C1085 CABA, Argentina
                </li>
                <li class="mb-3">
                    <strong><asp:Literal ID="litTelefono" runat="server">Teléfono:</asp:Literal></strong> +54 11 5555-2025
                </li>
                <li class="mb-3">
                    <strong><asp:Literal ID="litEmailInfo" runat="server">Email:</asp:Literal></strong> contacto@skillmirror.com.ar
                </li>
                 <li class="mb-3">
                    <strong><asp:Literal ID="litHorario" runat="server">Horario:</asp:Literal></strong> Lunes a Viernes, 9:00 - 18:00 hs.
                 </li>
            </ul>
        </div>
    </div>

</asp:Content>