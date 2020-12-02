using System;
using System.Diagnostics;

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


}
