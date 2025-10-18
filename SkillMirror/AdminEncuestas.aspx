<%@ Page Title="Administración de Encuestas" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminEncuestas.aspx.cs" Inherits="SkillMirror.AdminEncuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Encuestas</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevaEncuesta" runat="server" Text="Crear Nueva Encuesta" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevaEncuesta_Click" />
        </div>
    </div>
    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>
    <div class="table-responsive">
        <asp:GridView ID="gvEncuestas" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvEncuestas_RowCommand" OnRowDataBound="GvEncuestas_RowDataBound" OnRowCreated="GvEncuestas_RowCreated">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="FechaCreacion" HeaderText="Creada el" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="FechaVencimiento" HeaderText="Vence el" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate><asp:Literal ID="litEstado" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" CommandName="EditarEncuesta" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger me-2" CommandName="EliminarEncuesta" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnEnviar" runat="server" CssClass="btn btn-sm btn-outline-info" CommandName="EnviarEncuesta" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnResultados" runat="server" CssClass="btn btn-sm btn-outline-primary me-2" CommandName="VerResultados" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>