<%@ Page Title="Resultados de Encuesta" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ResultadosEncuesta.aspx.cs" Inherits="SkillMirror.ResultadosEncuesta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <%-- Incluimos la librería Chart.js desde un CDN --%>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <div>
            <h1 id="headerTitle" runat="server" class="h2">Resultados de Encuesta</h1>
            <asp:Label ID="lblTotalRespuestas" runat="server" CssClass="text-muted"></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblSeleccionarOtra" runat="server" CssClass="form-label me-2"></asp:Label>
            <asp:DropDownList ID="ddlOtrasEncuestas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlOtrasEncuestas_SelectedIndexChanged" CssClass="form-select form-select-sm" Style="width: auto;"></asp:DropDownList>
        </div>
    </div>

    <%-- Un PlaceHolder donde generaremos los gráficos desde el code-behind --%>
    <asp:PlaceHolder ID="phGraficos" runat="server"></asp:PlaceHolder>
</asp:Content>