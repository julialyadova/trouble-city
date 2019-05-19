using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Blaster
    {
        public double RotationAngle = 0;
        public Vector Position { get; set; }

        public Shot Shoot()
        {
            var realAngle = Math.PI * (RotationAngle + 90) / 180;
            var directionVector = new Vector(-Math.Acos(realAngle), -Math.Asin(realAngle));
            return new Shot(Position, directionVector);
        }
    }
}
