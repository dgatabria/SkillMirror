using System;
using System.Collections.Generic;
using System.IO;
using BE;
using MPP;

namespace BLL
{
    public class BLLBackup
    {
        private readonly MPPBackup oMPPBackup;
        public BLLBackup() { oMPPBackup = new MPPBackup(); }

        public void CrearBackup(BEUsuario usuario, string serverPath)
        {
            // 1. Definir el nombre y la ruta completa del archivo
            string fileName = $"SkillMirror_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string fullPath = Path.Combine(serverPath, fileName);

            var backupInfo = new BEBackup
            {
                Path = fullPath,
                Usuario = usuario
            };

            // 2. Ejecutar el comando de backup en la base de datos
            oMPPBackup.CrearBackup(backupInfo);

            // 3. Obtener el tamaño del archivo creado
            FileInfo fileInfo = new FileInfo(fullPath);
            backupInfo.Size = fileInfo.Length;

            // 4. Registrar el backup en nuestra tabla de auditoría
            oMPPBackup.RegistrarBackup(backupInfo);
        }

        public List<BEBackup> Listar() => oMPPBackup.Listar();

        public BEBackup ObtenerPorId(int id) => oMPPBackup.ObtenerPorId(id);
        public void RestaurarBackup(string path) => oMPPBackup.RestaurarBackup(path);

    }
}