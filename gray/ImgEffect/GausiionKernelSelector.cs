﻿using System;
using System.Drawing;

namespace Gray
{
    public class GausiionKernelSelector
    {
        /// <summary>
        /// 滤波器的卷积核
        /// </summary>
        public readonly ConvolutionType Kernel;
        /// <summary>
        /// 单位化需要用到的矩阵元素和
        /// </summary>
        public readonly int Sum;

        /// <summary>
        /// 高斯卷积核
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="σ"></param>
        /// <returns></returns>
        public double GaussianKernel(int x, int y, double σ)
        {
            return 1 / (Math.PI * 2 * σ * σ) * Math.Exp(-(double)(x * x + y * y) / 2 / σ / σ);
        }

        private double[][] Ergodic(double[][] origin, double sum)
        {
            for (int i = 0; i < origin.Length; i++)
            {
                for (int j = 0; j < origin[0].Length; j++)
                {
                    origin[i][j] /= sum;
                }
            }
            return origin;
        }

        public GausiionKernelSelector(ConvolutionType convolutionType)
        {
            this.Kernel = convolutionType;
            this.Sum = Kernel.KernelSize;
        }
        /// <summary>
        /// 使用高斯卷积核对分块进行平滑,矩形平滑
        /// </summary>
        /// <returns></returns>
        public int GausiionSmoothing(int[][] bitmapM, int width, int height)
        {
            int sum = 0;
            int[][] kernel = Kernel.Kernel;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    sum += bitmapM[i][j] * kernel[i][j];
                }
            }
            return sum / Sum;
        }
        /// <summary>
        /// 使用高斯卷积核对分块进行平滑,矩形平滑,需要子方块
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maxIntager"></param>
        /// <returns></returns>
        public int GausiionSmoothing(int[][] bitmap, int x, int y, int width, int height, int maxIntager)
        {
            int sum = 0, Kwidth = Kernel.Width, Kheight = Kernel.Height;
            int Px, Py;
            for (int i = 0; i < Kheight; i++)
            {
                for (int j = 0; j < Kwidth; j++)
                {
                    Px = x - maxIntager + j;
                    Px = BorderAdjust(Px, 0, width - 1);
                    Py = y - maxIntager + i;
                    Py = BorderAdjust(Py, 0, height - 1);
                    sum += bitmap[Py][Px] * Kernel.Kernel[i][j];
                }
            }
            return sum / Sum;
        }
        /// <summary>
        /// 十字形高斯卷积平滑方法,其中x,y是十字中心位置,该方法需要传入分割好的小方块
        /// </summary>
        /// <param name="bitmapM"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int GausiionSmoothingCrossShape(int[][] bitmapM, int x, int y, int width, int height)
        {
            int sum = 0;
            int CrossSum = 0;
            int[][] kernel = Kernel.Kernel;
            for (int i = 0; i < height; i++)
            {
                if (i != y)
                {
                    sum += bitmapM[x][i] * kernel[x][i];
                    CrossSum += kernel[x][i];
                }
            }
            for (int j = 0; j < width; j++)
            {
                if (j != x)
                {
                    sum += bitmapM[j][y] * kernel[j][y];
                    CrossSum += kernel[j][y];
                }
            }
            sum += bitmapM[x][y] * kernel[x][y];
            CrossSum += kernel[x][y];
            return sum / CrossSum;
        }
        /// <summary>
        /// 十字形高斯卷积平滑方法,其中x,y是十字中心位置,该方法不需要传入分割好的小方块
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maxIntager"></param>
        /// <returns></returns>
        public int GausiionSmoothingCrossShape(int[][] bitmap, int x, int y, int width, int height, int maxIntager)
        {
            int sum = 0, CrossSum = 0;
            int[][] kernel = Kernel.Kernel;
            int Kwidth = Kernel.Width, Kheight = Kernel.Height;
            int Px, Py;
            for (int i = 0; i < Kheight; i++)
            {
                Py = y - maxIntager + i;
                Py = BorderAdjust(Py, 0, height - 1);
                if (Py != y)
                {
                    sum += bitmap[Py][x] * kernel[i][maxIntager];
                    CrossSum += kernel[i][maxIntager];
                }
            }
            for (int j = 0; j < Kwidth; j++)
            {
                Px = x - maxIntager + j;
                Px = BorderAdjust(Px, 0, width - 1);
                if (Px != x)
                {
                    sum += bitmap[y][Px] * kernel[maxIntager][j];
                    CrossSum += kernel[maxIntager][j];
                }
            }
            sum += bitmap[y][x] * kernel[maxIntager][maxIntager];
            CrossSum += kernel[maxIntager][maxIntager];
            return sum / CrossSum;
        }
        /// <summary>
        /// 边界调整
        /// </summary>
        /// <param name="n"></param>
        /// <param name="lBorder"></param>
        /// <param name="uborder"></param>
        /// <returns></returns>
        private int BorderAdjust(int n, int lBorder, int uborder)
        {
            if (n < lBorder)
                return 2 * lBorder - n;
            if (n > uborder)
                return 2 * uborder - n;
            return n;
        }
    }
    public delegate double CustomKernel(int x, int y, double σ);

    public class ConvolutionType
    {
        public readonly int[][] Kernel;
        public readonly int KernelSize;
        public readonly Point CenterPoint;
        public readonly int Width;
        public readonly int Height;
        // 自定义核函数
        public readonly CustomKernel CustomKernelFunc;
        public readonly double σ;
        static public readonly int Multiple = 1000;
        public ConvolutionType(int[][] kernel, int x, int y, double σ)
        {
            this.Height = kernel.Length;
            this.Width = kernel[0].Length;
            this.Kernel = kernel;
            this.CenterPoint = new Point(x, y);
            this.σ = σ;
            KernelSize = SumKernel();
        }
        public ConvolutionType(int width, int height, int x, int y, double σ)
        {
            this.Width = width;
            this.Height = height;
            this.CenterPoint = new Point(x, y);
            this.σ = σ;
            // 委托正态分布函数为卷积函数
            this.CustomKernelFunc = new CustomKernel(SNormalDistribution);
            this.Kernel = CalculateKernel(width, height, out this.KernelSize);
        }
        public ConvolutionType(int width, int height, Point centerPoint, double σ)
        {
            this.Kernel = ImageAnalyse.InitMatrix<int>(width, height);
            this.Width = width;
            this.Height = height;
            this.CenterPoint = centerPoint;
            this.σ = σ;
            // 委托正态分布函数为卷积函数
            this.CustomKernelFunc = new CustomKernel(SNormalDistribution);
            this.Kernel = CalculateKernel(width, height, out this.KernelSize);
        }
        public ConvolutionType(int[][] kernel, Point centerPoint, double σ)
        {
            this.Height = kernel.Length;
            this.Width = kernel[0].Length;
            this.Kernel = kernel;
            this.CenterPoint = centerPoint;
            KernelSize = SumKernel();
            this.σ = σ;
        }
        public ConvolutionType(int width, int height, Point centerPoint, double σ, CustomKernel customKernel)
        {
            this.Kernel = ImageAnalyse.InitMatrix<int>(width, height);
            this.Width = width;
            this.Height = height;
            this.CenterPoint = centerPoint;
            this.σ = σ;
            this.CustomKernelFunc = customKernel;
            this.Kernel = CalculateKernel(width, height, out this.KernelSize);
        }
        /// <summary>
        /// 已知核函数对和函数求和
        /// </summary>
        /// <returns></returns>
        private int SumKernel()
        {
            int Sum = 0;
            foreach (var item in Kernel)
                foreach (var ele in item)
                    Sum += ele;
            return Sum;
        }
        /// <summary>
        /// 未给定核函数时,按照预定卷积函数计算核函数模板Kernel
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="Sum"></param>
        /// <returns></returns>
        public int[][] CalculateKernel(int width, int height, out int Sum)
        {
            Point relaventP;
            Sum = 0;
            int[][] kernel = new int[height][];
            double temp;
            for (int i = 0; i < height; i++)
            {
                int[] row = new int[width];
                for (int j = 0; j < width; j++)
                {
                    relaventP = new Point(i - CenterPoint.X, j - CenterPoint.Y);
                    temp = CustomKernelFunc(relaventP.X, relaventP.Y, σ) * Multiple;
                    row[j] = (int)temp;
                    Sum += row[j];
                }
                kernel[i] = row;
            }
            return kernel;
        }
        /// <summary>
        /// 标准正态分布
        /// </summary>
        /// <param name="center"></param>
        /// <param name="current"></param>
        /// <param name="σ"></param>
        /// <returns></returns>
        static public double SNormalDistribution(int x, int y, double σ)
        {
            return Math.Exp(-1.0 * (x * x + y * y) / 2 / σ / σ);
        }
    }
}