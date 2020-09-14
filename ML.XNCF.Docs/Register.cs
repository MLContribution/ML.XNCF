using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ML.Xncf.Docs.Functions;
using ML.Xncf.Docs.Models;
using ML.Xncf.Docs.Models.DatabaseModel;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Areas;
using Senparc.Ncf.Core.Config;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.XncfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ML.Xncf.Docs
{
    public partial class Register : XncfRegisterBase,
        IXncfRegister, //注册 Xncf 基础模块接口（必须）
        IAreaRegister //注册 Xncf 页面接口（按需选用）
                      //IXncfDatabase,  //注册 Xncf 模块数据库（按需选用）
                      //IXncfRazorRuntimeCompilation  //需要使用 RazorRuntimeCompilation，在开发环境下实时更新 Razor Page
    {
        public Register()
        { }


        #region IXncfRegister 接口

        public override string Name => "ML.Xncf.Docs";
        public override string Uid => "b091bfd3-3f96-4e10-aa0c-06829ee84f90";//必须确保全局唯一，生成后必须固定
        public override string Version => "2.0.51";//必须填写版本号

        public override string MenuName => "开发者文档";
        public override string Icon => "fa fa-dot-circle-o";//参考如：https://colorlib.com/polygon/gentelella/icons.html
        public override string Description => "这是一个开发者文档项目，用于阐述SCF的架构,便于开发者快速上手并掌握SCF的使用规范及开发方法";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new Type[] {
            typeof(UpdateDocs)
        };


        /// <summary>
        /// 安装或更新过程需要执行的业务
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="installOrUpdate"></param>
        /// <returns></returns>
        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            //更新数据库
            await base.MigrateDatabaseAsync<MLEntities>(serviceProvider);
            UpdateDocs updateDocs = new UpdateDocs(serviceProvider);
            IFunctionParameter functionParameter = null;

            switch (installOrUpdate)
            {
                case InstallOrUpdate.Install:
                    //新安装,建目录
                    var catalogService = serviceProvider.GetService<CatalogService>();
                    var catalogRows = catalogService.GetCount(z => true);
                    if (catalogRows <= 0)
                    {
                        await catalogService.InitCatalog();
                    }
                    var articleService = serviceProvider.GetService<ArticleService>();
                    var articleRows = articleService.GetCount(w => true);
                    if (articleRows <= 0)
                    {
                        await articleService.InitArticle();
                    }
                    _ = updateDocs.Run(functionParameter);
                    break;
                case InstallOrUpdate.Update:
                    //更新
                    _ = updateDocs.Run(functionParameter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// 删除模块时需要执行的业务
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="unsinstallFunc"></param>
        /// <returns></returns>
        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            MLEntities mlEntities = serviceProvider.GetService<MLEntities>();

            //指定需要删除的数据实体

            //注意：这里作为演示，删除了所有的表，实际操作过程中，请谨慎操作，并且按照删除顺序对实体进行排序！
            var dropTableKeys = EntitySetKeys.GetEntitySetInfo(this.XncfDatabaseDbContextType).Keys.ToArray();
            await base.DropTablesAsync(serviceProvider, mlEntities, dropTableKeys);

            await unsinstallFunc().ConfigureAwait(false);
        }

        public override IApplicationBuilder UseXncfModule(IApplicationBuilder app, IRegisterService registerService)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot")
            });

            return base.UseXncfModule(app, registerService);
        }
        #endregion
    }
}
