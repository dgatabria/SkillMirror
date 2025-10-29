<%@ Page Title="Registro" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="SkillMirror.Registro" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Script de Google reCAPTCHA --%>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    
    <style>
        .registration-container { display: flex; align-items: center; justify-content: center; padding: 2rem 0; }
        .registration-card { max-width: 600px; width: 100%; }
        .card-header { background-color: var(--primary-color); color: white; text-align: center; }
        .btn-register-submit { background-color: var(--primary-color); border-color: var(--primary-color); width: 100%; padding: 10px; font-size: 1.1rem; }
        .btn-register-submit:hover { background-color: #006666; border-color: #006666; }
        .g-recaptcha { margin-top: 1rem; transform: scale(1.05); transform-origin: center; }
    </style>

    <div class="registration-container">
        <div class="card registration-card shadow-lg">
            <div class="card-header">
                <h3><asp:Literal ID="litHeaderTitle" runat="server">Crear una Cuenta</asp:Literal></h3>
            </div>
            <div class="card-body p-4">
                
                <%-- PANEL 1: FORMULARIO DE REGISTRO --%>
                <asp:Panel ID="pnlRegistro" runat="server">
                    <asp:ValidationSummary ID="valSummary" runat="server" ForeColor="Red" CssClass="mb-3" />

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" CssClass="form-label">Nombre</asp:Label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NombreValidator" runat="server" ControlToValidate="txtNombre" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                            <asp:RegularExpressionValidator ID="NombreRegexValidator" runat="server" 
                                ControlToValidate="txtNombre"
                                ValidationExpression="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"
                                ForeColor="Red" 
                                Display="Dynamic" 
                                ValidationGroup="Registro" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido" CssClass="form-label">Apellido</asp:Label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ApellidoValidator" runat="server" ControlToValidate="txtApellido" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                            <asp:RegularExpressionValidator ID="ApellidoRegexValidator" runat="server" 
                                    ControlToValidate="txtApellido"
                                    ValidationExpression="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"
                                    ForeColor="Red" 
                                    Display="Dynamic" 
                                    ValidationGroup="Registro" />
                        </div>
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblDNI" runat="server" AssociatedControlID="txtDNI" CssClass="form-label">DNI (sin puntos)</asp:Label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DNIRequiredValidator" runat="server" ControlToValidate="txtDNI" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Correo Electrónico</asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                        <asp:RegularExpressionValidator ID="EmailFormatValidator" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                    </div>
                    
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Contraseña</asp:Label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequiredValidator" runat="server" ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label">Confirmar Contraseña</asp:Label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" Type="String" ForeColor="Red" Display="Dynamic" ValidationGroup="Registro" />
                        </div>
                    </div>

                    <div class="d-flex justify-content-center g-recaptcha" data-sitekey="6LeuLLcrAAAAAAJeiPm7oHTlJs97dRbw0Vd51xcH"></div>
                    <asp:Label ID="lblRecaptchaError" runat="server" ForeColor="Red" CssClass="d-block text-center mt-2 mb-3"></asp:Label>
                    
                    <div class="form-check mt-3">
                        <asp:CheckBox ID="chkAceptoTerminos" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label" for="<%= chkAceptoTerminos.ClientID %>">
                            <asp:Literal ID="litAceptoTerminos" runat="server"></asp:Literal>
                            <asp:HyperLink ID="linkTerminos" runat="server" NavigateUrl="~/TerminosYCondiciones.aspx" Target="_blank">Términos y Condiciones</asp:HyperLink>.
                        </label>
                        <asp:CustomValidator ID="TerminosValidator" runat="server" ForeColor="Red" Display="Dynamic" OnServerValidate="Terminos_ServerValidate" ValidateEmptyText="true" CssClass="d-block mt-1" ValidationGroup="Registro" />
                    </div>
                    
                    <div class="d-grid mt-4">
                        <asp:Button ID="btnRegister" runat="server" Text="Registrarse" CssClass="btn btn-primary btn-register-submit" OnClick="BtnRegister_Click" ValidationGroup="Registro" />
                    </div>
                </asp:Panel>

                <%-- PANEL 2: MENSAJE DE CONFIRMACIÓN --%>
                <asp:Panel ID="pnlConfirmacion" runat="server" Visible="false">
                    <div class="alert alert-success text-center">
                        <h4 class="alert-heading"><asp:Literal ID="litConfirmacionTitulo" runat="server"></asp:Literal></h4>
                        <p><asp:Literal ID="litConfirmacionTexto1" runat="server"></asp:Literal> <strong><asp:Literal ID="litEmailConfirmacion" runat="server"></asp:Literal></strong>.</p>
                        <p><asp:Literal ID="litConfirmacionTexto2" runat="server"></asp:Literal></p>
                        <hr>
                        <p class="mb-0"><asp:Literal ID="litConfirmacionTexto3" runat="server"></asp:Literal></p>
                    </div>
                    <div class="text-center mt-3">
                        <asp:HyperLink ID="linkVolverInicio" runat="server" NavigateUrl="~/Default.aspx">Volver al Inicio</asp:HyperLink>
                    </div>
                </asp:Panel>
            </div>
            <div class="card-footer text-center">
                 <small><asp:Literal ID="litFooterTexto" runat="server"></asp:Literal> <asp:HyperLink ID="linkIniciarSesion" runat="server" NavigateUrl="~/Login.aspx">Inicia Sesión</asp:HyperLink></small>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        // El script no necesita cambios
    </script>
</asp:Content>