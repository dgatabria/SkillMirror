<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="SkillMirror.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Mi Perfil</h1>
    </div>

    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

    <div class="row">
        <%-- Columna para Datos Personales --%>
        <div class="col-lg-6">
            <div class="card">
                <div class="card-header">
                    <h5 id="cardDatosTitle" runat="server">Datos Personales</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" CssClass="form-label">Nombre</asp:Label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valNombre" runat="server" ControlToValidate="txtNombre" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido" CssClass="form-label">Apellido</asp:Label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valApellido" runat="server" ControlToValidate="txtApellido" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblDNI" runat="server" AssociatedControlID="txtDNI" CssClass="form-label">DNI</asp:Label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valDNI" runat="server" ControlToValidate="txtDNI" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="form-check my-3">
                        <asp:CheckBox ID="chkSuscrito" runat="server" CssClass="form-check-input" />
                        <asp:Label ID="lblSuscrito" runat="server" CssClass="form-check-label" AssociatedControlID="chkSuscrito"></asp:Label>
                    </div>
                    <div class="form-check my-3">
                        <asp:CheckBox ID="chkSuscritoEncuestas" runat="server" CssClass="form-check-input" />
                        <asp:Label ID="lblSuscritoEncuestas" runat="server" CssClass="form-check-label" AssociatedControlID="chkSuscritoEncuestas"></asp:Label>
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnGuardarDatos" runat="server" Text="Guardar Datos" CssClass="btn btn-primary" OnClick="BtnGuardarDatos_Click" ValidationGroup="Datos" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Columna para Cambiar Contraseña --%>
        <div class="col-lg-6">
            <div class="card">
                <div class="card-header">
                    <h5 id="cardPasswordTitle" runat="server">Cambiar Contraseña</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <asp:Label ID="lblPassActual" runat="server" AssociatedControlID="txtPassActual" CssClass="form-label">Contraseña Actual</asp:Label>
                        <asp:TextBox ID="txtPassActual" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valPassActual" runat="server" ControlToValidate="txtPassActual" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblPassNueva" runat="server" AssociatedControlID="txtPassNueva" CssClass="form-label">Nueva Contraseña</asp:Label>
                        <asp:TextBox ID="txtPassNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valPassNueva" runat="server" ControlToValidate="txtPassNueva" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblConfirmPassNueva" runat="server" AssociatedControlID="txtConfirmPassNueva" CssClass="form-label">Confirmar Nueva Contraseña</asp:Label>
                        <asp:TextBox ID="txtConfirmPassNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:CompareValidator ID="valComparePass" runat="server" ControlToValidate="txtConfirmPassNueva" ControlToCompare="txtPassNueva" Operator="Equal" Type="String" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnCambiarPassword" runat="server" Text="Cambiar Contraseña" CssClass="btn btn-secondary" OnClick="BtnCambiarPassword_Click" ValidationGroup="Password" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>