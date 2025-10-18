<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SkillMirror._Default" %>
<%@ Register Src="~/Controles/BannerPublicidad.ascx" TagPrefix="uc" TagName="Banner" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Estilos específicos para esta página (sin cambios) --%>
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
            color: var(--primary-color);
            margin-bottom: 20px;
        }
    </style>

    <%-- SECCIÓN 1: HERO (PORTADA PRINCIPAL) --%>
    <div class="hero-section">
        <div class="container">
            <h1 id="heroTitle" runat="server" class="display-4">El Reflejo del Verdadero Talento</h1>
            <p id="heroSubtitle" runat="server" class="lead">
                Optimiza tu proceso de selección con nuestra plataforma de evaluación basada en IA y juegos cognitivos.
            </p>
            <asp:HyperLink ID="heroButton" runat="server" NavigateUrl="~/Contactenos.aspx" CssClass="btn btn-light btn-cta">Solicitar una Demo</asp:HyperLink>
        </div>
    </div>

    <div class="container mt-5 pt-5">
        <%-- SECCIÓN 2: ¿QUÉ NOS HACE ÚNICOS? --%>
        <section class="text-center mb-5 pb-5">
            <h2 id="uniqueTitle" runat="server" class="section-title">¿Qué hace único a SkillMirror?</h2>
            <div class="row">
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🎮</div>
                        <h3 id="feature1Title" runat="server">Evaluación Gamificada</h3>
                        <p id="feature1Text" runat="server">Metodología basada en juegos diseñados para captar datos conductuales y medir soft skills con mayor precisión.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🧠</div>
                        <h3 id="feature2Title" runat="server">Análisis con IA</h3>
                        <p id="feature2Text" runat="server">Un motor de análisis potenciado por inteligencia artificial que genera perfiles a partir del comportamiento real.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">🔄</div>
                        <h3 id="feature3Title" runat="server">Integración Simple</h3>
                        <p id="feature3Text" runat="server">Capacidad de integración con los procesos de RRHH existentes sin necesidad de reemplazar herramientas ya utilizadas.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card">
                        <div class="icon">😊</div>
                        <h3 id="feature4Title" runat="server">Experiencia Positiva</h3>
                        <p id="feature4Text" runat="server">Una experiencia atractiva para el candidato que mejora el engagement y la imagen de tu marca durante la selección.</p>
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
                    <h2 id="valueTitle" runat="server">Decisiones basadas en datos, no en percepciones</h2>
                    <p id="valueText" runat="server">
                        Nuestra solución brinda resultados objetivos, dinámicos y difíciles de falsear, lo que mejora la calidad del proceso de selección. El valor principal es la optimización del tiempo y los recursos invertidos en la preselección, reduciendo la rotación y permitiendo tomar decisiones basadas en datos reales y observables.
                    </p>
                    <asp:HyperLink ID="valueButton" runat="server" NavigateUrl="~/QuienesSomos.aspx" CssClass="btn btn-outline-primary">Conocer más</asp:HyperLink>
                </div>
            </div>
        </section>
        <uc:Banner runat="server" />
        <%-- SECCIÓN 4: CALL TO ACTION FINAL --%>
        <section class="text-center bg-light py-5 rounded">
            <h2 id="ctaTitle" runat="server">Transforma tu Proceso de Selección Hoy</h2>
             <p id="ctaSubtitle" runat="server" class="lead">
                 Descubre cómo SkillMirror puede ayudarte a encontrar el talento que tu empresa necesita para crecer.
             </p>
             <asp:HyperLink ID="ctaButton" runat="server" NavigateUrl="~/Contactenos.aspx" CssClass="btn btn-primary btn-lg">Contactar a Ventas</asp:HyperLink>
        </section>
    </div>
</asp:Content>