using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ShareX.Editor.ImageEffects;
using System;
using System.Linq;

namespace ShareX.Editor.Controls
{
    public partial class EffectsMenuDropdown : UserControl
    {
        public event EventHandler<ImageEffect>? EffectRequested;

        public EffectsMenuDropdown()
        {
            AvaloniaXamlLoader.Load(this);
            PopulateMenu();
        }

        private void PopulateMenu()
        {
            var container = this.FindControl<StackPanel>("EffectsContainer");
            if (container == null) return;

            container.Children.Clear();

            AddEffectsGroup(container, "ADJUSTMENTS", ImageEffectCategory.Adjustments);
            
            // Add Separator
            var separator = new Border { Classes = { "menu-separator" } };
            container.Children.Add(separator);

            AddEffectsGroup(container, "FILTERS", ImageEffectCategory.Filters);
        }

        private void AddEffectsGroup(StackPanel container, string headerText, ImageEffectCategory category)
        {
            // Add Header
            var header = new TextBlock { Text = headerText, Classes = { "menu-header" } };
            container.Children.Add(header);

            if (category == ImageEffectCategory.Adjustments)
            {
                // Add Separator if needed or just mix it in?
                // The registry items come next.
            }


            // Add Effects
            var effects = ImageEffectRegistry.GetByCategory(category);
            
            foreach (var effect in effects)
            {
                var btn = CreateEffectButton(effect);
                container.Children.Add(btn);
            }
        }

        private Button CreateEffectButton(ImageEffect effect)
        {
            var btn = new Button
            {
                Classes = { "menu-item" },
                Tag = effect
            };
            btn.Click += OnEffectClick;

            var stack = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            
            // Icon
            var iconText = new TextBlock { Classes = { "menu-icon" } };
            if (!string.IsNullOrEmpty(effect.IconKey) && this.TryFindResource(effect.IconKey, out var iconRes))
            {
                iconText.Text = iconRes as string;
            }
            stack.Children.Add(iconText);

            // Text
            var textBlock = new TextBlock { Classes = { "menu-text" } };
            textBlock.Text = effect.HasParameters ? $"{effect.Name}…" : effect.Name;
            stack.Children.Add(textBlock);

            btn.Content = stack;
            return btn;
        }

        private void OnEffectClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            if (sender is Button btn && btn.Tag is ImageEffect effect)
            {
                EffectRequested?.Invoke(this, effect);
            }
        }

        private void OnDropdownButtonClick(object? sender, RoutedEventArgs e)
        {
            var popup = this.FindControl<Popup>("EffectsPopup");
            if (popup != null)
            {
                popup.IsOpen = !popup.IsOpen;
            }
        }



        private void ClosePopup()
        {
            var popup = this.FindControl<Popup>("EffectsPopup");
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }
    }
}
