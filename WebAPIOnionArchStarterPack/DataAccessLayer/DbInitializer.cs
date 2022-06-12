// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            var tasks = new List<Task>
            {
                // TODO: we are going to add tasks here which is needed to initialize while project first run.
            };
            Task.WaitAll(tasks.ToArray());
        }
    }
}