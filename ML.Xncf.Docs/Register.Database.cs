using ML.Xncf.Docs.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Ncf.XncfBase;
using System;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;

namespace ML.Xncf.Docs
{
  public partial class Register :
      IXncfDatabase  //注册 XNCF 模块数据库（按需选用）
  {
    #region IXncfDatabase 接口

    /// <summary>
    /// 数据库前缀
    /// </summary>
    public const string DATABASE_PREFIX = "ML_Docs_";

    /// <summary>
    /// 数据库前缀
    /// </summary>
    public string DatabaseUniquePrefix => DATABASE_PREFIX;
    /// <summary>
    /// 设置 XncfSenparcEntities 类型
    /// </summary>
    public Type XncfDatabaseDbContextType => typeof(DocsSenparcEntities);

    public void AddXncfDatabaseModule(IServiceCollection services)
    {
      //DOT REMOVE OR MODIFY THIS LINE 请勿移除或修改本行 - Entities Point
      //ex. services.AddScoped(typeof(Color));

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
  }
}
