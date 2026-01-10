using Avalonia.Controls;
using Avalonia.Threading;
using ShareX.Editor.ViewModels;
using SkiaSharp;
using System.IO;
using System.Reflection;
using System;

namespace ShareX.Editor.Loader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize ViewModel
            var vm = new MainViewModel();
            this.DataContext = vm;
            
            // Load sample image asynchronously
            Dispatcher.UIThread.Post(() => LoadExampleImage(vm));
        }

        private void LoadExampleImage(MainViewModel vm)
        {
            try 
            {
                // Path to the asset - since we set CopyToOutputDirectory, it should be in the bin folder under Assets/
                var location = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(location, "Assets", "Sample.png");

                if (File.Exists(path))
                {
                    using (var stream = File.OpenRead(path))
                    {
                         // Use SKBitmap.Decode to load file
                         var skBitmap = SKBitmap.Decode(stream);
                         vm.UpdatePreview(skBitmap);
                    }
                }
                else
                {
                    // Fallback to generated if file missing
                    GenerateSampleImage(vm);
                }
            }
            catch (Exception)
            {
                 GenerateSampleImage(vm);
            }
        }

        private void GenerateSampleImage(MainViewModel vm)
        {
            // Create a sample SKBitmap
            var width = 800;
            var height = 600;
            var info = new SKImageInfo(width, height);
            var skBitmap = new SKBitmap(info);

            using (var canvas = new SKCanvas(skBitmap))
            {
                canvas.Clear(SKColors.White);
                using (var paint = new SKPaint { Color = SKColors.LightBlue, IsAntialias = true })
                {
                    canvas.DrawCircle(width / 2, height / 2, 100, paint);
                }
            }
            vm.UpdatePreview(skBitmap);
        }
    }
}
