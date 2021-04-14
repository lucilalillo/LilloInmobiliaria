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

        public InmueblesController(IConfiguration configuration) {
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
                    ViewBag.Mensaje = TempData["Mensaje"];
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
            return View();
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
