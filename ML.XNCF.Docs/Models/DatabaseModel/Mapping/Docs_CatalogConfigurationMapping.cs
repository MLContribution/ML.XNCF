using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Ncf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Models
{
    public class Docs_CatalogConfigurationMapping : ConfigurationMappingWithIdBase<Catalog, int>
    {
        public override void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.ParentId).IsRequired();
        }
    }
}
