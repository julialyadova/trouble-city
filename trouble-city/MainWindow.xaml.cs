using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Media;

namespace trouble_city
{
    public partial class MainWindow : Window
    {
        public DispatcherTimer Timer = new DispatcherTimer();
        int sizeNumber = 0;
        Size[] resolutions = new Size[] { new Size(1024, 786), new Size(1280, 1024), new Size(1600, 900)};

        public MainWindow()
        {
            InitializeComponent();
            Game.Initialize(this);
            //UpdateSize();
            Timer.Interval = new TimeSpan(0,0,0,0,20);
            Timer.Start();
            Timer.Tick += new EventHandler(PlanetMovement);
            Timer.Tick += new EventHandler(MessageVanish);
            Timer.Tick += new EventHandler(Game.Move);
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
            Game.Start();
            StartButton.IsEnabled = false;
            StartButton.Opacity = 0.4;
        }

        private void PlanetMovement(object sender, EventArgs e)
        {
            var planetX = Planet.Margin.Left;
            if (planetX > Width + Planet.Width)
                Planet.Margin = new Thickness(-Planet.Width, SkyCanvas.ActualHeight*0.25 , 0, 0);
            else
                Planet.Margin = new Thickness(Planet.Margin.Left+0.5, SkyCanvas.ActualHeight * 0.25, 0, 0);
        }

        private void MessageVanish(object sender, EventArgs e)
        {
            if (EventTextBlock.Opacity > 0)
                EventTextBlock.Opacity -= 0.006;
        }

        private void UpdateSize()
        {
            Width = resolutions[sizeNumber].Width;
            Height = resolutions[sizeNumber].Height;
            Canvas.SetLeft(Center, (Width - Center.Width) / 2);
            Canvas.SetTop(Center, SkyCanvas.ActualHeight - Center.Height-10);
            Canvas.SetLeft(Blaster, (Width - Blaster.Width) / 2);
            Canvas.SetTop(Blaster, Canvas.GetTop(Center));
            Game.Resize();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Game.SwitchOnPauseMode();
            Settings.Opacity = (Settings.IsEnabled) ? 0 : 1;
            Settings.IsEnabled = !Settings.IsEnabled;
        }

        private void SwitchSize_Click(object sender, RoutedEventArgs e)
        {
            sizeNumber = (sizeNumber == resolutions.Length - 1) ? 0 : sizeNumber + 1;
            SwitchSize.Content = resolutions[sizeNumber].Width + " x " + resolutions[sizeNumber].Height;
            UpdateSize();
        }

        private void GiveUpButton_Click(object sender, RoutedEventArgs e) => Game.End("Поражение!");
    }
}
