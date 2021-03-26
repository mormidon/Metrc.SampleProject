using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text;
using Metrc.SampleProject.Deployments;

namespace Metrc.SampleProject.WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public partial class DeploymentController : Controller
    {
        [HttpGet("Migrate")]
        public virtual IActionResult Migrate()
        {
            var result = new StringBuilder();

            result.AppendFormat("Deployment Migrate:{0}{0}", Environment.NewLine);
            result.AppendFormat("Deployment Started at {0}{1}", DateTime.Now, Environment.NewLine);

            var stopwatch = Stopwatch.StartNew();
            Deployment.MigrateDatabase(Startup.ConnectionString);
            result.AppendFormat("Deployment Completed in {0:N0} ms{1}", stopwatch.ElapsedMilliseconds, Environment.NewLine);

            result.AppendFormat("Deployment Ended at {0}{1}", DateTime.Now, Environment.NewLine);

            return Content(result.ToString());
        }

        [HttpGet("Clean")]
        public virtual IActionResult Clean()
        {
            var result = new StringBuilder();

            result.AppendFormat("Deployment Clean:{0}{0}", Environment.NewLine);
            result.AppendFormat("Deployment Started at {0}{1}", DateTime.Now, Environment.NewLine);

            var stopwatch = Stopwatch.StartNew();
            Deployment.CleanDatabase(Startup.ConnectionString);
            Deployment.MigrateDatabase(Startup.ConnectionString);
            result.AppendFormat("Deployment Completed in {0:N0} ms{1}", stopwatch.ElapsedMilliseconds, Environment.NewLine);

            result.AppendFormat("Deployment Ended at {0}{1}", DateTime.Now, Environment.NewLine);

            return Content(result.ToString());
        }

    }
}
