<%@ Page Title="Restablecer Contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SkillMirror.ResetPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .reset-container { display: flex; align-items: center; justify-content: center; min-height: 70vh; }
        .reset-card { max-width: 450px; width: 100%; }
    </style>

    <div class="reset-container">
        <div class="card reset-card shadow-lg">
            <div class="card-header text-white text-center" style="background-color: var(--primary-color);">
                <h3><asp:Literal ID="litHeaderTitle" runat="server">Crear Nueva Contraseña</asp:Literal></h3>
            </div>
            <div class="card-body p-4">
                
                <asp:Panel ID="pnlFormulario" runat="server">
                    <asp:ValidationSummary ID="valSummary" runat="server" ForeColor="Red" CssClass="mb-3" ValidationGroup="Reset" />
                    <asp:HiddenField ID="hfToken" runat="server" />

                    <div class="mb-3">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="form-label">Nueva Contraseña</asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequiredValidator" runat="server" ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" ValidationGroup="Reset" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label">Confirmar Nueva Contraseña</asp:Label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" Type="String" ForeColor="Red" Display="Dynamic" ValidationGroup="Reset" />
                    </div>
                    <div class="d-grid mt-4">
                        <asp:Button ID="btnReset" runat="server" Text="Guardar Contraseña" CssClass="btn btn-primary" OnClick="BtnReset_Click" ValidationGroup="Reset" />
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlResultado" runat="server" Visible="false">
                    <asp:Literal ID="litResultado" runat="server"></asp:Literal>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>