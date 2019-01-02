using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class GameState
{
    public static PlayerController selectedPlayer = null;
    public static Unit selectedUnit = null;
    public static volatile bool UnitMoving = false;
}

