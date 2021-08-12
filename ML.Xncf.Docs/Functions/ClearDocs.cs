using ML.Core;
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

        public ClearDocs(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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
                    //执行Dos命令
                    sb = ExecDosCommand(sb, wwwrootDir, copyDir);
                    //写入批处理文件并执行
                    //sb = WriteBatchFile(sb, wwwrootDir, copyDir);
                }
                catch (Exception)
                {
                    sb.AppendLine("清理失败");
                }
                //sb.AppendLine($"清理目录 {copyDir}");
                //\r\n<br /> {sb.ToString()}
                result.Message = $"清理成功，清理时间：{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            });
        }

        private StringBuilder ExecDosCommand(StringBuilder sb, string rootPath, string destPath)
        {
            List<string> lstCMD = new List<string>();
            Random random = new Random();
            sb.Append($"wwwrootDir：{rootPath}\r\n");
            sb.Append($"copyDir：{destPath}\r\n");
            string[] saDir = DirFileHelper.GetDirectories(rootPath);
            sb.Append($"saDir:{saDir.ToJson()}");
            for (int i = 0; i < saDir.Length; i++)
            {
                string[] saDirPath = saDir[i].Split("\\");
                string dirName = saDirPath[saDirPath.Length - 1];
                //sb.Append($"dirName:{dirName}");
                if (dirName.Equals("NcfDocs"))
                {
                    lstCMD.Add($"cd wwwroot");
                    sb.Append($"dirName----{dirName}         ");
                    string strNewName = $"NcfDocs-Old{DateTime.Now.ToString("yyyyMMddHHmmss")}{random.Next(1000, 9999).ToString()}";
                    lstCMD.Add($"REN NcfDocs {strNewName}");
                    continue;
                }
                else if (dirName.Contains("NcfDocs-Old"))
                {
                    lstCMD.Add($"RD /S /Q {dirName}");
                    continue;
                }
                else
                {
                    continue;
                }
                //lstCMD.Add($"RD /S /Q {destPath}");
            }
            string strExecRes = CmdHelper.ExeCommand(lstCMD.ToArray());
            sb.Append(strExecRes);
            return sb;
        }

    }
}
