using LibGit2Sharp;
using ML.Core;
using ML.Xncf.Docs.Domain.Models;
using ML.Xncf.Docs.OHS.PL;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.AppServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ML.Xncf.Docs.OHS.Local.AppService
{
    public class UpdateDocsAppService : AppServiceBase
    {
        public UpdateDocsAppService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected void CopyFolder(string sourcePath, string distPath, Dictionary<string, string> dicExclude)
        {
            DirFileHelper.CopyFolder(sourcePath, distPath, dicExclude);
        }

        protected void CloneRepository(string requestUrl, string distFolder, List<string> lstBranchName)
        {
            //克隆源
            for (int i = 0; i < lstBranchName.Count; i++)
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

            UpdateDocsVersion versionData = null;
            var versionFile = Path.Combine($"{distFolder}/branch/master", "version.json");
            using (var fs = new FileStream(versionFile, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var versionJson = sr.ReadToEnd();
                    versionData = versionJson.GetObject<UpdateDocsVersion>();
                }
            }
            return versionData.StableBranch;
        }

        protected List<string> CloneSpecifiedRepositoryVersion(string requestUrl, string distFolder, string branchName)
        {
            Repository.Clone(requestUrl, $"{distFolder}/branch/{branchName}", new CloneOptions()
            {
                IsBare = false,
                Checkout = true,
                BranchName = branchName
            });

            UpdateDocsVersion versionData = null;
            var versionFile = Path.Combine($"{distFolder}/branch/{branchName}", "version.json");
            using (var fs = new FileStream(versionFile, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var versionJson = sr.ReadToEnd();
                    versionData = versionJson.GetObject<UpdateDocsVersion>();
                }
            }
            return versionData.StableBranch;
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [FunctionRender("更新文档", "从 GitHub 上更新最新的官方文档。官方地址：https://gitee.com/NeuCharFramework/NcfDocs", typeof(Register))]
        public async Task<StringAppResponse> Run(Docs_RunRequest request)
        {
            StringBuilder sb = new StringBuilder();
            return await this.GetResponseAsync<StringAppResponse, string>(async (response, logger) =>
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
                    lstBranchName = CloneSpecifiedRepositoryVersion(gitUrl, copyDir, "Developer-zmz");
                    SenparcTrace.Log(lstBranchName.ToJson());
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

                UpdateDocsVersion versionData = null;
                var versionFile = Path.Combine($"{copyDir}/branch/Developer-zmz", "version.json");
                using (var fs = new FileStream(versionFile, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        var versionJson = sr.ReadToEnd();
                        versionData = versionJson.GetObject<UpdateDocsVersion>();
                    }
                }

                response.Data = $"更新成功，当前版本：{versionData.Version}，更新时间：{versionData.UpdateTime.ToShortDateString()}，What's New：{versionData.WhatsNew ?? "无"}";

                logger.Append(response.Data);
                return null;
            }, saveLogAfterFinished: true);
        }
    }
}
