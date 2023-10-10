using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace Lunistice_DebugConsole.UI;

public class GameTabUI : TabPageUI
{
    public GameTabUI(GameObject parent) : base(parent)
    {
    }

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("GameTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        CreateInputField(_uiRoot, "TimeScale", "Value", Plugin.TimeScale);
        CreateInputField(_uiRoot, "GravityX", "Value", Plugin.GravityX);
        CreateInputField(_uiRoot, "GravityY", "Value", Plugin.GravityY);
        CreateInputField(_uiRoot, "GravityZ", "Value", Plugin.GravityZ);
    }

    public InputFieldRef CreateInputField<T>(GameObject parent, string name, string placeHolder,
        ConfigWrapper<T> config)
    {
        var group = UIFactory.CreateHorizontalGroup(parent, $"{name}__HorizontalGroup", true, false, true, true, 4);
        
        var displayText = UIFactory.CreateLabel(group, $"{name}__Label", $"{config.GetDefinition().Key}: {config.GetBoxedValue()}");
        var input = UIFactory.CreateInputField(group, $"{name}__InputField", placeHolder);
        var defaultButton = UIFactory.CreateButton(group, $"{name}__Button", "Default");
        UIFactory.SetLayoutElement(defaultButton.GameObject, minHeight: 20);
        
        input.Text = config.GetBoxedValue().ToString();
        
        input.Component.GetOnEndEdit().AddListener(value =>
        {
            var configType = config.GetSettingType();
            if (configType == typeof(int) && int.TryParse(value, out var @int))
                config.SetBoxedValue(@int);
            else if (configType == typeof(float) && float.TryParse(value, out var @float))
                config.SetBoxedValue(@float);
            else if (configType == typeof(string))
                config.SetBoxedValue(value);

            input.Text = config.GetBoxedValue().ToString();
            displayText.text = $"{config.GetDefinition().Key}: {config.GetBoxedValue()}";
        });

        defaultButton.OnClick += () =>
        {
            config.SetBoxedValue(config.GetDefaultValue());
            input.Text = config.GetDefaultValue().ToString();
            displayText.text = $"{config.GetDefinition().Key}: {config.GetBoxedValue()}";
        };
    
        return input;
    }
}