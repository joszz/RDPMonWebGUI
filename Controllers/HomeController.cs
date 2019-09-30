using LiteDB;
using Microsoft.AspNetCore.Mvc;
using RDPMonWebGUI.Models;
using RDPMonWebGUI.ViewModels;
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

            return View(new HomeViewModel<Connection>
            {
                Title = "Connections",
                Records = collection.FindAll().OrderBy(con => con.Last).ToList()
            });
        }

        public IActionResult Sessions()
        {
            LiteCollection<Session> collection = _database.GetCollection<Session>("Session");

            return View(new HomeViewModel<Session>
            {
                Title = "Sessions",
                Records = collection.FindAll().OrderBy(con => con.Start).ToList()
            });
        }
    }
}
