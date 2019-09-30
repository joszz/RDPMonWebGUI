using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace RDPMonWebGUI.Models
{
    public class LiteDbContext
    {
        public readonly LiteDatabase Context;

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
}
