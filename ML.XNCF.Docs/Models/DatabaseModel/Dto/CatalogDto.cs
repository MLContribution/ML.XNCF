using Senparc.Ncf.Core.Models;
using System.Collections.Generic;

namespace ML.Xncf.Docs.Models.DatabaseModel.Dto
{
    public class CatalogDto : DtoBase
    {
        /// <summary>
        /// 目录Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 目录父级Id
        /// </summary>
        public int ParentId { get; set; }

        public CatalogDto() { }
    }

    /// <summary>
    /// 目录树
    /// </summary>
    public class CatalogTreeItemDto
    {
        public string CatalogName { get; set; }

        public int Id { get; set; }

        public bool IsMenu { get; set; }

        public IList<CatalogTreeItemDto> Children { get; set; }
        public string Icon { get; set; }

        public string Url { get; set; }
    }
}
