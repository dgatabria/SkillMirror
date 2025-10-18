using System;
using System.IO;
using System.Web.UI.WebControls;
using BE;
using BLL;


namespace SkillMirror
{
    public partial class BackupPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            BLLBackup bll = new BLLBackup();
            gvBackups.DataSource = bll.Listar();
            gvBackups.DataBind();
        }

        protected void BtnNuevoBackup_Click(object sender, EventArgs e)
        {
            try
            {
                BLLUsuario bllUsuario = new BLLUsuario();
                BEUsuario usuario = bllUsuario.ListarObjeto(new BEUsuario { Email = User.Identity.Name });

                string backupFolder = @"C:\backups\";
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                BLLBackup bllBackup = new BLLBackup();
                bllBackup.CrearBackup(usuario, backupFolder);

                litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("Backup_Msg_ExitoCrear")}</div>";
                BindGrid();
            }
            catch (Exception ex)
            {
                litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Backup_Msg_ErrorCrear")}: {ex.Message}</div>";
            }
        }

        protected void BtnConfirmarRestore_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar la contraseña
                BLLUsuario bllUsuario = new BLLUsuario();
                var usuario = new BEUsuario { Email = User.Identity.Name };
                bool passwordValida = bllUsuario.VerificarPassword(usuario, txtConfirmPassword.Text);

                if (!passwordValida)
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Backup_Msg_ErrorPassword")}</div>";
                    return;
                }

                // 2. Si la contraseña es válida, proceder con el restore
                int backupId = Convert.ToInt32(hfRestoreBackupId.Value);
                BLLBackup bllBackup = new BLLBackup();
                BEBackup backup = bllBackup.ObtenerPorId(backupId);

                if (backup != null && File.Exists(backup.Path))
                {
                    bllBackup.RestaurarBackup(backup.Path);

                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx?restore=success", false);
                }
                else
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Backup_Msg_ErrorNoEncontrado")}</div>";
                }
            }
            catch (Exception ex)
            {
                litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Backup_Msg_ErrorCritico")}: {ex.Message}</div>";
            }
        }

        protected void GvBackups_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("Backup_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("Backup_Grid_Fecha");
                e.Row.Cells[2].Text = _traductor.Traducir("Backup_Grid_Tamano");
                e.Row.Cells[3].Text = _traductor.Traducir("Backup_Grid_Usuario");
                e.Row.Cells[4].Text = _traductor.Traducir("Backup_Grid_Acciones");
            }
        }

        protected void GvBackups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var backup = (BEBackup)e.Row.DataItem;

                var litTamano = (Literal)e.Row.FindControl("litTamano");
                litTamano.Text = String.Format("{0:0.00} MB", (double)backup.Size / (1024 * 1024));

                var btnDescargar = (LinkButton)e.Row.FindControl("btnDescargar");
                btnDescargar.Text = _traductor.Traducir("Backup_Boton_Descargar");

                var btnRestaurar = (LinkButton)e.Row.FindControl("btnRestaurarFila");
                if (btnRestaurar != null)
                {
                    btnRestaurar.Text = _traductor.Traducir("Backup_Boton_RestaurarFila");
                    string fechaFormateada = backup.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss");
                    btnRestaurar.OnClientClick = $"return mostrarModalRestore('{backup.Codigo}', '{fechaFormateada}');";
                }
            }
        }

        protected void GvBackups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Descargar")
            {
                int backupId = Convert.ToInt32(e.CommandArgument);
                BLLBackup bll = new BLLBackup();
                BEBackup backup = bll.ObtenerPorId(backupId);

                if (backup != null && File.Exists(backup.Path))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(backup.Path));
                    Response.TransmitFile(backup.Path);
                    Response.End();
                }
                else
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Backup_Msg_ErrorNoEncontrado")}</div>";
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Backup_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Backup_Header_Titulo");
            btnNuevoBackup.Text = _traductor.Traducir("Backup_Boton_Crear");

            litModalTitle.Text = _traductor.Traducir("Backup_Modal_Titulo");
            lblConfirmPassword.Text = _traductor.Traducir("Backup_Modal_LabelPassword");
            btnModalCancelar.Text = _traductor.Traducir("Backup_Modal_Cancelar");
            btnConfirmarRestore.Text = _traductor.Traducir("Backup_Modal_Confirmar");

            headerHistorial.InnerText = _traductor.Traducir("Backup_Grid_Historial");
            hfRestoreInfoText.Value = _traductor.Traducir("Backup_Restore_InfoModal");

            BindGrid(); // Vuelve a enlazar la grilla para refrescar textos internos
        }
    }
}