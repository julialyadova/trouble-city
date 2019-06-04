using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace trouble_city
{
    static class Game
    {
        static MainWindow window;
        static DispatcherTimer meteoriteTimer = new DispatcherTimer();
        static Vector centerPoint;
        static int goal;
        static int rainAngle;

        public static Vector CenterPoint { get { return centerPoint; } }
        public static bool GameOver =true;
        public static int Materials = 0;
        public static List<IVisualised> CanvasObjects { get { return canvasObjects; } }
        static List<IVisualised> canvasObjects = new List<IVisualised>();
        public static List<IVisualised> ToAddObjects = new List<IVisualised>();

        public static void Initialize(MainWindow gameWindow) => window = gameWindow;

        public static void Start()
        {
            if (window == null) throw new NullReferenceException("Initialize() must be called before Start()");
            centerPoint = new Vector(Canvas.GetLeft(window.Blaster) + window.Blaster.Width / 2,
                Canvas.GetTop(Game.window.Blaster) + window.Blaster.Height / 2);
            goal = 10;
            rainAngle = -35;
            GameOver = false;
            Materials = 0;
            canvasObjects.Clear();
            ToAddObjects.Clear();
            RestoreHealth();
            meteoriteTimer.Interval = new TimeSpan(0, 0, 0, 5);
            meteoriteTimer.Start();
            meteoriteTimer.Tick += new EventHandler(CreateMeteorite);
            window.Timer.IsEnabled = true;
            window.GiveUpButton.IsEnabled = true;
        }

        public static void Resize()
        {
            centerPoint = new Vector(Canvas.GetLeft(window.Blaster) + window.Blaster.Width / 2,
                Canvas.GetTop(window.Blaster) + window.Blaster.Height / 2);
        }

        public static void Move(object sender, EventArgs e)
        {
            List<IVisualised> toDestroy = new List<IVisualised>();
            foreach (var obj in CanvasObjects)
                if (OutsideTheWindow(obj) || obj.Health <= 0) toDestroy.Add(obj);
                else obj.Act();
            foreach (var obj in toDestroy)
                Destroy(obj);
            foreach (var obj in ToAddObjects)
                CanvasObjects.Add(obj);
            ToAddObjects.Clear();
            if (Materials > 200) AddHouse();
            window.ScoreTextBlock.Text = Materials.ToString();
        }

        public static void CreateMeteorite(object sender, EventArgs e)
        {
            Add(new Meteorite(Vector.FromAngle(rainAngle)), -50, new Random().Next(0, (int)centerPoint.X));
        }

        public static void Add(IVisualised obj, int top, int left)
        {
            window.SkyCanvas.Children.Add(obj.Img);
            Canvas.SetTop(obj.Img, top);
            Canvas.SetLeft(obj.Img, left);
            ToAddObjects.Add(obj);
        }

        public static void AddHouse()
        {
            Materials -= 200;
            var n = new Random().Next(1, 6);
            window.Houses.Children.Add(GetImageByName("house" + n));
            SendMessage("Построен новый дом!");
            rainAngle -= 5;
            if (window.Houses.Children.Count == goal) End("Победа!");
        }

        public static void DecreaseHealth()
        {
            if (window.HealthPanel.Children.Count == 0) return;
            window.HealthPanel.Children.RemoveAt(0);
        }

        public static void RestoreHealth()
        {
            while (window.HealthPanel.Children.Count != 6) return;
            window.HealthPanel.Children.Add(GetImageByName("heart"));
        }

        public static void SendMessage(string text)
        {
            window.EventTextBlock.Text = text;
            window.EventTextBlock.Opacity = 1;
        }

        public static Image GetImageByName(string name)
        {
            var img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + name + ".png"));
            return img;
        }

        public static bool InCityBounds(IVisualised obj)
            => Math.Abs(obj.Position.X + obj.Radius - centerPoint.X) < window.Houses.ActualWidth / 2;

        public static bool ReachedBottomLine(IVisualised obj)
            => obj.Position.Y + obj.Radius*2 >= window.SkyCanvas.ActualHeight;

        public static bool OutsideTheWindow(IVisualised obj)
        {
            return obj.Position.X < -100 || obj.Position.X > window.SkyCanvas.Width
                || obj.Position.Y < -100 || obj.Position.Y > window.SkyCanvas.Height;
        }

        static void Destroy(IVisualised obj)
        {
            CanvasObjects.Remove(obj);
            window.SkyCanvas.Children.Remove(obj.Img);
        }

        public static void SwitchOnPauseMode()
        {
            window.Timer.IsEnabled = !window.Timer.IsEnabled;
            meteoriteTimer.IsEnabled = !meteoriteTimer.IsEnabled;
            window.TopMenu.IsEnabled = !window.TopMenu.IsEnabled;
            window.BlasterControlPanel.IsEnabled = !window.BlasterControlPanel.IsEnabled;
        }

        public static void End(string text)
        {
            SendMessage(text + " Общий счёт: " + (Materials + window.Houses.Children.Count*200));
            GameOver = true;
            window.StartButton.IsEnabled = true;
            window.GiveUpButton.IsEnabled = false;
            window.StartButton.Opacity = 1;
            meteoriteTimer = new DispatcherTimer();
            window.StartButton.IsEnabled = true;
            foreach (var obj in canvasObjects)
                window.SkyCanvas.Children.Remove(obj.Img);
        }
    }
}
