using DreamCatcher.Core.Models;
using DreamCatcher.ViewModel;
using ImageMagick;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DreamCatcher.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateEditDreamDialog.xaml
    /// </summary>
    public partial class CreateEditDreamDialog : UserControl
    {
        public byte[]? Image;

        public CreateEditDreamDialog(Dream dream)
        {
            InitializeComponent();
            this.Image = dream.Picture;
            DataContext = DreamSummaryViewModel.ToViewModel(dream);
        }

        public CreateEditDreamDialog(DateTime dateTime)
        {
            InitializeComponent();
            DataContext = new DreamSummaryViewModel(dateTime);
        }

        private void ImageSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;)|*.png;*.jpeg;*jpg";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog.ShowDialog() == true)
            {
                var img = File.ReadAllBytes(openFileDialog.FileName);

                using (var mgk = new MagickImage(img))
                {
                    mgk.Strip();
                    mgk.ColorSpace = ColorSpace.RGB;

                    mgk.WriteAsync(openFileDialog.FileName);
                }


                var image = new Image();
                image.Stretch = System.Windows.Media.Stretch.Fill;
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                ImageSelector.Content = image;
            }
        }
    }
}
