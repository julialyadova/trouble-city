using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public int Size = 100;
        public readonly string ImageName = "meteorite.png";
        public int Health { get { return Size; } set { if (value <= 0) Destroy(); } }
        public Vector Position { get; set; }
        Vector direction;

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
