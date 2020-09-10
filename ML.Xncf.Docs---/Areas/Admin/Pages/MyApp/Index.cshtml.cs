using Microsoft.AspNetCore.Mvc;
using ML.Xncf.Docs;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Service;
using System;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.Areas.MyApp.Pages
{
    public class Index : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
  {
        public CatalogDto CatalogDto { get; set; }

        private readonly CatalogService _catalogService;
        private readonly IServiceProvider _serviceProvider;
        public Index(IServiceProvider serviceProvider, CatalogService catalogService, Lazy<XncfModuleService> xscfModuleService)
            : base(xscfModuleService)
        {
            _catalogService = catalogService;
            _serviceProvider = serviceProvider;
        }

        public Task OnGetAsync()
        {
            var catalog = _catalogService.GetObject(z => true, z => z.Id, OrderingType.Descending);
            CatalogDto = _catalogService.Mapper.Map<CatalogDto>(catalog);
            return Task.CompletedTask;
        }

        public async Task OnGetCreateParentCatalogAsync()
        {
            CatalogDto = await _catalogService.CreateNewParentCatalog("¹þà¶").ConfigureAwait(false);
        }

        //public async Task OnGetDarkenAsync()
        //{
        //    CatalogDto = await _catalogService.Darken().ConfigureAwait(false);
        //}
        //public async Task OnGetRandomAsync()
        //{
        //    CatalogDto = await _catalogService.Random().ConfigureAwait(false);
        //}
    }
}
