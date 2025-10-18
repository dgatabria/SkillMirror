<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NuevoMensaje.aspx.cs" Inherits="SkillMirror.NuevoMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2"></h1>
    </div>

    <asp:Literal ID="litMensajeExito" runat="server" EnableViewState="false"></asp:Literal>

    <asp:Panel ID="pnlFormulario" runat="server">
        <div class="card">
            <div class="card-body">
                <div class="mb-3">
                    <asp:Label ID="lblAsunto" runat="server" AssociatedControlID="txtAsunto" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtAsunto" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAsunto" runat="server" ControlToValidate="txtAsunto"
                        Display="Dynamic" CssClass="text-danger" ValidationGroup="NuevoMensaje" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblMensaje" runat="server" AssociatedControlID="txtMensaje" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="8"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMensaje" runat="server" ControlToValidate="txtMensaje"
                        Display="Dynamic" CssClass="text-danger" ValidationGroup="NuevoMensaje" />
                </div>
                <div class="text-end">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary me-2" OnClick="BtnCancelar_Click" CausesValidation="false" />
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click" ValidationGroup="NuevoMensaje" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>