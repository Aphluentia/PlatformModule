﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPlatform.Services;

namespace WebPlatform.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ICLogger _logger;
        public ModulesController(ICLogger logger)
        {
            _logger = logger;
            _logger.System("Initialized Modules Controller");
        }
        // GET: ModulesController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModulesController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModulesController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModulesController1/Create
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

        // GET: ModulesController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModulesController1/Edit/5
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

        // GET: ModulesController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModulesController1/Delete/5
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