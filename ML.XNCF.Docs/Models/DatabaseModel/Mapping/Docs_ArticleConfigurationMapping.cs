using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.XNCF.Docs.Models
{
    public class Docs_ArticleConfigurationMapping : ConfigurationMappingWithIdBase<Article, int>
    {
        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(e => e.CatalogId).IsRequired();
            builder.Property(e => e.CatalogMark).IsRequired();
            builder.Property(e => e.Content).IsRequired();
        }
    }
}
