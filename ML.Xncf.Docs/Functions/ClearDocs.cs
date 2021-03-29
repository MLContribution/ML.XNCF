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
                    //执行Dos命令
                    sb = ExecDosCommand(sb,wwwrootDir, copyDir);
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

        private StringBuilder ExecDosCommand(StringBuilder sb,string rootPath,string destPath)
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

        private StringBuilder WriteBatchFile(StringBuilder sb, string rootPath, string destPath)
        {
            string strFile = $"{rootPath}\\ClearDoc.bat";
            if (!File.Exists(strFile))
            {
                FileStream mapFile = File.Open(strFile, FileMode.Append);
                StringBuilder sbBody = new StringBuilder();

                List<string> lstCMD = new List<string>();
                Random random = new Random();
                string[] saDir = DirFileHelper.GetDirectories(rootPath);
                for (int i = 0; i < saDir.Length; i++)
                {
                    string[] saDirPath = saDir[i].Split("\\");
                    string dirName = saDirPath[saDirPath.Length - 1];
                    if (dirName.Equals("NcfDocs"))
                    {
                        sbBody.AppendLine($"cd {rootPath}");
                        lstCMD.Add($"cd {rootPath}");
                        string strNewName = $"{rootPath}\\NcfDocs-Old{DateTime.Now.ToString("yyyyMMddHHmmss")}{random.Next(1000, 9999).ToString()}";
                        sbBody.AppendLine($"REN {rootPath}\\NcfDocs {strNewName}");
                        lstCMD.Add($"REN {rootPath}\\NcfDocs {strNewName}");
                        continue;
                    }
                    else if (dirName.Contains("NcfDocs-Old"))
                    {
                        sbBody.AppendLine($"RD /S /Q {Path.Combine(rootPath, dirName)}");
                        lstCMD.Add($"RD /S /Q {Path.Combine(rootPath, dirName)}");
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                    //sbBody.AppendLine($"RD /S /Q {destPath}");
                    //lstCMD.Add($"RD /S /Q {destPath}");
                }
                //string strExecRes = CmdHelper.ExeCommand(lstCMD.ToArray());
                sbBody.AppendLine($"DEL /S /Q {rootPath}\\ClearDoc.bat");
                //sbBody.Append($"new AreaPageMenuItem(GetAreaUrl($\"/Admin/{strFileName}/Index\"),\"用户管理\",\"fa fa-bookmark-o\"),\r\n");
                byte[] cMapFile = Encoding.UTF8.GetBytes(sbBody.ToString());
                mapFile.Write(cMapFile, 0, cMapFile.Length);
                mapFile.Close();
            }
            Thread.Sleep(3000);
            CmdHelper.StartAppointApp($"{rootPath}\\ClearDoc.bat");
            sb.AppendLine($"{rootPath}\\ClearDoc.bat");
            return sb;
        }
    }
}
