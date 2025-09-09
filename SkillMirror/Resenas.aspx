<%@ Page Title="Reseñas de Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Resenas.aspx.cs" Inherits="SkillMirror.Resenas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- CAMBIO: El CSS ahora es más simple --%>
    <style>
        .star-rating { display: flex; justify-content: center; }
        .star-rating .star-label { font-size: 2.5rem; color: #ddd; cursor: pointer; padding: 0 0.2rem; }
        .star-rating .star-label:hover,
        .star-rating .star-label.hovered { color: #ffc107; }
        .star-rating .star-label.selected { color: #ffc107; }
        .star-rating input[type=radio] { display: none; }
    </style>

    <div class="container py-5">
        <%-- (El resto del HTML de la página no cambia) --%>
        <div class="text-center mb-5">
            <h1 id="headerTitle" runat="server" class="display-5">Lo que Nuestros Usuarios Opinan</h1>
            <p id="headerSubtitle" runat="server" class="lead">Reseñas y testimonios reales...</p>
        </div>

        <asp:Panel ID="pnlNuevaResena" runat="server" Visible="false">
            <div class="card review-card shadow-sm">
                <div class="card-header"><h4><asp:Literal ID="litFormTitle" runat="server">Deja tu Reseña</asp:Literal></h4></div>
                <div class="card-body">
                    <asp:Literal ID="litFormularioError" runat="server" EnableViewState="false"></asp:Literal>
                    <div class="mb-3">
                        <asp:Label ID="lblTuPuntuacion" runat="server" CssClass="form-label">Tu Puntuación:</asp:Label>
                        
                        <%-- CAMBIO: La estructura de las estrellas es más simple --%>
                        <div class="star-rating">
                            <asp:RadioButton ID="star1" runat="server" GroupName="rating" value="1" CssClass="star-radio" /><label id="lblStar1" for="<%= star1.ClientID %>" class="star-label">☆</label>
                            <asp:RadioButton ID="star2" runat="server" GroupName="rating" value="2" CssClass="star-radio" /><label id="lblStar2" for="<%= star2.ClientID %>" class="star-label">☆</label>
                            <asp:RadioButton ID="star3" runat="server" GroupName="rating" value="3" CssClass="star-radio" /><label id="lblStar3" for="<%= star3.ClientID %>" class="star-label">☆</label>
                            <asp:RadioButton ID="star4" runat="server" GroupName="rating" value="4" CssClass="star-radio" /><label id="lblStar4" for="<%= star4.ClientID %>" class="star-label">☆</label>
                            <asp:RadioButton ID="star5" runat="server" GroupName="rating" value="5" CssClass="star-radio" /><label id="lblStar5" for="<%= star5.ClientID %>" class="star-label">☆</label>
                        </div>

                        <div id="star-rating-error" class="text-danger text-center mt-2" style="display: none;"></div>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblAsunto" runat="server" AssociatedControlID="txtAsunto" CssClass="form-label">Asunto:</asp:Label>
                        <asp:TextBox ID="txtAsunto" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valAsunto" runat="server" ControlToValidate="txtAsunto" ErrorMessage="El asunto es obligatorio." ForeColor="Red" Display="Dynamic" ValidationGroup="NuevaResena" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblComentario" runat="server" AssociatedControlID="txtComentario" CssClass="form-label">Comentario:</asp:Label>
                        <asp:TextBox ID="txtComentario" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valComentario" runat="server" ControlToValidate="txtComentario" ErrorMessage="El comentario es obligatorio." ForeColor="Red" Display="Dynamic" ValidationGroup="NuevaResena" />
                    </div>
                    <div class="text-end">
                        <asp:Button ID="btnEnviarResena" runat="server" Text="Enviar Reseña" CssClass="btn btn-primary" OnClick="BtnEnviarResena_Click" ValidationGroup="NuevaResena" OnClientClick="return validarPuntuacion();" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlGracias" runat="server" Visible="false">
             <div class="alert alert-success text-center">
                 <h4><asp:Literal ID="litGraciasTitle" runat="server">¡Gracias por tu reseña!</asp:Literal></h4>
                 <p><asp:Literal ID="litGraciasText" runat="server">Tu comentario ha sido enviado y será publicado una vez que sea revisado por nuestro equipo.</asp:Literal></p>
             </div>
        </asp:Panel>

        <%-- CONTENEDOR PARA LAS RESEÑAS QUE SE CARGAN CON JS --%>
        <div id="reviews-container">
            <div class="text-center">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p><asp:Literal ID="litCargando" runat="server">Cargando reseñas...</asp:Literal></p>
            </div>
        </div>
    </div>

<script type="text/javascript">
    // 1. Inyectamos las traducciones necesarias desde el code-behind
    const traducciones = <%= _jsonTraducciones %>;

    // 2. Creamos una "bandera" global para la validación
    let puntuacionFueSeleccionada = false;

    // 3. Función de validación que será llamada por el botón "Enviar Reseña"
    function validarPuntuacion() {
        const errorContainer = document.getElementById('star-rating-error');

        // Revisa el estado de nuestra bandera
        if (puntuacionFueSeleccionada) {
            errorContainer.style.display = 'none';
            return true; // Permite el PostBack si la bandera está levantada
        } else {
            errorContainer.innerText = traducciones.errorPuntuacion;
            errorContainer.style.display = 'block';
            return false; // Detiene el PostBack si la bandera no se levantó
        }
    }

    // 4. Función auxiliar para dibujar las estrellas en las reseñas cargadas
    function renderStars(puntuacion) {
        let stars = '';
        for (let i = 0; i < 5; i++) {
            stars += i < puntuacion ? '★' : '☆';
        }
        return stars;
    }

    // 5. El código principal que se ejecuta cuando la página está lista
    document.addEventListener('DOMContentLoaded', function () {

        // --- LÓGICA PARA EL FORMULARIO DE ESTRELLAS (AHORA CONDICIONAL) ---

        const starRatingContainer = document.querySelector('.star-rating');

        // ¡LA SOLUCIÓN!
        // Solo ejecutamos el código relacionado con las estrellas SI el contenedor existe.
        if (starRatingContainer) {
            puntuacionFueSeleccionada = false;
            const stars = starRatingContainer.querySelectorAll('.star-label');

            function setRating(rating) {
                stars.forEach((star, index) => {
                    // Cambiamos el carácter y el color
                    if (index < rating) {
                        star.textContent = '★'; // Estrella rellena
                        star.classList.add('selected'); // Aseguramos el color amarillo
                    } else {
                        star.textContent = '☆'; // Estrella hueca
                        star.classList.remove('selected'); // Aseguramos el color gris
                    }
                });
            }

            stars.forEach((star, index) => {
                star.addEventListener('click', () => {
                    puntuacionFueSeleccionada = true;
                    document.getElementById('star-rating-error').style.display = 'none';
                    setRating(index + 1);
                });

                star.addEventListener('mouseover', () => {
                    stars.forEach((s, i) => {
                        s.classList.toggle('hovered', i <= index);
                    });
                });
            });

            starRatingContainer.addEventListener('mouseout', () => {
                stars.forEach(s => s.classList.remove('hovered'));
            });
        }

        // --- LÓGICA PARA CARGAR LAS RESEÑAS (ahora siempre se ejecuta) ---
        const container = document.getElementById('reviews-container');

        fetch('/ResenasService.asmx/ObtenerResenasAprobadas', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => {
                if (!response.ok) { throw new Error('Network response was not ok'); }
                return response.json();
            })
            .then(result => {
                const reviews = result.d;
                container.innerHTML = '';

                if (reviews && reviews.length > 0) {
                    reviews.forEach(review => {
                        const fecha = new Date(parseInt(review.Fecha.substr(6)));
                        const fechaFormateada = fecha.toLocaleDateString(traducciones.locale, { year: 'numeric', month: 'long', day: 'numeric' });

                        const reviewHTML = `
                        <div class="card review-card">
                            <div class="card-header d-flex justify-content-between">
                                <div>
                                    <strong>${review.Asunto}</strong>
                                    <small class="text-muted ms-2">${traducciones.por} ${review.Autor.Nombre} ${review.Autor.Apellido}</small>
                                </div>
                                <span class="text-warning">${renderStars(review.Puntuacion)}</span>
                            </div>
                            <div class="card-body">
                                <p class="card-text">${review.Comentario}</p>
                                <small class="text-muted fst-italic">${traducciones.publicadoEl} ${fechaFormateada}</small>
                            </div>
                        </div>`;
                        container.innerHTML += reviewHTML;
                    });
                } else {
                    container.innerHTML = `<div class="alert alert-info text-center">${traducciones.noHayResenas}</div>`;
                }
            })
            .catch(error => {
                console.error('Error al cargar las reseñas:', error);
                container.innerHTML = `<div class="alert alert-danger text-center">${traducciones.errorCarga}</div>`;
            });
    });
</script>
</asp:Content>