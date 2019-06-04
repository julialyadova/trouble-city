using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        Vector direction;

        public Meteorite(Vector blasterDirection)
        {
            Img = Game.GetImageByName("meteorite");
            var size = new Random().Next(40, 160);
            Img.Width = size;
            Health = size;
            Radius = size / 2;
            direction = blasterDirection;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * 4);
            Canvas.SetLeft(Img, Position.X + direction.X * 4);
            foreach (var other in Game.CanvasObjects)
            {
                if (other == this || !CrashedInto(other)) continue;
                var enemyHealth = other.Health;
                other.Health -= Health;
                Health -= enemyHealth;
            }
            if (Health <= 0)
            {
                Game.Add(new Debris(this), (int)Position.Y, (int)Position.X);
                Game.Materials += Radius * 2;
            }
            if (Game.ReachedBottomLine(this))
            {
                Health = 0;
                Game.Add(new Explosion(this), (int)Position.Y - Radius*2, (int)Position.X - Radius);
                if (Game.InCityBounds(this))
                {
                    Game.SendMessage("Метеорит упал на город!");
                    Game.DecreaseHealth();
                }   
            }
        }

        public bool CrashedInto(IVisualised other)
        {
            return (Math.Abs(Position.X + Radius - other.Position.X - other.Radius) < Radius + other.Radius)
                && (Math.Abs(Position.Y + Radius - other.Position.Y - other.Radius) < Radius + other.Radius);
        }
    }
}
