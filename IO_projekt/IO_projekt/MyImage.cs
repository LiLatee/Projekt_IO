using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace IO_projekt 
{
    class MyImage : ICloneable
    {
        public double[,] pixels;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public MyImage(int width, int height)
        {
            genereateRandomImage(width, height);
        }

        public MyImage(MyImage myImage)
        {
            pixels = new double[myImage.getWidth(), myImage.getHeight()];
            Array.Copy(myImage.pixels, 0, pixels, 0, myImage.pixels.Length);
        }
        public MyImage(Image<Gray, Byte> image)
        {
            pixels = new double[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
            for (int j = 0; j < image.Height; j++)
                pixels[i, j] = image[i,j].Intensity;
        }

        public void genereateRandomImage(int width, int height)
        {
            pixels = new double[width+2, height+2];
            Random rnd = new Random();
            for (int i = 1; i < width - 1; i++)
            for (int j = 1; j < height - 1; j++)
                pixels[i, j] = rnd.Next(0, 255);
                //pixels[i, j] = 255;
        }

        public Image<Gray, Byte> toImage()
        {
            Image<Gray, Byte> resultImage = new Image<Gray, byte>(pixels.GetLength(0), pixels.GetLength(1));
            for (int i = 0; i < resultImage.Width; i++)
            for (int j = 0; j < resultImage.Height; j++)
                resultImage[i, j] = new Gray(pixels[i,j]);

            return resultImage;
        }

        public double getPixel(int width, int height)
        {
            return pixels[width, height];
        }

        public void setPixel(int width, int height, double value)
        {
            pixels[width, height] = value;
        }

        public int getWidth()
        {
            return pixels.GetLength(0);
        }

        public int getHeight()
        {
            return pixels.GetLength(1);
        }

        public void saveAsPGM(String path)
        {
            var stream = File.Open(path, FileMode.OpenOrCreate);

            StringBuilder sb = new StringBuilder();
            sb = sb.Append("P2\n");
            sb = sb.Append(pixels.GetLength(0)-2);
            sb = sb.Append(" ");
            sb = sb.Append(pixels.GetLength(1)-2);
            sb = sb.Append("\n");
            sb = sb.Append("255\n");
            for (int i = 1; i < pixels.GetLength(0) - 2; i++)
            {
                for (int j = 1; j < pixels.GetLength(1) - 2; j++)
                {
                    /*if (pixels[i,j] - (int)pixels[i,j] < 0.5)
                        sb = sb.Append((int)pixels[i, j]);
                    else
                        sb = sb.Append((int)pixels[i, j] + 1);*/
                    sb = sb.Append(Math.Floor(pixels[i, j]));
                    sb = sb.Append(" ");
                }
               sb = sb.Append("\n");

            }

            stream.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            stream.Close();
        }
    }
}
