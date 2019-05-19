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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<IVisualised, Image> images = new Dictionary<IVisualised, Image>();

        public MainWindow()
        {
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Run);
            timer.Tick += new EventHandler(InitiateMeteorite);
            timer.Tick += new EventHandler(DecreaseHealth);
            timer.Interval = new TimeSpan(0,0,0,0,500);
            timer.Start();
        }

        void Run(object sender, EventArgs e)
        {
                MeteoriteAngle.Angle +=3;
        }

        void InitiateMeteorite(object sender, EventArgs e)
        {
            Image img = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(@"..\..\Images\meteorite.jpg", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            SkyCanvas.Children.Add(img);
            SkyCanvas.UpdateLayout();
        }

        void Act(object sender, EventArgs e)
        {
            foreach (var gameObject in State.MovingObjects)
            {
                if (!images.ContainsKey(gameObject))
                {
                    SkyCanvas.Children.Remove(images[gameObject]);
                    images.Remove(gameObject);
                }
                gameObject.Act();
            }
        }

        public void DecreaseHealth(object sender, EventArgs e)
        {
            if (HealthPanel.Children.Count == 0) return;
            HealthPanel.Children.RemoveAt(0);
        }
    }


}
