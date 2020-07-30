using Microsoft.EntityFrameworkCore;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Models.DatabaseModel
{
    public class MLEntities : XncfDatabaseDbContext
    {
        public override IXncfDatabase XncfDatabaseRegister => new Register();
        public MLEntities(DbContextOptions<MLEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Article> Articles { get; set; }


        //如无特殊需需要，OnModelCreating 方法可以不用写，已经在 Register 中要求注册
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}
