<%@ Page Title="Administración de FAQs" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminFAQs.aspx.cs" Inherits="SkillMirror.AdminFAQs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de FAQs</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevaFAQ" runat="server" Text="Crear Nueva FAQ" CssClass="btn btn-sm btn-outline-primary" OnClick="BtnNuevaFAQ_Click" />
        </div>
    </div>
    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>
    <div class="table-responsive">
        <asp:GridView ID="gvFAQs" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvFAQs_RowCommand" OnRowDataBound="GvFAQs_RowDataBound" OnRowCreated="GvFAQs_RowCreated">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="Pregunta" HeaderText="Pregunta" />
                <asp:BoundField DataField="Orden" HeaderText="Orden" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate><asp:Literal ID="litEstado" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary me-2" CommandName="EditarFAQ" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnBaja" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="BajaFAQ" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>