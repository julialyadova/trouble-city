using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Blaster: IVisualised
    {
        public double RotationAngle = 0;
        public int Health { get { return 500; } set { if (value <= 0) Destroy(); } }
        public Vector Position { get; set; }

        public void Act()
        {

        }

        public void Shoot()
        {
            //State.MovingObjects.Add(new Shot());
        }

        public bool IsTriggered(IVisualised other)
        {
            return false;
        }

        public void Destroy() => State.MovingObjects.Remove(this);
    }
}
