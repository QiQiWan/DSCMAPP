using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gray
{
    class ImageHelper
    {
        /// <summary>
        /// 用内存法获得图像的灰度矩阵
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        static public double[][] GetGrayMatrix(Bitmap bitmap)
        {
            byte[] rgbValues = GetImgArr(bitmap);
            double[][] matrix = new double[bitmap.Height][];

            int position = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                double[] temp = new double[bitmap.Width];
                for (int j = 0; j < bitmap.Width; j++)
                {
                    temp[j] = rgbValues[position];
                    position += 3;
                }
                matrix[i] = temp;
            }
            return matrix;
        }

        /// <summary>
        /// 用内存法获得彩图的像素矩阵
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        static public Color[][] GetColorMatrix(Bitmap bitmap)
        {
            byte[] rgbValues = GetImgArr(bitmap);

            Color[][] colors = new Color[bitmap.Height][];

            int position = 0;

            for (int i = 0; i < bitmap.Height; i++)
            {
                Color[] temp = new Color[bitmap.Width];
                for (int j = 0; j < bitmap.Width; j++)
                {
                    temp[j] = Color.FromArgb(rgbValues[position], rgbValues[position + 1], rgbValues[position + 2]);
                    position += 3;
                }
                colors[i] = temp;
            }
            return colors;
        }

        /// <summary>
        /// 内存法获取图片的三色数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        static public byte[] GetImgArr(Bitmap bitmap)
        {
            Rectangle rec = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitData = bitmap.LockBits(rec, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bitData.Scan0;

            int bytes = bitmap.Width * bitmap.Height * 3;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            bitmap.UnlockBits(bitData);

            return rgbValues;
        }
        static public Bitmap WriteImg(byte[] rgbValues, int width, int height)
        {
            if (rgbValues.Length != width * height && rgbValues.Length != width * height * 3)
                throw new Exception("颜色数组长度不符合图片尺寸！");
            Bitmap bitmap = new Bitmap(width, height);
            Rectangle rec = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rec, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            IntPtr intPtr = bitmapData.Scan0;

            if(rgbValues.Length == width * height)
            {
                byte[] temp = new byte[rgbValues.Length * 3];
                for(int i = 0; i < rgbValues.Length; i++)
                {
                    temp[3 * i] = temp[3 * i + 1] = temp[3 * i + 2] = rgbValues[i];
                }
                rgbValues = temp;
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, intPtr, rgbValues.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}
