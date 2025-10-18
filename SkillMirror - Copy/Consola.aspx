<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Consola.aspx.cs" Inherits="SkillMirror.Consola" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">

    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Dashboard</h1>
    </div>

    <h2 id="welcomeTitle" runat="server">¡Bienvenido a la Consola de SkillMirror!</h2>
    <p id="welcomeSubtitle" runat="server" class="lead">Desde el menú de la izquierda podrás gestionar las funcionalidades clave del sistema.</p>

    <div class="row mt-4">
        <div class="col-md-4">
            <div class="card text-center shadow-sm">
                <div class="card-body">
                    <h5 id="cardUsuariosTitle" runat="server" class="card-title">Administrar Usuarios</h5>
                    <p id="cardUsuariosText" runat="server" class="card-text">Gestiona los roles, permisos y accesos de los usuarios del sistema.</p>
                    <asp:HyperLink ID="btnIrUsuarios" runat="server" CssClass="btn btn-primary" NavigateUrl="~/AdminUsuarios.aspx">Ir a Usuarios</asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card text-center shadow-sm">
                <div class="card-body">
                    <h5 id="cardBitacoraTitle" runat="server" class="card-title">Consultar Bitácora</h5>
                    <p id="cardBitacoraText" runat="server" class="card-text">Audita los eventos importantes y las acciones realizadas en la plataforma.</p>
                    <asp:HyperLink ID="btnVerBitacora" runat="server" CssClass="btn btn-primary" NavigateUrl="~/Bitacora.aspx">Ver Bitácora</asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card text-center shadow-sm">
                <div class="card-body">
                    <h5 id="cardBackupTitle" runat="server" class="card-title">Realizar Backup</h5>
                    <p id="cardBackupText" runat="server" class="card-text">Crea y gestiona las copias de seguridad de la base de datos.</p>
                    <asp:HyperLink ID="btnIrBackup" runat="server" CssClass="btn btn-primary" NavigateUrl="~/Backup.aspx">Ir a Backup</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

</asp:Content>