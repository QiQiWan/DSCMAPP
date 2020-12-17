using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gray.ImgEffect.Helper
{
    class DrawHelper
    {
        public Graphics DrawTools_Graphics;//目标绘图板
        private Pen p;
        private Image orginalImg;//原始画布，用来保存已完成的绘图过程
        private Color drawColor = Color.Red;//绘图颜色
        private Graphics newgraphics;//中间画板
        private Image finishingImg;//中间画布，用来保存绘图过程中的痕迹

        /// <summary>
        /// 绘图颜色
        /// </summary>
        public Color DrawColor
        {
            get { return drawColor; }
            set
            {
                drawColor = value;
                p.Color = value;
            }
        }

        /// <summary>
        /// 原始画布
        /// </summary>
        public Image OrginalImg
        {
            get { return orginalImg; }
            set
            {
                finishingImg = (Image)value.Clone();
                orginalImg = (Image)value.Clone();
            }
        }


        /// <summary>
        /// 表示是否开始绘图
        /// </summary>
        public bool startDraw = false;

        /// <summary>
        /// 绘图起点
        /// </summary>
        public PointF startPointF;

        /// <summary>
        /// 初始化绘图工具
        /// </summary>
        /// <param name="g">绘图板</param>
        /// <param name="c">绘图颜色</param>
        /// <param name="img">初始画布</param>
        public DrawHelper(Graphics g, Color c, Image img)
        {
            DrawTools_Graphics = g;
            drawColor = c;
            p = new Pen(c, 2);
            finishingImg = (Image)img.Clone();
            orginalImg = (Image)img.Clone();
        }

        /// <summary>
        /// 绘制直线，矩形，圆形
        /// </summary>
        /// <param name="e">鼠标参数</param>
        /// <param name="sType">绘图类型</param>
        public void Draw(MouseEventArgs e, string sType)
        {
            if (startDraw)
            {
                //为防止造成图片抖动，防止记录不必要的绘图过程中的痕迹，我们先在中间画板上将图片完成，然后在将绘制好的图片一次性画到目标画板上
                //步骤1实例化中间画板，画布为上一次绘制结束时的画布的副本（如果第一次绘制，那画布就是初始时的画布副本）
                //步骤2按照绘图样式在中间画板上进行绘制
                //步骤3将绘制结束的图片画到中间画布上
                //因为我们最终绘制结束时的图片应该是在鼠标松开时完成，所以鼠标移动中所绘制的图片都只画到中间画布上,但仍需要显示在目标画板上，否则鼠标移动过程中我们就看不到效果。
                //当鼠标松开时，才把最后的那个中间图片画到原始画布上

                Image img = (Image)orginalImg.Clone();//为防止直接改写原始画布，我们定义一个新的image去得到原始画布
                newgraphics = Graphics.FromImage(img);//实例化中间画                    板
                switch (sType)
                {
                    case "Line":
                        {//画直线
                            newgraphics.DrawLine(p, startPointF, new PointF(e.X, e.Y)); break;
                        }
                    case "Rect":
                        {//画矩形
                            float width = Math.Abs(e.X - startPointF.X);//确定矩形的宽
                            float heigth = Math.Abs(e.Y - startPointF.Y);//确定矩形的高
                            PointF rectStartPointF = startPointF;
                            if (e.X < startPointF.X)
                            {
                                rectStartPointF.X = e.X;
                            }
                            if (e.Y < startPointF.Y)
                            {
                                rectStartPointF.Y = e.Y;
                            }
                            newgraphics.DrawRectangle(p, rectStartPointF.X, rectStartPointF.Y, width, heigth);
                            break;
                        }
                    case "Circle":
                        {//画圆形
                            newgraphics.DrawEllipse(p, startPointF.X, startPointF.Y, e.X - startPointF.X, e.Y - startPointF.Y); break;
                        }
                    case "FillRect":
                        {//画实心矩形
                            float width = Math.Abs(e.X - startPointF.X);//确定矩形的宽
                            float heigth = Math.Abs(e.Y - startPointF.Y);//确定矩形的高
                            PointF rectStartPointF = startPointF;
                            if (e.X < startPointF.X)
                            {
                                rectStartPointF.X = e.X;
                            }
                            if (e.Y < startPointF.Y)
                            {
                                rectStartPointF.Y = e.Y;
                            }
                            newgraphics.FillRectangle(new SolidBrush(drawColor), rectStartPointF.X, rectStartPointF.Y, width, heigth);
                            break;
                        }
                    case "FillCircle":
                        {//画实心圆
                            newgraphics.FillEllipse(new SolidBrush(drawColor), startPointF.X, startPointF.Y, e.X - startPointF.X, e.Y - startPointF.Y); break;
                        }
                }
                newgraphics.Dispose();//绘图完毕释放中间画板所占资源
                newgraphics = Graphics.FromImage(finishingImg);//另建一个中间画板,画布为中间图片
                newgraphics.DrawImage(img, 0, 0);//将图片画到中间图片
                newgraphics.Dispose();
                DrawTools_Graphics.DrawImage(img, 0, 0);//将图片画到目标画板上
                img.Dispose();
            }

        }

        public void EndDraw()
        {
            startDraw = false;
            //为了让完成后的绘图过程保留下来，要将中间图片绘制到原始画布上
            newgraphics = Graphics.FromImage(orginalImg);
            newgraphics.DrawImage(finishingImg, 0, 0);
            newgraphics.Dispose();
        }

        /// <summary>
        /// 橡皮方法
        /// </summary>
        /// <param name="e">鼠标参数</param>
        public void Eraser(MouseEventArgs e)
        {
            if (startDraw)
            {
                newgraphics = Graphics.FromImage(finishingImg);
                newgraphics.FillRectangle(new SolidBrush(Color.White), e.X, e.Y, 20, 20);
                newgraphics.Dispose();
                DrawTools_Graphics.DrawImage(finishingImg, 0, 0);
            }
        }

        /// <summary>
        /// 铅笔方法
        /// </summary>
        /// <param name="e">鼠标参数</param>
        public void DrawDot(MouseEventArgs e)
        {
            if (startDraw)
            {
                newgraphics = Graphics.FromImage(finishingImg);
                PointF currentPointF = new PointF(e.X, e.Y);
                newgraphics.DrawLine(p, startPointF, currentPointF);
                startPointF = currentPointF;
                newgraphics.Dispose();
                DrawTools_Graphics.DrawImage(finishingImg, 0, 0);
            }
        }

        /// <summary>
        /// 清除变量，释放内存
        /// </summary>
        public void ClearVar()
        {
            DrawTools_Graphics.Dispose();
            finishingImg.Dispose();
            orginalImg.Dispose();
            p.Dispose();
        }

        /// <summary>
        /// 返回不同的变形程度对应的颜色
        /// </summary>
        /// <param name="Degree"></param>
        /// <returns></returns>
        public static int[] GetColorDegree(int Degree)
        {
            int R, G, B;

            int period = (int)(1.0 * Degree / 25.1);

            switch (period)
            {
                // Degree <= 25
                case 0:
                    {
                        R = 0;
                        G = 8 * Degree;
                        B = 200;
                        break;
                    }
                // Degree <= 50
                case 1:
                    {
                        R = 0;
                        G = 200;
                        B = 200 - (Degree - 25) * 8;
                        break;
                    }
                // Degree <= 75
                case 2:
                    {
                        R = 8 * (Degree - 50);
                        G = 200;
                        B = 0;
                        break;
                    }
                // Degree <= 100
                case 3:
                    {
                        R = 200;
                        G = 200 - (Degree - 75) * 8;
                        B = 0;
                        break;
                    }
                default:
                    {
                        R = 200;
                        G = 200;
                        B = 0;
                        break;
                    }
            }
            return new int[] { R, G, B};
        }
    }

    /// <summary>
    /// 为了不在其它类中引用 System.Drawing; 名字空间, 构造此类, 彩色图像使用一维 Byte 数组保存,同时保存图像的尺寸
    /// </summary>
    class RGBImgStruct
    {
        /// <summary>
        /// 图像的字节数组
        /// </summary>
        public byte[] RGBImgByte;

        /// <summary>
        /// 图像的整型数组
        /// </summary>
        public int[] RGBImgInt;

        /// <summary>
        /// 图像的尺寸
        /// </summary>
        public int Width;
        public int Height;

        public Size ImgSize
        {
            get { return ImgSize; }
            set {
                this.Width = value.Width;
                this.Height = value.Height;
                if (RGBImgByte.Length != Width * Height * 3)
                    throw new Exception("图片数组长度与所给尺寸大小不一致!");
            }
        }

        public RGBImgStruct(byte[] rGBImg, int width, int height)
        {
            this.RGBImgByte = rGBImg;
            ImgSize = new Size(width, height);
        }

        public RGBImgStruct(int[] rGBImg, int width, int height)
        {
            this.RGBImgByte = ImageHelper.IntArrToByteArr(rGBImg);
            this.RGBImgInt = rGBImg;

            ImgSize = new Size(width, height);
        }

        public RGBImgStruct(byte[] rGBImg, Size size)
        {
            this.RGBImgByte = rGBImg;
            this.ImgSize = size;
        }
        public RGBImgStruct(int[] rGBImg, Size size)
        {
            this.RGBImgByte = ImageHelper.IntArrToByteArr(rGBImg);
            this.RGBImgInt = rGBImg;
            Console.WriteLine(123);
            ImgSize = size;
        }

        public Bitmap GetBitmap()
        {
            return ImageHelper.WriteImg(RGBImgByte, Width, Height);
        }
    }
}
