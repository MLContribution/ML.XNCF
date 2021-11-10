using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Domain.Models
{
    /// <summary>
    /// 更新Docs版本实体
    /// </summary>
    public class UpdateDocsVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 默认分支
        /// </summary>
        public string DefaultBranck { get; set; }
        /// <summary>
        /// 稳定的分支集合
        /// </summary>
        public List<string> StableBranch { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 最新的信息
        /// </summary>
        public string WhatsNew { get; set; }
        /// <summary>
        /// 更新日志
        /// </summary>
        public List<UpdateDocsVersionLog> UpdateLogs { get; set; }
    }
}
