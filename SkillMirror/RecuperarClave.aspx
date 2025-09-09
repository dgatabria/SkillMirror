<%@ Page Title="Recuperar Contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecuperarClave.aspx.cs" Inherits="SkillMirror.RecuperarClave" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Script de Google reCAPTCHA --%>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    
    <style>
        .recovery-container { display: flex; align-items: center; justify-content: center; min-height: 70vh; }
        .recovery-card { max-width: 480px; width: 100%; }
        .card-header { background-color: var(--primary-color); color: white; text-align: center; }
        .btn-recovery-submit { background-color: var(--primary-color); border-color: var(--primary-color); width: 100%; padding: 10px; font-size: 1.1rem; }
        .btn-recovery-submit:hover { background-color: #006666; border-color: #006666; }
        .g-recaptcha { margin-bottom: 1rem; transform: scale(1.05); transform-origin: center; }
    </style>

    <div class="recovery-container">
        <div class="card recovery-card shadow-lg">
            <div class="card-header">
                <h3><asp:Literal ID="litHeaderTitle" runat="server">Recuperar Contraseña</asp:Literal></h3>
            </div>
            <div class="card-body p-4">
                <asp:Panel ID="pnlRequest" runat="server">
                    <p class="card-text text-center mb-4">
                        <asp:Literal ID="litFormText" runat="server">Ingresa tu correo electrónico y te enviaremos un enlace para restablecer tu contraseña.</asp:Literal>
                    </p>
                    <div class="mb-3">
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="form-label">Correo Electrónico Registrado</asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="tu-email@empresa.com"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" ControlToValidate="txtEmail" ErrorMessage="El correo es obligatorio." ForeColor="Red" Display="Dynamic" CssClass="mt-1" ValidationGroup="Recuperar" />
                        <asp:RegularExpressionValidator ID="EmailFormatValidator" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="El formato del correo no es válido." ForeColor="Red" Display="Dynamic" CssClass="mt-1" ValidationGroup="Recuperar" />
                    </div>

                    <%-- Contenedor de reCAPTCHA --%>
                    <div class="d-flex justify-content-center g-recaptcha" 
                         data-sitekey="6LeuLLcrAAAAAAJeiPm7oHTlJs97dRbw0Vd51xcH">
                    </div>
                    <asp:Label ID="lblRecaptchaError" runat="server" ForeColor="Red" CssClass="d-block text-center mb-3"></asp:Label>

                    <div class="d-grid mt-2">
                        <asp:Button ID="btnSendRecovery" runat="server" Text="Enviar Enlace de Recuperación" CssClass="btn btn-primary btn-recovery-submit" OnClick="BtnSendRecovery_Click" ValidationGroup="Recuperar" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="text-center">
                    <div class="alert alert-success">
                        <h4><asp:Literal ID="litSuccessTitle" runat="server">¡Enlace Enviado!</asp:Literal></h4>
                        <p><asp:Literal ID="litSuccessText" runat="server">Si la dirección de correo electrónico está registrada, recibirás un mensaje con las instrucciones.</asp:Literal></p>
                    </div>
                </asp:Panel>

                <div class="text-center mt-4">
                    <asp:HyperLink ID="linkVolverLogin" runat="server" NavigateUrl="~/Login.aspx">Volver a Iniciar Sesión</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</asp:Content>