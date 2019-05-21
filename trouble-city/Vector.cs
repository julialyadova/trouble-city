using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    struct Vector
    {
        public readonly double X;
        public readonly double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector Normalize()
        {
            var length = Math.Sqrt(X * X + Y * Y);
            return new Vector(X / length, Y / length);
        }        
    }
}
