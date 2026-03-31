using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ShareX.ImageEditor.Presentation.Views
{
    public partial class NewImageDialogView : UserControl
    {
        public NewImageDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
