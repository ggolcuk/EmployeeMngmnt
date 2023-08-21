using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;


namespace Common.WPF.UIUtilities
{
    public static class PopupFocusBehavior
    {
        public static readonly DependencyProperty IsPopupOpenOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "IsPopupOpenOnFocus",
                typeof(bool),
                typeof(PopupFocusBehavior),
                new PropertyMetadata(false, OnIsPopupOpenOnFocusChanged));

        public static bool GetIsPopupOpenOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsPopupOpenOnFocusProperty);
        }

        public static void SetIsPopupOpenOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(IsPopupOpenOnFocusProperty, value);
        }

        private static void OnIsPopupOpenOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.GotFocus += TextBox_GotFocus;
                    textBox.LostFocus += TextBox_LostFocus;
                }
                else
                {
                    textBox.GotFocus -= TextBox_GotFocus;
                    textBox.LostFocus -= TextBox_LostFocus;
                }
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var popup = FindVisualChild<Popup>(textBox);
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var popup = FindVisualChild<Popup>(textBox);
                if (popup != null)
                {
                    popup.IsOpen = true;
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }
    }
}
