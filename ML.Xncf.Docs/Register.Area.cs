using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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

		public string HomeUrl => "/Admin/Document/Index";

		//public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
		//	 new AreaPageMenuItem(GetAreaHomeUrl(),"首页","fa fa-laptop"),
		//	 			 new AreaPageMenuItem(GetAreaUrl($"/Admin/Docs/DatabaseSample"),"数据库操作示例","fa fa-bookmark-o"),
		//				 new AreaPageMenuItem(GetAreaUrl("/Admin/DocsArticle/Index"),"内容管理","fa fa-bookmark-o"),
		//				 new AreaPageMenuItem(GetAreaUrl("/Admin/MyApp/Index"),"随机目录生成","fa fa-bookmark-o"),
		//	 		};

		//public IMvcBuilder AuthorizeConfig(IMvcBuilder builder, IWebHostEnvironment env)
		//{
		//	builder.AddRazorPagesOptions(options =>
		//	{
		//		//此处可配置页面权限
		//	});

		//	SenparcTrace.SendCustomLog("Docs 启动", "完成 Area:ML.Xncf.Docs 注册");

		//	return builder;
		//}

		public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
						 new AreaPageMenuItem(GetAreaHomeUrl(),"目录管理","fa fa-laptop"),
						 new AreaPageMenuItem(GetAreaUrl("/Admin/DocsArticle/Index"),"内容管理","fa fa-bookmark-o"),
						 new AreaPageMenuItem(GetAreaUrl("/Admin/MyApp/Index"),"随机目录生成","fa fa-bookmark-o"),
				};

		public IMvcBuilder AuthorizeConfig(IMvcBuilder builder, IWebHostEnvironment env)
		{
			builder.AddRazorPagesOptions(options =>
			{
				//此处可配置页面权限
			});

			SenparcTrace.SendCustomLog("系统启动", "完成 Area:MyApp 注册");

			return builder;
		}

		public override IServiceCollection AddXncfModule(IServiceCollection services, IConfiguration configuration)
		{
			//任何需要注册的对象
			return base.AddXncfModule(services, configuration);
		}

		#endregion

		#region IXncfRazorRuntimeCompilation 接口
		public string LibraryPath => Path.GetFullPath(Path.Combine(SiteConfig.WebRootPath, "..", "..", "ML.Xncf.Docs"));
		#endregion
	}
}
