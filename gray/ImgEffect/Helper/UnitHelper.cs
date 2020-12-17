using Gray.ImgEffect;
using System;

namespace Gray.Unit
{
    class UnitHelper
    {
        /// <summary>
        /// 打印矩阵内容
        /// </summary>
        /// <param name="matrix"></param>
        static public void PrintMatrix(int[][] matrix)
        {
            int rows = matrix.Length, cols = matrix[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
            }
        }
        /// <summary>
        /// 打印两个矩阵的差值
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        static public void PrintMatrix(int[][] m1, int[][] m2)
        {
            int rows = m1.Length, cols = m1[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(m1[i][j] - m2[i][j] + " ");
                }
            }
        }
        /// <summary>
        /// 测试输出的DOG差分图片
        /// </summary>
        /// <param name="DogImg"></param>
        static public void OutputDogImg(double[][] DogImg)
        {
            Size size = ImageAnalyse.GetImgSize(DogImg);
            int width = size.Width, height = size.Height;

            string[] outputs = new string[height];
            string line;
            for (int i = 0; i < height; i++)
            {
                line = "";
                for (int j = 0; j < width; j++)
                {
                    line += DogImg[i][j] + ",";
                }
                outputs[i] = line;
            }
            FileHelper.WriteFile("result.csv", outputs, WriteMode.WriteAll);
        }
        /// <summary>
        /// 输出图片到文件
        /// </summary>
        /// <param name="img"></param>
        static public void OutputImg(int[][] img)
        {
            Size size = ImageAnalyse.GetImgSize(img);
            int width = size.Width, height = size.Height;

            string[] outputs = new string[height];
            string line;
            for (int i = 0; i < height; i++)
            {
                line = "";
                for (int j = 0; j < width; j++)
                {
                    line += img[i][j] + ",";
                }
                outputs[i] = line;
            }
            FileHelper.WriteFile("result.csv", outputs, WriteMode.WriteAll);
        }
        /// <summary>
        /// 测试输出极值点
        /// </summary>
        /// <param name="points"></param>
        static public void OutputExtremPoints(Point[][] points)
        {
            int l = points.Length;
            int w = points[0].Length;
            string[] outputs = new string[w + 1];
            string line = "";
            for (int i = 0; i < l; i++)
            {
                line += $"P{i + 1},";
            }
            outputs[0] = line;
            for (int j = 0; j < w; j++)
            {
                line = "";
                for (int i = 0; i < l; i++)
                {
                    if (j < points[i].Length)
                        line += $"{points[i][j]},";
                }
                outputs[j + 1] = line;
            }
            FileHelper.WriteFile("points.csv", outputs, WriteMode.WriteAll);
        }
        /// <summary>
        /// 测试二次函数求解方程
        /// </summary>
        static public void TestQuadraticEquation()
        {
            OrderedNumberPair o1 = new OrderedNumberPair(5, 73);
            OrderedNumberPair o2 = new OrderedNumberPair(6, 76);
            OrderedNumberPair o3 = new OrderedNumberPair(7, 92);
            QuadraticEquation quadraticEquation = new QuadraticEquation(o1, o2, o3);
            Console.WriteLine(quadraticEquation);
            Console.WriteLine(quadraticEquation.FindPeak());
            Console.WriteLine(123);
        }
        static public void TestInterPolation(int[][] img)
        {
            int[][] newImg = DSCMSelector.InterPolation(img, 4);

            OutputImg(newImg);
        }
        static public void TestCorrelation(int[][] img)
        {
            int[][] img1 = ImageAnalyse.InitMatrix<int>(100, 100);
            int[][] img2 = ImageAnalyse.InitMatrix<int>(100, 100);
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    img1[i][j] = img[i][j];
                    img2[i][j] = img[i + 6][j + 6];
                }
            }
            Console.WriteLine("自相关系数计算: ");
            Console.WriteLine(DSCMSelector.CalCorration(img1, img1));
            Console.WriteLine("非自相关系数: ");
            Console.WriteLine(DSCMSelector.CalCorration(img1, img2));
        }
        static public void TestFeaturePairFind(int[][] origin, int[][] deformation)
        {

            Size size = ImageAnalyse.GetImgSize<int>(origin) / 2;
            Point p = new Point(20 + new Random().Next(0, 20), 20 + new Random().Next(0, 20));
            int[][] selectedOrigin = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor2 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor3 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor4 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor5 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor6 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor7 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor8 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor9 = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            for (int i = 0; i < size.Height; i++)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    selectedOrigin[i][j] = origin[i + p.Y][j + p.X];
                    selectedDefor[i][j] = deformation[i + p.Y][j + p.X];
                    selectedDefor2[i][j] = deformation[i + p.Y][j + p.X - 1];
                    selectedDefor3[i][j] = deformation[i + p.Y][j + p.X + 1];
                    selectedDefor4[i][j] = deformation[i + p.Y - 1][j + p.X];
                    selectedDefor5[i][j] = deformation[i + p.Y + 1][j + p.X];
                    selectedDefor6[i][j] = deformation[i + p.Y - 1][j + p.X - 1];
                    selectedDefor7[i][j] = deformation[i + p.Y - 1][j + p.X + 1];
                    selectedDefor8[i][j] = deformation[i + p.Y + 1][j + p.X - 1];
                    selectedDefor9[i][j] = deformation[i + p.Y + 1][j + p.X + 1];
                }
            }
            Console.WriteLine($"原点变形相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor)}");
            Console.WriteLine($"在左形相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor2)}");
            Console.WriteLine($"在右形相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor3)}");
            Console.WriteLine($"在上形相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor4)}");
            Console.WriteLine($"在下形相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor5)}");
            Console.WriteLine($"在左上相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor6)}");
            Console.WriteLine($"在右上相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor7)}");
            Console.WriteLine($"在左下相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor8)}");
            Console.WriteLine($"在右下相关系数: {DSCMSelector.CalCorration(selectedOrigin, selectedDefor9)}");
            FPoint originPoint = new FPoint(p.X, p.Y, 1);
            FeaturePair dFeaturePair = DSCMSelector.FindFeaturePair(selectedOrigin, deformation, p.X, p.Y, originPoint, originPoint);
            Console.WriteLine(dFeaturePair);
        }
        static public void TestSubFinding(int[][] origin, int[][] defor, Point selectedPoint, Size size)
        {
            DSCMSelector dSCMSelector = new DSCMSelector(origin, defor, selectedPoint, size, 4);

            dSCMSelector.FindSubAreaPair(new Size(20, 20));

            FeaturePair[][] featurePairs = dSCMSelector.FeaturePairs;
            FeaturePair feature = dSCMSelector.ScaleFeaturePair;

            featurePairs = DSCMSelector.CalPairDegree(featurePairs);
            string[] result = new string[featurePairs.Length * featurePairs[0].Length + 1];
            int posi = 1;
            result[0] = feature.ToString();
            for (int i = 0; i < featurePairs.Length; i++)
            {
                for (int j = 0; j < featurePairs[i].Length; j++)
                {
                    if (featurePairs[i] == null)
                        continue;
                    result[posi++] = featurePairs[i][j].ToString();
                }
            }
            try
            {
                FileHelper.WriteFile("featurePairs.csv", result, WriteMode.WriteAll);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n请重试!");
            }
        }

    }
}
