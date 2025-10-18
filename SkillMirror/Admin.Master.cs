using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class AdminMaster : System.Web.UI.MasterPage, ITraducible
    {
        private Traductor _traductor;
        private BLLRbac _bllRbac; // Instancia de BLLRbac
        private BEUsuario _usuarioActual; // Variable para guardar el usuario logueado

        // Diccionario para mapear páginas a permisos requeridos
        private readonly Dictionary<string, string> _permisosPorPagina = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Consola.aspx", "Acceder_Consola_Admin" },
            { "DashboardGanancias.aspx", "VER_DASHBOARD_GANANCIAS" },
            { "DashboardGanancias", "VER_DASHBOARD_GANANCIAS" },
            { "Consola", "Acceder_Consola_Admin" },
            { "AdminUsuarios.aspx", "Listar_Usuarios" },
            { "AdminUsuarios", "Listar_Usuarios" },
            { "EditarUsuario.aspx", "Editar_Usuario" }, // O "Crear_Usuario" si es nuevo
            { "EditarUsuario", "Editar_Usuario" }, // O "Crear_Usuario" si es nuevo
            { "AdminRoles.aspx", "Listar_Roles" },
            { "AdminRoles", "Listar_Roles" },
            { "AdminNovedades.aspx", "Listar_Novedades" },
            { "AdminNovedades", "Listar_Novedades" },
            { "EditarNovedad.aspx", "Editar_Novedad" }, // O "Crear_Novedad"
            { "EditarNovedad", "Editar_Novedad" }, // O "Crear_Novedad"
            { "AdminFAQs.aspx", "Listar_FAQs" },
            { "AdminFAQs", "Listar_FAQs" },
            { "EditarFAQ.aspx", "Editar_FAQ" }, // O "Crear_FAQ"
            { "EditarFAQ", "Editar_FAQ" }, // O "Crear_FAQ"
            { "AdminEmpresas.aspx", "Listar_Empresas" },
            { "AdminEmpresas", "Listar_Empresas" },
            { "EditarEmpresa.aspx", "Editar_Empresa" }, // O "Crear_Empresa"
            { "EditarEmpresa", "Editar_Empresa" }, // O "Crear_Empresa"
            { "AdminEncuestas.aspx", "Listar_Encuestas" },
            { "AdminEncuestas", "Listar_Encuestas" },
            { "EditarEncuesta.aspx", "Editar_Encuesta" }, // O "Crear_Encuesta"
            { "EditarEncuesta", "Editar_Encuesta" }, // O "Crear_Encuesta"
            { "ResultadosEncuesta.aspx", "Ver_Resultados_Encuesta" },
            { "ResultadosEncuesta", "Ver_Resultados_Encuesta" },
            { "AdminPublicidad.aspx", "Listar_Publicidades" },
            { "AdminPublicidad", "Listar_Publicidades" },
            { "EditarPublicidad.aspx", "Editar_Publicidad" }, // O "Crear_Publicidad"
            { "EditarPublicidad", "Editar_Publicidad" }, // O "Crear_Publicidad"
            { "AdminResenas.aspx", "Listar_Resenas" },
            { "AdminResenas", "Listar_Resenas" },
            { "RankingResenas.aspx", "Ver_Estadisticas_Resenas" },
            { "RankingResenas", "Ver_Estadisticas_Resenas" },
            { "Bitacora.aspx", "Ver_Bitacora" },
            { "Bitacora", "Ver_Bitacora" },
            { "Backup.aspx", "Listar_Backups" }, // Asumimos Listar como permiso base para la página
            { "Backup", "Listar_Backups" }, // Asumimos Listar como permiso base para la página
            { "AdminIdiomas.aspx", "Listar_Idiomas" },
            { "AdminIdiomas", "Listar_Idiomas" },
            { "AdminNotasCredito.aspx", "Listar_Notas_Credito" },
            { "AdminNotasCredito", "Listar_Notas_Credito" },
            { "AdminConversaciones.aspx", "VER_MENSAJES_AGENTE_CUENTA" },
            { "AdminConversaciones", "VER_MENSAJES_AGENTE_CUENTA" },
            { "AdminVerConversacion.aspx", "VER_MENSAJES_AGENTE_CUENTA" }, // Misma página que la anterior
            { "AdminVerConversacion", "VER_MENSAJES_AGENTE_CUENTA" }, // Misma página que la anterior
            { "MiPerfil.aspx", "Editar_Mi_Perfil" }, // Usualmente todos tienen acceso a su perfil
            { "MiPerfil", "Editar_Mi_Perfil" } // Usualmente todos tienen acceso a su perfil
            // Agrega aquí otras páginas si es necesario
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            _traductor = Traductor.ObtenerInstancia();
            _traductor.Suscribir(this);
            _bllRbac = new BLLRbac();


            if ( HttpContext.Current.User.Identity.Name != "" )
            {
                BLLUsuario bllUsuario = new BLLUsuario();
                _usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
            }
            else
            {
                // Si no está autenticado, redirigir al login (ya lo hace tu código original)
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return; // Importante salir para evitar procesar más
            }
            VerificarPermisoPagina();
            if (!IsPostBack)
            {
                CargarDatosUsuario();
                CargarIdiomas();
                CargarMenu();
            }
        }

        private void VerificarPermisoPagina()
        {
            if (_usuarioActual == null) // Si no se pudo obtener el usuario, denegar
            {
                MostrarAccesoDenegado();
                return;
            }

            // Obtener el nombre del archivo de la página actual (ej: "AdminUsuarios.aspx")
            string paginaActual = System.IO.Path.GetFileName(Request.PhysicalPath);

            // Buscar el permiso requerido en el diccionario
            if (_permisosPorPagina.TryGetValue(paginaActual, out string permisoRequerido))
            {
                // Verificar si el usuario tiene el permiso
                if (!_bllRbac.UsuarioTienePermiso(_usuarioActual, permisoRequerido))
                {
                    // Si NO tiene permiso, mostrar mensaje y detener la carga del contenido
                    MostrarAccesoDenegado();
                }
                // Si tiene permiso, la carga continúa normalmente.
            }

            else if (!paginaActual.Equals("Consola.aspx", StringComparison.OrdinalIgnoreCase)) // Ejemplo: permitir siempre Consola.aspx
            {
                 MostrarAccesoDenegado();
            }
        }
        private void MostrarAccesoDenegado()
        {
            // Oculta el contenido principal
            ContentPlaceHolder mainContent = (ContentPlaceHolder)FindControl("MainContent");
            if (mainContent != null)
            {
                mainContent.Visible = false;
            }

            // Muestra un mensaje de error directamente en la Master Page
            // Podrías crear un Literal o Panel específico en Admin.Master para esto
            Response.Write($"<div class='alert alert-danger text-center' style='margin-top: 80px;'><h2>Acceso Denegado</h2><p>No tienes permiso para ver esta página.</p></div>");

            // Detiene la ejecución adicional de la página
            Response.Flush(); // Envía lo que hay en el buffer
            Response.SuppressContent = true; // No envía más contenido después de esto
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Detiene el ciclo de vida de la página
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_traductor != null)
            {
                _traductor.Desuscribir(this);
            }
        }

        public void ActualizarTraducciones()
        {
            this.Page.Title = _traductor.Traducir("AdminMaster_Page_Title") + " - Consola SkillMirror";
            headerTitle.InnerHtml = $"<img src='{ResolveUrl("~/img/skillmirror150px.png")}' alt='SkillMirror Logo' style='height: 35px;' /> {_traductor.Traducir("AdminMaster_Header_Titulo")}";
            litIdioma.Text = _traductor.Traducir("SiteMaster_Menu_Link_Idioma");
            linkMiPerfil.InnerText = _traductor.Traducir("SiteMaster_MenuUsuario_Link_MiPerfil");
            linkMisMensajes.InnerText = _traductor.Traducir("SiteMaster_MenuUsuario_Link_Mensajes");
            btnLogout.Text = _traductor.Traducir("SiteMaster_MenuUsuario_Boton_CerrarSesion");

            // --- TRADUCCIONES PARA LA BÚSQUEDA ---
            txtBusquedaGlobal.Attributes["placeholder"] = _traductor.Traducir("Busqueda_Placeholder");
            btnBusquedaGlobal.Text = _traductor.Traducir("Busqueda_Boton");

            CargarMenu();
        }

        private void CargarIdiomas()
        {
            RepeaterIdiomas.DataSource = _traductor.IdiomasDisponibles;
            RepeaterIdiomas.DataBind();
        }

        private void CargarMenu()
        {
            if (_usuarioActual == null) return; // Salir si no hay usuario
            var menuItems = new List<MenuItem>();

            /*menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Dashboard"), Url = "~/Consola.aspx" });

            if (rolUsuario == "Administrador")
            {
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Bitacora"), Url = "~/Bitacora.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminRoles"), Url = "~/AdminRoles.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEmpresas"), Url = "~/AdminEmpresas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminUsuarios"), Url = "~/AdminUsuarios.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminResenas"), Url = "~/AdminResenas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminIdiomas"), Url = "~/AdminIdiomas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_BackupRestore"), Url = "~/Backup.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminNovedades"), Url = "~/AdminNovedades.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminFAQs"), Url = "~/AdminFAQs.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEncuestas"), Url = "~/AdminEncuestas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminPublicidad"), Url = "~/AdminPublicidad.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_RankingResenas"), Url = "~/RankingResenas.aspx" });
                if (bllRbac.UsuarioTienePermiso(usuarioActual, "VER_MENSAJES_AGENTE_CUENTA"))
                {
                    menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Mensajeria"), Url = "~/AdminConversaciones.aspx" });
                }
            }
            */



            // --- Menú Filtrado por Permisos ---

            // Consola (Dashboard) - Asumimos que todos los que acceden a Admin.Master tienen este permiso base
            bool pepe = _bllRbac.UsuarioTienePermiso(_usuarioActual, "Acceder_Consola_Admin");
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Acceder_Consola_Admin"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Dashboard"), Url = "~/Consola.aspx" });

            // Administración de Usuarios
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Usuarios"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminUsuarios"), Url = "~/AdminUsuarios.aspx" });

            // Administración de Roles y Permisos
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Roles"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminRoles"), Url = "~/AdminRoles.aspx" });

            // Novedades
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Novedades"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminNovedades"), Url = "~/AdminNovedades.aspx" });

            // FAQs
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_FAQs"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminFAQs"), Url = "~/AdminFAQs.aspx" });

            // Empresas
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Empresas"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEmpresas"), Url = "~/AdminEmpresas.aspx" });

            // Encuestas
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Encuestas"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEncuestas"), Url = "~/AdminEncuestas.aspx" });

            // Publicidad
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Publicidades"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminPublicidad"), Url = "~/AdminPublicidad.aspx" });

            // Reseñas
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Resenas"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminResenas"), Url = "~/AdminResenas.aspx" });

            // Ranking Reseñas
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Ver_Estadisticas_Resenas"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_RankingResenas"), Url = "~/RankingResenas.aspx" });

            // Notas de Crédito
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Notas_Credito"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_NotasCredito"), Url = "~/AdminNotasCredito.aspx" });

            // Mensajería (Conversaciones Agente)
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "VER_MENSAJES_AGENTE_CUENTA"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Mensajeria"), Url = "~/AdminConversaciones.aspx" });

            // Idiomas
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Idiomas"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminIdiomas"), Url = "~/AdminIdiomas.aspx" });

            // Bitácora
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Ver_Bitacora"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Bitacora"), Url = "~/Bitacora.aspx" });

            // Backup y Restore
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "Listar_Backups"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_BackupRestore"), Url = "~/Backup.aspx" });

            // Ver Dashboard de facturacion
            if (_bllRbac.UsuarioTienePermiso(_usuarioActual, "VER_DASHBOARD_GANANCIAS"))
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Ganancias"), Url = "~/DashboardGanancias.aspx" });

            // --- Fin del Menú Filtrado ---
            //string currentPage = VirtualPathUtility.ToAppRelative(Request.Path);
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

        // --- EVENTO PARA EL BOTÓN DE BÚSQUEDA ---
        protected void btnBusquedaGlobal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBusquedaGlobal.Text))
            {
                // Codificamos el término de búsqueda para pasarlo de forma segura en la URL
                Response.Redirect($"~/Busqueda.aspx?q={Server.UrlEncode(txtBusquedaGlobal.Text)}");
            }
        }
    }

    public class MenuItem
    {
        public string Texto { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
    }
}

