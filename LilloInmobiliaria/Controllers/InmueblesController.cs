  using LilloInmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmueble repositorio;
        private readonly RepositorioPropietario repoPropietario;
        protected readonly IConfiguration config;

        public InmueblesController(IConfiguration configuration) {

            this.config = configuration;
            this.repositorio = new RepositorioInmueble(configuration);
            this.repoPropietario = new RepositorioPropietario(configuration) ;
        }

        // GET: InmuebleController
        public ActionResult Index()
        { 
                try
                {
                    var lista = repositorio.ObtenerTodos();
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                    return View();
                }
            }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            Inmueble inmu = new Inmueble();
            inmu = repositorio.ObtenerPorId(id);
            return View(inmu);
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repositorio.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmu)
        {
            try
            {
                inmu.IdInmueble = id;
                repositorio.Modificacion(inmu);
                TempData["id"] = "Se actualizo el inmueble";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }

        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble inmu)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE"))
                {
                    Inmueble p = repositorio.ObtenerPorId(id);
                    ViewBag.lugar = p.Prop.Nombre + " " + p.Prop.Apellido + " en " + p.Direccion;
                    ViewBag.Error = "No se puede eliminar el inmueble porque tiene contratos vigentes";
                }
                else
                {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(inmu);
            }
        }
    }
}
