using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public Image Img { get; }
        public double Radius { get { return Img.RenderSize.Width / 2; } }
        public readonly string ImageName = "meteorite.png";
        public int Health
        {
            get { return Health; }
            set
            {
                if (value <= 0) Destroy();
                else Health = value;
            }
        }
        public Vector Position { get; set; }

        Vector direction;

        public Meteorite(double positionX, Vector direction)
        {
            Health = 10 * (int) Math.Ceiling(Radius);
            Position = new Vector(positionX, 0);
            this.direction = direction;
        }

        public void Act()
        {
            Position = new Vector(Position.X + direction.X, Position.Y + direction.Y);
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
            return (Math.Abs(Position.X - other.Position.X) < Radius + other.Radius)
                && (Math.Abs(Position.Y - other.Position.Y) < Radius + other.Radius);
        }

        public void Destroy() => Game.CanvasObjects.Remove(this);
    }
}
