<%@ Page Title="Administración de Roles" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminRoles.aspx.cs" Inherits="SkillMirror.AdminRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Roles y Permisos</h1>
    </div>

    <%-- Mensajes para el usuario (éxito o error) --%>
    <asp:Literal ID="litMensaje" runat="server"></asp:Literal>

    <div class="row">
        <div class="col-md-4">
            <div class="card">
                <div class="card-header">
                    <asp:Literal ID="litCardRolesTitle" runat="server">Roles Existentes</asp:Literal>
                </div>
                <div class="card-body">
                    <p class="card-text text-muted small"><asp:Literal ID="litCardRolesText" runat="server">Seleccione un rol para editar sus permisos o presione 'Nuevo' para crear uno.</asp:Literal></p>
                    <asp:ListBox ID="lstRoles" runat="server" CssClass="form-control" Rows="15" AutoPostBack="true" OnSelectedIndexChanged="LstRoles_SelectedIndexChanged"></asp:ListBox>
                </div>
                <div class="card-footer text-end">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo Rol" CssClass="btn btn-secondary" OnClick="BtnNuevo_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

        <div class="col-md-8">
            <asp:Panel ID="pnlEdicion" runat="server" Visible="false">
                <div class="card">
                    <div class="card-header">
                        <asp:Literal ID="litEdicionTitulo" runat="server">Detalles del Rol</asp:Literal>
                    </div>
                    <div class="card-body">
                        <asp:HiddenField ID="hfRolId" runat="server" Value="0" />

                        <div class="mb-3">
                            <asp:Label ID="lblNombreRol" runat="server" AssociatedControlID="txtNombreRol" CssClass="form-label">Nombre del Rol</asp:Label>
                            <asp:TextBox ID="txtNombreRol" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valNombreRol" runat="server" ControlToValidate="txtNombreRol" ErrorMessage="El nombre del rol es obligatorio." ForeColor="Red" Display="Dynamic" ValidationGroup="EdicionRol" />
                        </div>

                        <div class="mb-3">
                            <asp:Label ID="lblPermisosAsignados" runat="server" CssClass="form-label">Permisos Asignados</asp:Label>
                            <asp:CheckBoxList ID="cblPermisos" runat="server" CssClass="form-check" RepeatLayout="Flow" RepeatDirection="Vertical">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="card-footer text-end">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" ValidationGroup="EdicionRol" />
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar Rol" CssClass="btn btn-danger" OnClick="BtnEliminar_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>