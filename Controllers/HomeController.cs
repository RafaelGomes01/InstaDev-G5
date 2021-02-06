﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using back_end_totoal.Models;
using Microsoft.AspNetCore.Http;

namespace back_end_totoal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // ViewBag que contem o nome logado na aplicação.
            ViewBag.UserName = HttpContext.Session.GetString("_UserName");
            return LocalRedirect("~/Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
