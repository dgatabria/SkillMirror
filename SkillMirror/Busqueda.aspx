<%@ Page Title="Resultados de Búsqueda" Language="C#" AutoEventWireup="true" CodeBehind="Busqueda.aspx.cs" Inherits="SkillMirror.Busqueda" %>

<%-- NO se especifica MasterPageFile aquí, se asigna en el code-behind --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h1 class="mb-4">
            <asp:Literal ID="litTituloResultados" runat="server"></asp:Literal>
            <em>"<asp:Literal ID="litTerminoBuscado" runat="server"></asp:Literal>"</em>
        </h1>

        <asp:Panel ID="pnlNoResultados" runat="server" Visible="false" CssClass="alert alert-warning">
            <asp:Literal ID="litNoResultados" runat="server"></asp:Literal>
        </asp:Panel>

        <asp:Repeater ID="rptResultados" runat="server">
            <ItemTemplate>
                <div class="card mb-3">
                    <div class="card-body">
                        <h5 class="card-title">
                            <a href="<%# ResolveUrl(Eval("URL").ToString()) %>"><%# Eval("Titulo") %></a>
                        </h5>
                        <h6 class="card-subtitle mb-2 text-muted"><%# Eval("Tipo") %></h6>
                        <p class="card-text"><%# Eval("Descripcion") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>