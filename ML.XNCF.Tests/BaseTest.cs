using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.RegisterServices;
using System.IO;

namespace ML.XNCF.Tests
{
    [TestClass]
    public class BaseTest
    {
        public ServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }

        public ServiceProvider ServiceProvider { get; set; }

        public BaseTest()
        {
            ServiceCollection = new ServiceCollection();

            var cb = new ConfigurationBuilder();
            Configuration = cb.Build();

            ServiceCollection.AddSenparcGlobalServices(Configuration);
            BuildServiceProvider();

            Senparc.CO2NET.Config.RootDictionaryPath = Directory.GetCurrentDirectory();

        }

        protected void BuildServiceProvider()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
