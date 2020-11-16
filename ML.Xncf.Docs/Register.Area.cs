using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Areas;
using Senparc.Ncf.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace ML.Xncf.Docs
{
	public partial class Register : IAreaRegister //注册 XNCF 页面接口（按需选用）
	{
		#region IAreaRegister 接口

		public string HomeUrl => "/Docs/Index";

		public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
			 new AreaPageMenuItem(GetAreaHomeUrl(),"开发者文档中心","fa fa-laptop"),
			 		};

		public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
		{
			builder.AddRazorPagesOptions(options =>
			{
				//此处可配置页面权限
			});

			SenparcTrace.SendCustomLog("Docs 启动", "完成 Area:ML.Xncf.Docs 注册");

			return builder;
		}

		#endregion

		#region IXncfRazorRuntimeCompilation 接口
		public string LibraryPath => Path.GetFullPath(Path.Combine(SiteConfig.WebRootPath, "..", "..", "ML.Xncf.Docs"));
		#endregion
	}
}
