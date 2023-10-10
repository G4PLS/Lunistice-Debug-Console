using System;
using System.Collections.Generic;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace Lunistice_DebugConsole.UI
{
    public class DebugMenuUI : PanelBase, ITabbable
    {
        public override string Name => "Debug Console";
        public override int MinWidth => 100;
        public override int MinHeight => 50;
        public override bool CanDragAndResize => true;
        public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
        public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);

        private int _selectedTab = 0;
        private readonly List<TabPageUI> _tabPages = new();
        private readonly List<ButtonRef> _tabButtons = new();
        
        public DebugMenuUI(UIBase owner) : base(owner) { }
        protected override void ConstructPanelContent()
        {
            GameObject tabGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "TabGroup", true, true, true, true);
            UIFactory.SetLayoutElement(tabGroup, minHeight: 25, flexibleHeight: 0);

            AddTab<GameTabUI>(tabGroup, "Game Tab", new object[] {ContentRoot});
            AddTab<PlayerTabUI>(tabGroup, "Player Tab", new object[] {ContentRoot});
            
            SetTab(0);
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

        public void AddTab<T>(GameObject tabGroup,  string label, object[] args) where T : TabPageUI
        {
            var tab = (T) Activator.CreateInstance(typeof(T), args);
            tab.ConstructUI(ContentRoot);
            _tabPages.Add(tab);

            ButtonRef button = UIFactory.CreateButton(tabGroup, $"{label}__Button", label);
            int id = _tabButtons.Count;
            
            button.OnClick += () => { SetTab(id); };
            _tabButtons.Add(button);
            DisableTab(_tabButtons.Count - 1);
        }
    }
}
