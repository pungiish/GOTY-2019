using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace UnitHelpFunctions
{
    static class PathFinding
    {
        public static readonly int[,] premik = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        private class StrComparer : IComparer<Str>
        {
            public int Compare(Str first, Str sec)
            {
                if (first.Dist != sec.Dist)
                    return first.Dist < sec.Dist ? 1 : -1;
                if (first.x != sec.x)
                    return first.x < sec.x ? 1 : -1;
                return first.y == sec.y ? 0 : (first.y < sec.y ? 1 : -1);
            }
        }
        private struct Str : IComparable
        {
            public int Dist, x, y;
            public Str(int Dist, int x, int y)
            {
                this.Dist = Dist;
                this.x = x;
                this.y = y;
            }
           
            public int CompareTo(object _sec)
            {
                Str sec = (Str)_sec;
                if (this.Dist != sec.Dist)
                    return this.Dist < sec.Dist ? 1 : -1;
                if (this.x != sec.x)
                    return this.x < sec.x ? 1 : -1;
                return this.y == sec.y ? 0 : (this.y < sec.y ? 1 : -1);
            }
        }

        public static bool InBounds(Tilemap t, int x, int y, int z = 0)
        {
            return t.cellBounds.Contains(new Vector3Int(x, y, z));
        }
        public static void FindPossibleMoves(int posX, int posY, int movePoints, int unitIndex, Tilemap map, 
            Tilemap highlight, bool colorTiles = true)
        {
            Clear(highlight);

            //sortedSet in priority queue ni podprt v Unity, zato uporabimo SortedDictionary kot priority queue
            SortedDictionary<Str, Char> Pq = new SortedDictionary<Str, Char>();

            //izhodisca ne postavimo kot mozen cilj, ker premik na isto mesto ni smiseln
            Vector3Int pos = new Vector3Int(posX, posY, 0);
            highlight.GetTile<HighlightTile>(pos).selectedUnitDistance = 0;

            Pq.Add(new Str(0, posX, posY), '.');
            Str s;
            HighlightTile ht;
            GameTile gt;

            while (Pq.Count > 0)
            {
                //Polje z najkrajso razdaljo vzamemo iz vrste
                s = Pq.Keys.First<Str>();
                
                Pq.Remove(s);

                pos.x = s.x; pos.y = s.y;
                ht = highlight.GetTile<HighlightTile>(pos);
                gt = map.GetTile<GameTile>(pos);

                //ce smo polje ze obdelali z manjso razdaljo (torej smo do njega prisli po drugi poti)
                //potem ga lahko preskocimo
                //do tega lahko pride, ce je eno sosednje polje blizje zacetku 
                //od drugega, vendar je njegova utez veliko vecja
                if (s.Dist > ht.selectedUnitDistance)
                    continue;

                for (int i = 0; i < 4; ++i)
                {
                    pos.x = s.x + premik[i, 0];
                    pos.y = s.y + premik[i, 1];
                    ht = highlight.GetTile<HighlightTile>(pos);
                    gt = map.GetTile<GameTile>(pos);

                    if(ht == null || gt == null) // out of bounds
                    {
                        continue;
                    }

                    //pri ceni premika se uposteva utez polja, na katerega gremo (in ne utez polja na katerem smo)
                   
                    int dbg1 = s.Dist + GameData.MoveWeights[unitIndex, (int)gt.type];
                    if (InBounds(highlight, pos.x, pos.y) &&
                        dbg1 < ht.selectedUnitDistance &&
                        dbg1 <= movePoints)
                    {
                        ht.selectedUnitDistance = s.Dist + dbg1;//UnitData.MoveWeights[unitIndex, (int)gt.type];
                        ht.selectedUnitPreviousPath = i;
                        if (colorTiles == true)
                            ht.changeColor(HighlightTile.TileColor.green);

                        Pq.Add(new Str(ht.selectedUnitDistance, pos.x, pos.y), '.');
                    }
                }
            }


        }

        public static LinkedList<int> GetPath(Tilemap highlight, Vector3Int pos)
        {
            LinkedList<int> rez = new LinkedList<int>();
            int index = highlight.GetTile<HighlightTile>(pos).selectedUnitPreviousPath;
            while (index != -1)
            {
                rez.AddFirst(index);
                pos.x -= premik[index, 0];
                pos.y -= premik[index, 1];
                index = highlight.GetTile<HighlightTile>(pos).selectedUnitPreviousPath;
            }
            return rez;
        }

        public static LinkedList<Vector3Int> GetMovePositions(Tilemap highlight, Vector3Int Endpos)
        {
            LinkedList<int> path = GetPath(highlight, Endpos);
            LinkedList<Vector3Int> positions = new LinkedList<Vector3Int>();
            if (path.Count == 0) //pot ne obstaja
                return null;
            LinkedListNode<int> n = path.Last;
            
            positions.AddFirst(Endpos);
            while (n != null)
            {
                Endpos.x -= premik[n.Value, 0];
                Endpos.y -= premik[n.Value, 1];
                positions.AddFirst(Endpos);
                if (n == path.First)
                    break;

                n = n.Previous;
            }
            if (positions.Count == 0)
                return null;

            return positions;
        }

        private static void Clear(Tilemap highlight)
        {
            Vector3Int min = highlight.cellBounds.min;
            Vector3Int max = highlight.cellBounds.max;
            Vector3Int vec = new Vector3Int(0, 0, 0);

            for (int x = min.x; x <= -min.x ; ++x)
            {
                vec.x = x;
                for (int y = min.y; y <= -min.y; ++y)
                {
                    vec.y = y;
                    HighlightTile t = highlight.GetTile<HighlightTile>(vec);
                    t.selectedUnitDistance = GameData.INF;
                    t.selectedUnitPreviousPath = -1;
                    t.changeColor(HighlightTile.TileColor.red);
                }
            }
        }
    }
}