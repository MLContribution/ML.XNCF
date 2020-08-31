using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Models.DatabaseModel.Dto
{
    public class ArticleDto : DtoBase
    {
        /// <summary>
        /// 内容Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 目录Id
        /// </summary>
        public int CatalogId { get; private set; }
        /// <summary>
        /// 目录标记
        /// </summary>
        public string CatalogMark { get; private set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }

        public ArticleDto()
        {
        }
    }
}
