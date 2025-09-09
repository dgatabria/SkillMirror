using System.Collections.Generic;
using System.Web.Script.Services; // Necesario para JSON
using System.Web.Services;
using BE;
using BLL;

namespace SkillMirror
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService] 
    public class ResenasService : WebService
    {
        [WebMethod] 
        public List<BEResena> ObtenerResenasAprobadas()
        {
            
            BLLResena bll = new BLLResena();
            return bll.ListarAprobadas();
        }
    }
}