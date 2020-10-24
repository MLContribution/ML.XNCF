using LibGit2Sharp;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.HttpUtility;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using Senparc.CO2NET;
using System.Diagnostics;

namespace ML.Xncf.Docs.Functions
{

    public class UpdateDocs : FunctionBase
    {
        public class UpdateDocs_Parameters : IFunctionParameter
        {

        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public class UpdateDocs_Version
        {
            public string Version { get; set; }
            public DateTime UpdateTime { get; set; }
            public string WhatsNew { get; set; }
        }

        //注意：Name 必须在单个 Xncf 模块中唯一！
        public override string Name => "更新文档";

        public override string Description => "从 GitHub 上更新最新的官方文档。官方地址：https://gitee.com/NeuCharFramework/NcfDocs";
        public override Type FunctionParameterType => typeof(UpdateDocs_Parameters);

        public UpdateDocs(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override FunctionResult Run(IFunctionParameter param)
        {
            /* 这里是处理文字选项（单选）的一个示例 */
            return FunctionHelper.RunFunction<UpdateDocs_Parameters>(param, (typeParam, sb, result) =>
               {
                   var wwwrootDir = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot");
                   var copyDir = Path.Combine(wwwrootDir, "NcfDocs");

                   //创建目录
                   FileHelper.TryCreateDirectory(wwwrootDir);
                   FileHelper.TryCreateDirectory(copyDir);

                   var gitUrl = "https://gitee.com/NeuCharFramework/NcfDocs";
                   try
                   {
                       Repository.Clone(gitUrl, copyDir, new CloneOptions()
                       {
                           IsBare = false,
                           RecurseSubmodules = true
                       });
                   }
                   catch (Exception)
                   {
                       if (Directory.Exists(copyDir))
                       {
                           string strClearDirCommand = $"rmdir /s /q {copyDir}";
                           string strExecRes = ExeCommand($"{strClearDirCommand}");
                           result.Message = $"清理完成,请再次执行更新";
                           return;
                       }

                       var mergeResult = LibGit2Sharp.Commands.Pull(
                                                 new Repository(copyDir),
                                                 new Signature("zhao365845726@163.com", "zhao365845726@163.com", SystemTime.Now),
                                                 new PullOptions());

                       sb.AppendLine("已有文件存在，开始 pull 更新");
                       sb.AppendLine(mergeResult.Status.ToString());
                   }


                   sb.AppendLine($"仓库创建于 {copyDir}");

                   UpdateDocs_Version versionData = null;
                   var versionFile = Path.Combine(copyDir, "version.json");
                   using (var fs = new FileStream(versionFile, FileMode.Open))
                   {
                       using (var sr = new StreamReader(fs))
                       {
                           var versionJson = sr.ReadToEnd();
                           versionData = versionJson.GetObject<UpdateDocs_Version>();
                       }
                   }

                   result.Message = $"更新成功，当前版本：{versionData.Version}，更新时间：{versionData.UpdateTime.ToShortDateString()}，What's New：{versionData.WhatsNew ?? "无"}";
               });
        }

        /// <summary>
        /// 清理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public FunctionResult Clear(IFunctionParameter param)
        {
            /* 这里是处理文字选项（单选）的一个示例 */
            return FunctionHelper.RunFunction<UpdateDocs_Parameters>(param, (typeParam, sb, result) =>
            {
                var wwwrootDir = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot");
                var copyDir = Path.Combine(wwwrootDir, "NcfDocs");
                try
                {
                    //清理目录
                    Directory.Delete(copyDir, true);
                    if (Directory.Exists(copyDir))
                    {
                        string strClearDirCommand = $"rmdir /s /q {copyDir}";
                        string strExecRes = ExeCommand($"{strClearDirCommand}");
                    }
                }
                catch (Exception)
                {
                    sb.AppendLine("清理失败");
                }
                sb.AppendLine($"清理目录 {copyDir}");

                result.Message = $"清理成功，清理时间：{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
            });
        }

        /// <summary>
        /// 执行cmd.exe命令
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <returns>命令输出文本</returns>
        private string ExeCommand(string commandText)
        {
            return ExeCommand(new string[] { commandText });
        }

        /// <summary>
        /// 执行多条cmd.exe命令
        /// </summary>
        /// <param name="commandTexts">命令文本数组</param>
        /// <returns>命令输出文本</returns>
        private string ExeCommand(string[] commandTexts)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                foreach (string item in commandTexts)
                {
                    p.StandardInput.WriteLine(item);
                }
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                //strOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(strOutput));
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
    }
}
