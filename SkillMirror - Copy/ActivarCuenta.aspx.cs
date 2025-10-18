using System;
using System.Web.UI;
using BLL;
using BE;

namespace SkillMirror
{
    // PASO 1: Implementamos la interfaz ITraducible
    public partial class ActivarCuenta : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // PASO 2: La página se suscribe al traductor
            Traductor.ObtenerInstancia().Suscribir(this);

            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                string ID = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(ID))
                {
                    try
                    {
                        BEUsuario blu = new BEUsuario();
                        BLLUsuario bLLUsuario = new BLLUsuario();
                        blu.Codigo = Convert.ToInt32(ID);
                        blu = bLLUsuario.ListarObjeto(blu);

                        if (blu != null && blu.TokenActivacion == token)
                        {
                            bool res = bLLUsuario.ActivarUsuarioConToken(blu);
                            if (res)
                            {
                                pnlExito.Visible = true;
                                pnlError.Visible = false;
                            }
                            else
                            {
                                pnlExito.Visible = false;
                                pnlError.Visible = true;
                            }
                        }
                        else
                        {
                            pnlExito.Visible = false;
                            pnlError.Visible = true;
                        }
                    }
                    catch
                    {
                        pnlExito.Visible = false;
                        pnlError.Visible = true;
                    }
                }
                else
                {
                    pnlExito.Visible = false;
                    pnlError.Visible = true;
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // (Buena práctica) Quitamos la suscripción
            Traductor.ObtenerInstancia().Desuscribir(this);
        }

        // PASO 3: Implementamos el método de la interfaz
        public override void ActualizarTraducciones()
        {
            var traductor = Traductor.ObtenerInstancia();

            this.Title = traductor.Traducir("ActivarCuenta_Pagina_Titulo");

            // Panel de Éxito
            litTituloExito.Text = traductor.Traducir("ActivarCuenta_Exito_Titulo");
            litSubtituloExito.Text = traductor.Traducir("ActivarCuenta_Exito_Texto1");
            litTextoExito.Text = traductor.Traducir("ActivarCuenta_Exito_Texto2");
            btnIrALogin.Text = traductor.Traducir("ActivarCuenta_Exito_Boton_IrLogin");

            // Panel de Error
            litTituloError.Text = traductor.Traducir("ActivarCuenta_Error_Titulo");
            litSubtituloError.Text = traductor.Traducir("ActivarCuenta_Error_Texto1");
            litTextoError.Text = traductor.Traducir("ActivarCuenta_Error_Texto2");
            btnVolverARegistrarse.Text = traductor.Traducir("ActivarCuenta_Error_Boton_VolverRegistro");
        }
    }
}