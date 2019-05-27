using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public Image Img { get; }
        public int Radius { get; }
        public int Health
        {
            get { return health; }
            set
            {
                if (value <= 0) Destroy();
                else health = value;
            }
        }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }

        int health;
        Vector direction;

        public Meteorite(Vector direction)
        {
            Img = new Image();
            Img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/meteorite.png"));
            var size = new Random().Next(40, 160);
            Img.Width = size;
            health = size;
            Radius = size / 2;
            this.direction = direction;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * 4);
            Canvas.SetLeft(Img, Position.X + direction.X * 4);
            foreach (var other in Game.CanvasObjects)
            {
                if (other == this || !CrashedInto(other)) continue;
                other.Health -= Health;
                Health -= other.Health;
            }
        }

        public bool CrashedInto(IVisualised other)
        {
            return (Math.Abs(Position.X - other.Position.X) < Radius + other.Radius)
                && (Math.Abs(Position.Y - other.Position.Y) < Radius + other.Radius);
        }

        public void Destroy() => Game.Destroy(this);
    }
}
