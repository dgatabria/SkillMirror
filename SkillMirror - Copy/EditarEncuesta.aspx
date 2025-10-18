<%@ Page Title="Gestionar Encuesta" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EditarEncuesta.aspx.cs" Inherits="SkillMirror.EditarEncuesta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 class="h2"><asp:Literal ID="litTitulo" runat="server"></asp:Literal></h1>
    </div>
    <div class="card">
        <div class="card-body">
            <asp:HiddenField ID="hfEncuestaId" runat="server" Value="0" />
            <asp:HiddenField ID="hfPreguntasData" runat="server" /> <%-- Campo oculto para el JSON --%>
            
            <div class="mb-3">
                <asp:Label ID="lblTitulo" runat="server" AssociatedControlID="txtTitulo" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblDescripcion" runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblFechaVencimiento" runat="server" AssociatedControlID="txtFechaVencimiento" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>

            <hr />
            <h5 id="preguntasTitle" runat="server">Preguntas de la Encuesta</h5>
            <div id="preguntas-container"></div>
            <button type="button" id="btnAgregarPregunta" class="btn btn-sm btn-outline-secondary mt-2">
                <asp:Literal ID="litBotonAgregarPregunta" runat="server"></asp:Literal>
            </button>
        </div>
        <div class="card-footer text-end">
            <asp:Button ID="btnGuardar" runat="server" OnClick="BtnGuardar_Click" OnClientClick="return prepararGuardado();" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" OnClick="BtnCancelar_Click" CssClass="btn btn-secondary" CausesValidation="false" />
        </div>
    </div>
    
    <script type="text/javascript">
        // Inyectamos las traducciones y los datos existentes
        const traducciones = <%= _jsonTraducciones %>;
        let preguntasExistentes = <%= _jsonPreguntas %>;
        let preguntaCounter = 0;

        function prepararGuardado() {
            const container = document.getElementById('preguntas-container');
            const preguntas = [];
            container.querySelectorAll('.pregunta-card').forEach((card, index) => {
                const pregunta = {
                    TextoPregunta: card.querySelector('.pregunta-texto').value,
                    TipoPregunta: card.querySelector('.pregunta-tipo').value,
                    Opciones: []
                };
                if (pregunta.TipoPregunta === 'OPCION_MULTIPLE') {
                    card.querySelectorAll('.opcion-texto').forEach(opcionInput => {
                        pregunta.Opciones.push({ TextoOpcion: opcionInput.value });
                    });
                }
                preguntas.push(pregunta);
            });
            document.getElementById('<%= hfPreguntasData.ClientID %>').value = JSON.stringify(preguntas);
            return true;
        }

        function crearEditorPregunta(pregunta = null) {
            preguntaCounter++;
            const preguntaId = `pregunta_${preguntaCounter}`;
            const tipo = pregunta ? pregunta.TipoPregunta : 'TEXTO_LIBRE';
            const texto = pregunta ? pregunta.TextoPregunta : '';

            const card = document.createElement('div');
            card.className = 'card mb-3 pregunta-card';
            card.id = preguntaId;
            card.innerHTML = `
                <div class="card-body">
                    <div class="d-flex justify-content-between">
                        <label class="form-label">${traducciones.pregunta} #${preguntaCounter}</label>
                        <button type="button" class="btn-close" onclick="this.closest('.pregunta-card').remove()"></button>
                    </div>
                    <textarea class="form-control mb-2 pregunta-texto" rows="2" placeholder="${traducciones.placeholderTextoPregunta}">${texto}</textarea>
                    <select class="form-select form-select-sm pregunta-tipo" onchange="tipoPreguntaCambiado(this)">
                        <option value="TEXTO_LIBRE" ${tipo === 'TEXTO_LIBRE' ? 'selected' : ''}>${traducciones.tipoTextoLibre}</option>
                        <option value="PUNTAJE_1_5" ${tipo === 'PUNTAJE_1_5' ? 'selected' : ''}>${traducciones.tipoPuntaje}</option>
                        <option value="OPCION_MULTIPLE" ${tipo === 'OPCION_MULTIPLE' ? 'selected' : ''}>${traducciones.tipoMultipleChoice}</option>
                    </select>
                    <div class="opciones-container mt-2"></div>
                </div>`;
            document.getElementById('preguntas-container').appendChild(card);
            
            // Si es múltiple choice, creamos el contenedor de opciones
            if (tipo === 'OPCION_MULTIPLE') {
                const opcionesContainer = card.querySelector('.opciones-container');
                opcionesContainer.innerHTML = `<button type="button" class="btn btn-sm btn-outline-success" onclick="agregarOpcion(this)">${traducciones.agregarOpcion}</button>`;
                if (pregunta && pregunta.Opciones) {
                    pregunta.Opciones.forEach(op => agregarOpcion(opcionesContainer.querySelector('button'), op.TextoOpcion));
                }
            }
        }

        function tipoPreguntaCambiado(select) {
            const opcionesContainer = select.closest('.card-body').querySelector('.opciones-container');
            if (select.value === 'OPCION_MULTIPLE') {
                opcionesContainer.innerHTML = `<button type="button" class="btn btn-sm btn-outline-success" onclick="agregarOpcion(this)">${traducciones.agregarOpcion}</button>`;
            } else {
                opcionesContainer.innerHTML = '';
            }
        }

        function agregarOpcion(btn, textoOpcion = '') {
            const div = document.createElement('div');
            div.className = 'input-group input-group-sm my-1';
            div.innerHTML = `
                <input type="text" class="form-control opcion-texto" placeholder="${traducciones.placeholderOpcion}" value="${textoOpcion}">
                <button class="btn btn-outline-danger" type="button" onclick="this.closest('.input-group').remove()">X</button>`;
            btn.parentElement.appendChild(div);
        }

        document.getElementById('btnAgregarPregunta').addEventListener('click', () => crearEditorPregunta());
        
        // Cargar las preguntas existentes al iniciar la página
        preguntasExistentes.forEach(p => crearEditorPregunta(p));
    </script>
</asp:Content>