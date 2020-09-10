using Microsoft.EntityFrameworkCore;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Database;

namespace ML.Xncf.Docs.Models.DatabaseModel
{
  public class DocsSenparcEntities : XncfDatabaseDbContext
  {
    public override IXncfDatabase XncfDatabaseRegister => new Register();
    public DocsSenparcEntities(DbContextOptions<DocsSenparcEntities> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Color> Colors { get; set; }

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Article> Articles { get; set; }

    //DOT REMOVE OR MODIFY THIS LINE 请勿移除或修改本行 - Entities Point
    //ex. public DbSet<Color> Colors { get; set; }

    //如无特殊需需要，OnModelCreating 方法可以不用写，已经在 Register 中要求注册
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //}
  }
}
