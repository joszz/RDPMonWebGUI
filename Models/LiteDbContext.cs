using Microsoft.Extensions.Options;

namespace RDPMonWebGUI.Models;

public class LiteDbContext
{
    public readonly LiteDatabase Context;

    /// <summary>
    /// Creates the LiteDatabase and sets it up to be disposed at the end of the request.
    /// </summary>
    /// <param name="configs">The LiteDbConfig, containing the DatabasePath to open the database with.</param>
    /// <param name="context">The IHttpContextAccessor to setup the disposing of the LiteDatabase at the end of a request.</param>
    public LiteDbContext(IOptions<LiteDbConfig> configs, IHttpContextAccessor context)
    {
        try
        {
            LiteDatabase database = new LiteDatabase(configs.Value.DatabasePath);

            if (database != null)
            {
                Context = database;
                context.HttpContext.Response.RegisterForDispose(database);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Can find or create LiteDb database.", ex);
        }
    }
}
