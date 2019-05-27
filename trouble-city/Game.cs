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
        public static bool GameOver = true;
        public static int Score = 0;
        public static List<IVisualised> CanvasObjects { get { return canvasObjects; } }
        static List<IVisualised> canvasObjects = new List<IVisualised>();
        public static List<IVisualised> ToAddObjects = new List<IVisualised>();

        public static void Start(MainWindow gameWindow)
        {
            meteoriteTimer.Interval = new TimeSpan(0, 0, 0, 5);
            meteoriteTimer.Start();
            meteoriteTimer.Tick += new EventHandler(CreateMeteorite);
            window = gameWindow;
            GameOver = false;
            window.Timer.Tick += new EventHandler(Move);
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
            if (Score > 400) AddHouse();
            window.ScoreTextBlock.Text = Score.ToString();
        }

        public static void CreateMeteorite(object sender, EventArgs e)
        {
            Add(new Meteorite(new Vector(1, 1)), -50, new Random().Next(0, 400));
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
            Score -= 400;
            var houseImg = new Image();
            var n = new Random().Next(1, 6);
            houseImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/house" + n + ".png"));
            window.Houses.Children.Add(houseImg);
            window.EventTextBlock.Text = "Построен новый дом!";
        }

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
    }
}
