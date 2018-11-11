using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Hulk : Warrior
    {
        public Hulk()
        {
            Type = 0;
            this.MoveRange = WarriorsData.BaseMoveRanges[Type];
        }

        public override string ToString()
        {
            return base.ToString() + ": Hulk";
        }
    }
}
