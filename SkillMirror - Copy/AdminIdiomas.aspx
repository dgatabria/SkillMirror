<%@ Page Title="Administración de Idiomas" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminIdiomas.aspx.cs" Inherits="SkillMirror.AdminIdiomas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Administración de Idiomas</h1>
    </div>

    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

    <div class="row">
        <%-- Columna para la Grilla de Idiomas --%>
        <div class="col-md-7">
            <div class="card">
                <div class="card-header">
                    <h5 id="cardExistentesTitle" runat="server">Idiomas Existentes</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvIdiomas" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-striped table-hover" GridLines="None"
                        OnRowCommand="GvIdiomas_RowCommand" OnRowDataBound="GvIdiomas_RowDataBound" OnRowCreated="GvIdiomas_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="Codigo" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-sm btn-outline-primary me-2" CommandName="Exportar" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="Eliminar" CommandArgument='<%# Eval("Codigo") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <%-- Columna para Importar un Nuevo Idioma --%>
        <div class="col-md-5">
            <div class="card">
                <div class="card-header">
                    <h5 id="cardImportarTitle" runat="server">Importar Nuevo Idioma desde XML</h5>
                </div>
                <div class="card-body">
                    <p id="importarText" runat="server" class="card-text text-muted">Selecciona un archivo XML con la estructura de traducciones para crear un nuevo idioma en el sistema.</p>
                    <div class="mb-3">
                        <asp:FileUpload ID="fileUploadXml" runat="server" CssClass="form-control" accept=".xml" />
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnImportar" runat="server" Text="Importar y Crear" CssClass="btn btn-success" OnClick="BtnImportar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>