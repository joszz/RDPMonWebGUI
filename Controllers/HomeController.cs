﻿namespace RDPMonWebGUI.Controllers;

public class HomeController : BaseController
{
    public HomeController(LiteDbContext context, IConfiguration configuration) : base(context, configuration) { }

    /// <summary>
    /// Shows the Connections view, listing all the connections in a paged and sorted fashion.
    /// </summary>
    /// <param name="viewModel">The HomeViewModel containing all the information used in the view. .NET binds properties for paging and sorting.</param>
    /// <returns>The connections list view.</returns>
    public IActionResult Index(HomeViewModel viewModel)
    {
        ViewData["Title"] = viewModel.Title = "Connections";
        viewModel.PageSize = _pageSize;
        viewModel.ModelType = typeof(Connection);
        viewModel.SortField = string.IsNullOrEmpty(viewModel.SortField) ? "Last" : viewModel.SortField;
        viewModel.Records = viewModel.SetOrderAndPaging(_database.GetCollection<Connection>("Addr").FindAll().ToList());

        return View("Index", viewModel);
    }

    /// <summary>
    /// Shows the Sessions view, listing all the sessions in a paged and sorted fashion.
    /// </summary>
    /// <param name="viewModel">The HomeViewModel containing all the information used in the view. .NET binds properties for paging and sorting.</param>
    /// <returns>The sessions list view.</returns>
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

    /// <summary>
    /// Shows the processes started in a given session. This is shown inside a modal dialog with an "_Empty" layout.
    /// </summary>
    /// <param name="id">The ID of the Session to get the processes for.</param>
    /// <returns>A view listing all the processes.</returns>
    public IActionResult Processes(long id) => View(new HomeViewModel
    {
        Title = "Processes",
        ModelType = typeof(Process),
        Records = _database.GetCollection<Process>("Process").Find(Query.EQ("ExecInfos[*].SessionId", id)).OrderBy(proc => proc.Path).ToList<object>()
    });
}