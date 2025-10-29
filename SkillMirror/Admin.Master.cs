using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; // Necesario para el control Label

namespace SkillMirror
{
    public partial class AdminMaster : System.Web.UI.MasterPage, ITraducible
    {
        private Traductor _traductor;
        private BLLRbac _bllRbac;
        private BEUsuario _usuarioActual;

        // Diccionario para mapear páginas a permisos requeridos
        private readonly Dictionary<string, string> _permisosPorPagina = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Mapeo de páginas a permisos (igual que en tu versión)
            { "Consola.aspx", "Acceder_Consola_Admin" },
            { "Contratar.aspx", "Contratar_Plan" },
            { "Contratar", "Contratar_Plan" },
            { "DashboardGanancias.aspx", "VER_DASHBOARD_GANANCIAS" },
            { "DashboardGanancias", "VER_DASHBOARD_GANANCIAS" },
            { "Consola", "Acceder_Consola_Admin" },
            { "AdminUsuarios.aspx", "Listar_Usuarios" },
            { "AdminUsuarios", "Listar_Usuarios" },
            { "EditarUsuario.aspx", "Editar_Usuario" },
            { "EditarUsuario", "Editar_Usuario" },
            { "AdminRoles.aspx", "Listar_Roles" },
            { "AdminRoles", "Listar_Roles" },
            { "AdminNovedades.aspx", "Listar_Novedades" },
            { "AdminNovedades", "Listar_Novedades" },
            { "EditarNovedad.aspx", "Editar_Novedad" },
            { "EditarNovedad", "Editar_Novedad" },
            { "AdminFAQs.aspx", "Listar_FAQs" },
            { "AdminFAQs", "Listar_FAQs" },
            { "EditarFAQ.aspx", "Editar_FAQ" },
            { "EditarFAQ", "Editar_FAQ" },
            { "AdminEmpresas.aspx", "Listar_Empresas" },
            { "AdminEmpresas", "Listar_Empresas" },
            { "EditarEmpresa.aspx", "Editar_Empresa" },
            { "EditarEmpresa", "Editar_Empresa" },
            { "AdminEncuestas.aspx", "Listar_Encuestas" },
            { "AdminEncuestas", "Listar_Encuestas" },
            { "EditarEncuesta.aspx", "Editar_Encuesta" },
            { "EditarEncuesta", "Editar_Encuesta" },
            { "ResultadosEncuesta.aspx", "Ver_Resultados_Encuesta" },
            { "ResultadosEncuesta", "Ver_Resultados_Encuesta" },
            { "AdminPublicidad.aspx", "Listar_Publicidades" },
            { "AdminPublicidad", "Listar_Publicidades" },
            { "EditarPublicidad.aspx", "Editar_Publicidad" },
            { "EditarPublicidad", "Editar_Publicidad" },
            { "AdminResenas.aspx", "Listar_Resenas" },
            { "AdminResenas", "Listar_Resenas" },
            { "RankingResenas.aspx", "Ver_Estadisticas_Resenas" },
            { "RankingResenas", "Ver_Estadisticas_Resenas" },
            { "Bitacora.aspx", "Ver_Bitacora" },
            { "Bitacora", "Ver_Bitacora" },
            { "Backup.aspx", "Listar_Backups" },
            { "Backup", "Listar_Backups" },
            { "AdminIdiomas.aspx", "Listar_Idiomas" },
            { "AdminIdiomas", "Listar_Idiomas" },
            { "AdminNotasCredito.aspx", "Listar_Notas_Credito" },
            { "AdminNotasCredito", "Listar_Notas_Credito" },
            { "AdminConversaciones.aspx", "VER_MENSAJES_AGENTE_CUENTA" },
            { "AdminConversaciones", "VER_MENSAJES_AGENTE_CUENTA" },
            { "AdminVerConversacion.aspx", "VER_MENSAJES_AGENTE_CUENTA" },
            { "AdminVerConversacion", "VER_MENSAJES_AGENTE_CUENTA" },
            { "MiPerfil.aspx", "Editar_Mi_Perfil" },
            { "MiPerfil", "Editar_Mi_Perfil" },
            { "AdminPlanes.aspx", "Administrar_Planes" },
            { "AdminPlanes", "Administrar_Planes" },
            { "EditarPlan.aspx", "Administrar_Planes" },
            { "EditarPlan", "Administrar_Planes" },
            { "Busqueda.aspx", "Realizar_Busqueda" },
            { "Busqueda", "Realizar_Busqueda" },
            { "MisMensajes.aspx", "Ver_Mis_Mensajes" },
            { "MisMensajes", "Ver_Mis_Mensajes" },
            { "NuevoMensaje.aspx", "Ver_Mis_Mensajes" },
            { "NuevoMensaje", "Ver_Mis_Mensajes" },
            { "MiVerConversacion.aspx", "Ver_Mis_Mensajes" },
            { "MiVerConversacion", "Ver_Mis_Mensajes" },
            { "ContratacionExitosa.aspx", "Contratar_Plan" },
            { "ContratacionExitosa", "Contratar_Plan" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            _traductor = Traductor.ObtenerInstancia();
            _traductor.Suscribir(this);
            _bllRbac = new BLLRbac();

            // Obtener usuario actual
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                BLLUsuario bllUsuario = new BLLUsuario();
                _usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
            }
            else
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return;
            }

            // Verificar permiso para la página actual
            VerificarPermisoPagina();

            if (!IsPostBack)
            {
                CargarDatosUsuario();
                CargarIdiomas();
                CargarMenu();
                // Llamamos explícitamente para asegurar la traducción inicial
                ActualizarTraducciones();
            }
        }

        private void VerificarPermisoPagina()
        {
            if (_usuarioActual == null)
            {
                MostrarAccesoDenegado();
                return;
            }

            string paginaActual = System.IO.Path.GetFileName(Request.PhysicalPath);

            // Permitir siempre Consola.aspx y páginas que no requieran permiso específico
            if (paginaActual.Equals("Consola.aspx", StringComparison.OrdinalIgnoreCase)) return;

            if (_permisosPorPagina.TryGetValue(paginaActual, out string permisoRequerido))
            {
                if (!_bllRbac.UsuarioTienePermiso(_usuarioActual, permisoRequerido))
                {
                    MostrarAccesoDenegado();
                }
            }
            else // Denegar por defecto si no está en el diccionario y no es Consola
            {
                MostrarAccesoDenegado();
            }
        }

        private void MostrarAccesoDenegado()
        {
            ContentPlaceHolder mainContent = (ContentPlaceHolder)FindControl("MainContent");
            if (mainContent != null) mainContent.Visible = false;

            Response.Write($"<div class='alert alert-danger text-center' style='margin-top: 80px;'><h2>Acceso Denegado</h2><p>No tienes permiso para ver esta página.</p></div>");
            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_traductor != null) _traductor.Desuscribir(this);
        }

        public void ActualizarTraducciones()
        {
            // Títulos y Navbar
            this.Page.Title = _traductor.Traducir("AdminMaster_Page_Title") + " - Consola SkillMirror";
            headerTitle.InnerHtml = $"<img src='{ResolveUrl("~/img/skillmirror150px.png")}' alt='SkillMirror Logo' style='height: 35px;' /> {_traductor.Traducir("AdminMaster_Header_Titulo")}";
            litIdioma.Text = _traductor.Traducir("SiteMaster_Menu_Link_Idioma");
            linkMiPerfil.InnerText = _traductor.Traducir("SiteMaster_MenuUsuario_Link_MiPerfil");
            linkMisMensajes.InnerText = _traductor.Traducir("SiteMaster_MenuUsuario_Link_Mensajes");
            btnLogout.Text = _traductor.Traducir("SiteMaster_MenuUsuario_Boton_CerrarSesion");
            txtBusquedaGlobal.Attributes["placeholder"] = _traductor.Traducir("Busqueda_Placeholder");
            btnBusquedaGlobal.Text = _traductor.Traducir("Busqueda_Boton");

            // --- TRADUCCIÓN PARA LA ETIQUETA DEL TOGGLE DE ACCESIBILIDAD ---
            var lblFocus = FindControl("lblEnhancedFocus") as Label;
            if (lblFocus != null)
            {
                lblFocus.Text = _traductor.Traducir("AdminMaster_Accesibilidad_Foco_Label");
            }
            // ----------------------------------------------------------------

            CargarMenu(); // Recargar el menú para aplicar traducciones
        }

        private void CargarIdiomas()
        {
            RepeaterIdiomas.DataSource = _traductor.IdiomasDisponibles;
            RepeaterIdiomas.DataBind();
        }

        private void CargarMenu()
        {
            if (_usuarioActual == null) return;
            var menuItems = new List<MenuItem>();

            // --- Menú Filtrado por Permisos ---
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Acceder_Consola_Admin")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Dashboard"), Url = "~/Consola.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Realizar_Busqueda")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Buscar"), Url = "~/Busqueda.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Ver_Mis_Mensajes")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("SiteMaster_MenuUsuario_Link_Mensajes"), Url = "~/MisMensajes.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Administrar_Planes")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminPlanes"), Url = "~/AdminPlanes.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Usuarios")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminUsuarios"), Url = "~/AdminUsuarios.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Roles")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminRoles"), Url = "~/AdminRoles.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Novedades")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminNovedades"), Url = "~/AdminNovedades.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_FAQs")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminFAQs"), Url = "~/AdminFAQs.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Empresas")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEmpresas"), Url = "~/AdminEmpresas.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Encuestas")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEncuestas"), Url = "~/AdminEncuestas.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Publicidades")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminPublicidad"), Url = "~/AdminPublicidad.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Resenas")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminResenas"), Url = "~/AdminResenas.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Ver_Estadisticas_Resenas")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_RankingResenas"), Url = "~/RankingResenas.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Notas_Credito")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_NotasCredito"), Url = "~/AdminNotasCredito.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "VER_MENSAJES_AGENTE_CUENTA")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Mensajeria"), Url = "~/AdminConversaciones.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Idiomas")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminIdiomas"), Url = "~/AdminIdiomas.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Ver_Bitacora")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Bitacora"), Url = "~/Bitacora.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Backups")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_BackupRestore"), Url = "~/Backup.aspx" });
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "VER_DASHBOARD_GANANCIAS")) menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Ganancias"), Url = "~/DashboardGanancias.aspx" });
            // --- Fin Menú Filtrado ---

            string currentPage = Request.AppRelativeCurrentExecutionFilePath;
            foreach (var item in menuItems)
            {
                item.CssClass = item.Url.Equals(currentPage, StringComparison.OrdinalIgnoreCase) ? "nav-link active" : "nav-link";
            }

            rptMenu.DataSource = menuItems;
            rptMenu.DataBind();
        }

        private void CargarDatosUsuario()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                lblUserName.Text = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        protected void btnBusquedaGlobal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBusquedaGlobal.Text))
            {
                Response.Redirect($"~/Busqueda.aspx?q={Server.UrlEncode(txtBusquedaGlobal.Text)}");
            }
        }
    }

    // Clase MenuItem (sin cambios)
    public class MenuItem
    {
        public string Texto { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
    }
}

