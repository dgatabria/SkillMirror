<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminConversaciones.aspx.cs" Inherits="SkillMirror.AdminConversaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2"></h1>
    </div>

    <div class="table-responsive">
        <asp:GridView ID="gvConversaciones" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-hover" GridLines="None"
            DataKeyNames="Codigo"
            OnRowCommand="GvConversaciones_RowCommand"
            OnRowDataBound="GvConversaciones_RowDataBound"
            OnRowCreated="GvConversaciones_RowCreated">
            <Columns>
                <%-- Los HeaderText se asignarán desde el code-behind en el evento RowCreated --%>
                <asp:BoundField DataField="Usuario.Nombre" HeaderText="Usuario" />
                <asp:BoundField DataField="Asunto" HeaderText="Asunto" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaUltimoMensaje" HeaderText="Última Actividad" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnVer" runat="server"
                            CssClass="btn btn-sm btn-primary"
                            CommandName="VerConversacion"
                            CommandArgument='<%# Eval("Codigo") %>'>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="alert alert-info">
                    <asp:Literal ID="litNoConversaciones" runat="server"></asp:Literal>
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>