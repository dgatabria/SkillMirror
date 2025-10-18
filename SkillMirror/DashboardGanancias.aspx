<%@ Page Title="Dashboard de Ganancias" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="DashboardGanancias.aspx.cs" Inherits="SkillMirror.DashboardGanancias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Dashboard de Ganancias</h1>
    </div>

    <%-- Panel de Acceso Denegado (se muestra si el usuario no tiene permisos) --%>
    <asp:Panel ID="pnlAccesoDenegado" runat="server" Visible="false" CssClass="alert alert-danger">
        <h4 id="headerAccesoDenegado" runat="server" class="alert-heading"></h4>
        <p id="textoAccesoDenegado" runat="server"></p>
    </asp:Panel>

    <%-- Panel Principal con las Estadísticas (visible solo si tiene permisos) --%>
    <asp:Panel ID="pnlDashboard" runat="server" Visible="true">
        <div class="row">
            <%-- Card para Ganancias Diarias --%>
            <div class="col-md-6 col-xl-3 mb-4">
                <div class="card shadow border-start-primary h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div id="cardHoyTitle" runat="server" class="text-xs font-weight-bold text-primary text-uppercase mb-1">Ganancias (Hoy)</div>
                                <div class="h5 mb-0 font-weight-bold text-gray-800">$<asp:Literal ID="litGananciasHoy" runat="server" Text="0.00"></asp:Literal></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Card para Ganancias Semanales --%>
            <div class="col-md-6 col-xl-3 mb-4">
                <div class="card shadow border-start-success h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div id="cardSemanaTitle" runat="server" class="text-xs font-weight-bold text-success text-uppercase mb-1">Ganancias (Semana)</div>
                                <div class="h5 mb-0 font-weight-bold text-gray-800">$<asp:Literal ID="litGananciasSemana" runat="server" Text="0.00"></asp:Literal></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Card para Ganancias Mensuales --%>
            <div class="col-md-6 col-xl-3 mb-4">
                <div class="card shadow border-start-info h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div id="cardMesTitle" runat="server" class="text-xs font-weight-bold text-info text-uppercase mb-1">Ganancias (Mes)</div>
                                <div class="h5 mb-0 font-weight-bold text-gray-800">$<asp:Literal ID="litGananciasMes" runat="server" Text="0.00"></asp:Literal></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Card para Ganancias Anuales --%>
            <div class="col-md-6 col-xl-3 mb-4">
                <div class="card shadow border-start-warning h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div id="cardAnioTitle" runat="server" class="text-xs font-weight-bold text-warning text-uppercase mb-1">Ganancias (Año)</div>
                                <div class="h5 mb-0 font-weight-bold text-gray-800">$<asp:Literal ID="litGananciasAnio" runat="server" Text="0.00"></asp:Literal></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
