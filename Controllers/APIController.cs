namespace RDPMonWebGUI.Controllers;

public class APIController : BaseController
{
    public APIController(LiteDbContext context, IConfiguration configuration) : base(context, configuration) { }

    /// <summary>
    /// Retrieves all the connections. 
    /// </summary>
    /// <returns>All the connections in JSON format.</returns>
    public IActionResult Connections() => Json(_database.GetCollection<Connection>("Addr").FindAll().ToList());

    /// <summary>
    /// Retrieves all the sessions. 
    /// </summary>
    /// <returns>All the sessions in JSON format.</returns>
    public IActionResult Sessions() => Json(_database.GetCollection<Session>("Session").FindAll().ToList());

    /// <summary>
    /// Retrieves the processess of a given session id.
    /// </summary>
    /// <param name="id">The id of the session to retrieve the processes for.</param>
    /// <returns>The processes in JSON format.</returns>
    public IActionResult Processes(long id) => Json(_database.GetCollection<Process>("Process").Find(Query.EQ("ExecInfos[*].SessionId", id)).OrderBy(proc => proc.Path).ToList<object>());
}
