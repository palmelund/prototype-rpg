using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets
{
    public class map : MonoBehaviour
    {

        public tile[,] Map;

        public static map Instance;
        public int Width;
        public int Height;

        // Use this for initialization
        void Start ()
        {
            Instance = this;
            
            Width = 10;
            Height = 10;

            Map = new tile[10,10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Map[x,y] = new tile(x,y);
                }
            }
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public List<tile> GetNeighbors(tile t)
        {
            return new List<tile>(8)
            {
                GetTileAt(t.XCoord + 1, t.YCoord + 1),
                GetTileAt(t.XCoord + 1, t.YCoord),
                GetTileAt(t.XCoord + 1, t.YCoord - 1),
                GetTileAt(t.XCoord, t.YCoord - 1),
                GetTileAt(t.XCoord - 1, t.YCoord - 1),
                GetTileAt(t.XCoord - 1, t.YCoord),
                GetTileAt(t.XCoord - 1, t.YCoord + 1),
                GetTileAt(t.XCoord, t.YCoord + 1)
            };
        }

        public tile GetTileAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Map[x, y];
            }
            else
            {
                return null;
            }
        }

        public PathFinderDirection GetPathFinderDirection(tile from, tile to)
        {
            if (from.Equals(to)) throw new Exception("how?");

            int rX = to.XCoord - from.XCoord;
            int rY = to.YCoord - from.YCoord;

            if (rX == 1 && rY == 0) return PathFinderDirection.Right;
            if (rX == -1 && rY == 0) return PathFinderDirection.Left;

            if (rX == 0 && rY == 1) return PathFinderDirection.Up;
            if (rX == 0 && rY == -1) return PathFinderDirection.Down;

            if (rX == 1 && rY == 1) return PathFinderDirection.UpRight;
            if (rX == 1 && rY == -1) return PathFinderDirection.DownRight;

            if (rX == -1 && rY == 1) return PathFinderDirection.UpLeft;
            if (rX == -1 && rY == -1) return PathFinderDirection.DownLeft;

            throw new Exception("The PathFinding Algorithm skipped a tile...");
        }
    }
}
