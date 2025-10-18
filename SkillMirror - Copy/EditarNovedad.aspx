<%@ Page Title="Gestionar Novedad" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarNovedad.aspx.cs" Inherits="SkillMirror.EditarNovedad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 class="h2"><asp:Literal ID="litTitulo" runat="server"></asp:Literal></h1>
    </div>
    <div class="card">
        <div class="card-body">
            <asp:HiddenField ID="hfNovedadId" runat="server" Value="0" />
            <div class="mb-3">
                <asp:Label ID="lblTitulo" runat="server" AssociatedControlID="txtTitulo" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblSubtitulo" runat="server" AssociatedControlID="txtSubtitulo" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtSubtitulo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblCuerpo" runat="server" AssociatedControlID="txtCuerpo" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtCuerpo" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10"></asp:TextBox>
            </div>
            <hr />
            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" OnClick="BtnGuardar_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancelar" runat="server" OnClick="BtnCancelar_Click" CssClass="btn btn-secondary" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>