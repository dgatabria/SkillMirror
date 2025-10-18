using System;
using System.Collections.Generic;
using BE;
using BLL;

namespace SkillMirror.Controles
{
    public partial class BannerPublicidad : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPublicidades();
            }
        }

        private void CargarPublicidades()
        {
            BLLPublicidad bll = new BLLPublicidad();
            List<BEPublicidad> activas = bll.ListarActivas();

            if (activas != null && activas.Count > 0)
            {
                pnlCarrusel.Visible = true;
                rptIndicadores.DataSource = activas;
                rptIndicadores.DataBind();
                rptBanners.DataSource = activas;
                rptBanners.DataBind();
            }
        }
    }
}