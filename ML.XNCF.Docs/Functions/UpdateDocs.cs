using LibGit2Sharp;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.HttpUtility;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Functions;
using System;
using System.Collections.Generic;
using System.IO;

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
                       });
                   }
                   catch (Exception)
                   {

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
    }
}
