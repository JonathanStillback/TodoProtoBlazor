using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Products.Models;

namespace Products.Controllers
{
    [Route("api/{controller}/{action}")]
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;

        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Create(Todo todo)
        {
            return View();
        }
        public IActionResult Get(int id)
        {
            return View();
        }
        public IActionResult Update(Todo todo)
        {
            return View();
        }
        public IActionResult Delete(Todo todo)
        {
            return View();
        }
    }
}
