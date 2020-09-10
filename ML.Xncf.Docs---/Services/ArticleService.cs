using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using System;
using System.Threading.Tasks;
//using Microsoft.Extensions.Caching.Distributed;

namespace ML.Xncf.Docs.Services
{
  public class ArticleService : ServiceBase<Article>
  {
    //private readonly IDistributedCache _distributedCache;

    public ArticleService(IRepositoryBase<Article> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
    {
      //_distributedCache = _serviceProvider.GetService<IDistributedCache>();
    }

    public async Task<int> InitArticle()
    {
      await CreateDemoContent("这是一个演示的文档内容");
      return 1;
    }

    /// <summary>
    /// TODO...重建目录缓存
    /// </summary>
    /// <param name="catalogDto"></param>
    /// <param name="buttons"></param>
    /// <returns></returns>
    public async Task<Article> CreateOrUpdateAsync(ArticleDto articleDto)
    {
      Article article;
      //SenparcTrace.SendCustomLog("catalogDto.Id:", catalogDto.Id.ToString());
      if (articleDto.Id == 0 || string.IsNullOrEmpty(articleDto.Id.ToString()))
      {
        article = new Article(articleDto);
      }
      else
      {
        //子目录
        article = await GetObjectAsync(_ => _.Id == articleDto.Id);
        article.Update(articleDto);
      }

      await SaveObjectAsync(article);
      //await GetCatalogDtoByCacheAsync(true);
      return article;
    }

    public async Task<ArticleDto> CreateDemoContent(string content)
    {
      //TODO:异步方法需要添加排序功能
      Article article = new Article();
      article.CreateContent(2, "简介", content);
      await base.SaveObjectAsync(article).ConfigureAwait(false);
      return base.Mapper.Map<ArticleDto>(article);
    }
  }
}
