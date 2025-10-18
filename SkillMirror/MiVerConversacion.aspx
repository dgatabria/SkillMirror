<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="MiVerConversacion.aspx.cs" Inherits="SkillMirror.MiVerConversacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .chat-container { max-height: 500px; overflow-y: auto; display: flex; flex-direction: column; }
        .mensaje { margin-bottom: 1rem; }
        .mensaje-propio { text-align: right; }
        .mensaje-propio .mensaje-burbuja { background-color: #dcf8c6; border-radius: 10px 10px 0 10px; }
        .mensaje-ajeno .mensaje-burbuja { background-color: #f1f0f0; border-radius: 10px 10px 10px 0; }
        .mensaje-burbuja { display: inline-block; padding: 10px 15px; max-width: 80%; text-align: left; }
        .mensaje-meta { font-size: 0.75rem; color: #888; }
    </style>

    <div class="d-flex justify-content-between align-items-center pt-3 pb-2 mb-3 border-bottom">
        <div>
            <h1 id="headerTitle" runat="server" class="h2"></h1>
        </div>
        <asp:HyperLink ID="btnVolver" runat="server" NavigateUrl="~/MisMensajes.aspx" CssClass="btn btn-outline-secondary"></asp:HyperLink>
    </div>

    <div class="card">
        <div id="chatContainer" class="card-body chat-container" runat="server">
            <asp:Repeater ID="rptMensajes" runat="server">
                <ItemTemplate>
                    <div class='<%# GetMensajeCssClass(Eval("Remitente.Codigo")) %>'>
                        <div class="mensaje-burbuja">
                            <p class="mb-1"><%# Eval("CuerpoMensaje") %></p>
                            <div class="mensaje-meta text-end">
                                <small><%# Eval("FechaEnvio", "{0:dd/MM/yy HH:mm}") %></small>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="card-footer">
            <asp:Panel ID="pnlResponder" runat="server">
                <div class="input-group">
                    <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click" ValidationGroup="EnviarMensaje" />
                </div>
                <asp:RequiredFieldValidator ID="rfvRespuesta" runat="server" ControlToValidate="txtRespuesta"
                    Display="Dynamic" CssClass="text-danger mt-1" ValidationGroup="EnviarMensaje" />
            </asp:Panel>
        </div>
    </div>
<script type="text/javascript">
        // Se ejecuta cuando el contenido de la página está listo.
        document.addEventListener('DOMContentLoaded', function () {
            // Obtenemos el contenedor del chat usando el ID que le asigna ASP.NET.
            const chatBox = document.getElementById('<%= chatContainer.ClientID %>');
            
            // Si el contenedor existe, hacemos que su scroll vertical...
            // ...sea igual a su altura total. Esto lo lleva hasta el fondo.
            if (chatBox) {
                chatBox.scrollTop = chatBox.scrollHeight;
            }
        });
    </script>+
</asp:Content>