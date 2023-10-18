using Il2CppSystem.Configuration;
using Luna.Config;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace Lunistice_DebugConsole.UI;

public class GameTab : TabPage
{
    public GameTab(GameObject parent) : base(parent)
    {}

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("GameTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true, padBottom: 10);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        CreateInputField(_uiRoot, "TimeScale", "Value", Plugin.TimeScale);
        CreateInputField(_uiRoot, "GravityX", "Value", Plugin.GravityX);
        CreateInputField(_uiRoot, "GravityY", "Value", Plugin.GravityY);
        CreateInputField(_uiRoot, "GravityZ", "Value", Plugin.GravityZ);
    }

    public InputFieldRef CreateInputField<T>(GameObject parent, string name, string placeHolder,
        Wrapper<T> config)
    {
        var group = UIFactory.CreateHorizontalGroup(parent, $"{name}__HorizontalGroup", true, false, true, true, 4);
        
        var displayText = UIFactory.CreateLabel(group, $"{name}__Label", $"{config.Definition.Key}: {config.BoxedValue}");
        var input = UIFactory.CreateInputField(group, $"{name}__InputField", placeHolder);
        var defaultButton = UIFactory.CreateButton(group, $"{name}__Button", "Default");
        UIFactory.SetLayoutElement(defaultButton.GameObject, minHeight: 20);

        input.Text = config.BoxedValue.ToString();
        
        input.Component.GetOnEndEdit().AddListener(value =>
        {
            var configType = config.SettingType;
            if (configType == typeof(int) && int.TryParse(value, out var @int))
                config.BoxedValue = @int;
            else if (configType == typeof(float) && float.TryParse(value, out var @float))
                config.BoxedValue = @float;
            else if (configType == typeof(string))
                config.BoxedValue = value;

            input.Text = config.BoxedValue?.ToString();
            displayText.text = $"{config.Definition.Key}: {config.BoxedValue}";
        });

        defaultButton.OnClick += config.ResetValue;
        
        config.OnSettingChanged += (_, val) =>
        {
            input.Text = val.BoxedValue.ToString();
            displayText.text = $"{config.Definition.Key}: {val.BoxedValue}";
        };
    
        return input;
    }
}