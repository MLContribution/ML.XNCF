using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML.Xncf.Docs.Models.VD;
using Senparc.Ncf.Service;

namespace ML.Xncf.Docs.Areas.Admin.Pages.Document
{
  public class PreviewModel : BaseAdminDocsModel
  {
    public PreviewModel(Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
    {
    }

    public void OnGet()
    {
    }
  }
}
