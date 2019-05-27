using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace trouble_city
{
    static class Game
    {
        static Canvas canvas;
        public static bool GameOver = true;
        static DispatcherTimer meteoriteTimer = new DispatcherTimer();
        public static int Score = 0;
        public static List<IVisualised> CanvasObjects = new List<IVisualised>();

        public static void Start(DispatcherTimer timer, Canvas gameCanvas)
        {
            meteoriteTimer.Interval = new TimeSpan(0, 0, 0, 5);
            meteoriteTimer.Start();
            meteoriteTimer.Tick += new EventHandler(CreateMeteorite);
            canvas = gameCanvas;
            GameOver = false;
            timer.Tick += new EventHandler(Move);
        }

        public static void Move(object sender, EventArgs e)
        {
            foreach (var obj in CanvasObjects)
                if (OutsideTheWindow(obj)) obj.Destroy();
                else obj.Act();
        }

        public static void CreateMeteorite(object sender, EventArgs e)
        {
            Add(new Meteorite(new Vector(1, 1)), -50, new Random().Next(0, 400));
        }

        public static void Add(IVisualised obj, int top, int left)
        {
            canvas.Children.Add(obj.Img);
            Canvas.SetTop(obj.Img, top);
            Canvas.SetLeft(obj.Img, left);
            CanvasObjects.Add(obj);
        }

        public static bool OutsideTheWindow(IVisualised obj)
        {
            return obj.Position.X < -100 || obj.Position.X > canvas.Width
                || obj.Position.Y < -100 || obj.Position.Y > canvas.Height;
        }

        public static void Destroy(IVisualised obj) => canvas.Children.Remove(obj.Img);
    }
}
