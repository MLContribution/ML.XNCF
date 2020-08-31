using Senparc.Ncf.Core.Models;
using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ML.Xncf.Docs
{
    /// <summary>
    /// 文档目录
    /// </summary>
    [Table(Register.DATABASE_PREFIX + nameof(Catalog))]
    [Serializable]
    public class Catalog : EntityBase<int>
    {
        /// <summary>
        /// 目录名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 目录父级Id
        /// </summary>
        public int ParentId { get; private set; }

        public Catalog() { }

        public Catalog(string name,int parentId)
        {
            Name = name;
            ParentId = parentId;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public Catalog(CatalogDto catalogDto)
        {
            Name = catalogDto.Name;
            ParentId = catalogDto.ParentId;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public void CreateParentCatalog(string name)
        {
            Name = name;
            ParentId = 0;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public void Update(CatalogDto catalogDto)
        {
            this.LastUpdateTime = DateTime.Now;
            Name = catalogDto.Name;
            ParentId = catalogDto.ParentId;
        }
    }
}
