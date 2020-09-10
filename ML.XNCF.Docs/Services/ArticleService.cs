using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Senparc.CO2NET.Trace;
using Microsoft.Extensions.Caching.Distributed;
using ML.Xncf.Docs.Models.DatabaseModel;
using System.Linq;
using Senparc.Ncf.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Senparc.Ncf.Core.Models.DataBaseModel;

namespace ML.Xncf.Docs.Services
{
    public class ArticleService : ClientServiceBase<Article>
    {
        private readonly IDistributedCache _distributedCache;

        public ArticleService(ClientRepositoryBase<Article> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            _distributedCache = _serviceProvider.GetService<IDistributedCache>();
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
            article.CreateContent(2,"简介",content);
            await base.SaveObjectAsync(article).ConfigureAwait(false);
            return base.Mapper.Map<ArticleDto>(article);
        }
    }
}
