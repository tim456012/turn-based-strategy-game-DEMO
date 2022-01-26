using System.Collections;
using UnityEngine;

public class PanelTest : MonoBehaviour
{
    private Panel panel;
    private const string Show = "Show";
    private const string Hide = "Hide";
    private const string Center = "Ceneter";

    // Use this for initialization
    void Start()
    {
        panel = GetComponent<Panel>();
        Panel.Position centerPos = new Panel.Position(Center, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter);
        panel.AddPosition(centerPos);
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,30), Show))
        {
            panel.SetPosition(Show, true);
        }
        if(GUI.Button(new Rect(10,50,100,30), Hide))
        {
            panel.SetPosition(Hide, true);
        }
        if(GUI.Button(new Rect(10,90,100,30), Center))
        {
            Tweener t = panel.SetPosition(Center, true);
            t.easingControl.equation = EasingEquations.EaseInBack;
        }
    }
}