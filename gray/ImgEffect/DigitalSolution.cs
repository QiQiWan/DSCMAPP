using Gray.ImgEffect;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Gray
{
    class DigitalSolution
    {
        /// <summary>
        /// 获得指定分割图像,去掉右和下的多余部分
        /// </summary>
        /// <param name="bitmap">要分割的图片</param>
        /// <param name="width">制定的宽</param>
        /// <param name="height">指定的高</param>
        /// <returns></returns>
        public static Bitmap[,] SplitImage(Bitmap bitmap, int width, int height)
        {
            int bitsWidth = bitmap.Width / width, bitsHeight = bitmap.Height / height;//去掉不足数的点
            Bitmap[,] bitmaps = new Bitmap[bitsWidth, bitsHeight];
            for (int i = 0; i < bitsWidth; i++)
            {
                for (int j = 0; j < bitsHeight; j++)
                {
                    Bitmap temp = new Bitmap(width, height);
                    for (int n = 0; n < width; n++)
                    {
                        for (int m = 0; m < height; m++)
                        {
                            temp.SetPixel(n, m, bitmap.GetPixel(i * width + n, j * height + m));//颜色转移
                        }
                    }
                    bitmaps[i, j] = temp;
                }
            }
            return bitmaps;
        }
        /// <summary>
        /// 计算相关系数
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        /// <returns></returns>
        public static double GetCorrelation(Bitmap bitmap1, Bitmap bitmap2)
        {
            if (bitmap1.Width != bitmap2.Width || bitmap1.Height != bitmap2.Height)
                return -1;
            double[,] origonMat = new double[bitmap1.Width, bitmap1.Height];
            double[,] deformationMat = new double[bitmap2.Width, bitmap2.Height];
            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    origonMat[i, j] = bitmap1.GetPixel(i, j).B;
                    deformationMat[i, j] = bitmap2.GetPixel(i, j).B;
                    sum1 += bitmap1.GetPixel(i, j).B;
                    sum2 += bitmap2.GetPixel(i, j).B;
                }
            }
            double _sum1 = (double)sum1 / (double)(bitmap1.Width * bitmap1.Height);
            double _sum2 = (double)sum2 / (double)(bitmap1.Width * bitmap1.Height);
            double _Denominator = 0;
            double _DivisorLeft = 0, _DivisorRight = 0;
            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    _Denominator += (origonMat[i, j] - _sum1) * (deformationMat[i, j] - _sum2);
                    _DivisorLeft += Math.Pow(origonMat[i, j] - _sum1, 2);
                    _DivisorRight += Math.Pow(deformationMat[i, j] - _sum2, 2);
                }
            }
            _Denominator *= _Denominator;
            return _Denominator / (_DivisorLeft * _DivisorRight);
        }

        public static double[,] GetCorrelations(Bitmap[,] bitmaps1, Bitmap[,] bitmaps2)
        {
            int cols = bitmaps1.GetLength(0);
            int rows = bitmaps1.Length / cols;
            double[,] correlations = new double[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    correlations[i, j] = DigitalSolution.GetCorrelation(bitmaps1[i, j], bitmaps2[i, j]);
                }
            }
            return correlations;
        }
        /// <summary>
        /// 在子区周围选取比对范围,返回该范围内所有的图片分布
        /// </summary>
        /// <param name="imageMap"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static imageMap[] BFSForCurrentArea(imageMap imageMap, int size)
        {
            Bitmap area = imageMap.bitmap;
            int count = area.Width - size;
            if (count < 0)//如果所需图片大小大于原始图片,停止操作
                return null;
            imageMap[] imageMaps = new imageMap[count * count];
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    int X = imageMap.positionX + i;
                    int Y = imageMap.positionY + j;
                    Bitmap temp = new Bitmap(size, size);
                    for (int m = 0; m < size; m++)
                    {
                        for (int n = 0; n < size; n++)
                        {
                            temp.SetPixel(m, n, area.GetPixel(i + m, j + n));
                        }
                    }
                    imageMaps[index++] = new imageMap(X, Y, temp);
                }
            }
            //if (imageMaps.Length == (count * count))
            //    Shell.WriteLine("$$$ 长度合格!");
            return imageMaps;
        }
        public static imageMap GetMaxCorrelationMap(imageMap orginImageMap, imageMap[] deforImageMaps)
        {
            imageMap MaxImageMap = null;
            double MaxCorrelation = 0;
            for (int i = 0; i < deforImageMaps.Length; i++)
            {
                double result = GetCorrelation(orginImageMap.bitmap, deforImageMaps[i].bitmap);
                if (result > 0.99)
                {
                    if (MaxCorrelation < result)
                    {
                        MaxCorrelation = result;
                        MaxImageMap = deforImageMaps[i];
                        if (MaxCorrelation == 1)
                            break;
                    }
                }
            }
            if (MaxCorrelation == 1)
                Shell.WriteLine(">>> 已找到相关系数为1的对应子区域!");
            return MaxImageMap;
        }

        /// <summary>
        /// 将bitmap保存为png
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filePath"></param>
        public static void SaveBitmapIntoFile(Bitmap bitmap, string filePath)
        {
            if (!Directory.Exists(filePath.Split('/')[0]))
                Directory.CreateDirectory(filePath.Split('/')[0]);
            using (FileStream fs = new FileStream(@filePath, FileMode.OpenOrCreate))
            {
                bitmap.Save(fs, ImageFormat.Png);
                fs.Flush();
                fs.Close();
            }
        }
        /// <summary>
        /// 把内存里的BitmapImage数据保存到硬盘中
        /// </summary>
        /// <param name="bitmapImage">BitmapImage数据</param>
        /// <param name="filePath">输出的文件路径</param>
        public static void SaveBitmapImageIntoFile(BitmapImage bitmapImage, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }

    public class ImageAnalyse
    {
        /// <summary>
        /// 计算积分图
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static int[][] IntegralGraph(int[][] origin)
        {
            int height = origin.Length, width = origin[0].Length;

            int[][] sum = origin;
            int[][] integralGraph = origin;
            // 逐行扫描
            for (int j = 0; j < height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    sum[j][i] = sum[j][i - 1] + sum[j][i];
                    if (j == 0)
                        integralGraph[j][i] = sum[j][i];
                    else
                        integralGraph[j][i] = integralGraph[j - 1][i] + sum[j][i];
                }
            }
            return integralGraph;
        }
        /// <summary>
        /// 将从内存中取出的灰度图片字节数组转化为整形矩阵
        /// </summary>
        /// <returns></returns>
        public static int[][] Bytes2IntMatrix(byte[] bitmapBytes, int width, int height)
        {
            int[][] bitmap = new int[height][];
            for (int j = 0; j < height; j++)
            {
                int[] temp = new int[width];
                for (int i = 0; i < width; i++)
                {
                    temp[i] = bitmapBytes[(j * width + i) * 3];
                }
                bitmap[j] = temp;
            }
            return bitmap;
        }
        /// <summary>
        /// 图片矩阵转byte数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] IntMatrix2Bytes(int[][] bitmap)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            byte[] bitmapBytes = new byte[width * height * 3];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    bitmapBytes[(j * width + i) * 3] = bitmapBytes[(j * width + i) * 3 + 1] = bitmapBytes[(j * width + i) * 3 + 2] = (byte)bitmap[j][i];
                }
            }
            return bitmapBytes;
        }
        /// <summary>
        /// 将整幅图片进行 3 * 3 的高斯滤波
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="σ">高斯模糊系数</param>
        /// <returns></returns>
        public static int[][] GaussionBlur(int[][] bitmap, double σ)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            int[][] GBitmap = InitMatrix<int>(width, height);
            int maxIntager = MaxIntager(σ);
            int SideLength = 2 * maxIntager + 1;

            ConvolutionType convolutionType = new ConvolutionType(SideLength, SideLength, maxIntager, maxIntager, σ);
            GausiionKernelSelector gausiionKernelSelector = new GausiionKernelSelector(convolutionType);

            int[][] temp;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    temp = CreateImgMatrix(bitmap, new Point(i, j), maxIntager);
                    GBitmap[j][i] = gausiionKernelSelector.GausiionSmoothing(temp, SideLength, SideLength);
                }
            }
            return GBitmap;
        }
        /// <summary>
        /// 边界调整,若值超出边界则取关于边界对称
        /// </summary>
        /// <param name="n"></param>
        /// <param name="lBorder"></param>
        /// <param name="uborder"></param>
        /// <returns></returns>
        private static int BorderAdjust(int n, int lBorder, int uborder)
        {
            if (n < lBorder)
                return 2 * lBorder - n;
            if (n > uborder)
                return 2 * uborder - n;
            return n;
        }

        /// <summary>
        /// 从大矩阵中构造小矩阵
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="center"></param>
        /// <param name="maxIntager"></param>
        /// <returns></returns>
        public static int[][] CreateImgMatrix(int[][] bitmap, Point center, int maxIntager)
        {
            int SideLength = 2 * maxIntager + 1;
            int width = bitmap[0].Length, height = bitmap.Length;
            int[][] matrix = InitMatrix<int>(SideLength, SideLength);
            int x, y;
            for(int i = 0; i < SideLength; i++)
            {
                for(int j = 0; j < SideLength; j++)
                {
                    x = center.X - maxIntager + j;
                    x = BorderAdjust(x, 0, width - 1);
                    y = center.Y - maxIntager + i;
                    y = BorderAdjust(y, 0, height - 1);
                    matrix[i][j] = bitmap[y][x];
                }
            }
            return matrix;
        }
        public static int MaxIntager(double σ)
        {
            return (int)(3 * σ) + 1;
        }
        public static T[][] InitMatrix<T>(int width, int height)
        {
            T[][] matrix = new T[height][];
            for (int i = 0; i < height; i++)
            {
                matrix[i] = new T[width];
            }
            return matrix;
        }
        /// <summary>
        /// dogo取异
        /// </summary>
        /// <returns></returns>
        public static int[][] DogeDiff(int[][] p1, int[][] p2)
        {
            Size size = CompareImageSize(p1, p2);
            if (size.IsNull)
                return null;
            int with = size.Width, height = size.Height;
            int[][] DMatrix = InitMatrix<int>(with, height);
            for(int i = 0; i < height; i++)
            {
                for(int j = 0;j < with; j++)
                {
                    DMatrix[i][j] = p1[i][j] - p2[i][j];
                }
            }
            return DMatrix;
        }

        public static Size CompareImageSize(int[][] p1, int[][] p2)
        {
            int w1 = p1[0].Length, h1 = p1.Length;
            int w2 = p2[0].Length, h2 = p2.Length;
            if (w1 == w2 && h1 == h2)
                return new Size(w1, w2);
            return new Size();
        }
    }
    public class Size
    {
        public readonly bool IsNull;
        public readonly int Width;
        public readonly int Height;
        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.IsNull = false;
        }
        public Size()
        {
            this.IsNull = true;
        }
    }
}
