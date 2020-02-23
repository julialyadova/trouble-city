using System.Windows.Controls;

namespace trouble_city
{
    class Shot : IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        Vector direction;
        int speed;

        public Shot(Vector blasterDirection, string imageName, int size, int preSpeed, int preDestiny)
        {
            Img = Game.GetImageByName(imageName);
            Img.Width = size;
            Radius = size/2;
            Health = size*preDestiny;
            direction = blasterDirection;
            speed = preSpeed;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * speed);
            Canvas.SetLeft(Img, Position.X + direction.X * speed);
        }

        public void Set()
        {
            Game.Add(this, (int)(Game.CenterPoint.Y + direction.Y * 75) - Radius,
                (int)(Game.CenterPoint.X + direction.X * 75)- Radius);
        }
    }
}
