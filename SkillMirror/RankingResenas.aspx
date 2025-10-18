<%@ Page Title="Ranking de Valoración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="RankingResenas.aspx.cs" Inherits="SkillMirror.RankingResenas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Incluimos la librería Chart.js desde un CDN --%>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Ranking de Valoración de Clientes</h1>
    </div>

    <div class="container-fluid">
        <asp:Panel ID="pnlDashboard" runat="server">
            <%-- Fila de Indicadores Principales --%>
            <div class="row text-center mb-5">
                <div class="col-md-6">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <h5 id="cardPromedioTitle" runat="server" class="card-title">Puntuación Promedio</h5>
                            <p class="display-4 fw-bold text-primary"><asp:Literal ID="litPromedio" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <h5 id="cardTotalTitle" runat="server" class="card-title">Reseñas Totales</h5>
                            <p class="display-4 fw-bold text-primary"><asp:Literal ID="litTotalResenas" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Fila para el Gráfico y Reseñas Destacadas --%>
            <div class="row">
                <div class="col-lg-7">
                    <div class="card">
                        <div class="card-header">
                            <h5 id="graficoTitle" runat="server">Distribución de Puntuaciones</h5>
                        </div>
                        <div class="card-body">
                            <canvas id="graficoEstrellas"></canvas>
                        </div>
                    </div>
                </div>
                <div class="col-lg-5">
                     <h4 id="destacadasTitle" runat="server">Últimas Opiniones</h4>
                     <asp:Repeater ID="rptResenasDestacadas" runat="server">
                         <ItemTemplate>
                             <div class="card mb-3">
                                 <div class="card-body">
                                     <h6 class="card-title"><%# Eval("Asunto") %></h6>
                                     <p class="card-text fst-italic">"<%# Eval("Comentario") %>"</p>
                                     <div class="d-flex justify-content-between align-items-center">
                                         <small class="text-muted">- <%# Eval("Autor.Nombre") %></small>
                                         <span class="text-warning"><%# RenderStars(Eval("Puntuacion")) %></span>
                                     </div>
                                 </div>
                             </div>
                         </ItemTemplate>
                     </asp:Repeater>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlNoHayDatos" runat="server" Visible="false">
             <div class="alert alert-info text-center">
                 <h4 id="noDatosTitle" runat="server">Aún no hay datos</h4>
                 <p id="noDatosText" runat="server">Todavía no tenemos suficientes reseñas para generar un ranking.</p>
             </div>
        </asp:Panel>
    </div>

    <%-- Script para dibujar el gráfico --%>
    <script type="text/javascript">
        const chartData = <%= _jsonChartData %>;
        
        document.addEventListener('DOMContentLoaded', function () {
            if (chartData && document.getElementById('graficoEstrellas')) {
                new Chart(document.getElementById('graficoEstrellas'), {
                    type: 'bar',
                    data: {
                        labels: chartData.labels,
                        datasets: [{
                            label: chartData.datasetLabel,
                            data: chartData.data,
                            backgroundColor: 'rgba(0, 128, 128, 0.6)',
                            borderColor: 'rgba(0, 128, 128, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        indexAxis: 'y',
                        scales: { x: { ticks: { precision: 0 }, beginAtZero: true } },
                        plugins: { legend: { display: false } }
                    }
                });
            }
        });
    </script>
</asp:Content>