using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;

namespace Lunistice_DebugConsole.UI;

public class SettingsTab : TabPage
{
    public SettingsTab(GameObject parent) : base(parent)
    {
    }

    public override void ConstructUI(GameObject parent)
    {
        _uiRoot = UIFactory.CreateUIObject("SettingsTab", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(_uiRoot, true, false, true, true);
        UIFactory.SetLayoutElement(_uiRoot, minHeight:25);

        UIFactory.CreateToggle(_uiRoot, "ShowOnStart__Toggle", out var toggle, out var text);

        toggle.isOn = Plugin.ShowOnStart.Value;
        
        toggle.onValueChanged.AddListener(var =>
        {
            Plugin.ShowOnStart.Value = var;
        });

        text.text = "Show on Start";
    }
}