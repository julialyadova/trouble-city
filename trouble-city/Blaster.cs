using System;
using System.Windows;
using System.Windows.Controls;

namespace trouble_city
{
    class Blaster
    {
        public double RotationAngle = 0;
        public Vector Position { get; set; }

        public void Shoot()
        {
            var realAngle = Math.PI * (RotationAngle + 90) / 180;
            var directionVector = new Vector(-Math.Acos(realAngle), -Math.Asin(realAngle)).Normalize();
            Game.CanvasObjects.Add(new Shot(directionVector));
        }
    }
}
