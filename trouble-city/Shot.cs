using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace trouble_city
{
    class Shot : IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        Vector direction;

        public Shot(Vector blasterDirection)
        {
            Img = new Image();
            Img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/shot.png"));
            Img.Width = 20;
            Radius = 10;
            Health = 20;
            direction = blasterDirection;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * 10);
            Canvas.SetLeft(Img, Position.X + direction.X * 10);
        }

        public void Set()
        {
            Game.Add(this, (int)(Game.CenterPoint.Y + direction.Y * 75) - Radius,
                (int)(Game.CenterPoint.X + direction.X * 75)- Radius);
        }
    }
}
