//#define SHOW_DIST

using System;
using System.Collections.Generic;

namespace PathFinding
{
    //Zaenkrat sem v razred Warrior dodal samo spremenljivke, ki so potrebne za path finding.
    //Ce bo smiselno, bom ob razsiritvi tega razreda, del za iskanje poti premaknil v podrazred
    abstract class Warrior
    {
        public const int INF = Int32.MaxValue;
        public const int INF_WEIGHT = INF - 1;
        protected int MoveRange = -1;
        protected int Type = -1;
        
        struct Str
        {
            public int Dist, x, y;
            public Str(int Dist, int x, int y)
            {
                this.Dist = Dist;
                this.x = x;
                this.y = y;
            }
        }
        private readonly int[,] premik = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        int CurrX, CurrY; //to je zacasna resitev, v koncnem izdelku bo imela enota referenco na Tile na katerem se nahaja
        private static bool InBounds(int x, int y, int indexPremika, Tile[,] map)
        {
            bool b = false;
            switch (indexPremika)
            {
                case 0:
                    if (x < map.GetLength(0))
                        b = true;
                    break;
                case 1:
                    if (x >= 0)
                        b = true;
                    break;
                case 2:
                    if (y < map.GetLength(1))
                        b = true;
                    break;
                case 3:
                    if (y >= 0)
                        b = true;
                    break;
            }
            return b;
        }

        public void FindPossibleMoves(int posX, int posY, Tile[,] map)
        {
            CurrX = posX;
            CurrY = posY;
            //pomozna matrika, v katerem hranimo razdaljo od zacetnega do trenutnega polja
            Int32[,] distance = new int[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < distance.GetLength(0); ++i)
                for (int j = 0; j < distance.GetLength(1); ++j)
                    distance[i, j] = INF;


            //ker C# nima standardne prednostne vrste, bomo uporabili SortedSet
            //pri compare funkciji je zaradi brisanja pomembno, da razlikujemo tudi med razlicnimi elementi z enakimi razdaljami
            SortedSet<Str> Pq = new SortedSet<Str>(Comparer<Str>.Create(delegate (Str a, Str b) {
                if (a.Dist != b.Dist)
                    return a.Dist < b.Dist ? 1 : -1;
                if (a.x != b.x)
                    return a.x < b.x ? 1 : -1;
                return a.y == b.y ? 0 : (a.y < b.y ? 1 : -1);
            }));

            //izhodisca ne postavimo kot mozen cilj, ker premik na isto mesto ni smiseln
            distance[posX, posY] = 0;
            Pq.Add(new Str(0, posX, posY));
            Str s;

            while (Pq.Count > 0)
            {
                //Polje z najkrajso razdaljo vzamemo iz vrste
                s = Pq.Min;
                Pq.Remove(s);

                //ce smo polje ze obdelali z manjso razdaljo (torej smo do njega prisli po drugi poti)
                //potem ga lahko preskocimo
                //do tega lahko pride, ce je eno sosednje polje blizje zacetku 
                //od drugega, vendar je njegova utez veliko vecja
                if (s.Dist > distance[s.x, s.y])
                    continue;

                for (int i = 0; i < 4; ++i)
                {
                    int nx = s.x + premik[i, 0];
                    int ny = s.y + premik[i, 1];

                    //pri ceni premika se uposteva utez polja, na katerega gremo (in ne utez polja na katerem smo)
                    if (InBounds(nx, ny, i, map) &&
                        s.Dist + WarriorsData.MoveWeights[this.Type, map[nx, ny].Type] < distance[nx, ny] &&
                        s.Dist + WarriorsData.MoveWeights[this.Type, map[nx, ny].Type] <= MoveRange)
                    {
                        distance[nx, ny] = s.Dist + WarriorsData.MoveWeights[this.Type, map[nx, ny].Type];
                        map[nx, ny].PrevPath = i;
                        map[nx, ny].Color = 1;
                        Pq.Add(new Str(distance[nx, ny], nx, ny));
                    }
                }
            }

#if SHOW_DIST
            //za prikazovanje razdalje med zacetnim in ostalimi polji v prvi vrstici napisi #define SHOW_DIST
            for (int i = 0; i < distance.GetLength(1); ++i)
            {
                for (int j = 0; j < distance.GetLength(0); ++j)
                    Console.Write((distance[j, i]).ToString() + " ");
                Console.WriteLine();
            }
#endif
        }

        public LinkedList<Tuple<int, int>> GetPath(int destX, int destY, Tile[,] map)
        {
            LinkedList<Tuple<int, int>> path = new LinkedList<Tuple<int, int>>();

            //Ce barva koncnega polja ni 1, to pomeni, da enota na tisto polje ne more priti
            //zato vrnemo prazen list
            if (map[destX, destY].Color != 1)
                return path;

            path.AddFirst(new Tuple<int, int>(destX, destY));
            while (destX != CurrX || destY != CurrY)
            {
                int prevX = destX - premik[map[destX, destY].PrevPath, 0];
                int prevY = destY - premik[map[destX, destY].PrevPath, 1];
                destX = prevX;
                destY = prevY;

                path.AddFirst(new Tuple<int, int>(destX, destY));
            }
            return path;
        }
        override public string ToString()
        {
            return "Warrior";
        }
    }
}
