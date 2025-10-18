<%@ Page Title="Editar FAQ" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarFAQ.aspx.cs" Inherits="SkillMirror.EditarFAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Editar FAQ</h1>
    </div>
    
    <asp:HiddenField ID="hfFAQId" runat="server" Value="0" />

    <div class="row">
        <div class="col-md-9">
            
            <%-- CAMPO PARA EL ORDEN --%>
            <div class="mb-4 col-md-4">
                <asp:Label ID="lblOrden" runat="server" Text="Orden de Visualización" For="txtOrden" CssClass="form-label fw-bold"></asp:Label>
                <asp:TextBox ID="txtOrden" runat="server" CssClass="form-control" TextMode="Number" Text="0"></asp:TextBox>
                <asp:CompareValidator ID="cvOrden" runat="server" ControlToValidate="txtOrden" Operator="DataTypeCheck" Type="Integer"
                    ErrorMessage="El orden debe ser un número entero." CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
            </div>

            <hr />

            <%-- REPEATER PARA LAS TRADUCCIONES --%>
            <h5 id="h5Traducciones" runat="server" class="mb-3">Traducciones</h5>
            <asp:Repeater ID="rptTraducciones" runat="server">
                <ItemTemplate>
                    <div class="card mb-3">
                        <div class="card-header fw-bold">
                            <%# Eval("Nombre") %>
                        </div>
                        <div class="card-body">
                            <asp:HiddenField ID="hfIdiomaId" runat="server" Value='<%# Eval("Codigo") %>' />
                            
                            <div class="mb-3">
                                <label for="txtPregunta" class="form-label"><asp:Literal ID="litPreguntaLabel" runat="server" Text="Pregunta"></asp:Literal></label>
                                <asp:TextBox ID="txtPregunta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                            <div>
                                <label for="txtRespuesta" class="form-label"><asp:Literal ID="litRespuestaLabel" runat="server" Text="Respuesta"></asp:Literal></label>
                                <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <%-- BOTONES DE ACCIÓN --%>
            <div class="d-flex justify-content-end mt-4">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary me-2" OnClick="BtnCancelar_Click" CausesValidation="false" />
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" />
            </div>

        </div>
    </div>
</asp:Content>