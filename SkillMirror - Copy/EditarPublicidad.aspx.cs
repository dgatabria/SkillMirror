using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class EditarPublicidad : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int pubId = Convert.ToInt32(Request.QueryString["id"]);
                    CargarPublicidad(pubId);
                }
            }
        }

        private void CargarPublicidad(int id)
        {
            BLLPublicidad bll = new BLLPublicidad();
            BEPublicidad pub = bll.ObtenerPorId(new BEPublicidad { Codigo = id });
            if (pub != null)
            {
                hfPublicidadId.Value = pub.Codigo.ToString();
                txtTitulo.Text = pub.Titulo;
                txtURL.Text = pub.URLDestino;
                txtFechaInicio.Text = pub.FechaInicio.ToString("yyyy-MM-dd");
                txtFechaExpiracion.Text = pub.FechaExpiracion.ToString("yyyy-MM-dd");
                chkActivo.Checked = pub.Activo;
                hfRutaImagenActual.Value = pub.RutaImagen;

                if (!string.IsNullOrEmpty(pub.RutaImagen))
                {
                    imgPreview.Visible = true;
                    imgPreview.ImageUrl = pub.RutaImagen;
                }
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string rutaImagen = hfRutaImagenActual.Value; // Por defecto, mantenemos la imagen actual si no se sube una nueva.

                // Lógica para procesar la nueva imagen si se subió una.
                if (fileImagen.HasFile)
                {
                    try
                    {
                        // 1. Definimos el tamaño estándar para todos los banners.
                        int bannerWidth = 1200;
                        int bannerHeight = 300;

                        string carpeta = "~/img/banners/";
                        string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileImagen.FileName);
                        string rutaGuardado = Path.Combine(Server.MapPath(carpeta), nombreArchivo);

                        // 2. Cargamos la imagen original en memoria.
                        using (var imagenOriginal = System.Drawing.Image.FromStream(fileImagen.FileContent))
                        {
                            // 3. Creamos un nuevo lienzo (Bitmap) con el tamaño deseado.
                            using (var nuevoBitmap = new Bitmap(bannerWidth, bannerHeight))
                            {
                                // 4. Dibujamos la imagen original sobre el nuevo lienzo, redimensionándola.
                                using (var graphics = Graphics.FromImage(nuevoBitmap))
                                {
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage(imagenOriginal, 0, 0, bannerWidth, bannerHeight);
                                }

                                // 5. Guardamos la NUEVA imagen redimensionada en el disco.
                                nuevoBitmap.Save(rutaGuardado);
                            }
                        }

                        // 6. Actualizamos la ruta de la imagen a guardar en la base de datos.
                        rutaImagen = carpeta + nombreArchivo;
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }

                BEPublicidad pub = new BEPublicidad
                {
                    Codigo = Convert.ToInt32(hfPublicidadId.Value),
                    Titulo = txtTitulo.Text,
                    URLDestino = txtURL.Text,
                    RutaImagen = rutaImagen,
                    FechaInicio = Convert.ToDateTime(txtFechaInicio.Text),
                    FechaExpiracion = Convert.ToDateTime(txtFechaExpiracion.Text),
                    Activo = chkActivo.Checked
                };

                BLLPublicidad bll = new BLLPublicidad();
                bll.Guardar(pub);

                Response.Redirect("AdminPublicidad.aspx");
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPublicidad.aspx");
        }

        public override void ActualizarTraducciones()
        {
            // Determinar si estamos en modo Edición o Creación
            bool esEdicion = !string.IsNullOrEmpty(Request.QueryString["id"]);

            // Título de la página y encabezado principal
            this.Title = _traductor.Traducir(esEdicion ? "EditarPublicidad_Page_Title_Editar" : "EditarPublicidad_Page_Title_Crear");
            headerTitle.InnerText = _traductor.Traducir(esEdicion ? "EditarPublicidad_Header_Titulo_Editar" : "EditarPublicidad_Header_Titulo_Crear");

            // Labels de los campos del formulario
            lblTitulo.Text = _traductor.Traducir("EditarPublicidad_Label_Titulo");
            lblURL.Text = _traductor.Traducir("EditarPublicidad_Label_URL");
            lblImagen.Text = _traductor.Traducir("EditarPublicidad_Label_Imagen");
            lblFechaInicio.Text = _traductor.Traducir("EditarPublicidad_Label_FechaInicio");
            lblFechaExpiracion.Text = _traductor.Traducir("EditarPublicidad_Label_FechaExpiracion");
            lblActivo.Text = _traductor.Traducir("EditarPublicidad_Label_Activo");

            // Mensajes de validación
            rfvTitulo.ErrorMessage = _traductor.Traducir("EditarPublicidad_Validator_Titulo");
            rfvURL.ErrorMessage = _traductor.Traducir("EditarPublicidad_Validator_URL");

            // Botones de acción
            btnCancelar.Text = _traductor.Traducir("Admin_Boton_Cancelar");
            btnGuardar.Text = _traductor.Traducir("Admin_Boton_Guardar");
        }
    }
}
