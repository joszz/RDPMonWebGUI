using LiteDB;
using Microsoft.AspNetCore.Mvc;
using RDPMonWebGUI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RDPMonWebGUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteDatabase _database;

        public HomeController(LiteDbContext context)
        {
            _database = context.Context;
        }

        public IActionResult Index()
        {
            LiteCollection<Connection> collection = _database.GetCollection<Connection>("Addr");
            List<Connection> connections = collection.FindAll().OrderBy(con => con.Last).ToList();

            _database.Dispose();
            return View(connections);
        }
    }
}
