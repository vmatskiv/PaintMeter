using System.Windows;
using PaintMeter.ViewModel;
using System.ComponentModel;
using System.Reflection;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace PaintMeter
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            AppDomain appdomain = AppDomain.CurrentDomain;
            appdomain.UnhandledException += UnhandledException;
            InitializeComponent();
            
            BitmapImage glowIcon = new BitmapImage();
            glowIcon.BeginInit();
            glowIcon.UriSource = new Uri("pack://application:,,,/PaintMeter;component/Resources\\MiernikLakieru.ico");
            glowIcon.EndInit();
            this.Icon = glowIcon;
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void OnAutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

        }

        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}