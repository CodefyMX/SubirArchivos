using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubirArchivos.Helpers;

namespace SubirArchivos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SubirArchivo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubirArchivo(FormCollection forms)
        {
            var gestor = new GestorArchivos(HttpContext);
            gestor.GuardarArchivo("imagenes");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SubirArchivoAjax(FormCollection forms)
        {
            return View();
        }
        [HttpPost]
        public JsonResult SubirArchivoAjax(string carpeta = "imagenes")
        {
            var gestor = new GestorArchivos(HttpContext);
            var resultado = gestor.GuardarArchivo(carpeta);
            return Json(resultado,JsonRequestBehavior.AllowGet);
        }
    }
}