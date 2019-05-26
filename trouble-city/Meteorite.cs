using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public Image Img { get; }
        public int Radius { get { return (int)Img.Width / 2; } }
        public readonly string ImageName = "meteorite.png";
        public int Health
        {
            get { return health; }
            set
            {
                if (value <= 0) Destroy();
                else health = value;
            }
        }
        public Vector Position { get; set; }
        int health;
        Vector direction;

        public Meteorite(Vector direction)
        {
            this.direction = direction;
            Img = new Image();
            Img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/meteorite.png"));
            var size = new Random().Next(20, 200);
            Img.Width = size;
            health = size;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * 10);
            Canvas.SetLeft(Img, Position.X + direction.X * 10);
            foreach (var other in Game.CanvasObjects)
            {
                if (other == this) continue;
                if (CrashedInto(other))
                {
                    other.Health -= Health;
                    Health -= other.Health;
                }
            }
        }

        public bool CrashedInto(IVisualised other)
        {
            return false;
            return (Math.Abs(Position.X - other.Position.X) < Radius + other.Radius)
                && (Math.Abs(Position.Y - other.Position.Y) < Radius + other.Radius);
        }

        public void Destroy() => Game.Destroy(this);
    }
}
