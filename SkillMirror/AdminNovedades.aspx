<%@ Page Title="Administrar Novedades" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminNovedades.aspx.cs" Inherits="SkillMirror.AdminNovedades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Novedades</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevaNovedad" runat="server" Text="Crear Nueva Novedad" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevaNovedad_Click" />
        </div>
    </div>
    <div class="table-responsive">
        <asp:GridView ID="gvNovedades" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvNovedades_RowCommand" OnRowDataBound="GvNovedades_RowDataBound" OnRowCreated="GvNovedades_RowCreated">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="FechaPublicacion" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Autor.Nombre" HeaderText="Autor" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate><asp:Literal ID="litEstado" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" CommandName="EditarNovedad" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="EliminarNovedad" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>