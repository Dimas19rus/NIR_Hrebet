using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для WindowDisplayImages.xaml
    /// </summary>
    public partial class WindowDisplayImages : Window
    {
        public WindowDisplayImagesModel Model => (WindowDisplayImagesModel)DataContext;

        public WindowDisplayImages()
        {
            DataContext = new WindowDisplayImagesModel();
            InitializeComponent();

            //BitmapImage imageOrig = new BitmapImage(uriImageOrig);
            //BitmapImage imageBlackAndWhite = new BitmapImage(uriImageBlackAndWhite);
            //BitmapImage imageProcessed = new BitmapImage(uriImageProcessed);

            //ImageOrig.Source = imageOrig;
            //ImageBlackAndWhite.Source = imageBlackAndWhite;
            //ImageProcessed.Source = imageProcessed;

            //WxHOrig.Content = imageOrig.Width + "X" + imageOrig.Height;
            //WxHProc.Content = imageProcessed.Width + "X" + imageProcessed.Height;
        }

        public void SetImageOrig(Uri uriImageOrig)
        {
            BitmapImage imageOrig = new BitmapImage(uriImageOrig);
            ImageOrig.Source = imageOrig;
            WxHOrig.Content = imageOrig.PixelWidth + " X " + imageOrig.PixelHeight;
        }

        public void SetImageBlackAndWhite(Uri uriImageBlackAndWhite)
        {
            BitmapImage imageBlackAndWhite = new BitmapImage(uriImageBlackAndWhite);
            ImageBlackAndWhite.Source = imageBlackAndWhite;
        }
        public void SetImageProcessed(Uri uriImageProcessed)
        {
            BitmapImage imageProcessed = new BitmapImage(uriImageProcessed);
            ImageProcessed.Source = imageProcessed;
            WxHProc.Content = imageProcessed.PixelWidth + " X " + imageProcessed.PixelHeight;
        }


    }
}
