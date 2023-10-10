using System.Collections.Generic;
using BepInEx.Configuration;
using Project_Luna;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace Lunistice_DebugConsole.UI;

public class PlayerStatsUI : TabPageUI
{
    private Timer.Character _character;
    
    public PlayerStatsUI(GameObject parent, Timer.Character character) : base(parent)
    {
        _character = character;
    }

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("PlayerStats", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);
        
        switch (_character)
        {
            case Timer.Character.Hana:
                CreateInputField(_uiRoot, "HanaMaxLife", "Input Value", Plugin.HanaMaxLife);
                CreateInputField(_uiRoot, "HanaSprintSpeed", "Input Value", Plugin.HanaSprintSpeed);
                CreateInputField(_uiRoot, "HanaRunSpeed", "Input Value", Plugin.HanaRunSpeed);
                CreateInputField(_uiRoot, "HanaTurboSpeed", "Input Value", Plugin.HanaTurboSpeed);
                CreateInputField(_uiRoot, "HanaAttackJump", "Input Value", Plugin.HanaAttackJump);
                CreateInputField(_uiRoot, "HanaMaxDoubleJumps", "Input Value", Plugin.HanaMaxDoubleJumps);
                CreateInputField(_uiRoot, "HanaCoyoteTime", "Input Value", Plugin.HanaCoyoteTime);
                CreateInputField(_uiRoot, "HanaFriction", "Input Value", Plugin.HanaFriction);
                CreateInputField(_uiRoot, "HanaAirFriction", "Input Value", Plugin.HanaAirFriction);
                CreateInputField(_uiRoot, "HanaAcceleration", "Input Value", Plugin.HanaAcceleration);
                break;
            case Timer.Character.Toree:
                CreateInputField(_uiRoot, "ToreeMaxLife", "Input Value", Plugin.ToreeMaxLife);
                CreateInputField(_uiRoot, "ToreeSprintSpeed", "Input Value", Plugin.ToreeSprintSpeed);
                CreateInputField(_uiRoot, "ToreeRunSpeed", "Input Value", Plugin.ToreeRunSpeed);
                CreateInputField(_uiRoot, "ToreeTurboSpeed", "Input Value", Plugin.ToreeTurboSpeed);
                CreateInputField(_uiRoot, "ToreeAttackJump", "Input Value", Plugin.ToreeAttackJump);
                CreateInputField(_uiRoot, "ToreeMaxDoubleJumps", "Input Value", Plugin.ToreeMaxDoubleJumps);
                CreateInputField(_uiRoot, "ToreeCoyoteTime", "Input Value", Plugin.ToreeCoyoteTime);
                CreateInputField(_uiRoot, "ToreeFriction", "Input Value", Plugin.ToreeFriction);
                CreateInputField(_uiRoot, "ToreeAirFriction", "Input Value", Plugin.ToreeAirFriction);
                CreateInputField(_uiRoot, "ToreeAcceleration", "Input Value", Plugin.ToreeAcceleration);
                break;
            case Timer.Character.Toukie:
                CreateInputField(_uiRoot, "ToukieMaxLife", "Input Value", Plugin.ToukieMaxLife);
                CreateInputField(_uiRoot, "ToukieSprintSpeed", "Input Value", Plugin.ToukieSprintSpeed);
                CreateInputField(_uiRoot, "ToukieRunSpeed", "Input Value", Plugin.ToukieRunSpeed);
                CreateInputField(_uiRoot, "ToukieTurboSpeed", "Input Value", Plugin.ToukieTurboSpeed);
                CreateInputField(_uiRoot, "ToukieAttackJump", "Input Value", Plugin.ToukieAttackJump);
                CreateInputField(_uiRoot, "ToukieMaxDoubleJumps", "Input Value", Plugin.ToukieMaxDoubleJumps);
                CreateInputField(_uiRoot, "ToukieCoyoteTime", "Input Value", Plugin.ToukieCoyoteTime);
                CreateInputField(_uiRoot, "ToukieFriction", "Input Value", Plugin.ToukieFriction);
                CreateInputField(_uiRoot, "ToukieAirFriction", "Input Value", Plugin.ToukieAirFriction);
                CreateInputField(_uiRoot, "ToukieAcceleration", "Input Value", Plugin.ToukieAcceleration);
                break;
        }
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