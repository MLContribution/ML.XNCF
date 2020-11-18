using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ML.Xncf.Docs.Util
{
    public class CMD
    {
        /// <summary>
        /// 执行cmd.exe命令
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <returns>命令输出文本</returns>
        public string ExeCommand(string commandText)
        {
            return ExeCommand(new string[] { commandText });
        }

        /// <summary>
        /// 执行多条cmd.exe命令
        /// </summary>
        /// <param name="commandTexts">命令文本数组</param>
        /// <returns>命令输出文本</returns>
        public string ExeCommand(string[] commandTexts)
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
