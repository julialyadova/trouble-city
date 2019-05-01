using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    class Meteorite
    {
        public int Size = 1;
        Vector position;

        public void Act()
        {

        }

        public bool IsTriggered(Shot shot)
        {
            return false;
        }
    }
}
