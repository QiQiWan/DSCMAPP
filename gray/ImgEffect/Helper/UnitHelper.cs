using Gray.ImgEffect;
using System;

namespace Gray.Unit
{
    class UnitHelper
    {
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
    }
}
