using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    private Color selectedTileColor = new Color(0, 1, 1, 1);
    private Color defaultTileColor = new Color(1, 1, 1, 1);

    private Point[] dirs = new Point[4]
    {
            new Point(0, 1),
            new Point(0, -1),
            new Point(1, 0),
            new Point(-1, 0)
    };

    public void Load (LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while(checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            
            for(int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if(next == null || next.distance <= t.distance + 1)
                {
                    continue;
                }

                if(addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }

            if(checkNow.Count == 0)
            {
                SwapReference(ref checkNow, ref checkNext);
            }
        }

        return retValue;
    }

    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    public void SelectTiles(List<Tile> tiles)
    {
        for(int i = tiles.Count - 1; i>=0; --i)
        {
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
        }
    }

    public void DeSelectTiles(List<Tile> tiles)
    {
        for(int i = tiles.Count - 1; i >=0; --i)
        {
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
        }
    }

    private void ClearSearch()
    {
        foreach(Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    private void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }


}

