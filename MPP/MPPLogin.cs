using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;


namespace MPP
{
    public class MPPLogin
    {
        Acceso bd;

        public int doLogin(BEUsuario usuario, string pw)
        {
            bd = new Acceso();
            MPPCrypto h = new MPPCrypto();

            byte[] sal = h.TraerSalt(usuario.Email);

            // Si la sal es null, asumo que es porque el email no existe en la base. 
            if (sal == null) { return 1; }

            //Hash de la pw
            string hashParaLaDB = h.HashPassword(pw, sal);

            string Query = "doLogin";
            Hashtable ht = new Hashtable();
            ht.Add("@username", usuario.Email);
            ht.Add("@hashedPassword", hashParaLaDB);
            int i = bd.LeerSPRT(Query, ht);
            return i;
        }

        public int changePass(BEUsuario usuario, string pw)
        {
            bd = new Acceso();
            string Query = "ChangePW";
            Hashtable ht = new Hashtable();
            ht.Add("@Codigo", usuario.Codigo);
            ht.Add("@hashedPassword", pw);
            int i = bd.LeerSPRT(Query, ht);
            return i;
        }

    }
}
