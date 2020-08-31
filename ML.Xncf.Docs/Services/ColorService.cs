﻿using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using System;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.Services
{
    public class ColorService : ServiceBase<Color>
    {
        public ColorService(IRepositoryBase<Color> repo, IServiceProvider serviceProvider)
            : base(repo, serviceProvider)
        {
        }

        public async Task<ColorDto> CreateNewColor()
        {
            Color color = new Color(-1, -1, -1);
            await base.SaveObjectAsync(color).ConfigureAwait(false);
            ColorDto colorDto = base.Mapper.Map<ColorDto>(color);
            return colorDto;
        }

        public async Task<ColorDto> Brighten()
        {
            //TODO:异步方法需要添加排序功能
            var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
            obj.Brighten();
            await base.SaveObjectAsync(obj).ConfigureAwait(false);
            return base.Mapper.Map<ColorDto>(obj);
        }

        public async Task<ColorDto> Darken()
        {
            //TODO:异步方法需要添加排序功能
            var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
            obj.Darken();
            await base.SaveObjectAsync(obj).ConfigureAwait(false);
            return base.Mapper.Map<ColorDto>(obj);
        }

        public async Task<ColorDto> Random()
        {
            //TODO:异步方法需要添加排序功能
            var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
            obj.Random();
            await base.SaveObjectAsync(obj).ConfigureAwait(false);
            return base.Mapper.Map<ColorDto>(obj);
        }

        //TODO: 更多业务方法可以写到这里
    }
}
