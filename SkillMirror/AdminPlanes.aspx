<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminPlanes.aspx.cs" Inherits="SkillMirror.AdminPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2"></h1>
        <div>
            <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-primary" OnClick="btnNuevo_Click" />
        </div>
    </div>

    <%-- GridView para mostrar la lista de planes, ahora sin UpdatePanel --%>
    <asp:GridView ID="gvPlanes" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover"
        DataKeyNames="ID" OnRowCommand="gvPlanes_RowCommand" OnRowDataBound="gvPlanes_RowDataBound" OnRowCreated="gvPlanes_RowCreated">
        <Columns>
            <asp:BoundField DataField="Nombre" />
            <asp:BoundField DataField="PrecioMensual" DataFormatString="{0:C}" />
            <asp:BoundField DataField="Orden" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkActivo" runat="server" Checked='<%# Eval("Activo") %>' Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkDestacado" runat="server" Checked='<%# Eval("EsDestacado") %>' Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%-- Usamos LinkButton como en el ejemplo de Encuestas --%>
                    <asp:LinkButton ID="btnEditar" runat="server" CommandName="EditarPlan" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-secondary" ></asp:LinkButton>
                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="EliminarPlan" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-danger ms-1" ></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

