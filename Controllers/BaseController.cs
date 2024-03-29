﻿namespace RDPMonWebGUI.Controllers;

[AutoValidateAntiforgeryToken]
public class BaseController : Controller
{
    protected readonly IConfiguration _configuration;
    protected readonly LiteDatabase _database;
    protected readonly int _pageSize;

    public BaseController(LiteDbContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _database = context.Context;

        _ = int.TryParse(configuration["PageSize"], out _pageSize);
    }
}