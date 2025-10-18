<%@ Page Title="Administración de Reseñas" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminResenas.aspx.cs" Inherits="SkillMirror.AdminResenas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Reseñas</h1>
    </div>

    <asp:Literal ID="litMensaje" runat="server"></asp:Literal>

    <div class="table-responsive">
        <asp:GridView ID="gvResenas" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-hover" GridLines="None"
            OnRowCommand="GvResenas_RowCommand" 
            OnRowDataBound="GvResenas_RowDataBound"
            OnRowCreated="GvResenas_RowCreated"> <%-- ¡CAMBIO CLAVE AQUÍ! --%>
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Autor">
                    <ItemTemplate>
                        <%# Eval("Autor.Nombre") %> <%# Eval("Autor.Apellido") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Asunto">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnVerComentario" runat="server" 
                            Text='<%# Eval("Asunto") %>' 
                            OnClientClick='<%# "abrirModalResena(\"" + HttpUtility.JavaScriptStringEncode(Eval("Asunto").ToString()) + "\", \"" + HttpUtility.JavaScriptStringEncode(Eval("Comentario").ToString()) + "\"); return false;" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Puntuación">
                    <ItemTemplate>
                        <span class="text-warning"><%# RenderStars(Eval("Puntuacion")) %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAprobar" runat="server" CssClass="btn btn-sm btn-outline-success me-2"
                            CommandName="AprobarResena" CommandArgument='<%# Eval("Codigo") %>' Visible='<%# !(bool)Eval("Aprobado") %>'>Aprobar</asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger"
                            CommandName="EliminarResena" CommandArgument='<%# Eval("Codigo") %>'>Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <%-- Modal para ver el detalle de la reseña --%>
    <div class="modal fade" id="modalResena" tabindex="-1" aria-labelledby="modalResenaLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalResenaLabel">Detalle de la Reseña</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p style="white-space: pre-wrap;"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        function abrirModalResena(asunto, comentario) {
            var modal = new bootstrap.Modal(document.getElementById('modalResena'));
            document.getElementById('modalResenaLabel').innerText = asunto;
            document.querySelector('#modalResena .modal-body p').innerText = comentario;
            modal.show();
        }
    </script>
</asp:Content>