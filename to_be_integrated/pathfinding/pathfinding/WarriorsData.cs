using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public static class WarriorsData
    {
        public const int NumOfDifferentUnits = 1;
        public const int NumOfDifferentTerrainTypes = 5;
        public const int INF = Int32.MaxValue;
        public const int INF_WEIGHT = INF - 1;

        public static readonly int[,] MoveWeights = {
            //ground, water, forest, mountain, border
            {2, 15, 10, INF_WEIGHT, INF_WEIGHT }, //HULK
            {1, 7, 3, 8,  INF_WEIGHT}//Robin hood
        };

        public static readonly int[] BaseMoveRanges = new int[] {
            15, //Hulka
            20  //Robin Hood
        };

        public static readonly int[] BaseMelee = new int[]
        {
            30, //Hulk
            15 //Robin Hood
        };

        public static readonly int[] Health = new int[]
        {
            150, //Hulk
            100 //Robin Hood
        };

        public static readonly int[] BaseRanged = new int[]
        {
            0, //Hulk
            30 //RobinHood
        };
        public static readonly int[] FightRange = new int[]
        {
            0, //Hulk
            10 //RobinHood
        };
        public static readonly int[] Defence = new int[]
        {
            10, //Hulk
            1 //robinHood
        };
    }
}
