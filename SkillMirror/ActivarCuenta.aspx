<%@ Page Title="Activación de Cuenta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActivarCuenta.aspx.cs" Inherits="SkillMirror.ActivarCuenta" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .activation-container {
            display: flex;
            align-items: center;
            justify-content: center;
            min-height: 60vh;
        }
        .activation-card {
            max-width: 550px;
            width: 100%;
        }
    </style>

    <div class="activation-container">
        <div class="activation-card">
            
            <%-- PANEL DE ÉXITO --%>
            <asp:Panel ID="pnlExito" runat="server" Visible="false">
                <div class="card text-center shadow">
                     <div class="card-body p-5">
                        <h2 class="card-title text-success"><asp:Literal ID="litTituloExito" runat="server">¡Cuenta Activada Exitosamente!</asp:Literal></h2>
                        <p class="lead mt-3"><asp:Literal ID="litSubtituloExito" runat="server">Tu cuenta ha sido verificada y ya está lista para usarse.</asp:Literal></p>
                        <p><asp:Literal ID="litTextoExito" runat="server">Ya puedes iniciar sesión con las credenciales que elegiste durante el registro.</asp:Literal></p>
                        <asp:HyperLink ID="btnIrALogin" runat="server" CssClass="btn btn-primary btn-lg mt-3" NavigateUrl="~/Login.aspx">Ir al Login</asp:HyperLink>
                    </div>
                </div>
            </asp:Panel>

            <%-- PANEL DE ERROR --%>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="card text-center shadow">
                     <div class="card-body p-5">
                        <h2 class="card-title text-danger"><asp:Literal ID="litTituloError" runat="server">Error de Activación</asp:Literal></h2>
                        <p class="lead mt-3"><asp:Literal ID="litSubtituloError" runat="server">No se pudo activar tu cuenta.</asp:Literal></p>
                        <p><asp:Literal ID="litTextoError" runat="server">El enlace de activación puede ser inválido, haber expirado o ya fue utilizado. Por favor, intenta registrarte nuevamente.</asp:Literal></p>
                        <asp:HyperLink ID="btnVolverARegistrarse" runat="server" CssClass="btn btn-secondary mt-3" NavigateUrl="~/Registro.aspx">Volver a Registrarse</asp:HyperLink>
                    </div>
                </div>
            </asp:Panel>

        </div>
    </div>
</asp:Content>