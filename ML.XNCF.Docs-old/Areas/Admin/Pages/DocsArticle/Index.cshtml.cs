using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Models;

namespace ML.Xncf.Docs.Areas.Admin.Pages.DocsArticle
{
    public class IndexModel : BaseAdminPageModel
    {
        private readonly ArticleService _articleService;

        public IndexModel(ArticleService _articleService)
        {
            CurrentMenu = "Article";
            this._articleService = _articleService;
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
