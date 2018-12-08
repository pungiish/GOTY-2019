using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class RobinHood : Warrior
    {
        public RobinHood()
        {
            this.Type = 1;
            this.MoveRange = WarriorsData.BaseMoveRanges[Type];
            this.HealthPoints = WarriorsData.Health[Type];
        }

        public override string ToString()
        {
            return base.ToString() + ": Robin Hood " + ", HP: " + HealthPoints;
        }
    }
}
