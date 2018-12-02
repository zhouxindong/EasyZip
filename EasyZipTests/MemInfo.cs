using System.Diagnostics;
using System.Globalization;

namespace EasyZipTests
{
    public class MemInfo
    {
        /// <summary>
        /// 获取进程内存
        /// </summary>
        /// <returns></returns>
        public static string GetCurProcessMem()
        {
            var ps = Process.GetCurrentProcess();
            var pf1 = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);   //第二个参数就是得到只有工作集

            return (pf1.NextValue() / 1024 / 1024).ToString(CultureInfo.InvariantCulture);
        }
    }
}