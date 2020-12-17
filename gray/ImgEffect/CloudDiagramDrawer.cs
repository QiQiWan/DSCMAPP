using System.Drawing;
using System.Windows.Forms;

namespace Gray.ImgEffect
{
    class CloudDiagramDrawer
    {
        /// <summary>
        /// 直接绘制图片的原始画布
        /// </summary>
        public Graphics CDDrawer;
        /// <summary>
        /// 加载画布时的原始图片
        /// </summary>
        private Image OriginImg;
        /// <summary>
        /// 一层中间画布
        /// </summary>
        private Graphics MGraphics;
        /// <summary>
        /// 保存每次阶段绘图的结果
        /// </summary>
        private Image FinishImg;
        /// <summary>
        /// 绘制的颜色
        /// </summary>
        private Color DrawColor;
        /// <summary>
        /// 计算出的位移对应对
        /// </summary>
        private FeaturePair[] featurePairs;

        private bool StartDraw = false;
        private Point StartPoint;

        public CloudDiagramDrawer(Graphics g, Image img, FeaturePair[] featurePairs)
        {
            this.CDDrawer = g;
            this.OriginImg = (Image)img.Clone();
            this.FinishImg = (Image)img.Clone();
            this.featurePairs = featurePairs;
        }

        public void DrawStart(MouseEventArgs e)
        {
            StartDraw = true;
            StartPoint = new Point(e.X, e.Y);
        }

        public void DrawEnd()
        {
            StartDraw = false;
        }

    }
}
