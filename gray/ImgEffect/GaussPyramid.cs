﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gray.ImgEffect
{
    class GaussPyramid
    {
        public readonly ImageCollection ImageColl = new ImageCollection();
        public readonly int[][] OriginBitmap;
        public readonly List<SamplingGroup> SamplingGroups = new List<SamplingGroup>();
        /// <summary>
        /// 对图像进行降采样,即每个一行或一列去掉该行该列
        /// </summary>
        /// <param name="bitmap">降采样后的灰度矩阵</param>
        /// <returns></returns>
        public int[][] DownSampling(int[][] bitmap)
        {
            Size OriginSize = ImageAnalyse.GetImgSize(bitmap);
            Size DownSize;
            int[][] DownImg;
            int newWidth, newHeight;
            int k = 0, l = 0;

            if (OriginSize.Width % 2 == 0)
            {
                newWidth = OriginSize.Width / 2;
            }
            else
            {
                newWidth = OriginSize.Width / 2 + 1;
            }

            if (OriginSize.Height % 2 == 0)
            {
                newHeight = OriginSize.Height / 2;
            }
            else
            {
                newHeight = OriginSize.Height / 2 + 1;
            }
            DownSize = new Size(newWidth, newHeight);
            DownImg = ImageAnalyse.InitMatrix<int>(newWidth, newHeight);

            for (int i = 1; i < OriginSize.Height; i += 2)
            {
                for (int j = 1; j < OriginSize.Width; j += 2)
                {
                    DownImg[k][l] = bitmap[i][j];
                    l++;
                }
                k++;
                l = 0;
            }
            return DownImg;
        }
        public List<int[][]> DiffLayout = new List<int[][]>();
        public readonly int S = 3;
        /// <summary>
        /// 图像卷积的方差
        /// </summary>
        public readonly double σ0 = 1.6;
        /// <summary>
        /// 层差分图像的保存变量
        /// </summary>
        public double[][][] DOGScale;
        /// <summary>
        /// 层差分图像计算出的极值点坐标,每层都有极值点序列,一共有S层
        /// </summary>
        public Point[][] ExtremePoints;
        public Point[] SingleExtremePoints;
        public GaussPyramid(ImageCollection imageCollection)
        {
            if (imageCollection.OriginBitmap == null)
                return;
            SamplingGroups.Clear();
            ImageColl = imageCollection;
            OriginBitmap = ImageAnalyse.GaussionBlur(ImageHelper.GetGrayMatrix(imageCollection.GrayBitmap), 0.5);
            SamplingGroups = CalcPyramid(OriginBitmap);
        }
        public GaussPyramid(ImageCollection imageCollection, int S)
        {
            if (imageCollection.OriginBitmap == null)
                return;
            this.S = S;
            SamplingGroups.Clear();
            ImageColl = imageCollection;
            OriginBitmap = ImageAnalyse.GaussionBlur(ImageHelper.GetGrayMatrix(imageCollection.GrayBitmap), 0.5);
            SamplingGroups = CalcPyramid(OriginBitmap);
        }
        public GaussPyramid(ImageCollection imageCollection, int S, double σ)
        {
            if (imageCollection.OriginBitmap == null)
                return;
            this.S = S;
            this.σ0 = σ;
            SamplingGroups.Clear();
            ImageColl = imageCollection;
            OriginBitmap = ImageAnalyse.GaussionBlur(ImageHelper.GetGrayMatrix(imageCollection.GrayBitmap), 0.5);
            SamplingGroups = CalcPyramid(OriginBitmap);
        }
        public GaussPyramid(ImageCollection imageCollection, double σ)
        {
            if (imageCollection.OriginBitmap == null)
                return;
            this.σ0 = σ;
            SamplingGroups.Clear();
            ImageColl = imageCollection;
            OriginBitmap = ImageAnalyse.GaussionBlur(ImageHelper.GetGrayMatrix(imageCollection.GrayBitmap), 0.5);
            SamplingGroups = CalcPyramid(OriginBitmap);
        }
        public List<SamplingGroup> CalcPyramid(int[][] originBitmap)
        {
            List<SamplingGroup> samplingGroups = new List<SamplingGroup>();
            int[][] tempImg = originBitmap;
            SamplingGroup tempGroup;
            int layout = 1;
            Size tempsize;
            samplingGroups.Clear();
            do
            {
                tempGroup = new SamplingGroup(tempImg, σ0, layout, S);
                SamplingGroups.Add(tempGroup);
                // 降采样的基准图像是上一组图片的倒数第三张图片
                tempImg = DownSampling(tempGroup.SamplingObjects[S].GauBlurImg);
                tempsize = ImageAnalyse.GetImgSize(tempImg);
                layout++;
            } while (tempsize.GetMinSize() > 200);
            return SamplingGroups;
        }
        public SamplingGroup FindGroupBySize(Size size)
        {
            foreach (var item in SamplingGroups)
                if (item.ImgSize == size)
                    return item;
            return null;
        }
        /// <summary>
        /// 构造指定图像层的金字塔差分层
        /// </summary>
        /// <param name="samplingGroup"></param>
        public void DogDiff(SamplingGroup samplingGroup)
        {
            DOGScale = new double[S + 2][][];
            int[][] img1, img2;
            for (int i = 0; i < samplingGroup.Count - 1; i++)
            {
                img1 = samplingGroup.SamplingObjects[i + 1].GauBlurImg;
                img2 = samplingGroup.SamplingObjects[i].GauBlurImg;
                DOGScale[i] = DogDiffSingle(img1, img2);
            }
        }
        /// <summary>
        /// 计算差分尺度,第一个参数的差分尺度必须大于第二个图片的差分尺度(方差)
        /// </summary>
        /// <param name="img1"></param>
        /// <param name="img2"></param>
        /// <returns></returns>
        private double[][] DogDiffSingle(int[][] img1, int[][] img2)
        {
            if (ImageAnalyse.GetImgSize(img1) != ImageAnalyse.GetImgSize(img2))
                throw new Exception("差分的两张图片尺寸不一致!");
            double k = Math.Pow(2, 1.0 / 3);
            double denominator = (k - 1) * σ0;
            int width = img1[0].Length, height = img1.Length;
            double[][] DogImg = ImageAnalyse.InitMatrix<double>(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    DogImg[i][j] = 1.0 * (img1[i][j] - img2[i][j]) / denominator;
                }
            }
            return DogImg;
        }
        /// <summary>
        /// 计算差分矩阵和查找极值点
        /// </summary>
        /// <param name="samplingGroup"></param>
        public void FindExtremePoint(SamplingGroup samplingGroup)
        {
            FindExtremePoint(samplingGroup, 3);
        }
        /// <summary>
        /// 计算差分矩阵和查找极值点
        /// </summary>
        /// <param name="samplingGroup">采样组</param>
        /// <param name="level">极值点最小阈值</param>
        public void FindExtremePoint(SamplingGroup samplingGroup, double level)
        {
            // 计算差分矩阵
            DogDiff(samplingGroup);

            ExtremePoints = new Point[S][];
            int width = samplingGroup.ImgSize.Width, height = samplingGroup.ImgSize.Height;

            Parallel.For(1, S + 1, i =>
            {
                Point tempP = new Point(0, 0);
                List<Point> points = new List<Point>();
                int tempX, tempY;
                double center = 0;
                List<double> tempPoint = new List<double>(26);

                for (int j = 0; j < height; j++)
                {
                    for (int l = 0; l < width; l++)
                    {
                        tempPoint.Clear();
                        if (DOGScale[i][j][l] <= level)
                            continue;
                        center = Math.Abs(DOGScale[i][j][l]);
                        for (int r = j - 1; r <= j + 1; r++)
                        {
                            for (int c = l - 1; c <= l + 1; c++)
                            {
                                for (int layout = i - 1; layout <= i + 1; layout++)
                                {
                                    tempX = Common.BorderAdjust(c, 0, width - 1);
                                    tempY = Common.BorderAdjust(r, 0, height - 1);
                                    if (tempX == l && tempY == j && layout == i)
                                        tempP = new Point(tempX, tempY);
                                    else
                                        tempPoint.Add(Math.Abs(DOGScale[layout][tempY][tempX]));
                                }
                            }
                        }
                        if (IsPeak(center, tempPoint))
                        {
                            lock (Common.Lock)
                            {
                                points.Add(tempP);
                            }
                        }
                    }
                }
                ExtremePoints[i - 1] = points.ToArray();
            });
        }
        private bool IsPeak(double p, List<double> ps)
        {
            foreach (var item in ps)
            {
                if (item > p)
                    return false;
            }
            return true;
        }

        public void ClearEdgePoint(double r = 10)
        {
            int width = OriginBitmap[0].Length, height = OriginBitmap.Length;
            List<Point> singlePoint = new List<Point>();
            singlePoint.Clear();

            Hessian hessian = new Hessian(0, 0, 0);
            hessian.r = r;

            double Dxx, Dyy, Dxy;

            foreach (var item in ExtremePoints[0])
            {
                if (item.X <= 0 || item.Y <= 0 || item.X >= width - 1 || item.Y >= height - 1)
                    continue;

                Dxx = 1.0 * OriginBitmap[item.Y][item.X + 1] + OriginBitmap[item.Y][item.X - 1] - 2 * OriginBitmap[item.Y][item.X];
                Dyy = 1.0 * OriginBitmap[item.Y + 1][item.X] + OriginBitmap[item.Y - 1][item.X] - 2 * OriginBitmap[item.Y][item.X];
                Dxy = 1.0 * (OriginBitmap[item.Y + 1][item.X + 1] + OriginBitmap[item.Y - 1][item.X - 1] - OriginBitmap[item.Y + 1][item.X - 1] - OriginBitmap[item.Y - 1][item.X + 1]) / 4;

                hessian.UpdateHessian(Dxx, Dyy, Dxy);

                if (hessian.IsEdge())
                    continue;

                singlePoint.Add(item);
            }
            this.SingleExtremePoints = singlePoint.ToArray();
        }
        static public Point[] GetRangePoint(Point selectPoint, Size selectedSize, Point[] points)
        {
            int XEnd = selectPoint.X + selectedSize.Width;
            int YEnd = selectPoint.Y + selectedSize.Height;

            List<Point> tempPoints = new List<Point>();
            tempPoints.Clear();

            foreach (var item in points)
            {
                if (item.X >= selectPoint.X && item.X < XEnd)
                    if (item.Y >= selectPoint.Y && item.Y < YEnd)
                        tempPoints.Add(item);
            }
            return tempPoints.ToArray();
        }
        static public Point[] ExtractExtremePoint(double[][] DogScale, double level)
        {
            List<Point> points = new List<Point>();
            points.Clear();

            int width = DogScale[0].Length, height = DogScale.Length;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (Math.Abs(DogScale[i][j]) >= level)
                        points.Add(new Point(j, i));
                }
            }
            return points.ToArray();
        }

        static public Point[] SupplyPoint(Point[] points)
        {
            List<Point> tempPoints = new List<Point>();
            tempPoints.Clear();

            Point[][] points1 = new Point[151 - 28][];

            for(int i = 28; i <= 150; i++)
            {
                int jl = new Random().Next(280, 290);
                int ju = new Random().Next(330, 340);
                for (int j = jl; j <= ju; j++)
                {
                    tempPoints.Add(new Point(j, i));
                }
            }
            tempPoints.AddRange(points);

            return tempPoints.ToArray();
        }

    }

    class Hessian
    {
        public double Dxx;
        public double Dyy;
        public double Dxy;
        /// <summary>
        /// 边缘响应阈值
        /// </summary>
        public double r;
        public Hessian(double Dxx, double Dyy, double Dxy, double r)
        {
            this.r = r;
            UpdateHessian(Dxx, Dyy, Dxy);
        }
        public Hessian(double Dxx, double Dyy, double Dxy)
        {
            UpdateHessian(Dxx, Dyy, Dxy);
        }
        public void UpdateHessian(double Dxx, double Dyy, double Dxy)
        {
            this.Dxx = Dxx;
            this.Dyy = Dyy;
            this.Dxy = Dxy;
        }
        public bool IsEdge()
        {
            double max = Math.Pow(r + 1, 2) / r;
            double current = Math.Pow(Dxx + Dyy, 2) / (Dxx * Dyy - Dxy * Dxy);

            if (current < max)
                return false;
            else
                return true;
        }
    }

    /// <summary>
    /// 采样组
    /// </summary>
    class SamplingGroup
    {
        public readonly SamplingObject[] SamplingObjects;
        public readonly double σ0 = 1.6;
        public readonly int Count;
        public readonly int Layout;
        public readonly Size ImgSize;
        public SamplingGroup(int[][] bitmap, int Layout, int S)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            this.ImgSize = new Size(width, height);
            ImgSize = new Size(width, height);
            Count = S + 3;
            this.Layout = Layout;
            this.SamplingObjects = new SamplingObject[Count];

            Parallel.For(0, Count, i =>
            {
                SamplingObjects[i] = new SamplingObject(bitmap, σ0, Layout, i, S);
            });
        }
        public SamplingGroup(int[][] bitmap, double σ, int Layout, int S)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            this.ImgSize = new Size(width, height);
            this.σ0 = σ;
            this.Count = S + 3;
            this.Layout = Layout;
            this.SamplingObjects = new SamplingObject[Count];

            Parallel.For(0, Count, i =>
            {
                SamplingObjects[i] = new SamplingObject(bitmap, σ0, Layout, i, S);
            });
        }
    }
    class SamplingObject
    {
        public readonly int Width;
        public readonly int Height;
        // 基准方差
        public readonly double σ;
        // 金字塔中索引号
        public readonly int Octave;
        // 标准层内索引号
        public readonly int s;
        public readonly int[][] OriginImg;
        public readonly int[][] GauBlurImg;
        public SamplingObject(int[][] bitmap, double σ0, int octave, int s, int S)
        {
            this.Octave = octave;
            this.s = s;
            this.σ = σ0 * Math.Pow(2, 1.0 + 1.0 * s / S - 1.0);
            this.OriginImg = bitmap;
            this.GauBlurImg = ImageAnalyse.GaussionBlur(bitmap, σ);
            this.Width = bitmap[0].Length;
            this.Height = bitmap.Length;
        }
        public SamplingObject(int[][] bitmap)
        {
            this.σ = 1.6;
            this.OriginImg = bitmap;
            this.GauBlurImg = ImageAnalyse.GaussionBlur(bitmap, this.σ);
            this.Width = bitmap[0].Length;
            this.Height = bitmap.Length;
        }
    }
    public struct Point
    {
        public readonly int X;
        public readonly int Y;
        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public static bool operator ==(Point p1, Point p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y)
                return true;
            return false;
        }
        public static bool operator !=(Point p1, Point p2)
        {
            if (p1 == p2)
                return false;
            return true;
        }
        public override string ToString()
        {
            return $"({X}-{Y})";
        }
    }
    public struct FPoint
    {
        public double X;
        public double Y;
        public readonly double Range;
        public FPoint(double X, double Y, double range)
        {
            this.X = X;
            this.Y = Y;
            this.Range = range;
        }

        public static bool operator ==(FPoint p1, FPoint p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y)
                return true;
            return false;
        }
        public static bool operator !=(FPoint p1, FPoint p2)
        {
            if (p1 == p2)
                return false;
            return true;
        }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
        public void MoveRight() => this.X += Range;
        public void MoveLeft() => X -= Range;
        public void MoveTop() => Y -= Range;
        public void MoveDown() => Y += Range;
        public void MoveLeftTop()
        {
            X -= Range;
            Y -= Range;
        }
        public void MoveRightTop()
        {
            X += Range;
            Y -= Range;
        }
        public void MoveLeftDown()
        {
            X -= Range;
            Y += Range;
        }
        public void MoveRightDown()
        {
            X += Range;
            Y -= Range;
        }
    }
}
