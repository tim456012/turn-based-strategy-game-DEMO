using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityMenuPanelController : MonoBehaviour
{
    private const string ShowKey = "Show";
    private const string HideKey = "Hide";
    private const string EntryPoolKey = "AbilityMenuPanel.Entry";
    private const int MenuCount = 4;

    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private Panel panel;
    [SerializeField] private GameObject canvas;
    private List<AbilityMenuEntry> _menuEntries = new(MenuCount);
    public int selection { get; private set; }

    private void Awake()
    {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    private void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false);
    }

    private AbilityMenuEntry Dequeue()
    {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);
        entry.Reset();
        return entry;
    }

    private void Enqueue(AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    private void Clear()
    {
        for (int i = _menuEntries.Count - 1; i >= 0; --i)
        {
            Enqueue(_menuEntries[i]);
        }
        _menuEntries.Clear();
    }

    private Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    private bool SetSelection(int value)
    {
        if (_menuEntries[value].IsLocked)
            return false;

        if (selection >= 0 && selection < _menuEntries.Count)
        {
            _menuEntries[selection].IsSelected = false;
        }
        selection = value;

        if (selection >= 0 && selection < _menuEntries.Count)
        {
            _menuEntries[selection].IsSelected = true;
        }

        return true;
    }

    public void Next()
    {
        for (int i = selection + 1; i < selection + _menuEntries.Count; ++i)
        {
            int index = i % _menuEntries.Count;
            if(SetSelection(index))
                break;
        }
    }

    public void Previous()
    {
        for (int i = selection - 1 + _menuEntries.Count; i > selection; --i)
        {
            int index = i % _menuEntries.Count;
            if(SetSelection(index))
                break;
        }
    }

    public void Show(string title, List<string> options)
    {
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for (int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            _menuEntries.Add(entry);
        }
        SetSelection(0);
        TogglePos(ShowKey);
    }

    public void SetLocked(int index, bool value)
    {
        if (index < 0 || index >= _menuEntries.Count)
            return;

        _menuEntries[index].IsLocked = value;
        if (value && selection == index)
        {
            Next();
        }
    }

    public void Hide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate(object sender, EventArgs args)
        {
            if (panel.CurrentPosition != panel[HideKey])
                return;
            Clear();
            canvas.SetActive(false);
        };
    }
}
