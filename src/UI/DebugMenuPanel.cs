using System;
using System.Collections.Generic;
using Luna;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace Lunistice_DebugConsole.UI
{
    public class DebugMenuUI : PanelBase, ITabbable
    {
        public override string Name => $"{Plugin.Name} by {Plugin.Author}";
        public override int MinWidth => 100;
        public override int MinHeight => 50;
        public override bool CanDragAndResize => true;
        public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
        public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);

        private int _selectedTab = 0;
        private readonly List<TabPage> _tabPages = new();
        private readonly List<ButtonRef> _tabButtons = new();

        public DebugMenuUI(UIBase owner) : base(owner)
        {
            Player.OnPlayerLoaded += OnPlayerLoad;
        }
        protected override void ConstructPanelContent()
        {
            GameObject tabGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "TabGroup", true, true, true, true);
            UIFactory.SetLayoutElement(tabGroup, minHeight: 25, flexibleHeight: 0);

            AddTab<GameTab>(tabGroup, "Game", new object[] {ContentRoot});
            AddTab<LevelTab>(tabGroup, "Level", new object[] {ContentRoot});
            AddTab<PlayerTab>(tabGroup, "Player", new object[] {ContentRoot});
            AddTab<SettingsTab>(tabGroup, "Settings", new object[] {ContentRoot});
            
            SetTab(0);
            if (Plugin.ShowOnStart.GetValue()) Show();
            else Hide();
        }

        protected override void OnClosePanelClicked() => Hide();

        public void Show()
        {
            SetActive(true);
            ConfigManager.Force_Unlock_Mouse = true;
            Game.Pause(true);
        }

        public void Hide()
        {
            SetActive(false);
            ConfigManager.Force_Unlock_Mouse = false;
            Game.Pause(false);
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

        public void AddTab<T>(GameObject tabGroup,  string label, object[] args) where T : TabPage
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

        private static void OnPlayerLoad(Timer.Character character)
        {
            Player.Gravity = new Vector3(Plugin.GravityX.GetValue(), Plugin.GravityY.GetValue(),
                Plugin.GravityZ.GetValue());
            Player.GravityReset = Player.Gravity;
            Time.timeScale = Plugin.TimeScale.GetValue();
            switch (character)
            {
                case Timer.Character.Hana: 
                    Player.MaxLife          = Plugin.HanaMaxLife.GetValue();
                    Player.SprintSpeed      = Plugin.HanaSprintSpeed.GetValue(); 
                    Player.RunSpeed         = Plugin.HanaRunSpeed.GetValue();
                    Player.TurboSpeed       = Plugin.HanaTurboSpeed.GetValue();
                    Player.JumpHeight       = Plugin.HanaJumpHeight.GetValue();
                    Player.AttackJumpHeight = Plugin.HanaAttackJump.GetValue();
                    Player.MaxDoubleJumps   = Plugin.HanaMaxDoubleJumps.GetValue();
                    Player.CoyoteTime       = Plugin.HanaCoyoteTime.GetValue();
                    Player.Friction         = Plugin.HanaFriction.GetValue();
                    Player.AirFriction      = Plugin.HanaAirFriction.GetValue();
                    Player.Acceleration     = Plugin.HanaAcceleration.GetValue();
                    break;
                case Timer.Character.Toree:
                    Player.MaxLife          = Plugin.ToreeMaxLife.GetValue();
                    Player.SprintSpeed      = Plugin.ToreeSprintSpeed.GetValue(); 
                    Player.RunSpeed         = Plugin.ToreeRunSpeed.GetValue();
                    Player.TurboSpeed       = Plugin.ToreeTurboSpeed.GetValue();
                    Player.JumpHeight       = Plugin.ToreeJumpHeight.GetValue();
                    Player.AttackJumpHeight = Plugin.ToreeAttackJump.GetValue();
                    Player.MaxDoubleJumps   = Plugin.ToreeMaxDoubleJumps.GetValue();
                    Player.CoyoteTime       = Plugin.ToreeCoyoteTime.GetValue();
                    Player.Friction         = Plugin.ToreeFriction.GetValue();
                    Player.AirFriction      = Plugin.ToreeAirFriction.GetValue();
                    Player.Acceleration     = Plugin.ToreeAcceleration.GetValue();
                    break;
                case Timer.Character.Toukie:
                    Player.MaxLife          = Plugin.ToukieMaxLife.GetValue();
                    Player.SprintSpeed      = Plugin.ToukieSprintSpeed.GetValue(); 
                    Player.RunSpeed         = Plugin.ToukieRunSpeed.GetValue();
                    Player.TurboSpeed       = Plugin.ToukieTurboSpeed.GetValue();
                    Player.JumpHeight       = Plugin.ToukieJumpHeight.GetValue();
                    Player.AttackJumpHeight = Plugin.ToukieAttackJump.GetValue();
                    Player.MaxDoubleJumps   = Plugin.ToukieMaxDoubleJumps.GetValue();
                    Player.CoyoteTime       = Plugin.ToukieCoyoteTime.GetValue();
                    Player.Friction         = Plugin.ToukieFriction.GetValue();
                    Player.AirFriction      = Plugin.ToukieAirFriction.GetValue();
                    Player.Acceleration     = Plugin.ToukieAcceleration.GetValue();
                    break;
            }
        }
    }
}