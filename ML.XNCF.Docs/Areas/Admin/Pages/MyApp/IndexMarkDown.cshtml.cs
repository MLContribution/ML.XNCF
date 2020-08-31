using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Ncf.Service;

namespace ML.Xncf.Docs.Areas.Admin.Pages.MyApp
{
  public class IndexMarkDownModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
  {
    public IndexMarkDownModel(Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
    {
    }

    public void OnGet()
    {
    }
  }
}
