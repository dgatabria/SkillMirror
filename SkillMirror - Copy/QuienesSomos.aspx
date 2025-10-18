<%@ Page Title="Quiénes Somos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuienesSomos.aspx.cs" Inherits="SkillMirror.QuienesSomos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1 id="headerTitle" runat="server">Sobre SkillMirror</h1>
        <p id="headerSubtitle" runat="server" class="lead">Transformando la evaluación de talento con tecnología e innovación.</p>
    </div>

    <div class="row">
        <div class="col-md-8 offset-md-2">
            <h2 id="esenciaTitle" runat="server">Nuestra Esencia</h2>
            <p id="esenciaText1" runat="server">
                <strong>SkillMirror es una empresa tecnológica que ofrece soluciones innovadoras para la evaluación de talento</strong>, orientadas a procesos de selección de personal. Nuestra propuesta se basa en una plataforma digital que combina inteligencia artificial y juegos cognitivos para detectar habilidades blandas, capacidades cognitivas y patrones de comportamiento relevantes para el ámbito laboral.
            </p>
            <p id="esenciaText2" runat="server">
                El objetivo es servir como un primer filtro inteligente, reduciendo el universo de candidatos a aquellos que presentan un perfil cognitivo y conductual alineado, para que Recursos Humanos pueda concentrarse en evaluar los aspectos restantes.
            </p>

            <hr class="my-5">

            <h2 id="misionTitle" runat="server">Nuestra Misión</h2>
            <blockquote class="blockquote">
                <p id="misionText" runat="server">
                    "Nos dedicaremos a transformar el proceso de evaluación de talento mediante una plataforma innovadora basada en inteligencia artificial y juegos cognitivos, que permite a las empresas tomar decisiones de selección más precisas, objetivas y eficientes. Apostamos por una experiencia de evaluación moderna, inclusiva y científicamente validada, que beneficie tanto a organizaciones como a candidatos, impulsando un nuevo estándar en la gestión del talento".
                </p>
            </blockquote>

            <hr class="my-5">

             <h2 id="valorTitle" runat="server">Propuesta de Valor</h2>
             <p id="valorText" runat="server">
                El valor central para el cliente es la <strong>optimización del tiempo y los recursos</strong> invertidos en la preselección, la reducción de la rotación por mal encaje y la posibilidad de tomar decisiones basadas en datos reales y observables. Además, la experiencia de evaluación es más atractiva para el candidato, lo que refuerza la imagen de innovación y modernidad de la empresa contratante.
             </p>
        </div>
    </div>

</asp:Content>