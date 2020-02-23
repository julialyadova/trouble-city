using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
            Timer.Interval = new TimeSpan(0,0,0,0,20);
            Timer.Tick += new EventHandler(PlanetMovement);
            Timer.Tick += new EventHandler(MessageVanish);
            Timer.Tick += new EventHandler(Game.Move);
        }

        private void TurnRight_Click(object sender, RoutedEventArgs e) => MoveBlaster(10);

        private void TurnLeft_Click(object sender, RoutedEventArgs e) => MoveBlaster(-10);

        private void Shoot_Click(object sender, RoutedEventArgs e)
        {
            Game.Shoot();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Start();
            Goal.Text = Game.Goal.ToString();
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

        private void MoveBlaster(int degrees)
        {
            if (Math.Abs(BlasterRotation.Angle + degrees) < 80)
                BlasterRotation.Angle += degrees;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Opacity = (Settings.IsEnabled) ? 0 : 1;
            Settings.IsEnabled = !Settings.IsEnabled;
            Game.SwitchOnPauseMode();
            Game.PlaySound("click");
        }

        private void SwitchSize_Click(object sender, RoutedEventArgs e)
        {
            sizeNumber = (sizeNumber == resolutions.Length - 1) ? 0 : sizeNumber + 1;
            SizeText.Text = resolutions[sizeNumber].Width + " x " + resolutions[sizeNumber].Height;
            UpdateSize();
        }

        private void SwitchMeteor_Click(object sender, RoutedEventArgs e)
        {
            MeteorType.Text = Game.SwitchMeteorite();
        }

        private void SwitchShot_Click(object sender, RoutedEventArgs e)
        {
            ShotType.Text = Game.SwitchShoot();
        }

        private void GiveUpButton_Click(object sender, RoutedEventArgs e) => Game.End("Ты сам выбрал поражение");
    }
}
