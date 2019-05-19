using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Shot: IVisualised
    {
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

        public Shot(Vector blasterPosition, Vector blasterDirection)
        {
            Position = blasterPosition;
            direction = blasterDirection;
        }

        public void Act()
        {
            Position = new Vector(Position.X + direction.X, Position.Y + direction.Y);
        }

        public bool IsTriggered(IVisualised other)
        {
            return (Math.Sqrt(other.Position.X - Position.X)
                    + Math.Sqrt(other.Position.X - Position.X)
                    < Math.Sqrt(0.4 * 10));
        }

        public void Destroy() => State.MovingObjects.Remove(this);
    }
}
