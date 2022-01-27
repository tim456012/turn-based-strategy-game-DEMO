using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityMenuEntry : MonoBehaviour
{
    [System.Flags] 
    private enum States
    {
        None = 0,
        Selected = 1 << 0,
        Locked = 1 << 1
    }
    
    [SerializeField] private Image bullet;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private TextMeshProUGUI label;
    private Outline _outline;

    public bool IsLocked
    {
        get => (State & States.Locked) != States.None;
        set
        {
            if (value)
            {
                State |= States.Locked;
            }
            else
            {
                State &= ~States.Locked;
            }
        }
    }

    public bool IsSelected
    {
        get => (State & States.Selected) != States.None;
        set
        {
            if (value)
            {
                State |= States.Selected;
            }
            else
            {
                State &= ~States.Selected;
            }
        }
    }
    
    private States State
    {
        get => state;
        set
        {
            if(state == value)
                return;
            state = value;

            if (IsLocked)
            {
                bullet.sprite = disabledSprite;
                label.color = Color.gray;
                _outline.effectColor = new Color32(20, 36, 55, 255);
            }
            else if (IsSelected)
            {
                bullet.sprite = selectedSprite;
                label.color = new Color32(249, 210, 118, 255);
                _outline.effectColor = new Color32(255,160,72,255);
            }
            else
            {
                bullet.sprite = normalSprite;
                label.color = Color.white;
                _outline.effectColor = new Color32(20, 36, 44, 255);
            }
        }
    }
    private States state;
    
    public string Title
    {
        get => label.text;
        set => label.text = value;
    }

    private void Awake()
    {
        _outline = label.GetComponent<Outline>();
    }

    public void Reset()
    {
        State = States.None;
    }
}
