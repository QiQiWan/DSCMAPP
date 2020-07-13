using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Gray
{
    public class Shell
    {
        /// <summary>
        /// 接受所有类型的输出
        /// </summary>
        /// <param name="a"></param>
        static public void WriteLine(object a)
        {
            WriteLine(a.ToString());
        }
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="strFormat"></param>
        /// <param name="ps"></param>
        static public void WriteLine(string strFormat, params string[] ps)
        {
            WriteLine(String.Format(strFormat, ps));
        }

        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLine(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now, output);
        }

        /// <summary>  
        /// 根据输出文本选择控制台文字颜色  
        /// </summary>  
        /// <param name="output"></param>  
        /// <returns></returns>  
        private static ConsoleColor GetConsoleColor(string output)
        {
            if (output.StartsWith(">>>")) return ConsoleColor.White;//Normal
            if (output.StartsWith("###")) return ConsoleColor.Red;//ERROR
            if (output.StartsWith("$$$")) return ConsoleColor.Green;//Warning
            return ConsoleColor.Gray;
        }
        public static double GetSystemInfo(SYSTEMTYPE.Resource info)
        {
            double output = 0;
            Process CurrentProcess = Process.GetCurrentProcess();
            if(info == SYSTEMTYPE.Resource.CPU)
                output = GetCPUEffect(CurrentProcess);
            if (info == SYSTEMTYPE.Resource.RAM)
                output = GetRAMEffect(CurrentProcess);
            CurrentProcess.Dispose();
            return output;
        }
        /// <summary>
        /// 获取处理器使用率
        /// </summary>
        /// <param name="CurrentProcess"></param>
        /// <returns></returns>
        private static TimeSpan CPUEffect = TimeSpan.Zero;
        private static double GetCPUEffect(Process CurrentProcess)
        {
            double effect = (CurrentProcess.TotalProcessorTime - CPUEffect).TotalMilliseconds / 1000 / Environment.ProcessorCount * 100;//CPU
            CPUEffect = CurrentProcess.TotalProcessorTime;
            return effect;
        }
        /// <summary>
        /// 获取工作内存
        /// </summary>
        /// <param name="CurrentProcess"></param>
        /// <returns></returns>
        private static double GetRAMEffect(Process CurrentProcess)
        {
            return CurrentProcess.WorkingSet64 / 1024;
        }
        public static string PreDefineSearchCPU() => ">>> CPU使用率: " + Math.Round(Shell.GetSystemInfo(SYSTEMTYPE.Resource.CPU), 1) + " %";
        public static string PreDefineSearchRAM() => ">>> 工作内存: " + Math.Round(Shell.GetSystemInfo(SYSTEMTYPE.Resource.RAM) / 1024) + " KB";
    }
    public enum PrintType { Normal, Warning, ERROR};
    public class SYSTEMTYPE
    {
        public enum Resource { CPU, RAM };
    }

    public class StatusBar
    {
        public static void SetRAMStatus(ToolStripStatusLabel toolStrip)
        {
            toolStrip.Text = "工作内存: " + Math.Round(Shell.GetSystemInfo(SYSTEMTYPE.Resource.RAM) / 1024) + " KB";
        }
        public static void SetCPUStatus(ToolStripStatusLabel toolStrip)
        {
            toolStrip.Text = "CPU效能: " + Math.Round(Shell.GetSystemInfo(SYSTEMTYPE.Resource.CPU), 1) + " %";
        }

        public static void SetGrayLevel(ToolStripStatusLabel toolStrip, int graying)
        {
            toolStrip.Text = "平均灰度: " + graying.ToString();
        }

        public static void ChangeStatus(ToolStripStatusLabel toolStrip)
        {
            toolStrip.Text = toolStrip.Text == "分析完成!" ? "正在分析..." : "分析完成!";
        }

        public static void ChangeStatus(ToolStripStatusLabel toolStrip, StatusMode mode)
        {
            switch (mode)
            {
                case StatusMode.doing:
                    toolStrip.Text = "正在分析...";
                    break;
                case StatusMode.done:
                    toolStrip.Text = "分析完成!";
                    break;
            }
        }

        //多线程中执行委托
        public delegate void ChangeStatusDelegate(ToolStripStatusLabel toolStrip);
        public delegate void UpDateProcessDelegate(ToolStripProgressBar progressBar, int value);
        public delegate void SetTickTimeDelegate(ToolStripStatusLabel toolStrip, double mill);

        public static void SetTickTime(ToolStripStatusLabel toolStrip, double mill)
        {
            Shell.WriteLine($">>> 已用时: {mill} ms");
            toolStrip.Text = "已用时间: " + Math.Round(mill, 1) + "ms";
        }
        public static void UpDateProcess(ToolStripProgressBar progressBar, int value)
        {
            progressBar.Value = value;
        }

        public enum StatusMode {doing, done};
    }
}
