using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BEBackup
    {
        public int Codigo { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public BEUsuario Usuario { get; set; }
        public string UsuarioEmail { get; set; } 
    }
}
