using Microsoft.AspNetCore.Mvc;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Service;
using System;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.Areas.Admin.Pages.DocsArticle
{
  public class IndexModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
  {
    private readonly ArticleService _articleService;

    public IndexModel(IServiceProvider serviceProvider, ArticleService articleService, Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
    {
      CurrentMenu = "Article";
      _articleService = articleService;
    }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public PagedList<Article> Articles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task OnGetAsync()
    {
      Articles = await _articleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Senparc.Ncf.Core.Enums.OrderingType.Descending);
    }

    public IActionResult OnPostDelete(string[] ids)
    {
      foreach (var id in ids)
      {
        _articleService.DeleteObject(_ => _.Id == Convert.ToInt32(id));
      }

      return RedirectToPage("./Index");
    }
  }
}
