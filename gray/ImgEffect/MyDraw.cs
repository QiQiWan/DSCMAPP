using System.Drawing;

namespace Gray
{
    class imageCollection
    {
        public Bitmap bitmap = null;
        public ImageCollectionMode mode;
        public string filePath = null;
        public int grayLevel;

        public imageCollection()
        {
        }
        public imageCollection(Bitmap bitmap, string filePath, ImageCollectionMode mode)
        {
            this.bitmap = bitmap;
            this.filePath = filePath;
            this.mode = mode;
            UpdateGray();
        }
        public imageCollection(string filePath, ImageCollectionMode mode)
        {
            this.bitmap = new Bitmap(Image.FromFile(filePath));
            this.filePath = filePath;
            this.mode = mode;
            UpdateGray();
        }
        public void ChangeImage(string filePath)
        {
            bitmap = new Bitmap(Image.FromFile(filePath));
            bitmap = new Bitmap(Image.FromFile(filePath));
            this.filePath = filePath;
            UpdateGray();
        }

        public void UpdateGray()
        {
            grayLevel = RGBGraying.GetAverageGrayLevel(bitmap);
        }

        public override string ToString()
        {
            if (mode == ImageCollectionMode.Orgin)
                return "参考图片";
            if (mode == ImageCollectionMode.Deformation)
                return "变形图片";
            return "未定义";
        }
    }
    public enum ImageCollectionMode { Orgin, Deformation };
    class imageMap
    {
        public imageMap(int X, int Y, Bitmap bitmap)
        {
            this.positionX = X;
            this.positionY = Y;
            this.bitmap = bitmap;
        }
        public readonly int positionX;
        public readonly int positionY;
        public readonly Bitmap bitmap;
    }
}
