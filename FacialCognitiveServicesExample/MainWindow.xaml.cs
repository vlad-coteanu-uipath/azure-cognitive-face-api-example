using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace FacialCognitiveServicesExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string UPLOAD_HAPPY_IMAGE_TEXT = "Upload happy image";
        private static string UPLOAD_SAD_IMAGE_TEXT = "Upload sad image";
        private static string UPLOAD_SURPRISE_IMAGE_TEXT = "Upload surprised image";
        private string LeftImageFullPath = "";
        private string CenterImageFullPath = "";
        private string RightImageFullPath = "";

        public MainWindow()
        {
            InitializeComponent();
            PreProcess();
        }

        private void PreProcess()
        {
            LeftLabel.Content = UPLOAD_HAPPY_IMAGE_TEXT;
            RightLabel.Content = UPLOAD_SAD_IMAGE_TEXT;
            CenterLabel.Content = UPLOAD_SURPRISE_IMAGE_TEXT;
            ValidateButton.IsEnabled = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                LeftImageFullPath = openFileDialog.FileName;

            if(LeftImageFullPath != "" && RightImageFullPath != "" && CenterImageFullPath != "")
            {
                ValidateButton.IsEnabled = true;
            }

            BitmapImage image = new BitmapImage(new Uri(LeftImageFullPath, UriKind.Absolute));
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.Freeze();
            LeftImage.Source = image;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                RightImageFullPath = openFileDialog.FileName;

            if (LeftImageFullPath != "" && RightImageFullPath != "" && CenterImageFullPath != "")
            {
                ValidateButton.IsEnabled = true;
            }

            BitmapImage image = new BitmapImage(new Uri(RightImageFullPath, UriKind.Absolute));
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.Freeze();
            RightImage.Source = image;
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                CenterImageFullPath = openFileDialog.FileName;

            if (LeftImageFullPath != "" && RightImageFullPath != "" && CenterImageFullPath != "")
            {
                ValidateButton.IsEnabled = true;
            }

            BitmapImage image = new BitmapImage(new Uri(CenterImageFullPath, UriKind.Absolute));
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.Freeze();
            CenterImage.Source = image;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            LeftImageFullPath = "";
            RightImageFullPath = "";
            CenterImageFullPath = "";

            BitmapImage image = new BitmapImage(new Uri(@"pack://application:,,,/"
                + Assembly.GetExecutingAssembly().GetName().Name
                + ";component/"
                + "Images/addImage.png", UriKind.Absolute));
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.Freeze();
            LeftImage.Source = image;
            RightImage.Source = image;
            CenterImage.Source = image;

            ValidateButton.IsEnabled = false;
        }

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateButton.IsEnabled = false;
            bool answer = FacialCognitiveService.Instance.ValidateImages(LeftImageFullPath, RightImageFullPath, CenterImageFullPath);
            if (answer) 
            {
                MessageBox.Show("Validation passed");
            }
            else
            {
                MessageBox.Show("Validation failed");
            }
            ResetButton_Click(null, null);
        }
    }
}
