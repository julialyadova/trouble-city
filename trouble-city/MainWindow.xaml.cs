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
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            timer.Interval = new TimeSpan(0,0,0,0,20);
            timer.Start();
        }

        public void AddImage(Image img, int top, int left)
        {
            SkyCanvas.Children.Add(img);
            Canvas.SetTop(img, top);
            Canvas.SetLeft(img, left);
        }

        public void DecreaseHealth(object sender, EventArgs e)
        {
            if (HealthPanel.Children.Count == 0) return;
            HealthPanel.Children.RemoveAt(0);
        }

        private void TurnRight_Click(object sender, RoutedEventArgs e) 
            => BlasterRotation.Angle += (BlasterRotation.Angle < 80) ? 10 : 0;

        private void TurnLeft_Click(object sender, RoutedEventArgs e)
            => BlasterRotation.Angle -= (BlasterRotation.Angle > -80) ? 10 : 0;

        private void Shoot_Click(object sender, RoutedEventArgs e)
        {
            if (Game.GameOver) return;
            var realAngle = Math.PI *(90 - BlasterRotation.Angle) / 180;
            var directionVector = new Vector(Math.Cos(realAngle),-Math.Sin(realAngle)).Normalize();
            var shot = new Shot(directionVector);
            Game.Add(shot, 400 + (int)(directionVector.Y*70),
                512 + (int)(directionVector.X*70));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Start(timer, SkyCanvas);
            StartButton.IsEnabled = false;
        }
    }


}
