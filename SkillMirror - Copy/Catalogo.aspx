<%@ Page Title="Planes y Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="SkillMirror.Catalogo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 id="headerTitle" runat="server">Nuestros Planes</h1>
        <p id="headerSubtitle" runat="server" class="lead">Soluciones escalables para optimizar tu proceso de selección.</p>
    </div>

    <div class="container">
        <div class="row text-center">
            <%-- Plan BASIC --%>
            <div class="col-lg-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-header bg-light">
                        <h4 id="basicCardTitle" runat="server" class="my-0 fw-normal">SkillMirror BASIC</h4>
                    </div>
                    <div class="card-body">
                        <h1 class="card-title pricing-card-title">$29.000<small id="basicPriceTerm" runat="server" class="text-muted fw-light">/mes</small></h1>
                        <p id="basicCardText" runat="server" class="mt-3 mb-4">
                            Ideal para empresas con procesos de selección puntuales.
                        </p>
                        <div class="form-check d-inline-block">
                            <input class="form-check-input plan-checkbox" type="checkbox" value="basic" id="chkBasic">
                            <label class="form-check-label" for="chkBasic">
                                <asp:Literal ID="litBasicCompare" runat="server">Comparar Plan</asp:Literal>
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Plan PRO --%>
            <div class="col-lg-4 mb-4">
                <div class="card h-100 shadow">
                    <div class="card-header text-white" style="background-color: var(--primary-color);">
                        <h4 id="proCardTitle" runat="server" class="my-0 fw-normal">SkillMirror PRO</h4>
                    </div>
                    <div class="card-body">
                        <h1 class="card-title pricing-card-title">$89.000<small id="proPriceTerm" runat="server" class="text-muted fw-light">/mes</small></h1>
                        <p id="proCardText" runat="server" class="mt-3 mb-4">
                            La solución completa para empresas con reclutamiento continuo.
                        </p>
                        <div class="form-check d-inline-block">
                            <input class="form-check-input plan-checkbox" type="checkbox" value="pro" id="chkPro" checked>
                            <label class="form-check-label" for="chkPro">
                                <asp:Literal ID="litProCompare" runat="server">Comparar Plan</asp:Literal>
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Plan ENTERPRISE --%>
            <div class="col-lg-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-header bg-light">
                        <h4 id="enterpriseCardTitle" runat="server" class="my-0 fw-normal">SkillMirror ENTERPRISE</h4>
                    </div>
                    <div class="card-body">
                        <h1 id="enterprisePrice" runat="server" class="card-title pricing-card-title">A convenir</h1>
                        <p id="enterpriseCardText" runat="server" class="mt-3 mb-4">
                            Un plan a medida para grandes corporaciones con necesidades específicas.
                        </p>
                        <div class="form-check d-inline-block">
                            <input class="form-check-input plan-checkbox" type="checkbox" value="enterprise" id="chkEnterprise" checked>
                            <label class="form-check-label" for="chkEnterprise">
                                <asp:Literal ID="litEnterpriseCompare" runat="server">Comparar Plan</asp:Literal>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%-- Tabla Comparativa --%>
        <div id="comparison-section" class="mt-5" style="display: none;">
            <h2 id="comparisonTitle" runat="server" class="text-center mb-4">Tabla Comparativa</h2>
            <div class="table-responsive">
                <table class="table table-striped table-hover text-center">
                    <thead id="comparison-head"></thead>
                    <tbody id="comparison-body"></tbody>
                </table>
            </div>
        </div>
    </div>

    <%-- Script que ahora recibe los datos traducidos desde el code-behind --%>
    <script type="text/javascript">
        // Los datos ahora se inyectan desde el servidor
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

            // Usamos la etiqueta 'label' traducida para la primera columna
            let headerHTML = `<tr><th class="text-start" style="width: 25%;">${features[0].headerLabel}</th>`;
            selectedPlans.forEach(planId => {
                headerHTML += `<th>${planData[planId].name}</th>`;
            });
            headerHTML += '</tr>';
            thead.innerHTML = headerHTML;

            let bodyHTML = '';
            features.forEach(feature => {
                // Iteramos sobre las features traducidas (excluyendo la primera que es el header)
                if (feature.key !== 'header') {
                    bodyHTML += `<tr><td class="text-start fw-bold">${feature.label}</td>`;
                    selectedPlans.forEach(planId => {
                        bodyHTML += `<td>${planData[planId][feature.key]}</td>`;
                    });
                    bodyHTML += '</tr>';
                }
            });
            tbody.innerHTML = bodyHTML;
        }

        document.querySelectorAll('.plan-checkbox').forEach(checkbox => {
            checkbox.addEventListener('change', drawComparisonTable);
        });

        document.addEventListener('DOMContentLoaded', drawComparisonTable);
    </script>
</asp:Content>