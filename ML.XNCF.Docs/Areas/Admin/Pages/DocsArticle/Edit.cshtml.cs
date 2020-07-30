using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Models;

namespace ML.Xncf.Docs.Areas.Admin.Pages.DocsArticle
{
    public class EditModel : BaseAdminPageModel
    {
        private readonly ArticleService _articleService;
        private readonly CatalogService _catalogService;

        public EditModel(ArticleService _articleService,CatalogService _catalogService)
        {
            CurrentMenu = "Article";
            this._articleService = _articleService;
            this._catalogService = _catalogService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ArticleDto ArticleDto { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var entity = await _articleService.GetObjectAsync(_ => _.Id == Convert.ToInt32(Id));
                //SysButtons = await _sysButtonService.GetFullListAsync(_ => _.MenuId == Id);

                //SysMenuDto = _sysMenuService.Mapper.Map<SysMenuDto>(entity);
            }
            else
            {
                ArticleDto = new ArticleDto() { Flag = false };

                //SysMenuDto = new SysMenuDto() { Visible = true };
                //SysButtons = new List<SysButton>() { new SysButton() };
            }
        }

        public async Task<IActionResult> OnGetDetailAsync(int id)
        {
            var entity = await _articleService.GetObjectAsync(_ => _.Id == id);
            var articleDto = _articleService.Mapper.Map<CatalogDto>(entity);
            return Ok(new { articleDto });
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetCatalogAsync()
        {
            return Ok(await _catalogService.GetCatalogDtoByCacheAsync());
        }

        //public async Task<IActionResult> OnPostDeleteAsync(string[] ids)
        //{
        //    var entity = await _sysMenuService.GetFullListAsync(_ => ids.Contains(_.Id) && _.IsLocked == false);
        //    var buttons = await _sysButtonService.GetFullListAsync(_ => ids.Contains(_.MenuId));

        //    await _sysButtonService.DeleteAllAsync(buttons);
        //    await _sysMenuService.DeleteAllAsync(entity);
        //    await _sysMenuService.RemoveMenuAsync();
        //    IEnumerable<string> unDeleteIds = ids.Except(entity.Select(_ => _.Id));
        //    return Ok(unDeleteIds);
        //}

        public async Task<IActionResult> OnPostAddArticleAsync(ArticleDto article)
        {
            if (string.IsNullOrEmpty(article.CatalogMark))
            {
                return Ok("���ݱ�ǲ���Ϊ��", false, "���ݱ�ǲ���Ϊ��");
            }
            var entity = await _articleService.CreateOrUpdateAsync(article);
            return Ok(entity.Id);
        }

        //public async Task<IActionResult> OnPostAsync(CatalogDto catalogDto, IEnumerable<CatalogDto> subCatalog)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Ok("ģ����֤δͨ��", false, "ģ����֤δͨ��");
        //    }

        //    await _catalogService.CreateOrUpdateAsync(catalogDto, subCatalog);

        //    return Ok(new { subCatalog, catalogDto });
        //}
    }
}