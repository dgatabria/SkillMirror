<%@ Page Title="Admin Notas de Crédito" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeBehind="AdminNotasCredito.aspx.cs" Inherits="SkillMirror.AdminNotasCredito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Título de la página --%>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Notas de Crédito</h1>
    </div>

    <%-- Panel para mostrar mensajes de éxito o error --%>
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert" role="alert">
        <asp:Literal ID="litAlert" runat="server"></asp:Literal>
    </asp:Panel>

    <%-- Sección para Crear una Nueva Nota de Crédito --%>
    <div class="card mb-4">
        <div class="card-header">
            <h5 id="headerCrearNC" runat="server">Crear Nueva Nota de Crédito</h5>
        </div>
        <div class="card-body">
            <%-- Usamos un UpdatePanel para la carga dinámica de facturas --%>
            <asp:UpdatePanel ID="upCrearNC" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label id="lblEmpresa" runat="server" class="form-label">Empresa</label>
                            <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" ControlToValidate="ddlEmpresa" InitialValue="0"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label id="lblFacturaOrigen" runat="server" class="form-label">Factura de Origen</label>
                            <asp:DropDownList ID="ddlFacturaOrigen" runat="server" CssClass="form-select" Enabled="false"></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="rfvFactura" runat="server" ControlToValidate="ddlFacturaOrigen" InitialValue="0"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                             <label id="lblValor" runat="server" class="form-label">Valor</label>
                             <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvValor" runat="server" ControlToValidate="txtValor"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                             <asp:CompareValidator ID="cvValor" runat="server" ControlToValidate="txtValor" Operator="GreaterThan" ValueToCompare="0" Type="Currency"
                                 CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                        </div>
                         <div class="col-md-6 mb-3">
                             <label id="lblVigencia" runat="server" class="form-label">Vigencia (días)</label>
                             <asp:TextBox ID="txtDiasVigencia" runat="server" CssClass="form-control" TextMode="Number" Text="365"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvVigencia" runat="server" ControlToValidate="txtDiasVigencia"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                            <asp:CompareValidator ID="cvVigencia" runat="server" ControlToValidate="txtDiasVigencia" Operator="GreaterThan" ValueToCompare="0" Type="Integer"
                                 CssClass="text-danger" Display="Dynamic" ValidationGroup="CrearNC" />
                        </div>
                    </div>
                    <asp:Button ID="btnCrearNC" runat="server" OnClick="btnCrearNC_Click" CssClass="btn btn-primary" ValidationGroup="CrearNC" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%-- Grilla para visualizar las Notas de Crédito existentes --%>
    <div class="table-responsive">
        <asp:GridView ID="gvNotasCredito" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-hover" GridLines="None"
            EmptyDataText="No hay notas de crédito registradas.">
            <Columns>
                <asp:BoundField DataField="CodigoNC" HeaderText="Código" />
                <asp:BoundField DataField="NombreEmpresa" HeaderText="Empresa" />
                <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                <asp:BoundField DataField="FechaVencimiento" HeaderText="Vencimiento" DataFormatString="{0:dd/MM/yyyy}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

