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
using Senparc.Ncf.Database;

namespace ML.Xncf.Docs
{
    [XncfRegister]
    public partial class Register : XncfRegisterBase, IXncfRegister
    {
        #region IRegister 接口

        public override string Name => "ML.Xncf.Docs";

        public override string Uid => "C31EB06B-D0EE-4356-9313-9DD842E0B9D2";//必须确保全局唯一，生成后必须固定，已自动生成，也可自行修改

        public override string Version => "2.0.116";//必须填写版本号

        public override string MenuName => "开发者文档";

        public override string Icon => "fa fa-book";

        public override string Description => "这是一个开发者文档项目，用于阐述NCF的架构,便于开发者快速上手并掌握NCF的使用规范及开发方法";

        public override IList<Type> Functions => new Type[] { 
            typeof(UpdateDocs),
            typeof(ClearDocs),
        };


        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            //安装或升级版本时更新数据库
            await base.MigrateDatabaseAsync(serviceProvider);
            UpdateDocs updateDocs = new UpdateDocs(serviceProvider);

            //根据安装或更新不同条件执行逻辑
            switch (installOrUpdate)
            {
                case InstallOrUpdate.Install:
                    //新安装
                    _ = updateDocs.Run(null);
                    break;
                case InstallOrUpdate.Update:
                    //更新
                    _ = updateDocs.Run(null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            #region 删除数据库（演示）

            var mySenparcEntitiesType = this.TryGetXncfDatabaseDbContextType;
            DocsSenparcEntities mySenparcEntities = serviceProvider.GetService(mySenparcEntitiesType) as DocsSenparcEntities;

            //指定需要删除的数据实体

            UpdateDocs updateDocs = new UpdateDocs(serviceProvider);
            _ = updateDocs.Clear(null);

            //注意：这里作为演示，在卸载模块的时候删除了所有本模块创建的表，实际操作过程中，请谨慎操作，并且按照删除顺序对实体进行排序！
            var dropTableKeys = EntitySetKeys.GetEntitySetInfo(this.TryGetXncfDatabaseDbContextType).Keys.ToArray();
            await base.DropTablesAsync(serviceProvider, mySenparcEntities, dropTableKeys);

            #endregion

            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
