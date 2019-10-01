using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RDPMonWebGUI.Models;
using RDPMonWebGUI.ViewModels;
using System.Linq; 

namespace RDPMonWebGUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteDatabase _database;
        private readonly int _pageSize;

        public HomeController(LiteDbContext context, IConfiguration configuration)
        {
            _database = context.Context;

            int.TryParse(configuration["PageSize"], out _pageSize);
        }

        public IActionResult Index(HomeViewModel viewModel)
        {
            viewModel.Title = "Connections";
            viewModel.PageSize = _pageSize;
            viewModel.ModelType = typeof(Connection);
            viewModel.Records = viewModel.SetOrderAndPaging(_database.GetCollection<Connection>("Addr").FindAll().ToList());
            
            return View("Index", viewModel);
        }

        public IActionResult Sessions(HomeViewModel viewModel)
        {
            viewModel.Title = "Sessions";
            viewModel.PageSize = _pageSize;
            viewModel.ModelType = typeof(Session);
            viewModel.Records = viewModel.SetOrderAndPaging(_database.GetCollection<Session>("Session").FindAll());

            return View("Index", viewModel);
        }
    }
}
