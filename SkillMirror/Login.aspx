<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SkillMirror.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Script de Google reCAPTCHA --%>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    
    <style>
        .login-container { display: flex; align-items: center; justify-content: center; min-height: 70vh; }
        .login-card { max-width: 450px; width: 100%; }
        .card-header { background-color: var(--primary-color); color: white; text-align: center; }
        .btn-login-submit { background-color: var(--primary-color); border-color: var(--primary-color); width: 100%; padding: 10px; font-size: 1.1rem; }
        .btn-login-submit:hover { background-color: #006666; border-color: #006666; }
        .g-recaptcha { margin-bottom: 1rem; transform: scale(1.05); transform-origin: center; }
    </style>

    <div class="login-container">
        <div class="card login-card shadow-lg">
            <div class="card-header">
                <h3><asp:Literal ID="litHeaderTitle" runat="server">Iniciar Sesión</asp:Literal></h3>
            </div>
            <div class="card-body p-4">
                <asp:Literal ID="litErrorMessage" runat="server" EnableViewState="false"></asp:Literal>
             
                <div class="mb-3">
                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Correo Electrónico</asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="ejemplo@empresa.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailValidator" runat="server" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" ValidationGroup="Login" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Contraseña</asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" ValidationGroup="Login" />
                </div>

                <%-- Contenedor de reCAPTCHA --%>
                <div class="d-flex justify-content-center g-recaptcha" 
                     data-sitekey="6LeuLLcrAAAAAAJeiPm7oHTlJs97dRbw0Vd51xcH">
                </div>
                <asp:Label ID="lblRecaptchaError" runat="server" ForeColor="Red" CssClass="d-block text-center mb-3"></asp:Label>

                <div class="d-grid mt-2">
                    <asp:Button ID="btnLogin" runat="server" Text="Acceder" CssClass="btn btn-primary btn-login-submit" OnClick="BtnLogin_Click" ValidationGroup="Login" />
                </div>
                <div class="text-center mt-4">
                    <asp:HyperLink ID="linkOlvidoPassword" runat="server" NavigateUrl="~/RecuperarClave.aspx">¿Olvidaste tu contraseña?</asp:HyperLink>
                </div>
            </div>
            <div class="card-footer text-center">
                 <small><asp:Literal ID="litFooterText" runat="server">¿No tienes una cuenta?</asp:Literal> <asp:HyperLink ID="linkRegistro" runat="server" NavigateUrl="~/Registro.aspx">Regístrate aquí</asp:HyperLink></small>
            </div>
        </div>
    </div>
</asp:Content>