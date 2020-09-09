using System;
using System.Collections.Generic;
using Senparc.Ncf.XncfBase;
using ML.Xncf.Docs.Functions;
using ML.Xncf.Docs.Models.DatabaseModel;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ML.Xncf.Docs.Services;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using Senparc.Ncf.Core.Areas;
using Senparc.CO2NET.Trace;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ML.Xncf.Docs.Models;

namespace ML.Xncf.Docs
{
  [XncfRegister]
  public partial class Register : XncfRegisterBase, IXncfRegister
  {
    #region IRegister 接口

    public override string Name => "ML.Xncf.Docs";

    public override string Uid => "b091bfd3-3f96-4e10-aa0c-06829ee84f90";//必须确保全局唯一，生成后必须固定，已自动生成，也可自行修改

    public override string Version => "2.0.2";//必须填写版本号

    public override string MenuName => "NCF开发文档";

    public override string Icon => "fa fa-dot-circle-o";

    public override string Description => "这是一个开发者文档项目，用于阐述SCF的架构,便于开发者快速上手并掌握SCF的使用规范及开发方法";

    public override IList<Type> Functions => new Type[] {
          typeof(MyFunction), /*typeof(BuildXncf)*/ 
          typeof(UpdateDocs)
        };


    public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
    {
      //安装或升级版本时更新数据库
      await base.MigrateDatabaseAsync<DocsSenparcEntities>(serviceProvider);

      //根据安装或更新不同条件执行逻辑
      switch (installOrUpdate)
      {
        case InstallOrUpdate.Install:
          //新安装
          #region 初始化数据库数据
          var colorService = serviceProvider.GetService<ColorService>();
          var color = colorService.GetObject(z => true);
          if (color == null)//如果是纯第一次安装，理论上不会有残留数据
          {
            ColorDto colorDto = await colorService.CreateNewColor().ConfigureAwait(false);//创建默认颜色
          }
          #endregion

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
          break;
        case InstallOrUpdate.Update:
          //更新
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
    {
      #region 删除数据库（演示）

      DocsSenparcEntities mySenparcEntities = serviceProvider.GetService<DocsSenparcEntities>();

      //指定需要删除的数据实体

      //注意：这里作为演示，在卸载模块的时候删除了所有本模块创建的表，实际操作过程中，请谨慎操作，并且按照删除顺序对实体进行排序！
      var dropTableKeys = EntitySetKeys.GetEntitySetInfo(this.XncfDatabaseDbContextType).Keys.ToArray();
      await base.DropTablesAsync(serviceProvider, mySenparcEntities, dropTableKeys);

      #endregion

      await unsinstallFunc().ConfigureAwait(false);
    }

    #endregion

    #region IAreaRegister 接口

    public override IServiceCollection AddXncfModule(IServiceCollection services, IConfiguration configuration)
    {
      //任何需要注册的对象
      return base.AddXncfModule(services, configuration);
    }

    #endregion

    #region IXscfDatabase 接口

    public void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new Docs_CatalogConfigurationMapping());
      modelBuilder.ApplyConfiguration(new Docs_ArticleConfigurationMapping());
    }

    public void AddXscfDatabaseModule(IServiceCollection services)
    {
      //add catalog
      services.AddScoped(typeof(Catalog));
      services.AddScoped(typeof(CatalogDto));
      services.AddScoped(typeof(CatalogService));
      //add article
      services.AddScoped(typeof(Article));
      services.AddScoped(typeof(ArticleDto));
      services.AddScoped(typeof(ArticleService));
    }

    #endregion

    #region IXscfRazorRuntimeCompilation 接口
    //public string LibraryPath => Path.GetFullPath(Path.Combine(SiteConfig.WebRootPath, "..", "ML.Xscf.Docs"));
    #endregion
  }
}
