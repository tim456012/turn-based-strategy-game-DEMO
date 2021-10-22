using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;
    
    public GameObject heroPerfab;
    public Unit unit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState<InitBattleState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
