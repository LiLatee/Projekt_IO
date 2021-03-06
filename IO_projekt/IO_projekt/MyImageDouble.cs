﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

// zastosowanie tasków 
// double zamiast double
// tablice statyczne

namespace IO_projekt
{
    class MyImageDouble : ICloneable
    {
        public double[,] pixels;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public MyImageDouble(int width, int height)
        {
            genereateChessboard(width, height);
        }

        public MyImageDouble(MyImageDouble myImage)
        {
            pixels = new double[myImage.pixels.GetLength(0), myImage.pixels.GetLength(1)];
            Array.Copy(myImage.pixels, 0, pixels, 0, myImage.pixels.Length);
        }
        public MyImageDouble(Image<Gray, Byte> image)
        {
            pixels = new double[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                    pixels[i, j] = (double)image[i, j].Intensity;
        }

        public void genereateChessboard(int width, int height)
        {
            width += 2;
            height += 2;
            pixels = new double[width, height];
            int color = 255;
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    pixels[i, j] = color;
                    if (j % ((width - 2) / 8) == 0)
                    {
                        if (color == 255)
                            color = 1;
                        else color = 255;
                    }

                }

                if (i % ((width - 2) / 8) == 0)
                {
                    if (color == 255)
                        color = 1;
                    else color = 255;
                }

            }
        }

        public void genereateRandomImage(int width, int height)
        {
            width += 2;
            height += 2;
            pixels = new double[width, height];
            int color = 255;
            Random rnd = new Random();
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    pixels[i, j] = rnd.Next(0, 255);

                }


            }

        }

        public Image<Gray, Byte> toImage()
        {
            Image<Gray, Byte> resultImage = new Image<Gray, byte>(pixels.GetLength(0) - 2, pixels.GetLength(1) - 2);
            for (int i = 0; i < resultImage.Width; i++)
            for (int j = 0; j < resultImage.Height; j++)
                resultImage[i, j] = new Gray(pixels[i + 1, j + 1]);

            return resultImage;

        }

        public void saveAsPGM(String path)
        {
            var stream = File.Open(path, FileMode.OpenOrCreate);

            StringBuilder sb = new StringBuilder();
            sb = sb.Append("P2\n");
            sb = sb.Append(pixels.GetLength(0) - 2);
            sb = sb.Append(" ");
            sb = sb.Append(pixels.GetLength(1) - 2);
            sb = sb.Append("\n");
            sb = sb.Append("255\n");
            for (int i = 1; i < pixels.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < pixels.GetLength(1) - 1; j++)
                {
                    sb = sb.Append(Math.Floor(pixels[i, j]));
                    sb = sb.Append(" ");
                }
                sb = sb.Append("\n");

            }

            stream.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            stream.Close();
        }

        public int getWidth()
        {
            return pixels.GetLength(0);
        }

        public int getHeight()
        {
            return pixels.GetLength(1);
        }

        public double getPixel(int i, int j)
        {
            return pixels[i, j];
        }

        public void setPixel(int i, int j, double value)
        {
            pixels[i, j] = value;
        }
    }
}