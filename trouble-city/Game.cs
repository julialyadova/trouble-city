using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Media;


namespace trouble_city
{
    static class Game
    {
        static MainWindow window;
        static DispatcherTimer meteoriteTimer = new DispatcherTimer();
        static Vector centerPoint;
        static int goal;
        static int rainAngle;

        //метеориты
        static VisualizedPrefab[] meteorites;
        static int currentMeteorite = 0;

        //пульки
        static VisualizedPrefab[] shootings;
        static int currentShoot = 0;

        public static Vector CenterPoint { get { return centerPoint; } }
        public static int Goal { get { return goal; } }
        public static bool GameOver =true;
        public static int Materials = 0;
        public static List<IVisualised> CanvasObjects { get { return canvasObjects; } }
        static List<IVisualised> canvasObjects = new List<IVisualised>();
        public static List<IVisualised> ToAddObjects = new List<IVisualised>();

        public static void Initialize(MainWindow gameWindow)
        {
            window = gameWindow;
            centerPoint = new Vector(Canvas.GetLeft(window.Blaster) + window.Blaster.Width / 2,
                Canvas.GetTop(window.Blaster) + window.Blaster.Height / 2);
            meteorites = VisualizedPrefab.LoadFromJSON("meteorites");
            shootings = VisualizedPrefab.LoadFromJSON("shootings");
        }

        public static void Start()
        {
            if (window == null) throw new NullReferenceException("Initialize() must be called before Start()");

            goal = 10;
            rainAngle = -80;


            GameOver = false;
            Materials = 0;
            SetTimers();
            window.GiveUpButton.IsEnabled = true;
            RestoreHealth();
            canvasObjects.Clear();
            ToAddObjects.Clear();
            window.Houses.Children.Clear();
            window.HousesCount.Text = "0";
            DecreaseHealth();
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
            var meteor = meteorites[currentMeteorite];
            Add(new Meteorite(Vector.FromAngle(rainAngle),
                              new Random().Next(meteor.MinSize, meteor.MaxSize),
                              meteor.PNGName,
                              meteor.Speed,
                              meteor.Destiny),
                -50, new Random().Next(0, (int)centerPoint.X));
        }

        public static void Shoot()
        {
            var shot = shootings[currentShoot];
            if (Game.GameOver) return;
                new Shot(
                        Vector.FromAngle(90 - window.BlasterRotation.Angle),
                        shot.PNGName,
                        shot.MaxSize,
                        shot.Speed,
                        shot.Destiny
                        ).Set();
        }

        public static void Remove(IVisualised obj) => obj.Health = 0;

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
            window.HousesCount.Text = window.Houses.Children.Count.ToString();
            SendMessage("Построен новый дом!");
            rainAngle -= 5;
            if (window.Houses.Children.Count == goal) End("Победа!");
        }

        public static void DecreaseHealth()
        {
            if (window.HealthPanel.Children.Count == 0 )
                End("вы проиграли!");
            else
                window.HealthPanel.Children.RemoveAt(0);
        }

        public static void RestoreHealth()
        {
            while (window.HealthPanel.Children.Count != 5)
                window.HealthPanel.Children.Add(GetImageByName("heart"));
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

        public static void SendMessage(string message)
        {
            window.EventTextBlock.Text = message;
            window.EventTextBlock.Opacity = 1;
        }

        public static void PlaySound(string name)
        {
            var sound = new SoundPlayer();
            sound.SoundLocation = "../../Audio/" + name + ".wav";
            sound.Play();
        }

        public static void Resize()
        {
            centerPoint = new Vector(Canvas.GetLeft(window.Blaster) + window.Blaster.Width / 2,
                Canvas.GetTop(window.Blaster) + window.Blaster.Height / 2);
        }

        public static Image GetImageByName(string name)
        {
            var img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + name + ".png"));
            return img;
        }

        public static void SwitchOnPauseMode()
        {
            window.Timer.IsEnabled = !window.Settings.IsEnabled;
            meteoriteTimer.IsEnabled = !window.Settings.IsEnabled;
            window.TopMenu.IsEnabled = !window.Settings.IsEnabled;
            window.BlasterControlPanel.IsEnabled = !window.Settings.IsEnabled;
        }

        public static string SwitchMeteorite()
        {
            currentMeteorite++;
            if (currentMeteorite == meteorites.Length)
                currentMeteorite = 0;
            return meteorites[currentMeteorite].Name;
        }

        public static string SwitchShoot()
        {
            currentShoot++;
            if (currentShoot == shootings.Length)
                currentShoot = 0;
            return shootings[currentShoot].Name;
        }

        public static void End(string text)
        {
            SendMessage(text + " Общий счёт: " + (Materials + window.Houses.Children.Count*200));
            GameOver = true;
            window.StartButton.IsEnabled = true;
            window.GiveUpButton.IsEnabled = false;
            window.StartButton.Opacity = 1;
            window.Timer.Stop();
            meteoriteTimer.Stop();
            meteoriteTimer = new DispatcherTimer();

            foreach (var obj in canvasObjects)
            {
                obj.Health = 0;
                window.SkyCanvas.Children.Remove(obj.Img);
            }
            ToAddObjects.Clear();
        }

        static void Destroy(IVisualised obj)
        {
            CanvasObjects.Remove(obj);
            window.SkyCanvas.Children.Remove(obj.Img);
        }

        static void SetTimers()
        {
            meteoriteTimer = new DispatcherTimer();
            meteoriteTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            meteoriteTimer.Start();
            meteoriteTimer.Tick += new EventHandler(CreateMeteorite);
            window.Timer.IsEnabled = true;
        }
    }
}
