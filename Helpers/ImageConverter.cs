using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        public static BitmapImage StreamtoImage(Stream stream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static byte[]? ImageSourcetoByteArray(ImageSource imageSource)
        {
            if (imageSource is BitmapSource bitmapSource)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);

                    return memoryStream.ToArray();
                }
            }

            return null;
        }

        public static BitmapImage ResizeBitmap(BitmapImage bitmapImage, double ratio)
        {
            TransformedBitmap transformedBitmap = new TransformedBitmap(bitmapImage, new ScaleTransform(ratio, ratio));

            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(transformedBitmap));
                encoder.Save(stream);

                byte[] bytes = stream.ToArray();
                return ByteArraytoImage(bytes);
            }
        }

        public static BitmapImage ResizeBitmapUniformToFill(BitmapImage bitmapImage, int targetSize)
        {
            double ratio;
            if (bitmapImage.Width < bitmapImage.Height) ratio = (double)targetSize / bitmapImage.PixelWidth;
            else ratio = (double)targetSize / bitmapImage.PixelHeight;

            return ResizeBitmap(bitmapImage, ratio);
        }
    }
}
