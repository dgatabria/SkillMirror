using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEUsuario 
    {
        public BEUsuario()
        {
            Eliminado = false;
            Bloqueado = false;
            PrimerLogin = true;

        }
        public int Codigo { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public BEIdioma Idioma { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public List<RBAC> Roles { get; set; }
        public string HashedPassword {  get; set; }
        public bool Bloqueado { get; set; }
        public BEEmpresa Empresa { get; set; } 
        public BEIdioma IdiomaSeleccionado { get; set; }
        public bool PrimerLogin { get; set; }
        public string TokenRecupero { get; set; }
        public string TokenActivacion {  get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Eliminado { get; set; }


        public override string ToString()
        {
            return username + " (" + Nombre + " " + Apellido + ")";
        }

    }
}
