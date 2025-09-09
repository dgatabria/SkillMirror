using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using MPP;
using System.Collections;
using System.Data;

namespace BLL
{
    public class BLLogin
    {
        MPPLogin mppl;
        
        public int doLogin(BEUsuario usuario, string pw)
        {
            mppl = new MPPLogin();
            return mppl.doLogin(usuario, pw);
            
        }
    }

}

