using Microsoft.AspNetCore.Builder;
using Senparc.Ncf.XncfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using ML.Xncf.Docs.OHS.Local.AppService;
using ML.Xncf.Docs.OHS.PL;

namespace ML.Xncf.Docs
{
    public partial class Register : IXncfMiddleware  //需要引入中间件的模块
    {
        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            //string defaultBranck = "release-0.3";
            string defaultBranck = "v1.0";
            var docDir = Path.Combine(Senparc.Ncf.Core.Config.SiteConfig.WebRootPath, $"NcfDocs\\{defaultBranck}\\cn\\docs\\doc\\");
            var assetsDir = Path.GetFullPath(Path.Combine(docDir, "..\\", "assets"));

            app.Map("/Docs", async builder =>
              {
                  var indexHtmlFileDir = Path.Combine(Senparc.Ncf.Core.Config.SiteConfig.WebRootPath, $"NcfDocs\\{defaultBranck}\\cn\\docs\\assets\\");
                  var indexHtmlFilePath = Path.Combine(indexHtmlFileDir, "index.html");

                  if (!File.Exists(indexHtmlFilePath))
                  {
                      using (var scope = app.ApplicationServices.CreateScope())
                      {
                          //更新文档
                          var serviceProvider = scope.ServiceProvider; 
                          UpdateDocsAppService updateDocsAppService = serviceProvider.GetService<UpdateDocsAppService>();
                          var docsRunRequest = new Docs_RunRequest();
                          await updateDocsAppService.Run(docsRunRequest);

                          //等待更新
                          var dt1 = SystemTime.Now;
                          while (SystemTime.NowDiff(dt1) < TimeSpan.FromSeconds(10))
                          {
                              if (File.Exists(indexHtmlFilePath))
                              {
                                  break;
                              }
                              await Task.Delay(1000);
                          }
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

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(assetsDir),
                RequestPath = new PathString("")
            });

            return app;
        }
    }
}
