<%@ Page Title="Editar Publicidad" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarPublicidad.aspx.cs" Inherits="SkillMirror.EditarPublicidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Editar Publicidad</h1>
    </div>
    
    <asp:HiddenField ID="hfPublicidadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfRutaImagenActual" runat="server" Value="" />

    <div class="row">
        <div class="col-md-8">
            <div class="mb-3">
                <asp:Label ID="lblTitulo" runat="server" Text="Título (para tooltip y texto alternativo)" For="txtTitulo" CssClass="form-label fw-bold"></asp:Label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo" ErrorMessage="El título es obligatorio." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblURL" runat="server" Text="URL de Destino (al hacer clic)" For="txtURL" CssClass="form-label fw-bold"></asp:Label>
                <asp:TextBox ID="txtURL" runat="server" CssClass="form-control" TextMode="Url"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtURL" ErrorMessage="La URL es obligatoria." CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblImagen" runat="server" Text="Imagen del Banner" For="fileImagen" CssClass="form-label fw-bold"></asp:Label>
                <asp:FileUpload ID="fileImagen" runat="server" CssClass="form-control" />
                <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail mt-2" Width="200" Visible="false" />
                <small class="form-text text-muted">Si no seleccionas una nueva imagen, se conservará la actual (en modo edición).</small>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha de Inicio" For="txtFechaInicio" CssClass="form-label fw-bold"></asp:Label>
                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblFechaExpiracion" runat="server" Text="Fecha de Expiración" For="txtFechaExpiracion" CssClass="form-label fw-bold"></asp:Label>
                    <asp:TextBox ID="txtFechaExpiracion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>
            <div class="mb-3 form-check">
                <asp:CheckBox ID="chkActivo" runat="server" Checked="true" CssClass="form-check-input" />
                <asp:Label ID="lblActivo" runat="server" Text=" Activo" For="chkActivo" CssClass="form-check-label"></asp:Label>
            </div>
            <hr />
            <div class="d-flex justify-content-end">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary me-2" OnClick="BtnCancelar_Click" CausesValidation="false" />
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" />
            </div>
        </div>
    </div>
</asp:Content>