using System;
using System.Collections.Generic;
using Senparc.Ncf.XncfBase;
using ML.Xncf.Docs.Functions;
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

        public override string Version => "2.1.1";//必须填写版本号

        public override string MenuName => "开发者文档";

        public override string Icon => "fa fa-book";

        public override string Description => "这是一个开发者文档项目，用于阐述NCF的架构,便于开发者快速上手并掌握NCF的使用规范及开发方法";

        public override IList<Type> Functions => new Type[] { 
            typeof(UpdateDocs),
            typeof(ClearDocs),
        };


        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
        }

        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            //指定需要删除的数据实体
            ClearDocs clearDocs = new ClearDocs(serviceProvider);
            _ = clearDocs.Run(null);

            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
