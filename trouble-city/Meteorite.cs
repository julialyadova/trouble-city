using System;
using System.Windows.Controls;

namespace trouble_city
{
    class Meteorite: IVisualised
    {
        public Image Img { get; }
        public Vector Position { get { return new Vector(Canvas.GetLeft(Img), Canvas.GetTop(Img)); } }
        public int Radius { get; }
        public int Health { get; set; }

        public string ImageName;

        Vector direction;
        int speed;

        /// <summary>
        /// конструктор для создания метеоритоподбных
        /// </summary>
        /// <param name="directVector">направление падения</param>
        /// <param name="size">размер (адекватный - от 20 до 160)</param>
        /// <param name="imageName">нахвание изображения</param>
        /// <param name="preSpeed">скорость падения</param>
        /// <param name="preDestiny">плотность. влияет на здоровье и наносимый урон</param>
        public Meteorite(Vector directVector, int size, string imageName, int preSpeed, int preDestiny)
        {
            Img = Game.GetImageByName(imageName);
            Img.Width = size;
            Health = size * preDestiny;
            Radius = size / 2;
            ImageName = imageName;
            direction = directVector;
            speed = preSpeed;
        }

        public void Act()
        {
            Canvas.SetTop(Img, Position.Y + direction.Y * speed);
            Canvas.SetLeft(Img, Position.X + direction.X * speed);
            foreach (var other in Game.CanvasObjects)
            {
                if (other == this || !CrashedInto(other)) continue;
                var enemyHealth = other.Health;
                other.Health -= Health;
                Health -= enemyHealth;
            }
            if (Health <= 0) Crash();
            if (Game.ReachedBottomLine(this)) Explode();
        }

        public bool CrashedInto(IVisualised other)
        {
            return (Math.Abs(Position.X + Radius - other.Position.X - other.Radius) < Radius + other.Radius)
                && (Math.Abs(Position.Y + Radius - other.Position.Y - other.Radius) < Radius + other.Radius);
        }

        private void Crash()
        {
            Game.Add(new Debris(this), (int)Position.Y, (int)Position.X);
            Game.Materials += Radius * 2;
        }

        private void Explode()
        {
            Game.Add(new Explosion(this), (int)Position.Y - Radius * 2, (int)Position.X - Radius);
            if (Game.InCityBounds(this))
            {
                Game.SendMessage("Метеорит упал на город!");
                Game.DecreaseHealth();
            }
            Game.Remove(this);
        }
    }
}
