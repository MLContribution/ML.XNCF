using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Xncf.Docs.Domain.Models
{
    /// <summary>
    /// 更新Docs版本日志
    /// </summary>
    public class UpdateDocsVersionLog
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Note { get; set; }
    }
}
