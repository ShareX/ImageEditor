using Avalonia.Controls;
using Avalonia.Threading;
using ShareX.Editor.ViewModels;
using SkiaSharp;
using System.Threading.Tasks;

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
            
            // Load sample image asynchronously to ensure UI is ready (though not strictly necessary here)
            Dispatcher.UIThread.Post(() => LoadSampleImage(vm));
        }

        private void LoadSampleImage(MainViewModel vm)
        {
            // Create a sample SKBitmap
            var width = 800;
            var height = 600;
            var info = new SKImageInfo(width, height);
            var skBitmap = new SKBitmap(info);

            using (var canvas = new SKCanvas(skBitmap))
            {
                // Draw background
                canvas.Clear(SKColors.White);

                // Draw some shapes to make it interesting
                using (var paint = new SKPaint { Color = SKColors.LightBlue, IsAntialias = true })
                {
                    canvas.DrawCircle(width / 2, height / 2, 100, paint);
                }
                
                using (var paint = new SKPaint { Color = SKColors.Salmon, IsAntialias = true, TextSize = 48 })
                {
                    canvas.DrawText("ShareX Editor Loader", 50, 100, paint);
                }
            }

            // Load into ViewModel
            vm.UpdatePreview(skBitmap);
        }
    }
}
