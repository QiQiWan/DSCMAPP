using Gray.ImgEffect.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public readonly int[][] SelectedOrigin;
        public readonly int[][] SelectedDeformation;
        public readonly FeaturePair ScaleFeaturePair;
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
        public FeaturePair[][] FeaturePairs;
        /// <summary>
        /// 插值数量
        /// </summary>
        public int Step;
        public double Range;
        public DSCMSelector(int[][] origin, int[][] deformation, Point selectPoint, Size size, int step)
        {
            this.Origin = origin;
            this.Deformation = deformation;
            this.SelectSize = size;
            this.SelectPoint = selectPoint;
            this.Step = step;
            this.Range = 1.0 / (step + 1);

            int[][] selectedOrigin = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);
            int[][] selectedDefor = ImageAnalyse.InitMatrix<int>(size.Width, size.Height);


            for (int i = 0; i < size.Height; i++)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    selectedOrigin[i][j] = origin[i + selectPoint.Y][j + selectPoint.X];
                }
            }

            FPoint originPoint = new FPoint(selectPoint.X, selectPoint.Y, 1);
            FeaturePair dFeaturePair = FindFeaturePair(selectedOrigin, deformation, selectPoint.X, selectPoint.Y, originPoint, originPoint);
            for (int i = 0; i < size.Height; i++)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    selectedDefor[i][j] = deformation[(int)(i + dFeaturePair.DP.Y)][(int)(j + dFeaturePair.DP.X)];
                }
            }
            this.SelectedOrigin = selectedOrigin;
            this.SelectedDeformation = selectedDefor;
            this.ScaleFeaturePair = dFeaturePair;
        }
        /// <summary>
        /// 图像插值,二次函数插值,
        /// </summary>
        /// <param name="img">原始图像</param>
        /// <param name="step">两像素间的插值个数</param>
        /// <returns>插值后的图像</returns>
        public static int[][] InterPolation(int[][] img, int step)
        {
            if (step == 0)
                return img;
            int Multiple = step + 1;
            int width = img[0].Length, height = img.Length;
            int newWidth = width * Multiple - step, newHeight = height * Multiple - step;
            int[][] newImg = ImageAnalyse.InitMatrix<int>(newWidth, newHeight);
            double range = 1.0 / Multiple;
            int xposi = 0, yposi = 0;
            double xAp = 0, yAp = 0;
            int lx, ly;
            QuadraticEquation qx, qy;
            for (int i = 0; i < height; i++)
            {
                xposi = 0;
                xAp = 0;
                for (int j = 0; j < width; j++)
                {
                    if (j == width - 1)
                    {
                        newImg[i * Multiple][j * Multiple] = img[i][j] * 100;
                        continue;
                    }
                    lx = j + 2;
                    if (j == width - 2)
                        lx = width - 2;
                    qx = new QuadraticEquation(new OrderedNumberPair(j, img[i][j]), new OrderedNumberPair(j + 1, img[i][j + 1]), new OrderedNumberPair(j + 2, img[i][lx]));
                    for (int l = 0; l <= step; l++)
                    {
                        // 将数量维度扩展100倍,精确到小数点后两位
                        newImg[i * Multiple][xposi] = (int)(qx.GetValue(xAp) * 100);
                        xAp += range;
                        xposi += 1;
                    }
                }
            }

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height - 1; i++)
                {
                    for (int w = 0; w <= step; w++)
                    {
                        if (j == (width - 1) && w > 0)
                            break;
                        //if (i == height - 2)
                        //    ;
                        xposi = j * Multiple + w;
                        yposi = i * Multiple;
                        ly = yposi + 2 * Multiple;
                        if (i == height - 2)
                            ly = yposi;
                        qy = new QuadraticEquation(new OrderedNumberPair(yposi, newImg[yposi][xposi]), new OrderedNumberPair(yposi + Multiple, newImg[yposi + Multiple][xposi]), new OrderedNumberPair(yposi + 2 * Multiple, newImg[ly][xposi]));
                        for (int l = 0; l <= step; l++)
                        {
                            if (l == 0)
                                continue;
                            newImg[yposi + l][xposi] = (int)qy.GetValue(yposi + l);
                        }
                    }
                }
            }
            return newImg;
        }

        /// <summary>
        /// 以半边长为步长构造范围搜索相关子区域
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="OX"></param>
        /// <param name="OY"></param>
        /// <param name="OAP"></param>
        /// <param name="DAP"></param>
        /// <returns></returns>
        static public FeaturePair FindFeaturePairByStep(int[][] origin, int[][] scale, int OX, int OY, FPoint OAP, FPoint DAP)
        {
            int width = origin[0].Length, height = origin.Length;
            int SW = scale[0].Length, SH = scale.Length;

            if (width < 8)
                width = 8;
            if (height < 8)
                height = 8;

            int XStart = OX - width / 2 > 0 ? OX - width / 2 : 0;
            int XEnd = OX + width / 2 + width >= SW ? SW : OX + width / 2;
            int YStart = OY - height / 2 > 0 ? OY - height / 2 : 0;
            int YEnd = OY + height / 2 + height >= SH ? SH : OY + height / 2;

            // 记录相关系数和最大相关系数位置
            double Corre = 0, max = 0;
            int PX = OX, PY = OY;
            int[] originA = GetGrayArray(origin);

            for (int i = YStart; i < YEnd; i++)
            {
                for (int j = XStart; j < XEnd; j++)
                {
                    int[] deforA = GetGrayArray(scale, j, i, new Size(width, height));
                    Corre = CalCorration(originA, deforA);
                    if (Corre > max)
                    {
                        PX = j;
                        PY = i;
                        max = Corre;
                    }
                }
            }
            PX = PX - OX + (int)OAP.X;
            PY = PY - OY + (int)OAP.Y;
            return new FeaturePair(OAP, new FPoint(PX, PY, DAP.Range), new Size(width, height));
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
        public static FeaturePair FindFeaturePair(int[][] origin, int[][] scale, int OX, int OY, FPoint OAP, FPoint DAP)
        {
            int width = origin[0].Length, height = origin.Length;
            int SW = scale[0].Length, SH = scale.Length;
            if (width >= SW || height >= SH)
                throw new RankException("所查找的子区域大于查找范围!");
            int[] originA = GetGrayArray(origin);
            double UC = 0, UU = 0, UD = 0, UL = 0, UR = 0, ULU = 0, URU = 0, ULD = 0, URD = 0, max = 0;
            do
            {
                UC = CalCorration(originA, GetGrayArray(scale, OX, OY, width, height));
                max = UC;
                // 左侧
                if (OX > 0)
                {
                    UL = CalCorration(originA, GetGrayArray(scale, OX - 1, OY, width, height));
                    if (max < UL)
                        max = UL;
                }
                // 上侧
                if (OY > 0)
                {
                    if (OY == 0)
                        ;
                    UU = CalCorration(originA, GetGrayArray(scale, OX, OY - 1, width, height));
                    if (max < UU)
                        max = UU;
                }
                // 右侧
                if (OX + width < SW - 1)
                {
                    UR = CalCorration(originA, GetGrayArray(scale, OX + 1, OY, width, height));
                    if (max < UR)
                        max = UR;
                }
                // 下侧
                if (OY + height < SH - 1)
                {
                    UD = CalCorration(originA, GetGrayArray(scale, OX, OY + 1, width, height));
                    if (max < UD)
                        max = UD;
                }
                // 左上
                if (OX > 0 && OY > 0)
                {
                    if (OY == 0)
                        ;
                    ULU = CalCorration(originA, GetGrayArray(scale, OX - 1, OY - 1, width, height));
                    if (max < ULU)
                        max = ULU;
                }
                // 右上
                if (OX + width < SW - 1 && OY > 0)
                {
                    if (OY == 0)
                        ;
                    URU = CalCorration(originA, GetGrayArray(scale, OX + 1, OY - 1, width, height));
                    if (max < URU)
                        max = URU;
                }
                // 左下
                if (OX > 0 && OY + height < SH - 1)
                {
                    ULD = CalCorration(originA, GetGrayArray(scale, OX - 1, OY + 1, width, height));
                    if (max < ULD)
                        max = ULD;
                }
                // 右下
                if (OX + width < SW - 1 && OY + height < SH - 1)
                {
                    URD = CalCorration(originA, GetGrayArray(scale, OX + 1, OY + 1, width, height));
                    if (max < URD)
                        max = URD;
                }
                if (max == UL)
                {
                    DAP.MoveLeft();
                    //return FindFeaturePair(origin, scale, OX - 1, OY, OAP, DAP);
                    OX--;
                }
                if (max == UR)
                {
                    DAP.MoveRight();
                    //return FindFeaturePair(origin, scale, OX + 1, OY, OAP, DAP);
                    OX++;
                }
                if (max == UU)
                {
                    DAP.MoveTop();
                    //return FindFeaturePair(origin, scale, OX, OY - 1, OAP, DAP);
                    OY--;
                }
                if (max == UD)
                {
                    DAP.MoveDown();
                    //return FindFeaturePair(origin, scale, OX, OY + 1, OAP, DAP);
                    OY++;
                }
                if (max == URU)
                {
                    DAP.MoveRightTop();
                    //return FindFeaturePair(origin, scale, OX + 1, OY - 1, OAP, DAP);
                    OX++;
                    OY--;

                }
                if (max == ULU)
                {
                    DAP.MoveLeftTop();
                    //return FindFeaturePair(origin, scale, OX - 1, OY - 1, OAP, DAP);
                    OX--;
                    OY--;
                }
                if (max == ULD)
                {
                    DAP.MoveLeftDown();
                    //return FindFeaturePair(origin, scale, OX - 1, OY + 1, OAP, DAP);
                    OX--;
                    OY++;
                }
                if (max == URD)
                {
                    DAP.MoveRightDown();
                    //return FindFeaturePair(origin, scale, OX + 1, OY + 1, OAP, DAP);
                    OX++;
                    OY++;
                }
            } while (max != UC);

            return new FeaturePair(OAP, DAP, new Size(width, height));
        }
        public void FindSubAreaPair(Size subSize)
        {

            int[][] ExtendOrigin = InterPolation(SelectedOrigin, Step);
            int[][] ExtendDefor = InterPolation(SelectedDeformation, Step);
            Point StartPoint = SelectPoint;
            int width = SelectedOrigin[0].Length - subSize.Width, height = SelectedOrigin.Length - subSize.Height;
            int SW = ExtendOrigin[0].Length - subSize.Width, SH = ExtendDefor.Length - subSize.Height;
            int Multiple = Step + 1;

            // int[][] SubDefor = ImageAnalyse.InitMatrix<int>(subSize);
            FeaturePair[][] featurePairs = new FeaturePair[height][];

            for (int i = 0; i < height; i++)
            {
                FeaturePair[] temp = new FeaturePair[width];

                Parallel.For(0, width, j =>
                {

                    int[][] SubOrigin = ImageAnalyse.InitMatrix<int>(subSize);
                    // 不插值搜索
                    FPoint OAP = new FPoint(StartPoint.X + j, StartPoint.Y + i, 1);
                    FPoint DAP = new FPoint(ScaleFeaturePair.DP.X + j, ScaleFeaturePair.DP.Y + i, 1);
                    for (int l = 0; l < subSize.Height; l++)
                    {
                        for (int m = 0; m < subSize.Width; m++)
                        {
                            SubOrigin[l][m] = SelectedOrigin[i + l][j + m];
                        }
                    }

                    FeaturePair feature = FindFeaturePair(SubOrigin, SelectedDeformation, j, i, OAP, OAP);
                    if (feature.OP != feature.DP)
                    {
                        // OAP = new FPoint(StartPoint.X + Range * j, StartPoint.Y + Range * i, Range);
                        DAP = new FPoint(feature.DP.X, feature.DP.Y, Range);
                        for (int l = 0; l < subSize.Height; l++)
                        {
                            for (int m = 0; m < subSize.Width; m++)
                            {
                                SubOrigin[l][m] = ExtendOrigin[i + l][j + m];
                            }
                        }
                        feature = FindFeaturePair(SubOrigin, ExtendDefor, j * Multiple, i * Multiple, OAP, DAP);
                    }
                    temp[j] = feature;
                });
                featurePairs[i] = temp;
            }

            this.FeaturePairs = featurePairs;
        }
        static public FeaturePair[] FindSubAreaPair(Size subSize, int[][] origin, int[][] defor, Point[] ExtremePoints, Point selectPoint, Size selectedSize, int step)
        {
            int[][] selectedOrigin = GetGrayMatrix(origin, selectPoint, selectedSize);
            int[][] selectedDefor = GetGrayMatrix(defor, selectPoint, selectedSize);
            int[][] ExtendOrigin = InterPolation(selectedOrigin, step);
            int[][] ExtendDefor = InterPolation(selectedDefor, step);
            int Multiple = step + 1;
            double Range = 1.0 / Multiple;
            int len = ExtremePoints.Length;
            List<FeaturePair> featurePairs = new List<FeaturePair>();
            featurePairs.Clear();

            for (int i = 0; i < len; i++)
            {
                int[][] SubOrigin = ImageAnalyse.InitMatrix<int>(subSize);
                int oX = ExtremePoints[i].X - selectPoint.X;
                int oY = ExtremePoints[i].Y - selectPoint.Y;
                FPoint OAP = new FPoint(oX, oY, 1);


                for (int l = 0; l < subSize.Height; l++)
                {
                    for (int m = 0; m < subSize.Width; m++)
                    {
                        SubOrigin[l][m] = selectedOrigin[BorderAdjust(oY + l, 0, selectedSize.Height)][BorderAdjust(oX + m, 0, selectedSize.Width)];
                    }
                }
                FeaturePair feature = FindFeaturePair(SubOrigin, selectedDefor, oX, oY, OAP, OAP);
                if (feature.OP != feature.DP)
                {
                    FPoint DAP = new FPoint(feature.DP.X, feature.DP.Y, Range);
                    for (int l = 0; l < subSize.Height; i++)
                    {
                        for (int m = 0; m < subSize.Width; m++)
                        {
                            SubOrigin[l][m] = ExtendOrigin[oY + l][oX + m];
                        }
                    }
                    feature = FindFeaturePair(SubOrigin, ExtendDefor, oX * Multiple, oY * Multiple, OAP, DAP);
                    featurePairs.Add(feature);
                }
            }
            return featurePairs.ToArray();
        }

        public void FindSubAreaPair(Size subSize, int step = 1)
        {
            int SW = SelectedOrigin[0].Length, SH = SelectedOrigin.Length;
            int XStart = subSize.Width / 2;
            int YStart = subSize.Height / 2;
            int XEnd = SW - subSize.Width / 2 - subSize.Width;
            int YEnd = SH - subSize.Height / 2 - subSize.Height;
            FeaturePair[][] featurePairs = new FeaturePair[YEnd - YStart][];

            for (int i = YStart; i < YEnd; i++)
            {
                FeaturePair[] tempPairs = new FeaturePair[XEnd - XStart];
                // for (int j = XStart; j < XEnd; j++)
                Parallel.For(XStart, XEnd, j =>
                {
                    int[][] subOrigin = ImageAnalyse.InitMatrix<int>(subSize);

                    for (int l = 0; l < subSize.Height; l++)
                    {
                        for (int m = 0; m < subSize.Width; m++)
                        {
                            subOrigin[l][m] = SelectedOrigin[i + l][j + m];
                        }
                    }

                    FPoint OAP = new FPoint(SelectPoint.X + j, SelectPoint.Y + i, 1);

                    tempPairs[j - XStart] = FindFeaturePairByStep(subOrigin, SelectedDeformation, j, i, OAP, OAP);
                });

                featurePairs[i] = tempPairs;
            }

            FeaturePairs = featurePairs;
        }


        /// <summary>
        /// 从指定图片中提取指定大小和位置的图像
        /// </summary>
        /// <param name="scale">原图片</param>
        /// <param name="X">指定位置</param>
        /// <param name="Y">指定位置</param>
        /// <param name="width">指定尺寸</param>
        /// <param name="height">指定尺寸</param>
        /// <returns></returns>
        static public int[][] GetGrayMatrix(int[][] scale, int X, int Y, int width, int height)
        {
            int[][] matrix = new int[height][];
            int[] temp;
            int SW = scale[0].Length, SH = scale.Length;
            if (X + width >= SW || Y + height >= SH)
                throw new RankException("数组索引超出界限!");
            for (int i = 0; i < height; i++)
            {
                temp = new int[width];
                for (int j = 0; j < width; j++)
                {
                    temp[j] = scale[Y + i][X + j];
                }
                matrix[i] = temp;
            }
            return matrix;
        }
        static public int[][] GetGrayMatrix(int[][] scale, int X, int Y, Size selectSize)
        {
            return GetGrayMatrix(scale, X, Y, selectSize.Width, selectSize.Height);
        }
        static public int[][] GetGrayMatrix(int[][] scale, Point selectedPoing, Size selectedSize)
        {
            return GetGrayMatrix(scale, selectedPoing.X, selectedPoing.Y, selectedSize.Width, selectedSize.Height);
        }

        /// <summary>
        /// 直接获得源图像中指定位置和大小的一维数组
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        static public int[] GetGrayArray(int[][] scale, int X, int Y, int width, int height)
        {
            int len = width * height;
            int[] array = new int[len];
            int position = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[position] = scale[i + Y][j + X];
                    position++;
                }
            }
            return array;
        }
        static public int[] GetGrayArray(int[][] scale, int X, int Y, Size size)
        {
            return GetGrayArray(scale, X, Y, size.Width, size.Height);
        }
        static public int[] GetGrayArray(int[][] img)
        {
            int width = img[0].Length, height = img.Length;
            int position = 0, len = width * height;
            int[] array = new int[len];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[position] = img[i][j];
                    position++;
                }
            }
            return array;
        }
        /// <summary>
        /// 计算子区域的相关系数
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="defor"></param>
        /// <returns></returns>
        static public double CalCorration(int[][] origin, int[][] defor)
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
        static public double CalCorration(int[] origin, int[] defor)
        {
            bool isZero = true;
            foreach (var item in origin)
            {
                if (item != 0)
                {
                    isZero = false;
                    break;
                }
            }
            if (isZero)
                return 0;
            isZero = true;
            foreach (var item in defor)
            {
                if (item != 0)
                {
                    isZero = false;
                    break;
                }
            }
            if (isZero)
                return 0;
            return MathHelper.CorrelationCoefficient(origin, defor);
        }
        /// <summary>
        /// 边界调整,若值超出边界则取关于边界对称
        /// </summary>
        /// <param name="n"></param>
        /// <param name="lBorder"></param>
        /// <param name="uborder"></param>
        /// <returns></returns>
        static public int BorderAdjust(int n, int lBorder, int uborder)
        {
            if (n < lBorder)
                return 2 * lBorder - n;
            if (n > uborder)
                return 2 * uborder - n;
            return n;
        }

        /// <summary>
        /// 计算键值对阵列梯度和当前梯度大小与最大梯度大小的比值
        /// </summary>
        /// <param name="featurePairs"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        static public FeaturePair[][] CalPairDegree(FeaturePair[][] featurePairs, int step = 18)
        {
            if (IsEmptyFeaturePairs(featurePairs))
                throw new ArgumentNullException("键值对是空的!");

            // 保存最大梯度
            double max = 0;
            // 计算键值对阵列的梯度
            int width = featurePairs[0].Length, height = featurePairs.Length;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (featurePairs[i][j].Displacement.S == 0)
                    {
                        double dx = 0;
                        int border = j + step >= width ? width : j + step;
                        for (int l = j; l < border; l++)
                        {
                            dx += featurePairs[i][l].Displacement.Dx;
                        }
                        if (j > 0)
                        {
                            border = j - step < 0 ? 0 : j - step;
                            for (int l = j - 1; l >= border; l--)
                            {
                                dx -= featurePairs[i][l].Displacement.Dx;
                            }
                        }
                        double dy = 0;
                        border = i + step >= height ? height : i + step;
                        for (int l = i; l < border; l++)
                        {
                            dy += featurePairs[l][j].Displacement.Dy;
                        }
                        if (i > 0)
                        {
                            border = i - step < 0 ? 0 : i - step;
                            for (int l = i - 1; l >= border; l--)
                            {
                                dy -= featurePairs[l][j].Displacement.Dy;
                            }
                        }
                        featurePairs[i][j].Gradient = Math.Abs(dx) + Math.Abs(dy);
                    }
                    else
                    {
                        featurePairs[i][j].Gradient = featurePairs[i][j].Displacement.S;
                    }
                    if (max < featurePairs[i][j].Gradient)
                        max = featurePairs[i][j].Gradient;
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    featurePairs[i][j].SetDgree(max);
                }
            }
            return featurePairs;
        }

        static public RGBImgStruct OutPutCloudDiagram(int[][] origin, FeaturePair[][] featurePairs, Point SelectPoint, Size SelectedSize)
        {
            featurePairs = CalPairDegree(featurePairs);
            Size OSize = ImageAnalyse.GetImgSize<int>(origin);
            Size subSize = featurePairs[0][0].CSize;
            int[] rGBImg = new int[OSize.Width * OSize.Height * 3], RGB;
            int posi = 0;

            for (int i = 0; i < OSize.Height; i++)
            {
                for (int j = 0; j < OSize.Width; j++)
                {
                    if (IsInSelectedArea(new Point(j, i), SelectPoint, SelectedSize, subSize))
                    {
                        rGBImg[posi] = 200;
                        rGBImg[posi + 1] = 0;
                        rGBImg[posi + 2] = 0;
                    }
                    else
                        rGBImg[posi] = rGBImg[posi + 1] = rGBImg[posi + 2] = origin[i][j];
                    posi += 3;
                }
            }

            foreach (var items in featurePairs)
            {
                foreach (var item in items)
                {
                    RGB = DrawHelper.GetColorDegree(item.Degree);
                    posi = ((int)item.OP.Y * OSize.Width + (int)item.OP.X) * 3;
                    rGBImg[posi] = RGB[2];
                    rGBImg[posi + 1] = RGB[1];
                    rGBImg[posi + 2] = RGB[0];
                }
            }

            return new RGBImgStruct(rGBImg, OSize);
        }

        static private bool IsInSelectedArea(Point currentPoint, Point SelectPoint, Size SelectedSize, Size subSize)
        {
            if (currentPoint.X < SelectPoint.X || currentPoint.X > SelectPoint.X + SelectedSize.Width - subSize.Width)
                return false;
            if (currentPoint.Y < SelectPoint.Y || currentPoint.Y > SelectPoint.Y + SelectedSize.Height - subSize.Height)
                return false;
            return true;
        }

        /// <summary>
        /// 判断搜索位移对是否都是位移为零的对
        /// </summary>
        /// <param name="featurePairs"></param>
        /// <returns></returns>
        static public bool IsEmptyFeaturePairs(FeaturePair[] featurePairs)
        {
            foreach (var item in featurePairs)
            {
                if (item.OP != item.DP)
                    return false;
            }
            return true;
        }

        static public bool IsEmptyFeaturePairs(FeaturePair[][] featurePairs)
        {
            foreach (var items in featurePairs)
            {
                foreach (var item in items)
                {
                    if (item.OP != item.DP)
                        return false;
                }
            }
            return true;
        }

    }

    /// <summary>
    /// 键值对类型, 包括原图像位置和对应图像位置,和选取的子区域大小和位移结构
    /// </summary>
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
        public DisplacementStruct Displacement;
        /// <summary>
        /// 位移场的梯度
        /// </summary>
        public double Gradient = 0;
        /// <summary>
        /// 表示位移占总大小的程度,0 - 100;
        /// </summary>
        public int Degree = 0;
        public FeaturePair(FPoint op, FPoint dp, Size cSize)
        {
            this.OP = op;
            this.DP = dp;
            this.CSize = cSize;
            CalDisplacement();
        }
        public override string ToString()
        {
            return $"origin, {OP.ToString()}, defor, {DP.ToString()}, S, {Displacement.S}, size, {CSize.ToString()}; range, {OP.Range}, Gradient: {Gradient},Degree: {Degree}";
        }
        /// <summary>
        /// 计算位移信息
        /// </summary>
        public void CalDisplacement()
        {
            double w = CSize.Width * DP.Range;
            double h = CSize.Height * DP.Range;
            double X = DP.X - OP.X;
            double Y = DP.Y - OP.Y;
            double DX = X / w;
            double DY = Y / h;
            double D2X = 2.0 * X / Math.Pow(w, 2);
            double D2Y = 2.0 * Y / Math.Pow(h, 2);
            this.Displacement = new DisplacementStruct(X, Y, DX, DY, D2X, D2Y);
        }
        public void SetDgree(double max)
        {
            if (Gradient != 0)
                this.Degree = (int)(Gradient * 100 / max);
            else
                this.Degree = (int)(Displacement.S * 100 / max);
        }
    }
    public class DisplacementStruct
    {
        public bool IsNull = true;
        /// <summary>
        /// 水平位移
        /// </summary>
        public double DeltaX;
        /// <summary>
        /// 竖直位移
        /// </summary>
        public double DeltaY;
        /// <summary>
        /// 水平线应变
        /// </summary>
        public double Dx;
        /// <summary>
        /// 竖直线应变
        /// </summary>
        public double Dy;
        /// <summary>
        /// 水平移动的二阶导数
        /// </summary>
        public double D2x;
        /// <summary>
        /// 竖直移动的二阶导数
        /// </summary>
        public double D2y;
        /// <summary>
        /// 位移距离
        /// </summary>
        public double S;
        /// <summary>
        /// 构造位移结构类型,计算相关参数
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="d2x"></param>
        /// <param name="d2y"></param>
        public DisplacementStruct(double deltaX, double deltaY, double dx, double dy, double d2x, double d2y)
        {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.Dx = dx;
            this.Dy = dy;
            this.D2x = d2x;
            this.D2y = d2y;
            this.S = Math.Sqrt(DeltaX * DeltaX + DeltaY * deltaY);
            // this.S = Math.Abs(DeltaX);
            this.IsNull = false;
        }
        /// <summary>
        /// 输出类的细节信息
        /// </summary>
        /// <returns></returns>
        public string ToDetailString()
        {
            string detail = "";
            detail += $"水平位移: {DeltaX}, 竖直位移: {DeltaY} \n";
            detail += $"水平线应变: {Dx}, 竖直线应变: {Dy}, 角应变: {Dx * Dy} \n";
            detail += $"移动的直线距离: {S}";
            return detail;
        }
        public override string ToString()
        {
            return $"DeltaX: {DeltaX}, DeltaY: {DeltaY}, S: {S}";
        }
    }

}