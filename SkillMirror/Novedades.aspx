<%@ Page Title="Novedades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Novedades.aspx.cs" Inherits="SkillMirror.Novedades" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 id="headerTitle" runat="server">Novedades</h1>
        <p id="headerSubtitle" runat="server" class="lead">Mantente al día con las últimas noticias y actualizaciones de SkillMirror.</p>
    </div>
    <div class="container">
        <div id="novedades-container" class="row">
            <%-- Aquí se inyectará el HTML de las novedades desde JavaScript --%>
        </div>
    </div>

    <script type="text/javascript">
        const traducciones = <%= _jsonTraducciones %>;

        document.addEventListener('DOMContentLoaded', function () {
            const container = document.getElementById('novedades-container');
            fetch('/NovedadesService.asmx/ObtenerNovedadesPublicadas', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            })
            .then(response => response.json())
            .then(result => {
                const novedades = result.d;
                container.innerHTML = '';
                if (novedades && novedades.length > 0) {
                    novedades.forEach(novedad => {
                        const fecha = new Date(parseInt(novedad.FechaPublicacion.substr(6)));
                        const fechaFormateada = fecha.toLocaleDateString(traducciones.locale, { day: 'numeric', month: 'long', year: 'numeric' });
                        
                        const novedadHTML = `
                            <div class="col-md-10 offset-md-1 mb-4">
                                <div class="card">
                                    <div class="card-body">
                                        <h2 class="card-title">${novedad.Titulo}</h2>
                                        <h6 class="card-subtitle mb-2 text-muted">${novedad.Subtitulo}</h6>
                                        <small class="text-muted">${traducciones.publicadoEl} ${fechaFormateada} ${traducciones.por} ${novedad.Autor.Nombre} ${novedad.Autor.Apellido}</small>
                                        <hr/>
                                        <p class="card-text">${novedad.Cuerpo}</p>
                                    </div>
                                </div>
                            </div>`;
                        container.innerHTML += novedadHTML;
                    });
                } else {
                    container.innerHTML = `<div class="alert alert-info">${traducciones.noHayNovedades}</div>`;
                }
            });
        });
    </script>
</asp:Content>