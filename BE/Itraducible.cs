namespace BE
{
    /// <summary>
    /// Interfaz para todos los formularios y controles que necesiten ser notificados
    /// cuando el idioma de la aplicación cambie.
    /// </summary>
    public interface ITraducible
    {
        /// <summary>
        /// Método que será llamado por el gestor de traducciones para actualizar los textos.
        /// </summary>
        void ActualizarTraducciones();
    }
}