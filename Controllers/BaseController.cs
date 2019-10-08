using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RDPMonWebGUI.Models;

namespace RDPMonWebGUI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly LiteDatabase _database;
        protected readonly int _pageSize;

        public BaseController(LiteDbContext context, IConfiguration configuration)
        {
            _database = context.Context;

            int.TryParse(configuration["PageSize"], out _pageSize);
        }
    }
}
