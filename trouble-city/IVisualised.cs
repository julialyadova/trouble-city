using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trouble_city
{
    interface IVisualised
    {
        int Health { get; set; }
        Vector Position { get;}

        void Act();
        bool IsTriggered(IVisualised other);
        void Destroy();
    }
}
