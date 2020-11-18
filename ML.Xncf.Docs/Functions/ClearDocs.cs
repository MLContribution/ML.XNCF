using ML.Xncf.Docs.Util;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ML.Xncf.Docs.Functions
{
    public class ClearDocs : FunctionBase
    {
        public class ClearDocs_Parameters : IFunctionParameter
        {

        }

        public CMD cmdHelper;

        public ClearDocs(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            cmdHelper = new CMD();
        }

        public override string Name => "强制删除文档";

        public override string Description => "强制删除本地存在的文档内容";

        public override Type FunctionParameterType => typeof(ClearDocs_Parameters);

        public override FunctionResult Run(IFunctionParameter param)
        {
            /* 这里是处理文字选项（单选）的一个示例 */
            return FunctionHelper.RunFunction<ClearDocs_Parameters>(param, (typeParam, sb, result) =>
            {
                var wwwrootDir = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot");
                var copyDir = Path.Combine(wwwrootDir, "NcfDocs");
                try
                {
                    cmdHelper.ExeCommand($"TASKKILL /F /IM node.exe /T");
                    cmdHelper.ExeCommand($"RD /s /q {copyDir}");
                    //清理目录
                    Directory.Delete(copyDir, true);
                }
                catch (Exception)
                {
                    sb.AppendLine("清理失败");
                }
                sb.AppendLine($"清理目录 {copyDir}");

                result.Message = $"清理成功，清理时间：{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            });
        }
    }
}
