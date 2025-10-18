<%@ Page Title="Backup y Restore" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="SkillMirror.BackupPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <%-- Script de JavaScript para manejar el modal de confirmación --%>
    <script type="text/javascript">
        function mostrarModalRestore(backupId, backupFecha) {
            // Asigna el ID del backup al campo oculto para que el servidor lo pueda leer en el postback.
            document.getElementById('<%= hfRestoreBackupId.ClientID %>').value = backupId;

            // Personaliza el texto del modal para informar al usuario qué está a punto de restaurar.
            var infoElement = document.getElementById('modalRestoreInfo');
            var originalText = document.getElementById('<%= hfRestoreInfoText.ClientID %>').value;
            infoElement.innerText = originalText.replace('{FECHA}', backupFecha);

            // Limpia el campo de contraseña cada vez que se abre el modal.
            document.getElementById('<%= txtConfirmPassword.ClientID %>').value = '';

            // Muestra el modal utilizando la API de Bootstrap.
            var restoreModal = new bootstrap.Modal(document.getElementById('restoreModal'));
            restoreModal.show();

            // Retorna false para prevenir que el LinkButton de la grilla haga un postback.
            return false;
        }
    </script>
    
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Copia de Seguridad y Restauración</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <asp:Button ID="btnNuevoBackup" runat="server" Text="Crear Nuevo Backup" CssClass="btn btn-primary" OnClick="BtnNuevoBackup_Click" />
        </div>
    </div>

    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

    <%-- Modal de Confirmación de Restore --%>
    <div class="modal fade" id="restoreModal" tabindex="-1" aria-labelledby="restoreModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="restoreModalLabel"><asp:Literal ID="litModalTitle" runat="server">Confirmar Restauración</asp:Literal></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p id="modalRestoreInfo"></p>
                    <asp:HiddenField ID="hfRestoreInfoText" runat="server" />
                    <div class="mb-3">
                        <asp:Label ID="lblConfirmPassword" runat="server" For="txtConfirmPassword" CssClass="form-label">Para confirmar, ingrese su contraseña:</asp:Label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnModalCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" data-bs-dismiss="modal" CausesValidation="false" />
                    <asp:Button ID="btnConfirmarRestore" runat="server" Text="Restaurar Ahora" CssClass="btn btn-danger" OnClick="BtnConfirmarRestore_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfRestoreBackupId" runat="server" />
    <%-- Fin del Modal --%>

    <h3 id="headerHistorial" runat="server" class="mt-4">Historial de Backups</h3>
    <div class="table-responsive">
        <asp:GridView ID="gvBackups" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvBackups_RowCommand" OnRowDataBound="GvBackups_RowDataBound" OnRowCreated="GvBackups_RowCreated">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="ID" />
                <asp:BoundField DataField="TimeStamp" HeaderText="Fecha de Creación" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                <asp:TemplateField HeaderText="Tamaño">
                    <ItemTemplate>
                        <asp:Literal ID="litTamano" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UsuarioEmail" HeaderText="Creado por" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDescargar" runat="server" CssClass="btn btn-sm btn-outline-success me-2" CommandName="Descargar" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnRestaurarFila" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandArgument='<%# Eval("Codigo") %>' CommandName="Restaurar"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="alert alert-info" role="alert">
                    No se han encontrado backups registrados.
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>