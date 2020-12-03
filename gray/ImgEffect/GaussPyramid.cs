using System.Collections.Generic;

namespace Gray
{
    class GaussPyramid
    {
        public readonly ImageCollection ImageColl;
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

        public GaussPyramid(ImageCollection imageCollection)
        {
            SamplingGroups.Clear();
            ImageColl = imageCollection;
            OriginBitmap = ImageHelper.GetGrayMatrix(imageCollection.GrayBitmap);
            Size tempsize;
            int[][] tempImg = OriginBitmap;
            do
            {
                SamplingGroups.Add(new SamplingGroup(tempImg));
                tempImg = DownSampling(tempImg);
                tempsize = ImageAnalyse.GetImgSize(tempImg);
            } while (tempsize.GetMinSize() > 2);
        }
    }
    /// <summary>
    /// 采样组
    /// </summary>
    class SamplingGroup
    {
        public readonly SamplingObject[] SamplingObjects;
        public readonly double[] σ;
        public readonly int Count;
        public readonly Size ImgSize;
        public SamplingGroup(int[][] bitmap)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            ImgSize = new Size(width, height);
            σ = new double[] { 1, 5, 10, 12, 18, 21 };
            Count = σ.Length;
            SamplingObjects = new SamplingObject[Count];
            for (int i = 0; i < Count; i++)
            {
                SamplingObjects[i] = new SamplingObject(bitmap, σ[i]);
            }
        }
        public SamplingGroup(int[][] bitmap, double[] σ)
        {
            int width = bitmap[0].Length, height = bitmap.Length;
            ImgSize = new Size(width, height);
            this.σ = σ;
            Count = σ.Length;
            SamplingObjects = new SamplingObject[Count];
            for (int i = 0; i < Count; i++)
            {
                SamplingObjects[i] = new SamplingObject(bitmap, σ[i]);
            }
        }
    }
    class SamplingObject
    {
        public readonly int Width;
        public readonly int Height;
        public readonly double σ;
        public readonly int[][] OriginImg;
        public readonly int[][] GauBlurImg;
        public SamplingObject(int[][] bitmap, double σ)
        {
            this.σ = σ;
            this.OriginImg = bitmap;
            this.GauBlurImg = ImageAnalyse.GaussionBlur(bitmap, σ);
            this.Width = bitmap[0].Length;
            this.Height = bitmap.Length;
        }
        public SamplingObject(int[][] bitmap)
        {
            this.σ = 1;
            this.OriginImg = bitmap;
            this.GauBlurImg = ImageAnalyse.GaussionBlur(bitmap, this.σ);
            this.Width = bitmap[0].Length;
            this.Height = bitmap.Length;
        }
    }
}
