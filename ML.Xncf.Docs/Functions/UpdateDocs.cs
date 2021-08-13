﻿using LibGit2Sharp;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.XncfBase;
using Senparc.Ncf.XncfBase.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using ML.Core;

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
            public List<string> StableBranch { get; set; }
            public DateTime UpdateTime { get; set; }
            public string WhatsNew { get; set; }
            public List<UpdateDocs_Version_Log> UpdateLogs{ get; set; }
        }

        public class UpdateDocs_Version_Log
        {
            public string Version { get; set; }
            public string Note { get; set; }
        }

        //注意：Name 必须在单个 Xncf 模块中唯一！
        public override string Name => "更新文档";

        public override string Description => "从 GitHub 上更新最新的官方文档。官方地址：https://gitee.com/NeuCharFramework/NcfDocs";
        public override Type FunctionParameterType => typeof(UpdateDocs_Parameters);

        public UpdateDocs(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected void CopyFolder(string sourcePath, string distPath, Dictionary<string,string> dicExclude)
        {
            DirFileHelper.CopyFolder(sourcePath, distPath, dicExclude);
        }

        protected void CloneRepository(string requestUrl,string distFolder,List<string> lstBranchName)
        {
            //克隆源
            for(int i = 0; i < lstBranchName.Count; i++)
            {
                Repository.Clone(requestUrl, $"{distFolder}/branch/{lstBranchName[i]}", new CloneOptions()
                {
                    IsBare = false,
                    Checkout = true,
                    BranchName = lstBranchName[i]
                });
            }
            //定义排除文件及文件夹
            Dictionary<string, string> dicExclude = new Dictionary<string, string>();
            dicExclude.Add("FOLDER", ".git");
            //复制
            for (int j = 0; j < lstBranchName.Count; j++)
            {
                CopyFolder($"{distFolder}/branch/{lstBranchName[j]}", $"{distFolder}/{lstBranchName[j]}", dicExclude);
            }
        }

        protected List<string> CloneMasterRepositoryVersion(string requestUrl, string distFolder)
        {
            Repository.Clone(requestUrl, $"{distFolder}/branch/master", new CloneOptions()
            {
                IsBare = false,
                Checkout = true,
                BranchName = "master"
            });

            UpdateDocs_Version versionData = null;
            var versionFile = Path.Combine($"{distFolder}/branch/master", "version.json");
            using (var fs = new FileStream(versionFile, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var versionJson = sr.ReadToEnd();
                    versionData = versionJson.GetObject<UpdateDocs_Version>();
                }
            }
            return versionData.StableBranch;
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
                       List<string> lstBranchName = new List<string>();
                       lstBranchName = CloneMasterRepositoryVersion(gitUrl, copyDir);
                       //SenparcTrace.Log(lstBranchName.ToJson());
                       //lstBranchName.Add("release-0.3");
                       //lstBranchName.Add("v1.0");
                       //lstBranchName.Add("v2.0");
                       CloneRepository(gitUrl, copyDir, lstBranchName);
                   }
                   catch (Exception)
                   {
                       try
                       {
                           var mergeResult = LibGit2Sharp.Commands.Pull(
                                                new Repository(copyDir),
                                                new Signature("zhao365845726@163.com", "zhao365845726@163.com", SystemTime.Now),
                                                new PullOptions());

                           sb.AppendLine("已有文件存在，开始 pull 更新");
                           sb.AppendLine(mergeResult.Status.ToString());
                       }
                       catch (Exception ex)
                       {
                           SenparcTrace.BaseExceptionLog(ex);
                       }

                   }

                   sb.AppendLine($"仓库创建于 {copyDir}");

                   UpdateDocs_Version versionData = null;
                   var versionFile = Path.Combine($"{copyDir}/branch/master", "version.json");
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

        ///// <summary>
        ///// 清理
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public FunctionResult Clear(IFunctionParameter param)
        //{
        //    /* 这里是处理文字选项（单选）的一个示例 */
        //    return FunctionHelper.RunFunction<UpdateDocs_Parameters>(param, (typeParam, sb, result) =>
        //    {
        //        var wwwrootDir = Path.Combine(Senparc.CO2NET.Config.RootDictionaryPath, "wwwroot");
        //        var copyDir = Path.Combine(wwwrootDir, "NcfDocs");
        //        try
        //        {
        //            cmdHelper.ExeCommand($"TASKKILL /F /IM node.exe /T");
        //            cmdHelper.ExeCommand($"RD /s /q {copyDir}");
        //            //清理目录
        //            Directory.Delete(copyDir, true);
        //        }
        //        catch (Exception)
        //        {
        //            sb.AppendLine("清理失败");
        //        }
        //        sb.AppendLine($"清理目录 {copyDir}");

        //        result.Message = $"清理成功，清理时间：{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
        //    });
        //}


    }
}
