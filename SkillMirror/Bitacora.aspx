<%@ Page Title="Bitácora del Sistema" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="SkillMirror.Bitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Bitácora del Sistema</h1>
    </div>

    <div class="card mb-4">
        <div class="card-header">
            <asp:Literal ID="litFiltrosTitle" runat="server">Filtros de Búsqueda</asp:Literal>
        </div>
        <div class="card-body">
            <div class="row g-3 align-items-center">
                <div class="col-md-3">
                    <asp:Label ID="lblFechaDesde" runat="server" CssClass="form-label" AssociatedControlID="txtFechaDesde">Fecha Desde</asp:Label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblFechaHasta" runat="server" CssClass="form-label" AssociatedControlID="txtFechaHasta">Fecha Hasta</asp:Label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblAccion" runat="server" CssClass="form-label" AssociatedControlID="txtAccion">Acción (contiene)</asp:Label>
                    <asp:TextBox ID="txtAccion" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblDetalle" runat="server" CssClass="form-label" AssociatedControlID="txtDetalle">Detalle (contiene)</asp:Label>
                    <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-12 text-end">
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="BtnFiltrar_Click" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="upBitacora" runat="server">
        <ContentTemplate>
            <div class="d-flex justify-content-end align-items-center mb-2">
                <asp:Literal ID="litMostrar" runat="server">Mostrar: </asp:Literal>
                <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select form-select-sm ms-2" style="width: auto;"
                    AutoPostBack="true" OnSelectedIndexChanged="DdlPageSize_SelectedIndexChanged">
                    <asp:ListItem Text="10" Value="10" Selected="True" />
                    <asp:ListItem Text="25" Value="25" />
                    <asp:ListItem Text="50" Value="50" />
                </asp:DropDownList>
            </div>

            <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-hover" GridLines="None" EnableViewState="False"
                AllowPaging="True" PageSize="10" OnPageIndexChanging="GvBitacora_PageIndexChanging" AllowCustomPaging="True"
                OnRowCreated="GvBitacora_RowCreated">
                <Columns>
                    <asp:BoundField DataField="TimeStamp" HeaderText="Fecha y Hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="Accion" HeaderText="Acción" />
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                    <asp:BoundField DataField="IP" HeaderText="Dirección IP" />
                    <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
                </Columns>
                <PagerSettings Mode="NumericFirstLast" 
                    Position="Bottom" 
                    PageButtonCount="5"
                    FirstPageText="« Primero"
                    LastPageText="Último »"
                    NextPageText="›"
                    PreviousPageText="‹" />
                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>