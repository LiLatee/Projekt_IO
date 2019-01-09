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

namespace IO_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyImage randomImage, workingImage;
        private int countOfIterations;
        private int width, height;
        private float[,] tempImage;

        public MainWindow()
        {
            InitializeComponent();
            generateImage();
        }

        private void generateImage()
        {
            // generate image
            width = Int32.Parse(textBox_width.Text);
            height = Int32.Parse(textBox_height.Text);

            randomImage = new MyImage(width, height);
            imageBox_generated.Source = BitmapSourceConvert.ToBitmapSource(randomImage.toImage());
            //randomImage.toImage().Save("generated_image.pgm");
            randomImage.saveAsPGM("generatedImage.pgm");

        }
        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            workingImage = new MyImage(randomImage);
            tempImage = new float[width + 2, height + 2];
            countOfIterations = Int32.Parse(textBox_iterations.Text);

            //convolution
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int k = 0; k < countOfIterations; k++)
            {
                for (int i = 1; i < workingImage.pixels.GetLength(0) - 1; i++)
                    for (int j = 1; j < workingImage.pixels.GetLength(1) - 1; j++)
                    {
                        float newValue = workingImage.pixels[i, j] * 0.6f + workingImage.pixels[i, j - 1] * 0.1f + workingImage.pixels[i, j + 1] * 0.1f +
                                          workingImage.pixels[i - 1, j] * 0.1f + workingImage.pixels[i + 1, j] * 0.1f;

                        tempImage[i, j] = newValue;
                    }
                workingImage.pixels = tempImage;
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

        private async void button_Start2_Click(object sender, RoutedEventArgs e)
        {
            countOfIterations = Int32.Parse(textBox_iterations.Text);
            int tasksCount = 8;
            workingImage = new MyImage(randomImage);
            tempImage = new float[width + 2, height + 2];

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
                workingImage.pixels = tempImage;
            }
            stopwatch.Stop();
            Console.WriteLine((stopwatch.ElapsedMilliseconds / 1000.0).ToString() + " s");

            textBlock_asynchronous_time.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString() + " s";
            imageBox_after_convolution.Source = BitmapSourceConvert.ToBitmapSource(workingImage.toImage());
            //workingImage.toImage().Save("image_after_asynchronous.pgm");
            workingImage.saveAsPGM("image_after_asynchronous.pgm");

        }

        private Task ciach1(int j, int howManyRows)
        {
            return Task.Factory.StartNew(() =>
            {
                for (int h = j; h < j + howManyRows && h < workingImage.pixels.GetLength(0) - 1; h++)
                for (int i = 1; i < workingImage.pixels.GetLength(0) - 1; i++)
                {
                    float newValue = workingImage.pixels[i, h] * 0.6f +
                                     (workingImage.pixels[i, h - 1] + workingImage.pixels[i - 1, h] +
                                      workingImage.pixels[i + 1, h] + workingImage.pixels[i, h + 1]) * 0.1f;

                    tempImage[i, h] = newValue;

                }
            });

        }

        private void button_GenerateImage_Click(object sender, RoutedEventArgs e)
        {
            generateImage();
        }
    }
}