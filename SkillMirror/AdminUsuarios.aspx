<%@ Page Title="Administración de Usuarios" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="SkillMirror.AdminUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Usuarios</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevoUsuario" runat="server" Text="Crear Nuevo Usuario" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevoUsuario_Click" />
        </div>
    </div>

    <div class="table-responsive">
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvUsuarios_RowCommand"
            OnRowDataBound="GvUsuarios_RowDataBound">
<Columns>
    <asp:BoundField DataField="Codigo" HeaderText="ID" />
    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
    <asp:BoundField DataField="Email" HeaderText="Email" />
    
    
<asp:TemplateField HeaderText="Estado">
    <ItemTemplate>
        <asp:Literal ID="litEstado" runat="server"></asp:Literal>
    </ItemTemplate>
</asp:TemplateField>

    <asp:TemplateField HeaderText="Acciones">
        <ItemTemplate>
            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" 
                CommandName="EditarUsuario" CommandArgument='<%# Eval("Codigo") %>'>Editar</asp:LinkButton>
            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger" 
                CommandName="EliminarUsuario" CommandArgument='<%# Eval("Codigo") %>'>Eliminar</asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
        </asp:GridView>
    </div>
</asp:Content>