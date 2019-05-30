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
        static int goal = 10;
        static int rainAngle = -35;

        public static Vector CenterPoint { get { return centerPoint; } }
        public static bool GameOver = true;
        public static int Materials = 0;
        public static List<IVisualised> CanvasObjects { get { return canvasObjects; } }
        static List<IVisualised> canvasObjects = new List<IVisualised>();
        public static List<IVisualised> ToAddObjects = new List<IVisualised>();

        public static void Start(MainWindow gameWindow)
        {
            centerPoint = new Vector(Canvas.GetLeft(gameWindow.Blaster) + gameWindow.Blaster.Width / 2,
                Canvas.GetTop(gameWindow.Blaster) + gameWindow.Blaster.Height/2);
            meteoriteTimer.Interval = new TimeSpan(0, 0, 0, 5);
            meteoriteTimer.Start();
            meteoriteTimer.Tick += new EventHandler(CreateMeteorite);
            window = gameWindow;
            GameOver = false;
            window.Timer.Tick += new EventHandler(Move);
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
            var houseImg = new Image();
            var n = new Random().Next(1, 6);
            houseImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/house" + n + ".png"));
            window.Houses.Children.Add(houseImg);
            SendMessage("Построен новый дом!");
            rainAngle -= 5;
            if (window.Houses.Children.Count == goal) EndGame("Победа!");
        }

        public static void DecreaseHealth()
        {
            if (window.HealthPanel.Children.Count == 0) return;
            window.HealthPanel.Children.RemoveAt(0);
        }

        public static void SendMessage(string text) => window.EventTextBlock.Text = text;

        public static bool InCityBounds(IVisualised obj)
            => Math.Abs(obj.Position.X + obj.Radius - centerPoint.X) < window.Houses.ActualWidth / 2;

        public static bool ReachedBottomLine(IVisualised obj)
            => obj.Position.Y + obj.Radius*2 >= window.SkyCanvas.ActualHeight + 100;

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

        static void EndGame(string text)
        {
            SendMessage(text + " Общий счёт: " + (Materials + window.Houses.Children.Count*200));
            GameOver = true;
            meteoriteTimer.Stop();
            canvasObjects.Clear();
        }
    }
}
