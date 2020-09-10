﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Ncf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Models
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
