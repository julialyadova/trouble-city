using System;

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

        public static Vector FromAngle(double angle)
        {
            angle = Math.PI * angle / 180;
            return new Vector(Math.Cos(angle), -Math.Sin(angle)).Normalize();
        }
    }
}
