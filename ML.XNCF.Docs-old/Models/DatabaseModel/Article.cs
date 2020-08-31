using ML.Xncf.Docs.Models.DatabaseModel.Dto;
using Senparc.Ncf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ML.Xncf.Docs
{
    /// <summary>
    /// 文档内容
    /// </summary>
    [Table(Register.DATABASE_PREFIX + nameof(Article))]
    [Serializable]
    public class Article : EntityBase<int>
    {
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

        public Article()
        {
        }

        public Article(int catalogId,string catalogMark,string content)
        {
            CatalogId = catalogId;
            CatalogMark = catalogMark;
            Content = content;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public Article(ArticleDto articleDto)
        {
            CatalogId = articleDto.CatalogId;
            CatalogMark = articleDto.CatalogMark;
            Content = articleDto.Content;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public void CreateContent(int catalogId,string catalogMark,string content)
        {
            CatalogId = catalogId;
            CatalogMark = catalogMark;
            Content = content;
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public void Update(ArticleDto articleDto)
        {
            this.LastUpdateTime = DateTime.Now;
            CatalogId = articleDto.CatalogId;
            CatalogMark = articleDto.CatalogMark;
            Content = articleDto.Content;
        }
    }
}
