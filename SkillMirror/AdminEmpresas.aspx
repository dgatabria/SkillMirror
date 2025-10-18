<%@ Page Title="Administración de Empresas" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminEmpresas.aspx.cs" Inherits="SkillMirror.AdminEmpresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Empresas</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevaEmpresa" runat="server" Text="Crear Nueva Empresa" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevaEmpresa_Click" />
        </div>
    </div>

    <div class="table-responsive">
        <asp:GridView ID="gvEmpresas" runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvEmpresas_RowCommand"
            OnRowDataBound="GvEmpresas_RowDataBound"
            OnRowCreated="GvEmpresas_RowCreated"> <%-- ¡CAMBIO CLAVE AQUÍ! --%>
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="CUIT" HeaderText="CUIT" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" 
                                         CommandName="EditarEmpresa" CommandArgument='<%# Eval("Codigo") %>'>Editar</asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger" 
                            CommandName="EliminarEmpresa" CommandArgument='<%# Eval("Codigo") %>'>Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>