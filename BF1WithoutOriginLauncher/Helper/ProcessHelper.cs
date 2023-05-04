using System;
using System.IO;
using System.Diagnostics;

namespace BF1WithoutOriginLauncher.Helper
{
    public static class ProcessHelper
    {
        /// <summary>
        /// 判断程序是否运行
        /// </summary>
        /// <param name="appName">程序名称</param>
        /// <returns>正在运行返回true，未运行返回false</returns>
        public static bool IsAppRun(string appName)
        {
            return Process.GetProcessesByName(appName).Length > 0;
        }

        /// <summary>
        /// 打开http链接或者文件夹路径
        /// </summary>
        /// <param name="url"></param>
        public static void OpenLink(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                MsgBoxHelper.Exception(ex);
            }
        }

        /// <summary>
        /// 打开指定进程，可以附带运行参数
        /// </summary>
        /// <param name="path">目标路径</param>
        public static void OpenProcess(string path, string args = "")
        {
            try
            {
                if (!File.Exists(path))
                {
                    MsgBoxHelper.Warning($"目标文件路径不存在\n{path}");
                    return;
                }

                Process.Start(path, args);
            }
            catch (Exception ex)
            {
                MsgBoxHelper.Exception(ex);
            }
        }

        /// <summary>
        /// 根据进程名字关闭指定程序
        /// </summary>
        /// <param name="processName">程序名字，不需要加.exe</param>
        public static void CloseProcess(string processName)
        {
            var processArry = Process.GetProcesses();
            foreach (var process in processArry)
            {
                if (process.ProcessName.Equals(processName))
                    process.Kill();
            }
        }

        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCMD(string cmd)
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/k" + cmd;
            process.Start();
        }
    }
}
