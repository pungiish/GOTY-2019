//#define INTERACITVE

using System;
using System.Collections.Generic;

//#define INTERACTIVE

namespace PathFinding
{ 
    //Razred Tile ima 3 spremenljivke:
    //Weight pove, koliko tock premika enota porabi za premik cez to polje
    //Color sluzi za obarvanje polja (zaenkrat je pri neobarvanih poljih color 0, pri pobarvanih pa 1)
    //PrevPath sluzi za rekonstrukcijo poti od ciljega do zacetnega polja
    //V PrevPath hranim samo index premika, ce zelimo dobiti prejsnje polje, moramo od trenutnih koordinat odsteti premik
    class Tile
    {
        //public int Weight { get; set; }
        public int Type;
        public int Color { get; set; }
        public int PrevPath { get; set; }
        
        public Tile()
        {
            Type = 0;
            Color = 0;
            PrevPath = 0;
        }
    }
   

    //Program ki prebere podatke in poklice metodo za pathfinding
    class Program
    {
        static void Main(string[] args)
        {
            int x, y, startx, starty, destx, desty;
#if INTERACTIVE
            Console.WriteLine("1. vrstica: zacetna tocka figure(x, y)");
            Console.WriteLine("3. vrstica: (x, y) destinacija, do katere se naj izpise pot");
            Console.WriteLine("2.vrstica: dimenzije mape (x, y)");
            Console.WriteLine("Sledi y vrsitc s po x elementi, cene premika po polju v mapi");
#endif
            string[] line = Console.ReadLine().Split(' ');
            startx = int.Parse(line[0]);
            starty = int.Parse(line[1]);

            line = Console.ReadLine().Split(' ');
            destx = int.Parse(line[0]);
            desty = int.Parse(line[1]);


            line = Console.ReadLine().Split(' ');
            x = int.Parse(line[0]);
            y = int.Parse(line[1]);

            var map = new Tile[x, y];

            for (int i = 0; i < y; ++i)
            {
                line = Console.ReadLine().Split(' ');

                for (int j = 0; j < x; ++j)
                {
                    map[j, i] = new Tile();
                    map[j, i].Type = int.Parse(line[j]);
                }
            }


            Warrior[] warrior = new Warrior[2] { new Hulk(), new RobinHood() };

#if INTERACTIVE
            Console.WriteLine("Rezultati (enota se na polje lahko premakne, ce je tam vrednost 1):");
            Console.WriteLine();
#endif

            for (int wIndex = 0; wIndex < warrior.Length; ++wIndex)
            {
                Console.WriteLine(warrior[wIndex].ToString());
                warrior[wIndex].FindPossibleMoves(startx, starty, map);
                
                for (int i = 0; i < y; ++i)
                {
                    for (int j = 0; j < x; ++j)
                        Console.Write(map[j, i].Color.ToString() + ' ');

                    Console.WriteLine();
                }

                LinkedList<Tuple<int, int>> path = warrior[wIndex].GetPath(destx, desty, map);
                if (path.Count == 0)
                {
                    Console.WriteLine("Ne obstaja nobena pot med podanim zacetkom in koncem.");
                }
                else
                {
                    Console.WriteLine("Path:");


                    foreach (Tuple<int, int> p in path)
                    {
                        Console.WriteLine(p.Item1 + "  " + p.Item2);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
