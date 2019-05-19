using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public int Size = 144;
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
            Health = 5 * Size;
            Position = new Vector(positionX, 0);
            this.direction = direction;
        }

        public void Act()
        {
            Position = new Vector(Position.X + direction.X, Position.Y + direction.Y);
            foreach (var other in State.MovingObjects)
            {
                if (other == this) continue;
                if (other.IsTriggered(this as IVisualised))
                {
                    other.Health -= Health;
                    Health -= other.Health;
                }
            }
        }

        public bool IsTriggered(IVisualised other)
        {
            return (Math.Sqrt(other.Position.X - Position.X)
                    + Math.Sqrt(other.Position.X - Position.X)
                    < Math.Sqrt(0.4 * Size));
        }

        public void Destroy() => State.MovingObjects.Remove(this);
    }
}
