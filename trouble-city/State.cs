using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    static class State
    {
        public static int CityHealth;
        public static Blaster MainBlaster;
        public static bool GameOver;
        public static List<IVisualised> MovingObjects= new List<IVisualised>();

        public static void StartGame(EventHandler events)
        {
            GameOver = false;
            CityHealth = 1000;
            MainBlaster = new Blaster();
        }
    }
}
