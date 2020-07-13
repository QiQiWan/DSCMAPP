using MyTools.OpenProperties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Gray
{
    //对所有的for循环都是用多线程任务,防止UI阻塞

    //对所有的费时任务,都应设置状态指示,防止程序假死

    //对所有的多线程任务都应防止同时使用同一变量,防止线程丢失

    //使用缓存,以减少重复计算

    public partial class Main: Form
    {
        public Main()
        {
            InitializeComponent();
            Shell.WriteLine(">>> 内存处理器监控开始!");
            timer1.Interval = 1000;
            timer1.Start();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            StatusBar.SetCPUStatus(CPUDisplay);
            StatusBar.SetRAMStatus(RAMDisplay);
        }
        #region 窗口内变量
        /// <summary>
        /// 在此对变量进行注解
        /// </summary>
        private imageCollection[] imageCollection = new imageCollection[2];//本窗口的两幅主图像,包括参考图像和变形图像
        private Bitmap RGBBitmap = null;//原始彩色图像
        private Bitmap GrayBitmap = null;//处理后的灰度图像
        private Stopwatch stopwatch = new Stopwatch();
        private int index = -1;
        //private readonly object LOCK;//锁定线程

        //在多线程内使用控件委托
        StatusBar.ChangeStatusDelegate changeStatus = new StatusBar.ChangeStatusDelegate(StatusBar.ChangeStatus);
        StatusBar.UpDateProcessDelegate upDateProcess = new StatusBar.UpDateProcessDelegate(StatusBar.UpDateProcess);
        StatusBar.SetTickTimeDelegate setTickTime = new StatusBar.SetTickTimeDelegate(StatusBar.SetTickTime);
        #endregion

        private void Main_Load(object sender, EventArgs e)
        {
            imageCollection[0] = new imageCollection();
            imageCollection[1] = new imageCollection();
            imageCollection[0].mode = ImageCollectionMode.Orgin;
            imageCollection[1].mode = ImageCollectionMode.Deformation;
            imageCol.Items.AddRange(imageCollection);
        }

        #region 图片切换操作
        private void ImageCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = imageCol.Items.IndexOf(imageCol.SelectedItem);
            StatusBar.ChangeStatus(process);
            if (imageCollection[index].bitmap == null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "选择一个参考图片";
                dialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp;";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageCollection[index] = new imageCollection(dialog.FileName, index == 0 ? ImageCollectionMode.Orgin : ImageCollectionMode.Deformation);
                    dialog.Dispose();
                }
                else
                {
                    Shell.WriteLine("### 未选择图片,操作结束!");
                    return;
                }
            }
            ChangeCurrentImage(imageCollection[index]);
            StatusBar.ChangeStatus(process);

            ChangePicBox();
        }
        private void Shift_Click(object sender, EventArgs e)
        {
            if (freeDraw)
            {
                Shell.WriteLine(">>> 未关闭自由绘制,不能切换图片!");
                return;
            }
            if (index < 0)
                imageCol.SelectedIndex = 0;
            else
                imageCol.SelectedIndex = index == 0 ? 1 : 0;
        }
        private void ChooseFile_Click(object sender, EventArgs e)
        {
            if (imageCol.SelectedItem == null)
            {
                Shell.WriteLine("### 阻止未选择的赋值!");
                return;
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择一个图像文件";
            dialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileNameBox.Text = dialog.FileName;
                dialog.Dispose();
            }
            else
            {
                Shell.WriteLine(">>> 未选择图片");
                return;
            }
            StatusBar.ChangeStatus(process, StatusBar.StatusMode.doing);

            Shell.WriteLine(">>> 已选择文件: " + fileNameBox.Text);
            imageCollection[index].ChangeImage(fileNameBox.Text);

            ChangeCurrentImage(imageCollection[index]);
            GC.Collect();

            Shell.WriteLine(Shell.PreDefineSearchRAM());

            StatusBar.ChangeStatus(process, StatusBar.StatusMode.done);

            ChangePicBox();
        }
        private void ChangeCurrentImage(imageCollection imageCollection)
        {
            RGBBitmap = imageCollection.bitmap;
            GrayBitmap = null;
            fileNameBox.Text = imageCollection.filePath;
            orginBitmap = null;
            DisplayImage(previewBox, imageCollection.bitmap);
        }
        private void DisplayImage(PictureBox pictureBox, Bitmap bitmap)
        {
            previewBox.Height = (int)Math.Round((double)bitmap.Height / (double)bitmap.Width * (double)pictureBox.Width, 0);
            previewBox.Image = bitmap;
            ChangeLimit(pictureBox.Width, pictureBox.Height);
        }

        /// <summary>
        /// 当前显示图标改变之后触发的事件
        /// </summary>
        private void ChangePicBox()
        {
            StatusBar.SetGrayLevel(average, imageCollection[index].grayLevel);

        }
        #endregion

        private void Graying_Click(object sender, EventArgs e)
        {
            if (RGBBitmap == null)
            {
                Shell.WriteLine("### 未选择图片, 操作中止!");
                return;
            }

            Shell.WriteLine(">>> 正在灰度中...");
            StatusBar.ChangeStatus(process);

            stopwatch.Start();
            DisplayImage(previewBox, RGBGraying.GetGrayImage(new Bitmap(RGBBitmap)));
            GrayBitmap = new Bitmap(previewBox.Image);//更新灰度图片
            stopwatch.Stop();

            StatusBar.SetTickTime(timeTick, stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Reset();

            StatusBar.ChangeStatus(process);
            Shell.WriteLine(">>> 灰度完成!");
            Shell.WriteLine(Shell.PreDefineSearchRAM());

            GC.Collect();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ChangeLimit(int width, int height)
        {
            picWidth.Text = ">" + width;
            picHeight.Text = ">" + height;
        }

        private void BW2_Click(object sender, EventArgs e)
        {
            if (GrayBitmap == null)
            {
                Shell.WriteLine("### 未选择灰度图片, 操作中止!");
                return;
            }
            int level = int.Parse(textBox1.Text);

            Shell.WriteLine(">>> 正在二值化...");
            StatusBar.ChangeStatus(process);
            stopwatch.Start();

            DisplayImage(previewBox, RGBGraying.Get2BWImage(GrayBitmap, level));

            stopwatch.Stop();
            StatusBar.SetTickTime(timeTick, stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Reset();

            StatusBar.ChangeStatus(process);
            Shell.WriteLine(">>> 二值化完成");
            GC.Collect();
        }

        #region 图形绘制
        #region 绘制特定形状
        private void Equal_Click(object sender, EventArgs e)
        {
            if (RGBBitmap == null)
            {
                Shell.WriteLine("### 未选择图片, 操作中止!");
                return;
            }
            if (X.Text == "" || Y.Text == "" || RecWidth.Text == "" || RecHeight.Text == "")
            {
                Shell.WriteLine("### 右边四个框必填, 操作中止!");
                return;
            }
            StartDraw();
            using (Image TempImage = previewBox.Image)
            {
                using (Graphics g = Graphics.FromImage(TempImage))
                {
                    int x = int.Parse(X.Text);
                    int y = int.Parse(Y.Text);
                    int width = int.Parse(RecWidth.Text);
                    int height = int.Parse(RecHeight.Text);
                    Shell.WriteLine(">>> 获取长宽高");
                    if ((x + width) > previewBox.Image.Width || (y + height) > previewBox.Image.Height)
                    {
                        Shell.WriteLine("### 定位超限!");
                        return;
                    }
                    g.DrawRectangle(new Pen(Color.Green, 2), x, y, width, height);
                    DisplayImage(previewBox, new Bitmap(TempImage));
                }
            }
        }
        #endregion
        #region 自由绘制
        private static bool freeDraw = false;
        private static bool drawing = false;
        private static Point P1 = new Point();
        private static Bitmap orginBitmap = null;
        private void FreeDraw_Click(object sender, EventArgs e)
        {
            if (previewBox.Image == null)
            {
                Shell.WriteLine("### 需打开一张图片才能绘制!");
                return;
            }
            freeDraw = !freeDraw;
            if (freeDraw)
            {
                FreeDraw.Text = "自由绘制(开)";
                StartDraw();
                Shell.WriteLine("$$$ 打开自由绘制");
            }
            else
            {
                //updateImage();//关闭自由绘制后保存图片//不需要保存图片
                FreeDraw.Text = "自由绘制(关)";
                Shell.WriteLine("$$$ 关闭自由绘制");
            }
            GC.Collect();
        }
        private void PreviewBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!freeDraw)
                return;
            drawing = true;
            P1 = new Point(e.X, e.Y);
        }
        private void PreviewBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!freeDraw)
                return;
            drawing = false;
        }
        private void PreviewBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!freeDraw)
                return;
            if (!drawing)
                return;
            if (e.Button != MouseButtons.Left)
                return;
            try
            {
                using (Image tempImage = previewBox.Image)
                {
                    using (Graphics g = Graphics.FromImage(tempImage))
                    {
                        float widthPer = (float)tempImage.Width / (float)previewBox.Width;
                        float heightPer = (float)tempImage.Height / (float)previewBox.Height;
                        float widthInsert = 1 / widthPer;
                        float heightInsert = 1 / heightPer;
                        Point currentPoint = new Point((int)(((float)e.X + widthInsert) * widthPer), (int)(((float)e.Y + widthInsert) * widthPer));
                        P1.X = (int)(((float)P1.X + widthInsert) * widthPer);
                        P1.Y = (int)(((float)P1.Y + widthInsert) * widthPer);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.DrawLine(new Pen(Color.Green, 2), P1, currentPoint);
                        P1 = new Point(e.X, e.Y);
                        DisplayImage(previewBox, new Bitmap(tempImage));
                        SetMousePosition(e.X, e.Y);
                    }
                }
            }
            catch (Exception err)
            {
                Shell.WriteLine("### " + err.Message);
            }
        }
        private void ClearDraw_Click(object sender, EventArgs e)
        {
            if (orginBitmap == null)
                return;
            DisplayImage(previewBox, orginBitmap);
            orginBitmap = null;
        }
        private void StartDraw()
        {
            if (previewBox.Image == null)
                return;
            if (orginBitmap == null)
                orginBitmap = new Bitmap(previewBox.Image);
        }
        private void SetMousePosition(int x, int y)
        {
            mouseStrip.Text = "鼠标: " + x + ", " + y;
        }
        #endregion
        #endregion
        #region 限制输入框只能输入数字
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        private void X_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        private void Y_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        private void RecWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        private void RecHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }
        private void WidthLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))
                    e.Handled = true;
        }
        #endregion
        private void EstablishCoordinate_Click(object sender, EventArgs e)
        {
            if (previewBox.Image == null)
            {
                Shell.WriteLine("### 需要先有图片!");
                return;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (widthLimit.Text.Length < 1)
                return;
            if (imageCollection[0].bitmap == null || imageCollection[1].bitmap == null)
            {
                Shell.WriteLine("### 确保选好了参考图片和变形图片!");
                return;
            }
        }
        private void GetCorrelationMap_Click(object sender, EventArgs e)
        {
            previewBox.Image = RGBGraying.removeBG(previewBox.Image);
        }

        private void Average2BW_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(previewBox.Image);
            bitmap = RGBGraying.GetAve2BWImage(bitmap);
            DisplayImage(previewBox, bitmap);
        }

        private void RemoveBG_Click(object sender, EventArgs e)
        {
            previewBox.Image = RGBGraying.removeBG(previewBox.Image);
        }

        private void 退出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
