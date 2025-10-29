<%@ Page Title="Editar Usuario" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarUsuario.aspx.cs" Inherits="SkillMirror.EditarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 class="h2">
            <asp:Literal ID="litTitulo" runat="server" Text="Crear Nuevo Usuario"></asp:Literal>
        </h1>
    </div>
    
    <div class="card">
        <div class="card-body">
            
            <%-- Literal para mostrar mensajes de éxito o error --%>
            <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

            <asp:HiddenField ID="hfUsuarioId" runat="server" Value="0" />

            <%-- Campos de Nombre, Apellido, Email --%>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <asp:Label ID="lblNombre" runat="server" CssClass="form-label" AssociatedControlID="txtNombre">Nombre</asp:Label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <asp:Label ID="lblApellido" runat="server" CssClass="form-label" AssociatedControlID="txtApellido">Apellido</asp:Label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblEmail" runat="server" CssClass="form-label" AssociatedControlID="txtEmail">Email</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
            </div>
            
            <div class="col-md-4 mb-3"> <%-- NUEVO CAMPO DNI --%>
                    <asp:Label ID="lblDNI" runat="server" CssClass="form-label" AssociatedControlID="txtDNI"></asp:Label>
                    <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox> <%-- TextMode Number opcional --%>
                    <asp:RequiredFieldValidator ID="rfvDNI" runat="server" ControlToValidate="txtDNI" CssClass="text-danger" Display="Dynamic" ErrorMessage="" ValidationGroup="EdicionUsuario" />
                </div>
            <%-- SECCIÓN DE ROLES --%>
            <div class="mb-3">
                <asp:Label ID="lblRoles" runat="server" CssClass="form-label">Roles Asignados</asp:Label>
                <div class="border rounded p-2" style="max-height: 150px; overflow-y: auto;">
                    <asp:CheckBoxList ID="cblRoles" runat="server" CssClass="form-check-list"></asp:CheckBoxList>
                </div>
            </div>


            <div class="form-check mb-3">
                <asp:CheckBox ID="chkBloqueado" runat="server" CssClass="form-check-input" />
                <asp:Label ID="lblBloqueado" runat="server" CssClass="form-check-label" AssociatedControlID="chkBloqueado">Usuario Bloqueado</asp:Label>
            </div>
            
            <hr />

            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false" OnClick="BtnCancelar_Click" />
                
                <asp:Button ID="btnResetPassword" runat="server" Text="Enviar Email de Reseteo" CssClass="btn btn-warning float-end" CausesValidation="false" Visible="false" OnClick="BtnResetPassword_Click" />
            </div>
        </div>
    </div>
    
    <%-- El script no contiene texto visible, por lo que no necesita cambios --%>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            const rolesContainer = document.getElementById('<%= cblRoles.ClientID %>');
            const empresaContainer = document.getElementById('empresaContainer');

            function toggleEmpresaField() {
                if (!rolesContainer || !empresaContainer) {
                    return;
                }
                let isCandidato = false;
                const checkboxes = rolesContainer.getElementsByTagName('input');
                const labels = rolesContainer.getElementsByTagName('label');
                for (let i = 0; i < checkboxes.length; i++) {
                    if (labels[i].innerText.toLowerCase() === 'candidato' && checkboxes[i].checked) {
                        isCandidato = true;
                        break;
                    }
                }
                empresaContainer.style.display = isCandidato ? 'none' : 'block';
            }

            toggleEmpresaField();
            if (rolesContainer) {
                rolesContainer.addEventListener('click', toggleEmpresaField);
            }
        });
    </script>
</asp:Content>