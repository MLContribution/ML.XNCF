using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML.Xncf.Docs.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ML.Xncf.Tests.Functions
{
    [TestClass]
    public class UpdateDocsTests : BaseTest
    {
        public UpdateDocsTests()
        {
            //注册 SenparcHttpClient

        }

        [TestMethod]
        public void RunTest()
        {
            var function = new ML.Xncf.Docs.Functions.UpdateDocs(base.ServiceProvider);
            var result = function.Run(new UpdateDocs.UpdateDocs_Parameters());
            Console.WriteLine(result.Log);
            Console.WriteLine("===============");
            Console.WriteLine(result.Message);

            var filePath = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot", "NcfDocs", "README.md");
            Assert.IsTrue(File.Exists(filePath));//断言文件存在

            var fetchFilePath = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot", "NcfDocs", ".git", "FETCH_HEAD");
            if (File.Exists(fetchFilePath))
            {
                //更新项目
                Assert.IsTrue(SystemTime.DiffTotalMS(File.GetLastWriteTime(fetchFilePath)) < 100);//断言文件已被更新
            }
            else
            {
                //第一次拉取
            }

        }
    }
}
