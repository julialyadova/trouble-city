using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Resources;
using System.Timers;
using System.Windows.Threading;

namespace trouble_city
{
    public partial class MainWindow : Window
    {
        Dictionary<IVisualised, Image> images = new Dictionary<IVisualised, Image>();

        public MainWindow()
        {
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,0,0,20);
            timer.Start();
        }

        //void MoveNeteorite(object sender, EventArgs e)
        //{
        //    MeteoriteAngle.Angle +=3;
        //    Canvas.SetLeft(MeteoriteImg, left);
        //    Canvas.SetTop(MeteoriteImg, top);
        //    left += 4;
        //    top +=4;
        //}

        public void DecreaseHealth(object sender, EventArgs e)
        {
            if (HealthPanel.Children.Count == 0) return;
            HealthPanel.Children.RemoveAt(0);
        }

        private void TurnRight_Click(object sender, RoutedEventArgs e) => BlasterRotation.Angle += 5;

        private void TurnLeft_Click(object sender, RoutedEventArgs e) => BlasterRotation.Angle -= 5;

        private void Shoot_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
