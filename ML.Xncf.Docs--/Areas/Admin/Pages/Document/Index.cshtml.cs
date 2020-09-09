using Microsoft.AspNetCore.Mvc;
using ML.Xncf.Docs;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Service;
using System;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.Areas.Admin.Pages.Document
{
  public class IndexModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
  {
    private readonly CatalogService _catalogService;

    public IndexModel(IServiceProvider serviceProvider, CatalogService catalogService, Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
    {
      CurrentMenu = "Catalog";
      _catalogService = catalogService;
    }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public PagedList<Catalog> Catalogs { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task OnGetAsync()
    {
      Catalogs = await _catalogService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Senparc.Ncf.Core.Enums.OrderingType.Descending);
    }

    public IActionResult OnPostDelete(string[] ids)
    {
      foreach (var id in ids)
      {
        _catalogService.DeleteObject(_ => _.Id == Convert.ToInt32(id));
      }

      return RedirectToPage("./Index");
    }
  }
}
