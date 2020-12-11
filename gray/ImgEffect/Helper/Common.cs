using System;

namespace Gray
{
    //全局变量类型
    class Common
    {
        //线程锁
        public static object Lock = new object();


        /// <summary>
        /// 边界调整
        /// </summary>
        /// <param name="n"></param>
        /// <param name="lBorder"></param>
        /// <param name="uborder"></param>
        /// <returns></returns>
        public static int BorderAdjust(int n, int lBorder, int uborder)
        {
            if (n < lBorder)
                return 2 * lBorder - n;
            if (n > uborder)
                return 2 * uborder - n;
            return n;
        }
        public static double Max(double t1, double t2)
        {
            return t1 > t2 ? t1 : t2;
        }
        public static double Min(double t1, double t2)
        {
            return t1 < t2 ? t1 : t2;
        }
        public static double Diff(double t1, double t2)
        {
            return Math.Abs(t1 - t2);
        }
    }
}
