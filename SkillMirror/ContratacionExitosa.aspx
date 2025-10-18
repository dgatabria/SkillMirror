<%@ Page Title="Contratación Exitosa" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeBehind="ContratacionExitosa.aspx.cs" Inherits="SkillMirror.ContratacionExitosa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container text-center mt-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                <div class="card shadow-sm p-5">
                    <div class="card-body">
                        <%-- Icono de éxito --%>
                        <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="currentColor" class="bi bi-check-circle-fill text-success mb-4" viewBox="0 0 16 16">
                            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                        </svg>

                        <%-- Título principal --%>
                        <h1 id="headerTitle" runat="server" class="display-5 mb-3"></h1>
                        
                        <%-- Subtítulo con el nombre del plan --%>
                        <p id="subHeader" runat="server" class="lead">
                            <asp:Literal ID="litPlan" runat="server"></asp:Literal>
                        </p>

                        <%-- Texto de llamado a la acción --%>
                        <p id="textoAccion" runat="server" class="mt-4"></p>
                        
                        <%-- Botón para ir a la consola --%>
                        <asp:HyperLink ID="btnIrAConsola" runat="server" CssClass="btn btn-primary btn-lg mt-3" NavigateUrl="~/Consola.aspx"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
