using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Maintain
{
    public class CommandLine
    {
        public static Action<string> CmdLog { get; set; }
        public static Process P { get; set; }
        public static void CmdBatch()
        {
            P = new Process();
            // 设置要启动的程序
            P.StartInfo.FileName = "cmd.exe";
            //管理员模式
            P.StartInfo.Verb = "RunAs";
            //工作路径
            //P.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 设置启动为当前项目的子线层
            P.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            P.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            P.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            P.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            P.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            // 为异步获取订阅事件 
            P.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                CmdLog?.Invoke(e.Data);
            });
            // 启动
            P.Start();
            // 异步获取命令行内容  
            P.BeginOutputReadLine();
            P.StandardInput.AutoFlush = true;
        }

        public static void CmdDbBat(string path)
        {
            Process P = new Process();
            // 设置要启动的程序
            P.StartInfo.FileName = path;
            //管理员模式
            P.StartInfo.Verb = "RunAs";
            //工作路径
            //P.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 设置启动为当前项目的子线层
            P.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            P.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            P.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            P.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            P.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            // 为异步获取订阅事件 
            P.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data != null)
                {
                    var NetString = e.Data.Replace("jjwf1234,", "********");
                    CmdLog?.Invoke(NetString);
                }
            });
            // 启动
            P.Start();
            // 异步获取命令行内容  
            P.BeginOutputReadLine();
            P.StandardInput.AutoFlush = true;
        }

        public static void CmdBat(string path)
        {
            Process P = new Process();
            // 设置要启动的程序
            P.StartInfo.FileName = path;
            //管理员模式
            P.StartInfo.Verb = "RunAs";
            //工作路径
            //P.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 设置启动为当前项目的子线层
            P.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            P.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            P.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            P.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            P.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            // 为异步获取订阅事件 
            P.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                    CmdLog?.Invoke(e.Data);
            });
            // 启动
            P.Start();
            // 异步获取命令行内容  
            P.BeginOutputReadLine();
            P.StandardInput.AutoFlush = true;
        }
    }
}
