using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
