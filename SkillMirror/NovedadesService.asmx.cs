using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace SkillMirror
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class NovedadesService : WebService
    {
        [WebMethod]
        public List<BENovedad> ObtenerNovedadesPublicadas()
        {
            BLLNovedad bll = new BLLNovedad();
            return bll.ListarPublicadas();
        }
    }
}
