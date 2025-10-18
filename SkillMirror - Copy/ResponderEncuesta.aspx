<%@ Page Title="Responder Encuesta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResponderEncuesta.aspx.cs" Inherits="SkillMirror.ResponderEncuesta" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="col-md-8 offset-md-2">
            <asp:Panel ID="pnlEncuesta" runat="server">
                <div class="text-center">
                    <h1 class="display-5"><asp:Literal ID="litTituloEncuesta" runat="server"></asp:Literal></h1>
                    <p class="lead"><asp:Literal ID="litDescripcionEncuesta" runat="server"></asp:Literal></p>
                </div>
                <hr class="my-4" />
                <asp:HiddenField ID="hfInvitacionId" runat="server" />
                
                <%-- Aquí se dibujarán las preguntas dinámicamente --%>
                <asp:PlaceHolder ID="phPreguntas" runat="server"></asp:PlaceHolder>

                <div class="d-grid mt-4">
                    <asp:Button ID="btnEnviarRespuestas" runat="server" OnClick="BtnEnviarRespuestas_Click" CssClass="btn btn-primary btn-lg" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlResultado" runat="server" Visible="false" CssClass="text-center">
                <asp:Literal ID="litResultado" runat="server"></asp:Literal>
            </asp:Panel>
        </div>
    </div>
</asp:Content>