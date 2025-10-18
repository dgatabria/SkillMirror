<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SkillMirror._Default" %>
<%@ Register Src="~/Controles/BannerPublicidad.ascx" TagPrefix="uc" TagName="Banner" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Estilos específicos para la encuesta flotante y la portada --%>
    <style>
        .hero-section {
            background: linear-gradient(rgba(0, 128, 128, 0.7), rgba(0, 128, 128, 0.7)), url('https://images.unsplash.com/photo-1556761175-5973dc0f32e7?q=80&w=1932&auto=format&fit=crop') no-repeat center center;
            background-size: cover;
            color: white;
            padding: 120px 0;
            text-align: center;
        }
        .hero-section h1 {
            font-size: 3.5rem;
            font-weight: 700;
            color: white;
        }
        .hero-section .lead {
            font-size: 1.5rem;
            margin-bottom: 30px;
        }
        .hero-section .btn-cta {
            padding: 15px 40px;
            font-size: 1.2rem;
            border-radius: 50px;
        }
        .feature-card {
            text-align: center;
            padding: 20px;
            border: 1px solid #e9ecef;
            border-radius: 8px;
            margin-bottom: 20px;
            height: 100%;
        }
        .feature-card .icon {
            font-size: 3rem;
            color: #008080; /* Teal color */
            margin-bottom: 20px;
        }
        
        /* --- ESTILOS NUEVOS PARA LA ENCUESTA FLOTANTE --- */
        .encuesta-flotante {
            position: fixed;
            bottom: 20px;
            right: 20px;
            width: 320px;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            z-index: 1000;
            overflow: hidden;
            font-family: 'Roboto', sans-serif;
        }
        .encuesta-header {
            background-color: #008080; /* Teal */
            color: white;
            padding: 12px 15px;
            font-size: 1.1rem;
            font-weight: 500;
        }
        .encuesta-body {
            padding: 15px;
        }
        .encuesta-pregunta {
            font-size: 1rem;
            margin-bottom: 15px;
            color: #333;
        }
        .encuesta-opciones .btn-opcion {
            display: block;
            width: 100%;
            text-align: left;
            margin-bottom: 8px;
        }
        .resultado-barra-container {
            margin-bottom: 10px;
        }
        .resultado-texto {
            display: flex;
            justify-content: space-between;
            font-size: 0.9rem;
            margin-bottom: 4px;
        }
        .progreso-barra {
            background-color: #e9ecef;
            border-radius: .25rem;
            height: 1.2rem;
            overflow: hidden;
        }
        .progreso-valor {
            background-color: #008080;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100%;
            font-size: 0.8rem;
            transition: width 0.5s ease-in-out;
        }
    </style>

    <%-- SECCIÓN 1: HERO (PORTADA PRINCIPAL) --%>
    <div class="hero-section">
        <div class="container">
            <h1 id="heroTitle" runat="server">El Reflejo del Verdadero Talento</h1>
            <p id="heroSubtitle" runat="server" class="lead">
                Optimiza tu proceso de selección con nuestra plataforma de evaluación basada en IA y juegos cognitivos.
            </p>
            <asp:HyperLink ID="heroButton" runat="server" NavigateUrl="~/Contactenos.aspx" CssClass="btn btn-light btn-cta"></asp:HyperLink>
        </div>
    </div>

    <div class="container mt-5 pt-5">
        <%-- SECCIÓN 2: ¿QUÉ NOS HACE ÚNICOS? --%>
        <section class="text-center mb-5 pb-5">
            <h2 id="uniqueTitle" runat="server" class="mb-5">¿Qué hace único a SkillMirror?</h2>
            <div class="row">
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🎮</div>
                        <h3 id="feature1Title" runat="server"></h3>
                        <p id="feature1Text" runat="server"></p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🧠</div>
                        <h3 id="feature2Title" runat="server"></h3>
                        <p id="feature2Text" runat="server"></p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🔄</div>
                        <h3 id="feature3Title" runat="server"></h3>
                        <p id="feature3Text" runat="server"></p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">😊</div>
                        <h3 id="feature4Title" runat="server"></h3>
                        <p id="feature4Text" runat="server"></p>
                    </div>
                </div>
            </div>
        </section>

        <%-- SECCIÓN 3: PROPUESTA DE VALOR --%>
        <section class="mb-5 pb-5">
             <div class="row align-items-center">
                <div class="col-md-6">
                    <img src="https://images.unsplash.com/photo-1521791136064-7986c2920216?q=80&w=1769&auto=format&fit=crop" class="img-fluid rounded shadow" alt="Equipo de trabajo colaborando">
                </div>
                <div class="col-md-6 ps-md-5">
                    <h2 id="valueTitle" runat="server"></h2>
                    <p id="valueText" runat="server"></p>
                    <asp:HyperLink ID="valueButton" runat="server" NavigateUrl="~/QuienesSomos.aspx" CssClass="btn btn-outline-primary"></asp:HyperLink>
                </div>
            </div>
        </section>
        <uc:Banner runat="server" ID="Banner1" />
        
        <%-- SECCIÓN 4: CALL TO ACTION FINAL --%>
        <section class="text-center bg-light py-5 rounded">
            <h2 id="ctaTitle" runat="server"></h2>
             <p id="ctaSubtitle" runat="server" class="lead"></p>
             <asp:HyperLink ID="ctaButton" runat="server" NavigateUrl="~/Contactenos.aspx" CssClass="btn btn-primary btn-lg"></asp:HyperLink>
        </section>
    </div>

    <%-- --- NUEVO CONTROL DE ENCUESTA FLOTANTE --- --%>
    <%-- Este es el panel principal que se mostrará u ocultará desde el code-behind --%>
    <asp:Panel ID="pnlEncuestaFlotante" runat="server" Visible="false" CssClass="encuesta-flotante">
        <div class="encuesta-header">
            <asp:Literal ID="litEncuestaTitulo" runat="server"></asp:Literal>
        </div>
        <div class="encuesta-body">
            <%-- El UpdatePanel permite que el contenido cambie sin recargar toda la página --%>
            <asp:UpdatePanel ID="upEncuesta" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%-- Vista 1: Panel para mostrar la pregunta y las opciones --%>
                    <asp:Panel ID="pnlPregunta" runat="server" Visible="true">
                        <p class="encuesta-pregunta">
                            <asp:Literal ID="litPreguntaTexto" runat="server"></asp:Literal>
                        </p>
                        <div class="encuesta-opciones">
                            <asp:Repeater ID="rptOpciones" runat="server" OnItemCommand="rptOpciones_ItemCommand">
                                <ItemTemplate>
                                    <asp:Button ID="btnOpcion" runat="server" 
                                        Text='<%# Eval("TextoOpcion") %>' 
                                        CommandName="Votar" 
                                        CommandArgument='<%# Eval("Codigo") %>' 
                                        CssClass="btn btn-outline-secondary btn-opcion" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>

                    <%-- Vista 2: Panel para mostrar los resultados después de votar --%>
                    <asp:Panel ID="pnlResultados" runat="server" Visible="false">
                         <p class="encuesta-pregunta"><%= this.Traducir("Default_Encuesta_Resultados") %></p>
                        <asp:Repeater ID="rptResultados" runat="server">
                            <ItemTemplate>
                                <div class="resultado-barra-container">
                                    <div class="resultado-texto">
                                        <span><%# Eval("Key") %></span>
                                        <strong><%# Eval("Value") %>%</strong>
                                    </div>
                                    <div class="progreso-barra">
                                        <div class="progreso-valor" style='width: <%# Eval("Value") %>%;'>
                                            <%# Convert.ToInt32(Eval("Value")) > 15 ? Eval("Value") + "%" : "" %>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

</asp:Content>

