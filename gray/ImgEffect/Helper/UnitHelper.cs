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
                    if(j < points[i].Length)
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
    }
}
