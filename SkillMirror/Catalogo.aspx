<%@ Page Title="Planes y Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="SkillMirror.Catalogo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 id="headerTitle" runat="server">Nuestros Planes</h1>
        <p id="headerSubtitle" runat="server" class="lead">Soluciones escalables para optimizar tu proceso de selección.</p>
    </div>

    <div class="container">
        <div class="row text-center justify-content-center">
            <asp:Repeater ID="rptPlanes" runat="server" OnItemDataBound="RptPlanes_ItemDataBound" OnItemCommand="RptPlanes_ItemCommand">
                <ItemTemplate>
                    <div class="col-lg-4 mb-4">
                        <div class="card h-100 shadow-sm">
                            <div id="cardHeader" runat="server" class="card-header">
                                <h4 id="cardTitle" runat="server" class="my-0 fw-normal"></h4>
                            </div>
                            <div class="card-body d-flex flex-column">
                                <h1 class="card-title pricing-card-title">
                                    <asp:Literal ID="litPrecio" runat="server"></asp:Literal>
                                </h1>
                                <p id="cardText" runat="server" class="mt-3 mb-4">
                                </p>
                                
                                <%-- Checkbox para la tabla comparativa --%>
                                <div class="form-check d-inline-block mt-auto">
                                    <input class="form-check-input plan-checkbox" type="checkbox" value="<%# Eval("Nombre").ToString().ToLower() %>" id="chk_<%# Eval("Nombre") %>" checked>
                                    <label class="form-check-label" for="chk_<%# Eval("Nombre") %>">
                                        <asp:Literal ID="litComparar" runat="server">Comparar Plan</asp:Literal>
                                    </label>
                                </div>

                                <%-- Botón de suscribir (solo visible en modo contratación) --%>
                                <asp:Panel ID="pnlBotonContratar" runat="server" Visible="false" CssClass="mt-3">
                                    <asp:Button ID="btnSuscribir" runat="server" CssClass="btn btn-lg btn-success w-100" 
                                        CommandName="Suscribir" CommandArgument='<%# Eval("ID") %>' />
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- Tabla Comparativa (Ahora 100% dinámica) --%>
        <div id="comparison-section" class="mt-5">
            <h2 id="comparisonTitle" runat="server" class="text-center mb-4">Tabla Comparativa</h2>
            <div class="table-responsive">
                <table class="table table-striped table-hover text-center">
                    <thead id="comparison-head"></thead>
                    <tbody id="comparison-body"></tbody>
                </table>
            </div>
        </div>
    </div>

    <%-- Script de JavaScript (Sin cambios, pero ahora recibe datos dinámicos) --%>
    <script type="text/javascript">
        const planData = <%= _jsonPlanData %>;
        const features = <%= _jsonFeatures %>;

        function drawComparisonTable() {
            const checkboxes = document.querySelectorAll('.plan-checkbox:checked');
            const selectedPlans = Array.from(checkboxes).map(cb => cb.value);

            const section = document.getElementById('comparison-section');
            const thead = document.getElementById('comparison-head');
            const tbody = document.getElementById('comparison-body');

            thead.innerHTML = '';
            tbody.innerHTML = '';

            if (selectedPlans.length === 0) {
                section.style.display = 'none';
                return;
            }

            section.style.display = 'block';

            let headerHTML = `<tr><th class="text-start" style="width: 25%;">${features[0].headerLabel}</th>`;
            selectedPlans.forEach(planId => {
                if (planData[planId]) {
                    headerHTML += `<th>${planData[planId].name}</th>`;
                }
            });
            headerHTML += '</tr>';
            thead.innerHTML = headerHTML;

            let bodyHTML = '';
            features.forEach(feature => {
                if (feature.key !== 'header') {
                    bodyHTML += `<tr><td class="text-start fw-bold">${feature.label}</td>`;
                    selectedPlans.forEach(planId => {
                        if (planData[planId]) {
                            // Se busca la feature por su clave. Si no existe, se muestra un guión.
                            bodyHTML += `<td>${planData[planId][feature.key] || '-'}</td>`;
                        }
                    });
                    bodyHTML += '</tr>';
                }
            });
            tbody.innerHTML = bodyHTML;
        }

        document.querySelectorAll('.plan-checkbox').forEach(checkbox => {
            checkbox.addEventListener('change', drawComparisonTable);
        });

        // Asegurarse de que la tabla se dibuje después de que la página cargue completamente
        // Usamos DOMContentLoaded que es más fiable que 'load'
        document.addEventListener('DOMContentLoaded', drawComparisonTable);
    </script>
</asp:Content>
