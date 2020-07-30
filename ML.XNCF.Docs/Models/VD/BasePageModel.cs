﻿using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.VD;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.XNCF.Docs.Models.VD
{
    public interface IBasePageModel : IPageModelBase
    { }

    /// <summary>
    /// 当前项目供前端（非Areas）使用的PageModel全局基类
    /// </summary>
    public class BasePageModel : PageModelBase, IBasePageModel
    {
    }
}
