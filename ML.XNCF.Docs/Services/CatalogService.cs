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
    public class CatalogService : ClientServiceBase<Catalog>
    {
        private readonly IDistributedCache _distributedCache;

        public const string CatalogCacheKey = "AllCatalogs";
        public const string CatalogTreeCacheKey = "AllCatalogsTree";

        public CatalogService(ClientRepositoryBase<Catalog> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            _distributedCache = _serviceProvider.GetService<IDistributedCache>();
        }

        public async Task<int> InitCatalog()
        {
            await CreateNewCatalog("序言", 0);
            await CreateNewCatalog("基础", 0);
            await CreateNewCatalog("架构", 0);
            await CreateNewCatalog("路由", 0);
            await CreateNewCatalog("控制器", 0);
            await CreateNewCatalog("响应", 0);
            await CreateNewCatalog("数据库", 0);
            await CreateNewCatalog("模型", 0);
            await CreateNewCatalog("视图", 0);
            await CreateNewCatalog("模版", 0);
            await CreateNewCatalog("错误和日志", 0);
            await CreateNewCatalog("调试", 0);
            await CreateNewCatalog("验证", 0);
            await CreateNewCatalog("杂项", 0);
            await CreateNewCatalog("命令行", 0);
            await CreateNewCatalog("扩展库", 0);
            await CreateNewCatalog("安全和性能", 0);
            await CreateNewCatalog("附录", 0);
            #region 基础-2
            await CreateNewCatalog("安装", 2);
            await CreateNewCatalog("开发规范", 2);
            await CreateNewCatalog("目录结构", 2);
            await CreateNewCatalog("配置", 2);
            #endregion

            #region 架构-3
            await CreateNewCatalog("架构总览", 3);
            await CreateNewCatalog("入口文件", 3);
            await CreateNewCatalog("URL访问", 3);
            await CreateNewCatalog("模块设计", 3);
            await CreateNewCatalog("命名空间", 3);
            await CreateNewCatalog("容器和依赖注入", 3);
            await CreateNewCatalog("Facade", 3);
            await CreateNewCatalog("钩子和行为", 3);
            await CreateNewCatalog("中间件", 3);
            #endregion

            return 1;
        }

        public async Task<CatalogDto> CreateNewCatalog(string name,int parentId)
        {
            Catalog catalog = new Catalog(name, parentId);
            await base.SaveObjectAsync(catalog).ConfigureAwait(false);
            CatalogDto catalogDto = base.Mapper.Map<CatalogDto>(catalog);
            return catalogDto;
        }

        public async Task<CatalogDto> CreateNewParentCatalog(string name)
        {
            //TODO:异步方法需要添加排序功能
            Catalog catalog = new Catalog();
            catalog.CreateParentCatalog($"{name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}");
            await base.SaveObjectAsync(catalog).ConfigureAwait(false);
            return base.Mapper.Map<CatalogDto>(catalog);
        }

        /// <summary>
        /// TODO...重建目录缓存
        /// </summary>
        /// <param name="catalogDto"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public async Task<Catalog> CreateOrUpdateAsync(CatalogDto catalogDto)
        {
            Catalog catalog;
            SenparcTrace.SendCustomLog("catalogDto.Id:", catalogDto.Id.ToString());
            if(catalogDto.Id == 0 || string.IsNullOrEmpty(catalogDto.Id.ToString()))
            {
                catalog = new Catalog(catalogDto);
            }
            else
            {
                //子目录
                catalog = await GetObjectAsync(_ => _.Id == catalogDto.ParentId);
                catalog.Update(catalogDto);
            }

            await SaveObjectAsync(catalog);
            await GetCatalogDtoByCacheAsync(true);
            return catalog;
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task RemoveCatalogAsync()
        {
            await _distributedCache.RemoveAsync(CatalogCacheKey);
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CatalogDto>> GetCatalogDtoByCacheAsync(bool isRefresh = false)
        {
            List<CatalogDto> selectListItems = null;
            //byte[] selectLiteItemBytes = await _distributedCache.GetAsync(CatalogCacheKey);
            //if (selectLiteItemBytes != null || isRefresh)
            //{
            //    SenparcTrace.SendCustomLog("selectLiteItemBytes data is :", System.Text.Encoding.UTF8.GetString(selectLiteItemBytes));
            //    selectListItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CatalogDto>>(System.Text.Encoding.UTF8.GetString(selectLiteItemBytes));
            //    return selectListItems;
            //}

            List<Catalog> catalogs = (await GetFullListAsync(_ => _.Flag == false).ConfigureAwait(false)).OrderByDescending(z => z.AddTime).ToList();
            selectListItems = Mapper.Map<List<CatalogDto>>(catalogs);
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(selectListItems);
            //await _distributedCache.RemoveAsync(CatalogCacheKey);
            //await _distributedCache.RemoveAsync(CatalogTreeCacheKey);
            //await _distributedCache.SetAsync(CatalogCacheKey, System.Text.Encoding.UTF8.GetBytes(jsonStr));
            //await _distributedCache.SetStringAsync(CatalogTreeCacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(GetCatalogTreesMainRecursive(selectListItems)));

            return selectListItems;
        }

        public IEnumerable<CatalogTreeItemDto> GetCatalogTreesMainRecursive(IEnumerable<CatalogDto> catalogTreeItems)
        {
            List<CatalogTreeItemDto> catalogTrees = new List<CatalogTreeItemDto>();
            getCatalogTreesRecursive(catalogTreeItems, catalogTrees, 0);
            return catalogTrees;
        }

        private void getCatalogTreesRecursive(IEnumerable<CatalogDto> catalogTreeItems, IList<CatalogTreeItemDto> catalogTrees, int parentId)
        {
            foreach (var item in catalogTreeItems.Where(_ => _.ParentId == parentId && _.ParentId == 0))
            {
                CatalogTreeItemDto catalog = new CatalogTreeItemDto() { 
                    CatalogName = item.Name, 
                    Id = item.ParentId,
                    IsMenu = false,
                    Icon = "",
                    Url = "",
                    Children = new List<CatalogTreeItemDto>() 
                };
                catalogTrees.Add(catalog);
                getCatalogTreesRecursive(catalogTreeItems, catalog.Children, item.ParentId);
            }
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CatalogTreeItemDto>> GetCatalogTreeDtoByCacheAsync()
        {
            IEnumerable<CatalogTreeItemDto> catalogTreeItems = null;//
            string jsonStr = await _distributedCache.GetStringAsync(CatalogTreeCacheKey);
            if (string.IsNullOrEmpty(jsonStr))
            {
                await GetCatalogDtoByCacheAsync(true);
            }
            jsonStr = await _distributedCache.GetStringAsync(CatalogTreeCacheKey);
            catalogTreeItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CatalogTreeItemDto>>(jsonStr);
            return catalogTreeItems;
        }
    }
}
