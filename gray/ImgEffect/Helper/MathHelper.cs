using System;

namespace Gray.ImgEffect.Helper
{
    /// <summary>
    /// 数学帮助类,用来计算协方差等统计量
    /// </summary>
    class MathHelper
    {
        public static double Cov(int[] X, int[] Y)
        {
            if (X.Length != Y.Length)
                throw new RankException("计算协方差的数组维度不一致");
            int[] XY = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                XY[i] = X[i] * Y[i];
            }
            return Math.Abs(E(XY) - E(X) * E(Y));
        }
        public static double Cov(int[] X, int[] Y, double EX, double EY)
        {
            if (X.Length != Y.Length)
                throw new RankException("计算协方差的数组维度不一致");
            int[] XY = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                XY[i] = X[i] * Y[i];
            }
            return Math.Abs(E(XY) - EX * EY);
        }
        public static double Var(int[] X)
        {
            int[] X2 = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X2[i] = X[i] * X[i];
            }
            return Math.Abs(E(X2) - E(X) * E(X));
        }
        public static double Var(int[] X, double EX)
        {
            int[] X2 = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X2[i] = X[i] * X[i];
            }
            return Math.Abs(E(X2) - EX * EX);
        }
        public static double E(int[] X)
        {
            double s = 0;
            foreach (var item in X)
            {
                s += item;
            }
            return 1.0 * s / X.Length;
        }
        public static double CorrelationCoefficient(int[] X, int[] Y)
        {
            if (X.Length != Y.Length)
                throw new RankException("计算相关系数的数组维度不一致");
            double EX = E(X);
            double EY = E(Y);
            return Cov(X, Y, EX, EY) / Math.Sqrt(Var(X, EX)) / Math.Sqrt(Var(Y, EY));
        }
    }
}
/// <summary>
/// 二次方程类
/// </summary>
class QuadraticEquation
{
    /// <summary>
    /// 0次方系数
    /// </summary>
    public readonly double A0;
    /// <summary>
    /// 一次方系数
    /// </summary>
    public readonly double A1;
    /// <summary>
    /// 二次方系数
    /// </summary>
    public readonly double A2;
    public QuadraticEquation(OrderedNumberPair O1, OrderedNumberPair O2, OrderedNumberPair O3)
    {
        OrderedNumberPair[] orderedNumberPairs = new OrderedNumberPair[3];
        orderedNumberPairs[0] = O1;
        orderedNumberPairs[1] = O2;
        orderedNumberPairs[2] = O3;
    }

    private double[] SolveWquation(OrderedNumberPair[] Os)
    {
        double[][] matrix = new double[Os.Length][];
        double[] temp;
        for (int i = 0; i < Os.Length; i++)
        {
            temp = new double[4];
            temp[0] = 1;
            temp[1] = Os[i].X;
            temp[2] = Os[i].X * Os[i].X;
            temp[3] = Os[i].Y;
            matrix[i] = temp;
        }
        for (int i = 0; i < matrix[0].Length; i++)
        {
            matrix[1][i] = matrix[1][i] - matrix[0][i];
            matrix[2][i] = matrix[2][i] - matrix[2][i];
        }
        double S = matrix[2][1] / matrix[1][1];
        for (int i = 1; i < matrix[0].Length; i++)
        {
            matrix[2][i] = matrix[2][i] - matrix[1][i] * S;
        }
        double[] A = new double[3];
        A[2] = matrix[2][3] / matrix[2][2];
        A[1] = (matrix[1][3] - matrix[1][2] * A[2]) / matrix[1][1];
        A[0] = matrix[0][3] - matrix[0][2] * A[2] - matrix[0][1] * A[1];
        return A;
    }
    public double GetValue(double X)
    {
        return A0 + A1 * X + A2 * X * X;
    }
}
/// <summary>
/// 有序数对结构
/// </summary>
struct OrderedNumberPair
{
    public readonly double X;
    public readonly double Y;
    public OrderedNumberPair(double X, double Y)
    {
        this.X = X;
        this.Y = Y;
    }
}