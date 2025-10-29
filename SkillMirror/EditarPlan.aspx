<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarPlan.aspx.cs" Inherits="SkillMirror.EditarPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2"></h1>
    </div>

    <div class="card">
        <div class="card-body">
            <asp:HiddenField ID="hidPlanID" runat="server" Value="0" />
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="<%= txtNombre.ClientID %>" class="form-label" id="lblNombre" runat="server"></label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="<%= txtSubtitulo.ClientID %>" class="form-label" id="lblSubtitulo" runat="server"></label>
                    <asp:TextBox ID="txtSubtitulo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 mb-3">
                    <label for="<%= txtPrecio.ClientID %>" class="form-label" id="lblPrecio" runat="server"></label>
                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                </div>
                <div class="col-md-4 mb-3">
                    <label for="<%= txtOrden.ClientID %>" class="form-label" id="lblOrden" runat="server"></label>
                    <asp:TextBox ID="txtOrden" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                </div>
                <div class="col-md-4 mb-3 d-flex align-items-center">
                    <div class="form-check form-switch mt-4">
                        <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label" id="lblActivo" runat="server" for="<%= chkActivo.ClientID %>"></label>
                    </div>
                    <div class="form-check form-switch mt-4 ms-3">
                        <asp:CheckBox ID="chkDestacado" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label" id="lblDestacado" runat="server" for="<%= chkDestacado.ClientID %>"></label>
                    </div>
                </div>
            </div>
            <hr />
            <h5 id="headerFeatures" runat="server"></h5>
            <asp:Repeater ID="rptFeatures" runat="server">
                <ItemTemplate>
                    <div class="mb-3">
                        <asp:HiddenField ID="hidFeatureID" runat="server" Value='<%# Eval("ID_Feature") %>' />
                        <label class="form-label fw-bold"><%# Eval("FeatureNombre") %></label>
                        <asp:TextBox ID="txtFeatureDescripcion" runat="server" CssClass="form-control" Text='<%# Eval("Descripcion") %>' TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="mt-4">
                 <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                 <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-secondary" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>

