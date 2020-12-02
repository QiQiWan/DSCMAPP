using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Gray
{
    /// <summary>
    /// 控制底部状态栏行为
    /// </summary>
    class StripBarControl
    {
        public static StatusBar.ChangeStatusDelegate DChangeStatus;
        public static StatusBar.UpDateProcessDelegate DUpDateProcess;
        public static StatusBar.SetTickTimeDelegate DSetTickTime;
        public static Stopwatch stopwatch = new Stopwatch();
        public static bool start = false;
        public static ToolStripStatusLabel DStatusBar;
        public static ToolStripStatusLabel TickTimeBar;
        public static ToolStripProgressBar ProgressBar;

        static StripBarControl()
        {
            DChangeStatus = Main.changeStatus;
            DUpDateProcess = Main.upDateProcess;
            DSetTickTime = Main.setTickTime;
        }
        public static void Start()
        {
            start = true;
            Shell.WriteLine("开始计算...");
            DChangeStatus(DStatusBar, StatusBar.StatusMode.doing);
            DUpDateProcess(ProgressBar, 0);
            stopwatch.Restart();
            stopwatch.Start();
        }
        public static void Start(string message)
        {
            start = true;
            Shell.WriteLine(message);
            DChangeStatus(DStatusBar, StatusBar.StatusMode.doing);
            DUpDateProcess(ProgressBar, 0);
            stopwatch.Restart();
            stopwatch.Start();
        }
        public static void UpdateProcess(int value)
        {
            if (start)
                DUpDateProcess(ProgressBar, value);
        }
        public static void Stop()
        {
            if (!start)
                return;
            stopwatch.Stop();
            DUpDateProcess(ProgressBar, 100);
            DChangeStatus(DStatusBar, StatusBar.StatusMode.done);
            DSetTickTime(TickTimeBar, stopwatch.ElapsedMilliseconds);
            Shell.WriteLine("计算完成!");
            Console.WriteLine();
            start = false;
        }
        public static void Stop(string message)
        {
            if (!start)
                return;
            stopwatch.Stop();
            DUpDateProcess(ProgressBar, 100);
            DChangeStatus(DStatusBar, StatusBar.StatusMode.done);
            DSetTickTime(TickTimeBar, stopwatch.ElapsedMilliseconds);
            Shell.WriteLine(message);
            Console.WriteLine();
            start = false;
        }
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
        public delegate void ChangeStatusDelegate(ToolStripStatusLabel toolStrip, StatusMode mode);
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

        public enum StatusMode { doing, done };
    }

}
