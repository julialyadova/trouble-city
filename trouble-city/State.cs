using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    static class State
    {
        public static bool GameOver = false;
        public static List<IVisualised> MovingObjects= new List<IVisualised>();
    }
}
