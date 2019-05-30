using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace trouble_city
{
    public partial class MainWindow : Window
    {
        public DispatcherTimer Timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Interval = new TimeSpan(0,0,0,0,20);
            Timer.Start();
        }

        private void TurnRight_Click(object sender, RoutedEventArgs e) 
            => BlasterRotation.Angle += (BlasterRotation.Angle < 80) ? 10 : 0;

        private void TurnLeft_Click(object sender, RoutedEventArgs e)
            => BlasterRotation.Angle -= (BlasterRotation.Angle > -80) ? 10 : 0;

        private void Shoot_Click(object sender, RoutedEventArgs e)
        {
            if (Game.GameOver) return;
            new Shot(Vector.FromAngle(90 - BlasterRotation.Angle)).Set();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Start(this);
            StartButton.IsEnabled = false;
            StartButton.Opacity = 0.5;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(Center, (Width - Center.Width) / 2);
            Canvas.SetTop(Center, SkyCanvas.ActualHeight - Center.Height + 100);
            Canvas.SetLeft(Blaster, (Width - Blaster.Width) / 2);
            Canvas.SetTop(Blaster, Canvas.GetTop(Center));
            if (!Game.GameOver) Game.Resize();
        }
    }


}
