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
            ViewData["Title"] = viewModel.Title = "Connections";
            viewModel.PageSize = _pageSize;
            viewModel.ModelType = typeof(Connection);
            viewModel.SortField = string.IsNullOrEmpty(viewModel.SortField) ? "Last" : viewModel.SortField;
            viewModel.Records = viewModel.SetOrderAndPaging(_database.GetCollection<Connection>("Addr").FindAll().ToList());

            return View("Index", viewModel);
        }

        public IActionResult Sessions(HomeViewModel viewModel)
        {
            ViewData["Title"] = viewModel.Title = "Sessions";
            viewModel.PageSize = _pageSize;
            viewModel.ModelType = typeof(Session);
            viewModel.SortField = string.IsNullOrEmpty(viewModel.SortField) ? "Start" : viewModel.SortField;
            viewModel.Records = viewModel.SetOrderAndPaging(_database.GetCollection<Session>("Session").FindAll());
            viewModel.RecordActionButtons.Add(new RecordActionButton
            {
                Action = "Processes",
                CssClass = "fb-frame icon-cogs text-decoration-none",
            });

            return View("Index", viewModel);
        }

        public IActionResult Processes(long id) => View(new HomeViewModel
        {
            Title = "Processes",
            ModelType = typeof(Process),
            Records = _database.GetCollection<Process>("Process").Find(Query.EQ("ExecInfos[*].SessionId", id)).OrderBy(proc => proc.Path).ToList<object>()
        });
    }
}
