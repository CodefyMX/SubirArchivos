using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubirArchivos.Helpers
{
    public class ResultadoDeSubida
    {
        public Error Error { get; set; }
        public string CarpetaRelativa { get; set; }
        public string CarpetaAbsoluta { get; set; }
        public string DireccionEnServidor { get; set; }
    }
    public class Error
    {
        public string ErrorInfo { get; set; }
    }
}