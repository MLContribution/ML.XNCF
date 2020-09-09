using Microsoft.AspNetCore.Mvc;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Service;
using System;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.Areas.Admin.Pages.Document
{
  public class PreviewContainerModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
  {
    public PreviewContainerModel(Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
    {
    }

    public void OnGet()
    {
    }
  }
}
