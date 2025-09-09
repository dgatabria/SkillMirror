using System;
using System.Collections.Generic;
using System.Web;
using BE;

namespace BLL
{
    [Serializable]
    public sealed class Traductor
    {
        private const string TraductorSessionKey = "TraductorSession";
        private const string IdiomaIdSessionKey = "SelectedLanguageID";

        [NonSerialized]
        private List<ITraducible> _observadores = new List<ITraducible>();

        public void ForzarIdiomaActual()
        {
            if (HttpContext.Current.Session[IdiomaIdSessionKey] != null)
            {
                int idGuardado = (int)HttpContext.Current.Session[IdiomaIdSessionKey];
                if (IdiomaSeleccionado == null || IdiomaSeleccionado.Codigo != idGuardado)
                {
                    SeleccionarIdiomaInicial(idGuardado);
                }
            }
        }

        public BEIdioma IdiomaSeleccionado { get; private set; }
        public List<BEIdioma> IdiomasDisponibles { get; private set; }

        private Traductor()
        {
            // --- CAMBIO CLAVE 2: CREAMOS LA BLL SOLO CUANDO SE NECESITA ---
            BLLIdioma bll = new BLLIdioma();
            IdiomasDisponibles = bll.ListarTodos();

            if (HttpContext.Current.Session[IdiomaIdSessionKey] != null)
            {
                int idSeleccionado = (int)HttpContext.Current.Session[IdiomaIdSessionKey];
                SeleccionarIdiomaInicial(idSeleccionado);
            }
            else
            {
                if (IdiomasDisponibles.Count > 0)
                {
                    SeleccionarIdiomaInicial(IdiomasDisponibles[0].Codigo);
                }
                else
                {
                    IdiomaSeleccionado = new BEIdioma();
                }
            }
        }

        public static Traductor ObtenerInstancia()
        {
            if (HttpContext.Current.Session[TraductorSessionKey] == null)
            {
                HttpContext.Current.Session[TraductorSessionKey] = new Traductor();
            }
            return (Traductor)HttpContext.Current.Session[TraductorSessionKey];
        }

        public void CambiarIdioma(int codigoIdioma)
        {
            SeleccionarIdiomaInicial(codigoIdioma);
            HttpContext.Current.Session[IdiomaIdSessionKey] = codigoIdioma;
            Notificar();
        }

        private void SeleccionarIdiomaInicial(int codigoIdioma)
        {
            var idiomaEncontrado = IdiomasDisponibles.Find(i => i.Codigo == codigoIdioma);
            if (idiomaEncontrado != null)
            {
                if (idiomaEncontrado.Palabras.Count == 0)
                {
                    // --- CAMBIO CLAVE 3: CREAMOS LA BLL SOLO CUANDO SE NECESITA ---
                    BLLIdioma bll = new BLLIdioma();
                    IdiomaSeleccionado = bll.ListarIdioma(idiomaEncontrado);
                }
                else
                {
                    IdiomaSeleccionado = idiomaEncontrado;
                }
            }
        }

        public void Suscribir(ITraducible observador)
        {
            if (_observadores == null)
            {
                _observadores = new List<ITraducible>();
            }

            if (!_observadores.Contains(observador))
            {
                _observadores.Add(observador);
                observador.ActualizarTraducciones();
            }
        }

        // --- El resto de los métodos no cambian ---
        public void Desuscribir(ITraducible observador)
        {
            if (_observadores != null && _observadores.Contains(observador))
            {
                _observadores.Remove(observador);
            }
        }

        private void Notificar()
        {
            // Agregamos una comprobación de nulidad por si acaso
            if (_observadores == null) return;

            foreach (var observador in _observadores)
            {
                observador.ActualizarTraducciones();
            }
        }

        public string Traducir(string tag)
        {
            if (IdiomaSeleccionado != null && IdiomaSeleccionado.Palabras.ContainsKey(tag))
            {
                return IdiomaSeleccionado.Palabras[tag].ToString();
            }
            return $"[{tag}]";
        }
    }
}