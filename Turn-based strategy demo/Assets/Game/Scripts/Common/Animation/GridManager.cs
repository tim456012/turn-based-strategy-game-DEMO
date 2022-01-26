using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Hex; //used to store the hex model prefab
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;

    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Hexagon process
    //Initialise Hexagon
    void setSize()
    {
        hexWidth = Hex.GetComponent<Renderer>().bounds.size.x;
        hexHeight = Hex.GetComponent<Renderer>().bounds.size.y;
    }

    //Calculate the position of the first hexgon tile: center (0,0,0)
    internal Vector3 calcInitPos()
    {
        Vector3 initPos;
        //Initial position in the left upper corner
        initPos = new Vector3(-hexWidth * (gridWidthInHexes / 2f) + (hexWidth / 2), 
            0, 
            (gridHeightInHexes / 2f) * hexHeight - (hexHeight / 2));

        return initPos;
    }

    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
        Vector3 initPos = calcInitPos();

        float offset = 0f;
        if(gridPos.y % 2 != 0)
        {
            offset = hexWidth / 2;
        }
        float x = initPos.x + offset + gridPos.x * hexWidth;
        float z = initPos.z - gridPos.y * hexHeight * 0.75f;
        return new Vector3(x, 0, z);
    }

    internal void createGrid()
    {

    }

    #endregion
}
