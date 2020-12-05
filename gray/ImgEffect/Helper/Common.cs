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
    }
}
