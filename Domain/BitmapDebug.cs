using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.IO;

namespace Domain
{
    
    public class BitmapDebug
    {
        public Bitmap ImageOrig { get; set; }
        public Color[,] ImageColor { get; set; }
        public Bitmap ImageBlackAndWhite { get; set; }
        public Bitmap ImageProcessed { get; set; }
        public int[,] ImagePixcels{ get; set; }

        ObservableImage I { get; set; }

        public BitmapDebug(ObservableImage image)
        {
            I = image;
            ImageOrig = (Bitmap)Image.FromFile(image.PathData+"/"+ image.FileNameImageOrig, true);

            SetImageColor(ImageOrig);

            ImageBlackAndWhite = GetBlackandWhiteImage(ImageOrig, image.FileNameImageOrig, image.PathData + "/");

            ImageProcessed = GetProcessedImage(ImageBlackAndWhite, 40, 40, image.FileNameImageOrig, image.PathData + "/");
            
            
            SetImagePixcels();

        }

        public void SetImageColor(Bitmap image)
        {
            ImageColor = new Color[image.Height, image.Width];

            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    ImageColor[j, i] = image.GetPixel(i, j);
                }
            }

        }

        public Bitmap GetBlackandWhiteImage(Bitmap image, string fileName, string filePath)
        {

            Bitmap output = new Bitmap(image.Width, image.Height);

            for (int j = 0; j < image.Height; j++)
                for (int i = 0; i < image.Width; i++)
                {
                    UInt32 pixel = (UInt32)(image.GetPixel(i, j).ToArgb());
                    float R = (float)((pixel & 0x00FF0000) >> 16);
                    float G = (float)((pixel & 0x0000FF00) >> 8);
                    float B = (float)(pixel & 0x000000FF);
                    R = G = B = (R + G + B) / 3.0f;
                    UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                    output.SetPixel(i, j, Color.FromArgb((int)newPixel));
                }

            output.Save(filePath + "1"+fileName);
            I.FileNameImageBlackAndWhite = "1" + fileName;
            return output;
        }

        public Bitmap GetProcessedImage(Bitmap image, int x, int y, string fileName, string filePath)
        {
            Size size = new Size(x, y);
            Bitmap output = new Bitmap(image, size);

            output.Save(filePath + "2" + fileName);
            I.FileNameImageProcessed = "2" + fileName;
            

            return output;
        }

        public void SetImagePixcels()
        {
            ImagePixcels = new int[ImageProcessed.Height, ImageProcessed.Width];
            for (int j = 0; j < ImageProcessed.Height; j++)
                for (int i = 0; i < ImageProcessed.Width; i++)
                {
                    UInt32 pixel = (UInt32)(ImageProcessed.GetPixel(i, j).ToArgb());
                    float R = (float)((pixel & 0x00FF0000) >> 16);
                    float G = (float)((pixel & 0x0000FF00) >> 8);
                    float B = (float)(pixel & 0x000000FF);
                    R = G = B = (R + G + B) / 3.0f;
                    UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                    ImagePixcels[j,i] = (int)newPixel;
                }
                    
        }


    }
}
