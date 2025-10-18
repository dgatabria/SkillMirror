<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="SkillMirror.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 id="headerTitle" runat="server" class="h2">Mi Perfil</h1>
    </div>

    <asp:Literal ID="litMensaje" runat="server" EnableViewState="false"></asp:Literal>

    <div class="row">
        <%-- Columna para Datos Personales --%>
        <div class="col-lg-6 col-xl-3 mb-4">
            <div class="card h-100">
                <div class="card-header">
                    <h5 id="cardDatosTitle" runat="server">Datos Personales</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valNombre" runat="server" ControlToValidate="txtNombre" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valApellido" runat="server" ControlToValidate="txtApellido" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblDNI" runat="server" AssociatedControlID="txtDNI" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valDNI" runat="server" ControlToValidate="txtDNI" ForeColor="Red" Display="Dynamic" ValidationGroup="Datos" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblEmpresa" runat="server" AssociatedControlID="txtEmpresa" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-check my-3">
                        <asp:CheckBox ID="chkSuscrito" runat="server" CssClass="form-check-input" />
                        <asp:Label ID="lblSuscrito" runat="server" CssClass="form-check-label" AssociatedControlID="chkSuscrito"></asp:Label>
                    </div>
                    <div class="form-check my-3">
                        <asp:CheckBox ID="chkSuscritoEncuestas" runat="server" CssClass="form-check-input" />
                        <asp:Label ID="lblSuscritoEncuestas" runat="server" CssClass="form-check-label" AssociatedControlID="chkSuscritoEncuestas"></asp:Label>
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnGuardarDatos" runat="server" OnClick="BtnGuardarDatos_Click" ValidationGroup="Datos" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Columna para Cambiar Contraseña --%>
        <div class="col-lg-6 col-xl-3 mb-4">
            <div class="card h-100">
                <div class="card-header">
                    <h5 id="cardPasswordTitle" runat="server">Cambiar Contraseña</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <asp:Label ID="lblPassActual" runat="server" AssociatedControlID="txtPassActual" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtPassActual" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valPassActual" runat="server" ControlToValidate="txtPassActual" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblPassNueva" runat="server" AssociatedControlID="txtPassNueva" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtPassNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valPassNueva" runat="server" ControlToValidate="txtPassNueva" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblConfirmPassNueva" runat="server" AssociatedControlID="txtConfirmPassNueva" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtConfirmPassNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:CompareValidator ID="valComparePass" runat="server" ControlToValidate="txtConfirmPassNueva" ControlToCompare="txtPassNueva" Operator="Equal" Type="String" ForeColor="Red" Display="Dynamic" ValidationGroup="Password" />
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnCambiarPassword" runat="server" OnClick="BtnCambiarPassword_Click" ValidationGroup="Password" CssClass="btn btn-secondary" />
                    </div>
                </div>
            </div>
        </div>

        <%-- NUEVA COLUMNA: ESTADO DE CUENTA --%>
        <div class="col-lg-6 col-xl-3 mb-4">
            <div class="card h-100">
                <div class="card-header">
                    <h5 id="cardCuentaTitle" runat="server">Estado de Cuenta</h5>
                </div>
                <div class="card-body">
                    <h6><asp:Label ID="lblPlanActualHeader" runat="server"></asp:Label></h6>
                    <p>
                        <asp:Literal ID="litNombrePlan" runat="server"></asp:Literal>
                        (<asp:Literal ID="litDiasRestantes" runat="server"></asp:Literal>)
                    </p>

                    <h6 class="mt-4"><asp:Label ID="lblProximoPagoHeader" runat="server"></asp:Label></h6>
                    <p>$<asp:Literal ID="litProximoPago" runat="server"></asp:Literal></p>
                    
                    <%-- Panel que solo se muestra si hay notas de crédito --%>
                    <asp:Panel ID="pnlNotasCredito" runat="server" Visible="false">
                        <h6 class="mt-4"><asp:Label ID="lblNotasCreditoHeader" runat="server"></asp:Label></h6>
                        <ul class="list-group">
                           <asp:Repeater ID="rptNotasCredito" runat="server">
                               <ItemTemplate>
                                   <li class="list-group-item">
                                       <%# Eval("CodigoNC") %>: $<%# Eval("Valor", "{0:N2}") %>
                                       <br />
                                       <small class="text-muted"><%= this.Traducir("MiPerfil_CardCuenta_Vence") %> <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}") %></small>
                                   </li>
                               </ItemTemplate>
                           </asp:Repeater>
                        </ul>
                    </asp:Panel>
                </div>
                 <div class="card-footer text-end">
                    <asp:Button ID="btnActualizarPlan" runat="server" OnClick="BtnActualizarPlan_Click" CausesValidation="false" CssClass="btn btn-success btn-sm" />
                </div>
            </div>
        </div>

        <%-- NUEVA COLUMNA: HISTORIAL DE PAGOS --%>
        <div class="col-lg-6 col-xl-3 mb-4">
            <div class="card h-100">
                <div class="card-header">
                    <h5 id="cardPagosTitle" runat="server">Historial de Pagos</h5>
                </div>
                <div class="card-body p-2">
                    <asp:GridView ID="gvHistorialPagos" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-sm table-borderless" GridLines="None" EmptyDataText="No hay pagos registrados.">
                        <Columns>
                            <asp:BoundField DataField="FechaEmision" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Concepto" />
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto" DataFormatString="{0:C}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

