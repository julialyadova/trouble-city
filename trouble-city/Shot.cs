using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Shot: IVisualised
    {
        public Image Img { get; }
        public double Radius { get { return Img.RenderSize.Width / 2; } }
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

        public void Destroy() => Game.CanvasObjects.Remove(this);
    }
}
