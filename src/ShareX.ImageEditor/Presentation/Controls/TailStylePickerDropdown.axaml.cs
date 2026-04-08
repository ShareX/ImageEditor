using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.Core.Annotations;

namespace ShareX.ImageEditor.Presentation.Controls
{
    public partial class TailStylePickerDropdown : UserControl
    {
        public static readonly StyledProperty<StepTailStyle> SelectedTailStyleProperty =
            AvaloniaProperty.Register<TailStylePickerDropdown, StepTailStyle>(
                nameof(SelectedTailStyle),
                defaultValue: StepTailStyle.Triangle);

        public StepTailStyle SelectedTailStyle
        {
            get => GetValue(SelectedTailStyleProperty);
            set => SetValue(SelectedTailStyleProperty, value);
        }

        public event EventHandler<StepTailStyle>? TailStyleChanged;

        public TailStylePickerDropdown()
        {
            AvaloniaXamlLoader.Load(this);

            var popup = this.FindControl<Popup>("TailStylePopup");
            if (popup != null)
            {
                popup.Opened += OnPopupOpened;
            }
        }

        private void OnPopupOpened(object? sender, EventArgs e)
        {
            UpdateActiveStates();
        }

        private void UpdateActiveStates()
        {
            var popup = this.FindControl<Popup>("TailStylePopup");
            if (popup?.Child is Border border && border.Child is ItemsControl itemsControl)
            {
                foreach (var item in itemsControl.GetRealizedContainers())
                {
                    if (item is ContentPresenter presenter && presenter.Child is Button button)
                    {
                        if (button.CommandParameter is StepTailStyle style && style == SelectedTailStyle)
                        {
                            button.Classes.Add("active");
                        }
                        else
                        {
                            button.Classes.Remove("active");
                        }
                    }
                }
            }
        }

        private void OnDropdownButtonClick(object? sender, RoutedEventArgs e)
        {
            var popup = this.FindControl<Popup>("TailStylePopup");
            if (popup != null)
            {
                popup.IsOpen = !popup.IsOpen;
            }
        }

        private void OnTailStyleSelected(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is StepTailStyle style)
            {
                SelectedTailStyle = style;
                TailStyleChanged?.Invoke(this, style);
                UpdateActiveStates();

                var popup = this.FindControl<Popup>("TailStylePopup");
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
            }
        }
    }
}