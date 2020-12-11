using Gray.ImgEffect.Helper;
using System;

namespace Gray.ImgEffect
{
    class DSCMSelector
    {
        /// <summary>
        /// 储存选中的原图像变形区域
        /// </summary>
        public readonly int[][] Origin;
        /// <summary>
        /// 储存选中的变形图像的变形区域
        /// </summary>
        public readonly int[][] Deformation;
        /// <summary>
        /// 计算相关系数的尺寸
        /// </summary>
        public readonly Size SelectSize;
        /// <summary>
        /// 选中区域的起始点
        /// </summary>
        public readonly Point SelectPoint;
        /// <summary>
        /// 保存搜索到的子区域对
        /// </summary>
        public FeaturePair[] FeaturePairs;
        public DSCMSelector(int[][] origin, int[][] deformation, Point selectPoint, Size size)
        {
            this.Origin = origin;
            this.Deformation = deformation;
            this.SelectSize = size;
            this.SelectPoint = selectPoint;
        }
        /// <summary>
        /// 图像插值,二次函数插值,
        /// </summary>
        /// <param name="img">原始图像</param>
        /// <param name="step">两像素间的插值个数</param>
        /// <returns>插值后的图像</returns>
        public int[][] InterPolation(int[][] img, int step)
        {
            int Multiple = step + 1;
            int width = img[0].Length, height = img.Length;
            int newWidth = width * Multiple - step, newHeight = height * Multiple - step;
            int[][] newImg = new int[newHeight][];
            for(int i = 0; i < height - 1; i++)
            {
                for(int j = 0; j < width - 1; j++)
                {

                }
            }


            return null;
        }






        /// <summary>
        /// 递归计算给定点的相关系数峰值位置
        /// </summary>
        /// <param name="origin">原始子区</param>
        /// <param name="scale">搜索范围</param>
        /// <param name="OX">子区初始相对点位</param>
        /// <param name="OY">子区初始绝对点位</param>
        /// <param name="OAP">子区初始绝对点位</param>
        /// <param name="DAP">子区初始绝对点位</param>
        /// <returns></returns>
        public FeaturePair FeaturePairFind(int[][] origin, int[][] scale, int OX, int OY, FPoint OAP, FPoint DAP)
        {
            int width = origin[0].Length, height = origin.Length;
            int SW = scale[0].Length, SH = scale.Length;
            if (width >= SW || height >= SH)
                throw new RankException("所查找的子区域大于查找范围!");
            double UC = 0, UU = 0, UD = 0, UL = 0, UR = 0;
            UC = CalCorration(origin, GetGrayMatrix(scale, OX, OY, width, height));
            if (OX > 0)
                UL = CalCorration(origin, GetGrayMatrix(scale, OX - 1, OY, width, height));
            if (OY > 0)
                UU = CalCorration(origin, GetGrayMatrix(scale, OX, OY - 1, width, height));
            if (OX + width < SW - 1)
                UR = CalCorration(origin, GetGrayMatrix(scale, OX + 1, OY, width, height));
            if (OY + height < SH - 1)
                UD = CalCorration(origin, GetGrayMatrix(scale, OX, OY + 1, width, height));
            if (UC < UL)
            {
                DAP.MoveLeft();
                return FeaturePairFind(origin, scale, OX - 1, OY, OAP, DAP);
            }
            if (UC < UR)
            {
                DAP.MoveRight();
                return FeaturePairFind(origin, scale, OX + 1, OY, OAP, DAP);
            }
            if (UC < UU)
            {
                DAP.MoveTop();
                return FeaturePairFind(origin, scale, OX, OY - 1, OAP, DAP);
            }
            if (UC < UD)
            {
                DAP.MoveDown();
                return FeaturePairFind(origin, scale, OX, OY + 1, OAP, DAP);
            }
            return new FeaturePair(OAP, DAP, new Size(width, height));
        }
        public int[][] GetGrayMatrix(int[][] scale, int X, int Y, int width, int height)
        {
            int[][] matrix = new int[width][];
            int[] temp;
            int SW = scale[0].Length, SH = scale.Length;
            if (X + width >= SW || Y + height >= SH)
                throw new RankException("数组索引超出界限!");
            for (int i = 0; i < height; i++)
            {
                temp = new int[width];
                for (int j = 0; j < width; j++)
                {
                    temp[j] = scale[height + i][width + j];
                }
                matrix[i] = temp;
            }
            return matrix;
        }
        public double CalCorration(int[][] origin, int[][] defor)
        {
            if (origin.Length != defor.Length)
                throw new RankException("待搜索的方块大小不一致!");
            int width = origin[0].Length, height = origin.Length;
            int[] originA = new int[width * height];
            int[] deforA = new int[width * height];
            int position = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    originA[position] = origin[i][j];
                    deforA[position] = defor[i][j];
                    position++;
                }
            }
            return MathHelper.CorrelationCoefficient(originA, deforA);
        }
    }
    class FeaturePair
    {
        /// <summary>
        /// 原图像对应的特征位置
        /// </summary>
        public FPoint OP;
        /// <summary>
        /// 变形图像对应的特征位置
        /// </summary>
        public FPoint DP;
        /// <summary>
        /// 子区域尺寸
        /// </summary>
        public Size CSize;
        public FeaturePair(FPoint op, FPoint dp, Size cSize)
        {
            this.OP = op;
            this.DP = dp;
            this.CSize = cSize;
        }
    }
}