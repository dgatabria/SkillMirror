using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class RBAC
    {
        string _nombre;
        int _id;

        public RBAC(int id, string nombre)
        {
            _nombre = nombre;
            _id = id;
        }
        public RBAC()
        {
        }
        public string Nombre { get { return _nombre; } }
        public int Codigo { get { return _id; } }
        public abstract void AgregarHijo(RBAC c);

        //Lista todos los nodos hijos, sean roles o permisos
        public abstract IList<RBAC> ObtenerHijos();

        //Recorre todos los hijos y devuelve array de permisos
        public abstract IList<RBAC> Permisos { get; }
    }

 
    public class BERol : RBAC
    {
        private List<RBAC> _hijos;
        private string _nombre;
        private int _id;

        public BERol()
        {

        }
        public BERol(int id, string nombre) : base(id, nombre)
        {
            this._hijos = new List<RBAC>();
            this._id = id;
            this._nombre = nombre;
        }
        public override void AgregarHijo(RBAC c)
        {
            _hijos.Add(c);
        }

        public override IList<RBAC> ObtenerHijos()
        {
            return _hijos.ToArray();
        }

        public override string ToString()
        {
            return this._nombre;

        }

        public void EliminarHijos()
        {
            _hijos = null;
            this._hijos = new List<RBAC>();
        }
        public override IList<RBAC> Permisos
        {
            get
            {
                IList<RBAC> list = new List<RBAC>();

                foreach (var item in _hijos)
                {
                    if (item is BERol)
                    {
                        foreach (var item2 in item.Permisos)
                        {
                            if (item2 is BEPermiso)
                            {
                                list.Add(item2);
                            }
                        }
                    }
                    else
                    {
                        list.Add(item);
                    }

                }

                return list;
            }
        }
    }
    public class BEPermiso : RBAC
    {
        private string _nombre;
        private int _id;
        public override IList<RBAC> Permisos
        {
            get
            {
                IList<RBAC> lista = new List<RBAC>();
                lista.Add(this);
                return lista;
            }

        }

        public override void AgregarHijo(RBAC c)
        {
            throw new Exception("Un PERMISO no puede tener hijos");
        }

        public override IList<RBAC> ObtenerHijos()
        {
            throw new Exception("Un PERMISO no puede tener hijos");
        }
        public override string ToString()
        {
            return _nombre;
        }
        public BEPermiso(int id, string nombre) : base(id, nombre)
        {
            _nombre = nombre;
            _id = id;
        }
    }
}
