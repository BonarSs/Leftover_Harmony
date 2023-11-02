using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Leftover_Harmony.Helpers
{
    public static class ImageConverter
    {
        public static BitmapImage ByteArraytoImage(byte[] bytes)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new System.IO.MemoryStream(bytes);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static TransformedBitmap ResizeBitmap(BitmapImage bitmapImage, double ratio)
        {
            return new TransformedBitmap(bitmapImage, new ScaleTransform(ratio, ratio));
        }
    }
}
