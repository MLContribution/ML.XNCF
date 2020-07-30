using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ML.Xncf.Docs.Services;
using Senparc.Ncf.Core.Models.DataBaseModel;
using Senparc.Ncf.Service;
using Senparc.Ncf.XncfBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using Senparc.Ncf.Core.Enums;

namespace ML.Xncf.Docs.Areas.MyApp.Pages
{
    public class Index : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        public CatalogDto CatalogDto { get; set; }

        private readonly CatalogService _catalogService;
        private readonly IServiceProvider _serviceProvider;
        public Index(IServiceProvider serviceProvider, CatalogService catalogService, Lazy<XncfModuleService> XncfModuleService)
            : base(XncfModuleService)
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
            CatalogDto = await _catalogService.CreateNewParentCatalog("���").ConfigureAwait(false);
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
