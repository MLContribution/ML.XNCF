﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Ncf.Database;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.XncfBase.Database;
using System;
using System.IO;

namespace ML.Xncf.Docs.Models.DatabaseModel
{
    [MultipleMigrationDbContext(MultipleDatabaseType.SqlServer, typeof(Register))]
    public class DocsSenparcEntities_SqlServer : DocsSenparcEntities
    {
        public DocsSenparcEntities_SqlServer(DbContextOptions<DocsSenparcEntities_SqlServer> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
    

    /// <summary>
    /// 设计时 DbContext 创建（仅在开发时创建 Code-First 的数据库 Migration 使用，在生产环境不会执行）
    /// <para>1、切换至 Debug 模式</para>
    /// <para>2、运行：PM> add-migration [更新名称] -c DocsSenparcEntities_SqlServer -o Migrations/Migrations.SqlServer </para>
    /// </summary>
    public class SenparcDbContextFactory_SqlServer : SenparcDesignTimeDbContextFactoryBase<DocsSenparcEntities_SqlServer, Register>
    {
        protected override Action<IServiceCollection> ServicesAction => services =>
        {
            //指定其他数据库
            services.AddDatabase("Senparc.Ncf.Database.SqlServer", "Senparc.Ncf.Database.SqlServer", "SQLServerDatabaseConfiguration");
        };

        /// <summary>
        /// 用于寻找 App_Data 文件夹，从而找到数据库连接字符串配置信息
        /// </summary>
        private static string RootDictionaryPath => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"/*项目根目录*/, "..\\Senparc.Web"/*找到 Web目录，以获取统一的数据库连接字符串配置*/);

        public SenparcDbContextFactory_SqlServer() : base(RootDictionaryPath)
        {

        }
    }
}
