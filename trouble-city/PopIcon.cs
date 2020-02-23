using System.Windows.Controls;

namespace trouble_city
{
    class PopIcon: IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        public PopIcon(Vector position)
        {
            Img = Game.GetImageByName("settings");
            //Img.Width = meteor.Radius * 4;
            Health = 1;
            //Game.PlaySound("popicon");
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y - 12);
            Img.Opacity -= 0.1;
            if (Img.Opacity == 0) Health = 0;
        }
    }
}