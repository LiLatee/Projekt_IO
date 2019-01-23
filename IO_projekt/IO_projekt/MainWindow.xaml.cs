using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
// zmiana indeksów w wykonaniu pętli
// rozdzielenie operacji na dwie osobne pętle
// użycie float zamiast double
namespace IO_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int countOfIterations;
        private int size;

        public MainWindow()
        {

            InitializeComponent();

            randomImageFloat = new MyImageFloat(size, size);
            randomImageDouble = new MyImageDouble(size, size);

            generateChessboardImage();
        }

        private void generateRandomImage()
        {
            // generate image
            size = Int32.Parse(textBox_size.Text);

            randomImageFloat.genereateRandomImage(size, size);
            randomImageDouble.genereateRandomImage(size, size);

            imageBox_generated.Source = BitmapSourceConvert.ToBitmapSource(randomImageFloat.toImage());
            //randomImage.toImage().Save("generated_image.pgm");
            randomImageFloat.saveAsPGM("generatedImage.pgm");

        }

        private void generateChessboardImage()
        {
            // generate image
            size = Int32.Parse(textBox_size.Text);

            randomImageFloat.genereateChessboard(size, size);
            randomImageDouble.genereateChessboard(size, size);

            imageBox_generated.Source = BitmapSourceConvert.ToBitmapSource(randomImageFloat.toImage());
            //randomImage.toImage().Save("generated_image.pgm");
            randomImageFloat.saveAsPGM("generatedImage.pgm");

        }

        private MyImageDouble randomImageDouble;
        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            
            MyImageDouble workingImage = new MyImageDouble(randomImageDouble);
            MyImageDouble tempImageObj = new MyImageDouble(size + 2, size +2);
            countOfIterations = Int32.Parse(textBox_iterations.Text);

            //convolution
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int k = 0; k < countOfIterations; k++)
            {
                for (int j = 1; j < workingImage.getHeight() - 1; j++)
                    for (int i = 1; i < workingImage.getWidth() - 1; i++)
                    {
                        tempImageObj.setPixel(i, j, workingImage.getPixel(i + 1, j) * 0.1 + workingImage.getPixel(i, j - 1) * 0.1 + workingImage.getPixel(i - 1, j) * 0.1 + workingImage.getPixel(i, j + 1) * 0.1 +
                                                    workingImage.getPixel(i, j) * 0.6);
                    }
                MyImageDouble temp = new MyImageDouble(tempImageObj);
                workingImage = temp;
            }
            stopwatch.Stop();
            textBlock_synchronous_time.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString() + " s";
            imageBox_after_convolution.Source = BitmapSourceConvert.ToBitmapSource(workingImage.toImage());
            //workingImage.toImage().Save("image_after_synchronous.pgm");
            workingImage.saveAsPGM("image_after_synchronous.pgm");
        }

        public static class BitmapSourceConvert
        {
            [DllImport("gdi32")]
            private static extern int DeleteObject(IntPtr o);

            public static BitmapSource ToBitmapSource(IImage image)
            {
                using (System.Drawing.Bitmap source = image.Bitmap)
                {
                    IntPtr ptr = source.GetHbitmap();

                    BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ptr,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    DeleteObject(ptr);
                    return bs;
                }
            }
        }

        private MyImageFloat randomImageFloat, workingImage;
        public float[,] workingImageStaticArray;
        private float[,] tempImage;

        private async void button_Start2_Click(object sender, RoutedEventArgs e)
        {
            countOfIterations = Int32.Parse(textBox_iterations.Text);
            int tasksCount = Int32.Parse(textBox_tasksCount.Text); ;
            workingImage = new MyImageFloat(randomImageFloat);
            tempImage = new float[size + 2, size + 2];
            workingImageStaticArray = workingImage.pixels;
            int mod = (workingImage.pixels.GetLength(1) - 2) % tasksCount;
            int howManyRows = (workingImage.pixels.GetLength(1) - 2) / tasksCount;

            //convolution
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var tasks = new List<Task>();

            for (int k = 0; k < countOfIterations; k++)
            {
                for (int j = 0; j < tasksCount; j++)
                {
                    if (j == tasksCount - 1 && mod != 0)
                        tasks.Add(ciach1(j * howManyRows + 1, howManyRows + mod));
                    else
                        tasks.Add(ciach1(j * howManyRows + 1, howManyRows));
                }
                await Task.WhenAll(tasks.ToArray());
                workingImageStaticArray = tempImage;

            }
            stopwatch.Stop();
            workingImage.pixels = workingImageStaticArray;
            textBlock_asynchronous_time.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString() + " s";
            imageBox_after_convolution.Source = BitmapSourceConvert.ToBitmapSource(workingImage.toImage());
            //workingImage.toImage().Save("image_after_asynchronous.pgm");
            workingImage.saveAsPGM("image_after_asynchronous.pgm");

        }


        private Task ciach1(int j, int howManyRows)
        {
            return Task.Factory.StartNew(() =>
            {
                float[] newValue = new float[size];
                int i;
                for (int h = j; h < j + howManyRows && h < size - 2 - 1; h++)
                {

                    for (i = 1; i < size - 2 - 1; i++)
                    {
                        newValue[i - 1] = workingImageStaticArray[h, i] * 0.6f + (workingImageStaticArray[h - 1, i] + workingImageStaticArray[h, i - 1] +
                                          workingImageStaticArray[h, i + 1] + workingImageStaticArray[h + 1, i]) * 0.1f;
                    }

                    for (i = 1; i < size - 2 - 1; i++)
                    {
                        tempImage[h, i] = newValue[i - 1];
                    }

                }
            });

        }

        private void button_GenerateRandomImage_Click(object sender, RoutedEventArgs e)
        {
            generateRandomImage();
        }


        private void button_GenerateChessboardImage_Click(object sender, RoutedEventArgs e)
        {
            generateChessboardImage();
        }
    }
}