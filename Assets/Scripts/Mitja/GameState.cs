using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public static class GameState
{
    public static PlayerController selectedPlayer = null;
    public static Unit selectedUnit = null;

    public static void MovementStart()
    {
        Interlocked.Increment(ref numOfUnitsMoving);
    }
    public static void MovementEnd()
    {
        Interlocked.Decrement(ref numOfUnitsMoving);
    }
    public static bool UnitsMoving()
    {
        int val = numOfUnitsMoving; //load je atomarna operacija
        return val != 0;
    }

    private static int numOfUnitsMoving = 0;


}

