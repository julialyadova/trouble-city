using System.Windows.Controls;

namespace trouble_city
{
    class Debris : IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        public Debris(Meteorite meteor)
        {
            Img = Game.GetImageByName("meteorite_crash");
            Img.Width = meteor.Radius*2;
            Health = 1;
            Game.PlaySound("crash");
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + 12);
            Img.Opacity -= 0.05;
            if (Img.Opacity == 0) Health = 0;
        }
    }
}
