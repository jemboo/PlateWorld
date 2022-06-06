using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlateWorld.Mvvm.AttachedProps
{
    public static class TextBoxExtension
    {
        #region AllowOnlyString
        public static bool GetAllowOnlyString(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowOnlyStringProperty);
        }
        public static void SetAllowOnlyString(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowOnlyStringProperty, value);
        }
        // Using a DependencyProperty as the backing store for AllowOnlyString. This enables animation, styling, binding, etc...  
        public static readonly DependencyProperty AllowOnlyStringProperty =
        DependencyProperty.RegisterAttached("AllowOnlyString", typeof(bool), typeof(TextBoxExtension), new PropertyMetadata(false, AllowOnlyString));
        private static void AllowOnlyString(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                TextBox txtObj = (TextBox)d;
                var bb = txtObj.BorderBrush;
                txtObj.TextChanged += (s, arg) =>
                {
                    TextBox txt = s as TextBox;
                    if (!Regex.IsMatch(txt.Text, "^[a-zA-Z]*$"))
                    {
                        txtObj.BorderBrush = Brushes.Red;
                        MessageBox.Show("Only letter allowed!");
                    }
                    else
                    {
                        txtObj.BorderBrush = bb;
                    }
                };
            }
        }

        #endregion



    }
}
