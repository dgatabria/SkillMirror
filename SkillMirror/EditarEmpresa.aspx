<%@ Page Title="Gestionar Empresa" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarEmpresa.aspx.cs" Inherits="SkillMirror.EditarEmpresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 class="h2">
            <asp:Literal ID="litTitulo" runat="server" Text="Crear Nueva Empresa"></asp:Literal>
        </h1>
    </div>
    
    <div class="card">
        <div class="card-body">
            <asp:HiddenField ID="hfEmpresaId" runat="server" Value="0" />

            <div class="mb-3">
                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" CssClass="form-label">Nombre</asp:Label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." ForeColor="Red" Display="Dynamic" ValidationGroup="EditarEmpresa" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblCUIT" runat="server" AssociatedControlID="txtCUIT" CssClass="form-label">CUIT</asp:Label>
                <asp:TextBox ID="txtCUIT" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valCUIT" runat="server" ControlToValidate="txtCUIT" ErrorMessage="El CUIT es obligatorio." ForeColor="Red" Display="Dynamic" ValidationGroup="EditarEmpresa" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono" CssClass="form-label">Teléfono</asp:Label>
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblDomicilio" runat="server" AssociatedControlID="txtDomicilio" CssClass="form-label">Domicilio</asp:Label>
                <asp:TextBox ID="txtDomicilio" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblMedioDePago" runat="server" AssociatedControlID="txtMedioDePago" CssClass="form-label">Medio de Pago</asp:Label>
                <asp:TextBox ID="txtMedioDePago" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            
            <hr />

            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" ValidationGroup="EditarEmpresa" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false" OnClick="BtnCancelar_Click" />
            </div>
        </div>
    </div>
</asp:Content>