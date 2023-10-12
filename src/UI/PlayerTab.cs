using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace Lunistice_DebugConsole.UI;

public class PlayerTab : TabPage, ITabbable
{
    private int _selectedTab = 0;
    private readonly List<TabPage> _tabPages = new();
    private readonly List<ButtonRef> _tabButtons = new();
    
    public PlayerTab(GameObject parent) : base(parent)
    {}

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("PlayerTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        var tabGroup = UIFactory.CreateHorizontalGroup(_uiRoot, "PlayerStatsTabGroup", true, true, true, true);
        UIFactory.SetLayoutElement(tabGroup, minHeight: 25, flexibleHeight: 0);

        AddTab<PlayerStatsTab>(tabGroup, "Hana", new object[]
        {
            _uiRoot,
            Timer.Character.Hana
        });
        AddTab<PlayerStatsTab>(tabGroup, "Toree", new object[]
        {
            _uiRoot,
            Timer.Character.Toree
        });
        AddTab<PlayerStatsTab>(tabGroup, "Toukie", new object[]
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
        
        RuntimeHelper.SetColorBlock(_tabButtons[tabIndex].Component, UniversalUI.EnabledButtonColor, UniversalUI.EnabledButtonColor * 1.2f);

        UIModel content = _tabPages[tabIndex];
        content.SetActive(true);

        _selectedTab = tabIndex;
    }

    public void DisableTab(int tabIndex)
    {
        if (_tabPages.Count <= 0 || _tabButtons.Count <= 0 || tabIndex < 0 || tabIndex > _tabPages.Count -1)
            return;
        _tabPages[tabIndex].SetActive(false);
        RuntimeHelper.SetColorBlock(_tabButtons[tabIndex].Component, UniversalUI.DisabledButtonColor, UniversalUI.DisabledButtonColor * 1.2f);
    }

    public void AddTab<T>(GameObject tabGroup,  string label, object[] args = null) where T : TabPage
    {
        var tab = (T) Activator.CreateInstance(typeof(T), args);
        tab.ConstructUI(_uiRoot);
        _tabPages.Add(tab);

        var button = UIFactory.CreateButton(tabGroup, $"{label}__Button", label);
        var id = _tabButtons.Count;
            
        button.OnClick += () => { SetTab(id); };
        _tabButtons.Add(button);
        DisableTab(_tabButtons.Count - 1);
    }
}