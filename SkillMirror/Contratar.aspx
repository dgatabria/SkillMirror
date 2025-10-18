<%@ Page Title="Contratar Plan" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeBehind="Contratar.aspx.cs" Inherits="SkillMirror.Contratar" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function togglePaymentPanel(checkbox, panelId) {
            const panel = document.getElementById(panelId);
            if (panel) {
                panel.style.display = checkbox.checked ? 'block' : 'none';
            }
        }
    </script>

    <div class="container my-4">
        <div class="row justify-content-center">
            <div class="col-lg-9">
                
                <%-- Resumen de la Compra (Sin Cambios) --%>
                <div class="card mb-4 shadow-sm">
                    <div class="card-header bg-dark text-white">
                        <h2 id="headerResumen" runat="server">Resumen de la Compra</h2>
                    </div>
                    <div class="card-body">
                        <h4 class="card-title">
                            <asp:Label ID="lblPlanContratado" runat="server"></asp:Label>
                            <asp:Literal ID="litPlanNombre" runat="server" Text="..."></asp:Literal>
                        </h4>
                        <p class="card-text fs-3">
                            <asp:Label ID="lblTotalAPagar" runat="server"></asp:Label>
                            <strong>$<asp:Literal ID="litPlanPrecio" runat="server" Text="0.00"></asp:Literal></strong>
                        </p>
                    </div>
                </div>

                <%-- Panel de Datos de Empresa (Sin Cambios) --%>
                <asp:Panel ID="pnlDatosEmpresa" runat="server" Visible="false">
                    <div class="card mb-4 shadow-sm">
                        <div class="card-header">
                            <h3 id="headerDatosEmpresa" runat="server">Completa los Datos de tu Empresa</h3>
                        </div>
                        <div class="card-body">
                           <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_NombreEmpresa") %></label>
                                <asp:TextBox ID="txtNombreEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreEmpresa" runat="server" ValidationGroup="Empresa" ControlToValidate="txtNombreEmpresa" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_CUIT") %></label>
                                <asp:TextBox ID="txtCUIT" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCUIT" runat="server" ValidationGroup="Empresa" ControlToValidate="txtCUIT" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_Domicilio") %></label>
                                <asp:TextBox ID="txtDomicilio" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_Telefono") %></label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <%-- Panel de Medios de Pago (CORREGIDO) --%>
                <div class="card mb-4 shadow-sm">
                    <div class="card-header">
                        <h3 id="headerMedioPago" runat="server">Medio de Pago</h3>
                    </div>
                    <div class="card-body">
                        <%-- Opción Tarjeta de Crédito: Se quita el onclick --%>
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="chkTarjeta" runat="server" />
                        </div>
                        <asp:Panel ID="pnlTarjetaCredito" runat="server" style="display: none;" CssClass="p-3 border rounded mb-3 bg-light">
                           <%-- Contenido del panel (sin cambios) --%>
                           <h5><asp:Label ID="lblHeaderTarjeta" runat="server"></asp:Label></h5>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_NombreTitular") %></label>
                                <asp:TextBox ID="txtNombreTitular" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreTitular" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtNombreTitular" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_NumeroTarjeta") %></label>
                                <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control" MaxLength="19" placeholder="XXXX XXXX XXXX XXXX"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNumeroTarjeta" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtNumeroTarjeta" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label"><%= this.Traducir("Contratar_Label_Expiracion") %></label>
                                    <asp:TextBox ID="txtExpiracion" runat="server" CssClass="form-control" placeholder="MM/AA"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvExpiracion" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtExpiracion" CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revExpiracion" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtExpiracion" CssClass="text-danger" Display="Dynamic" ErrorMessage="Formato inválido." ValidationExpression="^(0[1-9]|1[0-2])\/?([0-9]{2})$" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">CVV</label>
                                    <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" MaxLength="4" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCVV" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtCVV" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_Monto") %></label>
                                <asp:TextBox ID="txtMontoTarjeta" runat="server" CssClass="form-control" TextMode="Number" step="0.01">0</asp:TextBox>
                            </div>
                        </asp:Panel>
                        
                        <%-- Opción Nota de Crédito: Se quita el onclick --%>
                        <asp:Panel ID="pnlNotaCreditoContainer" runat="server" Visible="false">
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="chkNotaCredito" runat="server" />
                            </div>
                            <asp:Panel ID="pnlNotaCreditoDetalle" runat="server" style="display: none;" CssClass="p-3 border rounded mb-3 bg-light">
                                <%-- Contenido del panel (sin cambios) --%>
                                <h5><asp:Label ID="lblHeaderNC" runat="server"></asp:Label></h5>
                                <asp:Repeater ID="rptNotasCredito" runat="server">
                                    <ItemTemplate>
                                        <div class="form-check">
                                            <asp:HiddenField ID="hfIdNC" runat="server" Value='<%# Eval("Codigo") %>' />
                                            <asp:CheckBox ID="chkUsarNC" runat="server" />
                                            <label class="form-check-label">
                                                <%# Eval("CodigoNC") %> - Saldo: $<%# Eval("Valor", "{0:F2}") %> (Vence: <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}") %>)
                                            </label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </asp:Panel>
                        </asp:Panel>

                        <%-- Opción Transferencia Bancaria: Se quita el onclick --%>
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="chkTransferencia" runat="server" />
                        </div>
                        <asp:Panel ID="pnlTransferencia" runat="server" style="display: none;" CssClass="p-3 border rounded mb-3 bg-light">
                            <%-- Contenido del panel (sin cambios) --%>
                            <h5><asp:Label ID="lblHeaderTransferencia" runat="server"></asp:Label></h5>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_Comprobante") %></label>
                                <asp:TextBox ID="txtComprobanteTransferencia" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvComprobante" runat="server" ValidationGroup="Pago" Enabled="false" ControlToValidate="txtComprobanteTransferencia" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label"><%= this.Traducir("Contratar_Label_Monto") %></label>
                                <asp:TextBox ID="txtMontoTransferencia" runat="server" CssClass="form-control" TextMode="Number" step="0.01">0</asp:TextBox>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                
                <%-- Botón y Error (Sin Cambios) --%>
                <div class="d-grid gap-2">
                    <asp:Button ID="btnConfirmarPago" runat="server" OnClick="btnConfirmarPago_Click" CssClass="btn btn-primary btn-lg" />
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger mt-2" Visible="false"></asp:Label>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

