using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Shot: IVisualised
    {
        double speed = 4;
        Vector direction;

        public int Health { get; set; }
        public Vector Position { get; set; }

        public Shot(Vector blasterPosition, Vector blasterDirection)
        {
        }

        public void Act()
        {
        }

        public bool IsTriggered(IVisualised other)
        {
            return false;
        }

        public void Destroy() => State.MovingObjects.Remove(this);
    }
}
