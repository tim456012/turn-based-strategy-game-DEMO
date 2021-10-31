using System;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    [SerializeField] private ConversationPanel leftPanel;
    [SerializeField] private ConversationPanel rightPanel;

    private Canvas canvas;
    //private IEnumerator conversation;
    private Tweener transition;

    private const string ShowTop = "Show Top";
    private const string ShowButtom = "Show Buttom";
    private const string HideTop = "Hide Top";
    private const string HideButtom = "Hide Buttom";

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if(leftPanel.panel.CurrentPosition == null)
        {
            leftPanel.panel.SetPosition(HideButtom, false);
        }
        if(rightPanel.panel.CurrentPosition == null)
        {
            rightPanel.panel.SetPosition(HideButtom, false);
        }
        canvas.gameObject.SetActive(false);
    }
}
