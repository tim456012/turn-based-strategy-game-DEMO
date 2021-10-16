using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] internal GameObject tileViewPrefab;
    [SerializeField] internal GameObject tileSelectionIndicatorPrefab;

    internal Transform marker
    {
        get
        {
            if(_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    internal Transform _marker;

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    [SerializeField] internal int width = 10;
    [SerializeField] internal int depth = 10;
    [SerializeField] internal int height = 8;

    [SerializeField] internal Point pos;

    [SerializeField] internal LevelData levelData;

    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    internal Rect RandomRect()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, depth);
        int w = Random.Range(1, width - x + 1);
        int h = Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    internal void GrowRect(Rect rect)
    {
        for(int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for(int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    internal void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    internal Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    internal Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
        {
            return tiles[p];
        }

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    internal void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if(t.height < height)
        {
            t.Grow();
        }
    }

    internal void ShrinkSingle(Point p)
    {
        if(!tiles.ContainsKey(p))
        {
            return;
        }

        Tile t = tiles[p];
        t.Shrink();

        if(t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    public void Grow()
    {
        GrowSingle(pos);
    }

    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    public void Clear()
    {
        for(int i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Game/Resources/Levels";
        if (!Directory.Exists(filePath))
        {
            CreateSaveDirectory();
        }
        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
        {
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));
        }

        string fileName = string.Format("Assets/Game/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    internal void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Game/Resources";
        if(!Directory.Exists(filePath))
        {
            AssetDatabase.CreateFolder("Assets/Game", "Resources");
        }
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
        {
            AssetDatabase.CreateFolder("Assets/Game/Resources", "Levels");     
        }
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        Clear();
        if(levelData == null)
        {
            return;
        }

        foreach(Vector3 v in levelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
}
