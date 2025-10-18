<%@ Page Title="Preguntas Frecuentes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQs.aspx.cs" Inherits="SkillMirror.FAQs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container mt-5 mb-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                
                <h1 id="headerTitle" runat="server" class="text-center mb-4">Preguntas Frecuentes</h1>

                <div class="accordion" id="faqAccordion">
                    <asp:Repeater ID="rptFAQs" runat="server">
                        <ItemTemplate>
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="heading_<%# Eval("Codigo") %>">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                            data-bs-target="#collapse_<%# Eval("Codigo") %>" aria-expanded="false" 
                                            aria-controls="collapse_<%# Eval("Codigo") %>">
                                        <%# Eval("Pregunta") %>
                                    </button>
                                </h2>
                                <div id="collapse_<%# Eval("Codigo") %>" class="accordion-collapse collapse" 
                                     aria-labelledby="heading_<%# Eval("Codigo") %>" data-bs-parent="#faqAccordion">
                                    <div class="accordion-body">
                                        <%# Eval("Respuesta") %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:Panel ID="pnlNoFAQs" runat="server" Visible="false" CssClass="text-center mt-4">
                    <p id="pNoFAQs" runat="server" class="text-muted">No hay preguntas frecuentes disponibles en este momento.</p>
                </asp:Panel>

            </div>
        </div>
    </div>

</asp:Content>