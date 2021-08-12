using Microsoft.AspNetCore.Builder;
using ML.Xncf.Docs.Functions;
using Senparc.Ncf.XncfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using static ML.Xncf.Docs.Functions.UpdateDocs;
using System.Threading.Tasks;

namespace ML.Xncf.Docs
{
    public partial class Register : IXncfMiddleware  //需要引入中间件的模块
    {
        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            app.Map("/Docs", builder =>
             {
                 var indexHtmlFileDir = Path.Combine(Senparc.Ncf.Core.Config.SiteConfig.WebRootPath, "NcfDocs\\cn\\docs\\assets\\");
                 var indexHtmlFilePath = Path.Combine(indexHtmlFileDir, "index.html");

                 if (!Directory.Exists(indexHtmlFileDir) && !File.Exists(indexHtmlFilePath))
                 {
                     using (var scope = app.ApplicationServices.CreateScope())
                     {
                         UpdateDocs updateDocs = new UpdateDocs(scope.ServiceProvider);
                         updateDocs.Run(new UpdateDocs_Parameters());
                     }
                 }

                 builder.Use(async (context, next) =>
                 {
                     context.Response.ContentType = "text/html; charset=utf-8";
                     using (var fs = new FileStream(indexHtmlFilePath, FileMode.Open))
                     {
                         await fs.CopyToAsync(context.Response.Body).ConfigureAwait(false);
                     }
                 });
             });
            return app;
        }
    }
}
