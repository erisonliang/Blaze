using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace BlazeUpdater
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            expander1.Expanded += new RoutedEventHandler(expander1_Expanded);
            expander1.Collapsed += new RoutedEventHandler(expander1_Collapsed);
            expander1.IsExpanded = false;
        }

        #region Expander Event Handlers
        void expander1_Collapsed(object sender, RoutedEventArgs e)
        {
            DoubleAnimation expanderAnimation = new DoubleAnimation();
            expanderAnimation.To = 23;
            expanderAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation windowAnimation = new DoubleAnimation();
            windowAnimation.To = 115;
            windowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Header = "More details";
            mainWindow.BeginAnimation(Window.HeightProperty, windowAnimation);
        }

        void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation expanderAnimation = new DoubleAnimation();
            expanderAnimation.To = 83;
            expanderAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation windowAnimation = new DoubleAnimation();
            windowAnimation.To = 200;
            windowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Header = "Less details";
            mainWindow.BeginAnimation(Window.HeightProperty, windowAnimation);
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
