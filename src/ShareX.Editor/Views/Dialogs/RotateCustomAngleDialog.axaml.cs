using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.Editor.ViewModels;

namespace ShareX.Editor.Views.Dialogs;

public partial class RotateCustomAngleDialog : UserControl
{
    public RotateCustomAngleDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.RotateAngleDegrees = 0;
        }
    }

    private void OnRotateLeftClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.RotateAngleDegrees -= 90;
        }
    }

    private void OnRotateRightClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.RotateAngleDegrees += 90;
        }
    }
}
