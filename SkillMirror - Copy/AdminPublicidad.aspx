<%@ Page Title="Administración de Publicidad" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminPublicidad.aspx.cs" Inherits="SkillMirror.AdminPublicidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Publicidad</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevaPublicidad" runat="server" Text="Crear Nueva Publicidad" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevaPublicidad_Click" />
        </div>
    </div>
    
    <div class="table-responsive">
        <asp:GridView ID="gvPublicidades" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvPublicidades_RowCommand" OnRowDataBound="GvPublicidades_RowDataBound" OnRowCreated="GvPublicidades_RowCreated">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="FechaInicio" HeaderText="Inicia el" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="FechaExpiracion" HeaderText="Expira el" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate><asp:Literal ID="litEstado" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" CommandName="EditarPub" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="EliminarPub" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>