using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ML.Core;

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
                    //Directory.Delete(copyDir, true);
                    List<string> lstCMD = new List<string>();
                    lstCMD.Add("TASKKILL /F /IM iisexpresstray.exe /T");
                    lstCMD.Add("TASKKILL /F /IM iisexpress.exe /T");

                    //cmdHelper.ExeCommand($"TASKKILL /F /IM iisexpresstray.exe /T");
                    //cmdHelper.ExeCommand($"TASKKILL /F /IM iisexpress.exe /T");
                    Random random = new Random();
                    sb.Append($"wwwrootDir：{wwwrootDir}\r\n");
                    sb.Append($"copyDir：{copyDir}\r\n");
                    string[] saDir = DirFileHelper.GetDirectories(wwwrootDir);
                    sb.Append($"saDir:{saDir.ToJson()}");
                    for(int i = 0; i < saDir.Length; i++)
                    {
                        string[] saDirPath = saDir[i].Split("\\");
                        string dirName = saDirPath[saDirPath.Length - 1];
                        //sb.Append($"dirName:{dirName}");
                        if (dirName.Equals("NcfDocs"))
                        {
                            lstCMD.Add($"cd {wwwrootDir}");
                            sb.Append($"dirName----{dirName}         ");
                            string strNewName = $"NcfDocs-Old{DateTime.Now.ToString("yyyyMMddHHmmss")}{random.Next(1000, 9999).ToString()}";
                            lstCMD.Add($"REN NcfDocs {strNewName}");
                        }
                        else if (dirName.Contains("NcfDocs-Old"))
                        {
                            lstCMD.Add($"RD /S /Q {Path.Combine(wwwrootDir, dirName)}");
                            
                        }
                        else
                        {
                            continue;
                        }
                        lstCMD.Add($"RD /S /Q {copyDir}");
                    }
                    string strExecRes = CmdHelper.ExeCommand(lstCMD.ToArray());
                    //sb.Append(strExecRes);
                }
                catch (Exception)
                {
                    sb.AppendLine("清理失败");
                }
                //sb.AppendLine($"清理目录 {copyDir}");

                result.Message = $"清理成功，清理时间：{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            });
        }
    }
}
