using System;
using System.Drawing;
using MyTools;
using MyTools.OpenProperties;

namespace Gray
{
    class SpeckleSearch
    {
        // 保存分析图象灰度矩阵
        private MyMatrix myMatrix;

        // 保存散斑列表
        private Speckle[] speckles;

        public SpeckleSearch(MyMatrix myMatrix)
        {
            this.myMatrix = myMatrix;
        }

        public void GetSpeckles()
        {

        }

    }
    class Speckle
    {
        // 左上角点坐标
        public readonly Point LeftTop;
        // 右下角点坐标
        public readonly Point RightBottom;

        public readonly int Width;
        public readonly int Height;

        public int size;
        public MyMatrix myMatrix;

        /// <summary>
        /// 判断点是否在图片内
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="bp"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private bool CheckPoint(Point ap, Point bp, Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            if (!(NumAndNum.IsIn<int>(ap.X, 0, width) && NumAndNum.IsIn<int>(bp.X, 0, width))) return false;
            if (!(NumAndNum.IsIn<int>(ap.Y, 0, height) && NumAndNum.IsIn<int>(bp.Y, 0, height))) return false;
            return true;
        }
        /// <summary>
        /// 判断左上和右下的点位位置
        /// </summary>
        /// <param name="leftTop"></param>
        /// <param name="rightBottom"></param>
        /// <returns></returns>
        private bool CheckPointPosition(Point leftTop, Point rightBottom)
        {
            if (leftTop.X >= rightBottom.X) return false;
            if (leftTop.Y >= rightBottom.Y) return false;
            return true;
        }

        public Speckle(Point leftTop, Point rightBottom, Bitmap bitmap)
        {
            if (!CheckPoint(leftTop, rightBottom, bitmap))
                throw new Exception("点不在图片内!");
            if (!CheckPointPosition(leftTop, rightBottom))
                throw new Exception("点位不对!");

            Width = rightBottom.X - leftTop.X;
            Height = rightBottom.Y - leftTop.Y;
            LeftTop = leftTop;
            RightBottom = rightBottom;

            double[][] temps = new double[Height + 1][];

            for (int i = LeftTop.Y; i <= RightBottom.Y; i++)
            {
                double[] temp = new double[Width + 1];
                int startX = LeftTop.X, startY = LeftTop.Y;
                for (int j = LeftTop.X; j <= RightBottom.X; j++)
                {
                    temp[j - startX] = bitmap.GetPixel(i, j).R;
                }
                temps[i - startY] = temp;
            }
            myMatrix = new MyMatrix(temps);
        }
        private Speckle GetSpeckleFromPoint(int leftTopX, int leftTopY, int rightBottomX, int rightBottomY, Bitmap bitmap)
        {
            Point leftTop = new Point(leftTopX, leftTopY);
            Point rightBottom = new Point(rightBottomX, rightBottomY);
            return new Speckle(leftTop, rightBottom, bitmap);
        }
    }
}
