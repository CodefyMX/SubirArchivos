using System;
using System.IO;
using System.Web;

namespace SubirArchivos.Helpers
{
    public class GestorArchivos
    {
        private readonly HttpContextBase _context;
        private const string Contentfolder = "Content";
        public GestorArchivos(HttpContextBase context)
        {
            _context = context;
        }
        /// <summary>
        /// Guarda el archivo en la carpeta especificada y regresa la ruta relativa
        /// </summary>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public ResultadoDeSubida GuardarArchivo(string carpeta)
        {
            //Cargamos el archivo
            var archivo = ResuelveArchivo();
            if(archivo == null) throw new Exception("No se subió ningún archivo");
            //Creamos la carpeta del archivo
            var carpetaLocal = CrearCarpeta(carpeta);
            var nombreDelArchivo = CreaNombreDeArchivo(archivo);
            try
            {
                var carpetaAbsoluta = $"{carpetaLocal}\\{nombreDelArchivo}";
                archivo.SaveAs(carpetaAbsoluta);

                var carpetaRelativa = $"/{Contentfolder}/{carpeta}/{nombreDelArchivo}";

                return new ResultadoDeSubida()
                {
                    CarpetaRelativa = carpetaRelativa,
                    CarpetaAbsoluta = carpetaAbsoluta,
                    DireccionEnServidor = $"{ServerUrl}{Contentfolder}/{carpeta}/{nombreDelArchivo}",
                    Error = null
                };
            }
            catch (Exception e)
            {
                return new ResultadoDeSubida()
                {
                    Error = new Error()
                    {
                        ErrorInfo = e.Message
                    }
                };
            }

        }
        /// <summary>
        /// Crea un nombre único para el archivo, útil para subir archivos con AJAX
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        private string CreaNombreDeArchivo(HttpPostedFileBase archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreUnico = Guid.NewGuid().ToString("N").Substring(0,10);
            return nombreUnico + extension;
        }

        /// <summary>
        /// Carga el archivo de la petición
        /// </summary>
        /// <returns></returns>
        private HttpPostedFileBase ResuelveArchivo()
        {
            //Request.Files contiene la colección de los archivos en la petición
            return _context.Request.Files.Count > 0 ? _context.Request.Files[0] : null;
        }

        /// <summary>
        /// Crea la carpeta en la cual se guardara el archivo
        /// </summary>
        /// <param name="carpeta"></param>
        private string CrearCarpeta(string carpeta)
        {
            var carpetaContent = _context.Server.MapPath($"/{Contentfolder}/{carpeta}");

            if (!Directory.Exists(carpetaContent))
            {
                Directory.CreateDirectory(carpetaContent);
            }
            return carpetaContent;
        }

        private string ServerUrl
        {
            get
            {
                if (_context.Request.Url != null)
                    if (_context.Request.ApplicationPath != null)
                        return _context.Request.Url.Scheme + "://" + _context.Request.Url.Authority +
                               _context.Request.ApplicationPath.TrimEnd('/') + "/";
                return string.Empty;
            }
        }
    }
}