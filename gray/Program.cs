using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gray
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();//弹出控制台
            Shell.WriteLine(">>> 程序已启动!");
            Shell.WriteLine(Shell.PreDefineSearchRAM());
            Shell.WriteLine(Shell.PreDefineSearchCPU());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
