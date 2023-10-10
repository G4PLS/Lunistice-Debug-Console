using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace Lunistice_DebugConsole.UI;

public class PlayerTabUI : TabPageUI, ITabbable
{
    private int _selectedTab = 0;
    private readonly List<TabPageUI> _tabPages = new();
    private readonly List<ButtonRef> _tabButtons = new();
    
    public PlayerTabUI(GameObject parent) : base(parent)
    {
    }

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("PlayerTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        GameObject tabGroup = UIFactory.CreateHorizontalGroup(_uiRoot, "PlayerStatsTabGroup", true, true, true, true);
        UIFactory.SetLayoutElement(tabGroup, minHeight: 25, flexibleHeight: 0);

        AddTab<PlayerStatsUI>(tabGroup, "Hana", new object[]
        {
            _uiRoot,
            Timer.Character.Hana
        });
        AddTab<PlayerStatsUI>(tabGroup, "Toree", new object[]
        {
            _uiRoot,
            Timer.Character.Toree
        });
        AddTab<PlayerStatsUI>(tabGroup, "Toukie", new object[]
        {
            _uiRoot,
            Timer.Character.Toukie
        });
    }

    public void SetTab(int tabIndex)
    {
        if (_selectedTab != -1)
            DisableTab(_selectedTab);

        if (_tabPages.Count <= 0 || _tabButtons.Count <= 0 || tabIndex < 0 || tabIndex > _tabPages.Count - 1)
            return;

        UIModel content = _tabPages[tabIndex];
        content.SetActive(true);

        _selectedTab = tabIndex;
    }

    public void DisableTab(int tabIndex)
    {
        if (_tabPages.Count <= 0 || _tabButtons.Count <= 0 || tabIndex < 0 || tabIndex > _tabPages.Count -1)
            return;
        _tabPages[tabIndex].SetActive(false);
    }

    public void AddTab<T>(GameObject tabGroup,  string label, object[] args = null) where T : TabPageUI
    {
        var tab = (T) Activator.CreateInstance(typeof(T), args);
        tab.ConstructUI(_uiRoot);
        _tabPages.Add(tab);

        ButtonRef button = UIFactory.CreateButton(tabGroup, $"{label}__Button", label);
        int id = _tabButtons.Count;
            
        button.OnClick += () => { SetTab(id); };
        _tabButtons.Add(button);
        DisableTab(_tabButtons.Count - 1);
    }
}