using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Usa HttpContext.Current.User para acceder a la identidad del usuario
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // El usuario ESTÁ logueado
                pnlAnonymous.Visible = false;
                pnlAuthenticated.Visible = true;
                lblUserName.Text = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                // El usuario NO ESTÁ logueado
                pnlAnonymous.Visible = true;
                pnlAuthenticated.Visible = false;
            }
        }
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            // Destruye la cookie de autenticación
            System.Web.Security.FormsAuthentication.SignOut();

            // Redirige al login
            Response.Redirect("~/Login.aspx");
        }
    }
}