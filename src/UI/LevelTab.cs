
using System;
using Luna;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace Lunistice_DebugConsole.UI;

public class LevelTab : TabPage
{
    public LevelTab(GameObject parent) : base(parent)
    {}

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("LevelTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true, 4, padBottom: 20);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        var quickLevel = UIFactory.CreateHorizontalGroup(_uiRoot, "QuickLevel", true, true, true, true);
        UIFactory.SetLayoutElement(quickLevel, minHeight: 25, flexibleHeight: 0);
        
        var btnMain = UIFactory.CreateButton(quickLevel, "MainMenuBtn", "Main Menu", normalColor: null);
        var btnRestart = UIFactory.CreateButton(quickLevel, "RestartLevel", "Restart Level", normalColor: null);
        var btnNext = UIFactory.CreateButton(quickLevel, "NextLevel", "Next Level", normalColor: null);
        var btnPrevious = UIFactory.CreateButton(quickLevel, "PreviousLevel", "Previous Level", normalColor: null);

        btnMain.OnClick = () => Game.CloseResults(false);
        btnRestart.OnClick = () => LevelLoader.LoadLevel(LevelLoader.GetCurrentLevel());
        btnNext.OnClick = LevelLoader.LoadNextLevel;
        btnPrevious.OnClick = LevelLoader.LoadPreviousLevel;
        
        var quickTimer = UIFactory.CreateHorizontalGroup(_uiRoot, "QuickTimer", true, true, true, true);
        UIFactory.SetLayoutElement(quickTimer, minHeight: 25, flexibleHeight: 0);

        var btnStart = UIFactory.CreateButton(quickTimer, "StartTimer", "Start Timer", normalColor: null);
        var btnStop = UIFactory.CreateButton(quickTimer, "StopTimer", "Stop Timer", normalColor: null);

        btnStart.OnClick += LevelTimer.StartTimer;
        btnStop.OnClick += LevelTimer.StopTimer;
        
        //Level list
        var verticalLevelGroup = UIFactory.CreateVerticalGroup(_uiRoot, "LevelListVertical", true, false, true, true);
        UIFactory.SetLayoutElement(verticalLevelGroup, minHeight: 25);
        
        var enumValues = Enum.GetValues(typeof(Level));
        var rows = 3;
        var max = enumValues.Length / rows;

        for (var i = 0; i < max; i++)
        {
            var temporaryHorizontalGroup = UIFactory.CreateHorizontalGroup(verticalLevelGroup, $"{max}__LevelList", true, true, true, true);
            UIFactory.SetLayoutElement(temporaryHorizontalGroup, minHeight: 25, flexibleHeight: 0);

            for (var j = 0; j < rows; j++)
            {
                var index = i * rows + j;
                if (index >= enumValues.Length) continue;
                
                var btnLevel = UIFactory.CreateButton(temporaryHorizontalGroup, $"{max}{i}{j}{index}__Button",
                    enumValues.GetValue(index)?.ToString(), normalColor: null);
                btnLevel.OnClick += () =>
                {
                    LevelLoader.LoadLevel((Level)enumValues.GetValue(index)!);
                    UiManager.RefreshMenus(UiManager.UIState.Mission);
                };
            }
        }
    }
}