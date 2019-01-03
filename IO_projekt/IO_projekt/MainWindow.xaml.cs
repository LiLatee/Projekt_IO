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
        private Image<Gray, Byte> randomImage;
        private int count_of_iterations;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void generateImage()
        {
            // generate image
            int width = Int32.Parse(textBox_width.Text);
            int height = Int32.Parse(textBox_height.Text);

            count_of_iterations = Int32.Parse(textBox_iterations.Text);

            randomImage = new Image<Gray, Byte>(width, height);

            Random rnd = new Random();
            for (int i = 0; i < randomImage.Width; i++)
            for (int j = 0; j < randomImage.Height; j++)
            {
                randomImage[i, j] = new Gray(rnd.Next(0, 255));
            }
            imageBox_generated.Source = BitmapSourceConvert.ToBitmapSource(randomImage);
            randomImage.Save("generated_image.png");
        }
        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            generateImage();
            //convolution
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int k = 0;k<count_of_iterations;k++ )
            for (int i = 1; i < randomImage.Width-1; i++)
            for (int j = 1; j < randomImage.Height-1; j++)
            {
                double newValue = randomImage[i, j].Intensity * 0.6 + randomImage[i, j - 1].Intensity * 0.1 + randomImage[i, j + 1].Intensity * 0.1 +
                                  randomImage[i - 1, j].Intensity * 0.1 + randomImage[i + 1, j].Intensity * 0.1;

                randomImage[i, j] = new Gray(newValue);
                    }
            stopwatch.Stop();
            textBlock_synchronous_time.Text = (stopwatch.ElapsedMilliseconds/1000.0).ToString() + " s";
            imageBox_after_convolution.Source = BitmapSourceConvert.ToBitmapSource(randomImage);
            randomImage.Save("image_after_convolution.png");


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


        private async void  button_Start2_Click(object sender, RoutedEventArgs e)
        {
            generateImage();
            //convolution
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var tasks = new List<Task>();
            for (int j = 1; j < randomImage.Height - 2; j++)
            {
                tasks.Add(ciach0(j));
            }

            await Task.WhenAll(tasks.ToArray());
            stopwatch.Stop();

            textBlock_asynchronous_time.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString() + " s";
            imageBox_after_convolution.Source = BitmapSourceConvert.ToBitmapSource(randomImage);
            randomImage.Save("image_after_convolution.png");

        }

        private Task ciach0(int j)
        {
            return Task.Factory.StartNew(() =>
            {
                for (int k = 0; k < count_of_iterations; k++)
                for (int i = 1; i < randomImage.Width - 1; i++)
                {
                    double newValue = randomImage[i, j].Intensity * 0.6 + randomImage[i, j - 1].Intensity * 0.1 + randomImage[i, j + 1].Intensity * 0.1 +
                                      randomImage[i - 1, j].Intensity * 0.1 + randomImage[i + 1, j].Intensity * 0.1;

                    randomImage[i, j] = new Gray(newValue);
                }
            });

        }
    }
}
