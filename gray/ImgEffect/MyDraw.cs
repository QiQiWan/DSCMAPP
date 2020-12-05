using System.Drawing;

namespace Gray
{
    class ImageCollection : object
    {
        public Bitmap OriginBitmap = null;
        public Bitmap GrayBitmap;
        public ImageCollectionMode mode;
        public string filePath = null;
        public int grayLevel;
        public ImageCollection()
        {
        }
        public ImageCollection(Bitmap bitmap, string filePath, ImageCollectionMode mode)
        {
            this.OriginBitmap = bitmap;
            this.filePath = filePath;
            this.mode = mode;
            UpdateGray();
        }
        public ImageCollection(string filePath, ImageCollectionMode mode)
        {
            this.OriginBitmap = new Bitmap(Image.FromFile(filePath));
            this.filePath = filePath;
            this.mode = mode;
            UpdateGray();
        }
        public void ChangeImage(string filePath)
        {
            OriginBitmap = new Bitmap(Image.FromFile(filePath));
            this.filePath = filePath;
            UpdateGray();
        }

        public void UpdateGray()
        {
            this.GrayBitmap = RGBGraying.GetGrayImage(OriginBitmap);
            // GrayBitmap = ImageAnalyse.GaussionBlur(GrayBitmap, 0.5);
            grayLevel = RGBGraying.GetAverageGrayLevel(GrayBitmap);
        }

        public override string ToString()
        {
            if (mode == ImageCollectionMode.Orgin)
                return "参考图片";
            if (mode == ImageCollectionMode.Deformation)
                return "变形图片";
            return "未定义";
        }
        public static bool operator ==(ImageCollection c1, ImageCollection c2)
        {
            if (c1.filePath == c2.filePath)
                return true;
            return false;
        }
        public static bool operator !=(ImageCollection c1, ImageCollection c2)
        {
            if (c1 == c2)
                return false;
            return true;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
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
