using System;
using BepInEx.Configuration;

namespace Lunistice_DebugConsole;

public class ConfigWrapper<T>
{
    public delegate void ChangeValue(T val);
    
    private ConfigFile _config;
    public ConfigEntry<T> Entry { get; private set; }
    public Action<T> OnValueChange;
    public Action<object> OnBoxedValueChange;
    public ChangeValue ChangeValueFunction;

    public ConfigWrapper(ConfigFile config, string section, string key, T defaultValue, string description, ChangeValue lunaConnectionFunction)
    {
        _config = config;
        Entry = _config.Bind(section, key, defaultValue, description);
        ChangeValueFunction = lunaConnectionFunction;
    }
    public void SetValue(T value)
    {
        Entry.Value = value;
        OnValueChange?.Invoke(value);
        ChangeValueFunction(Entry.Value);
    }
    public T GetValue() => Entry.Value;
    public void SetBoxedValue(object value)
    {
        if (value is not T val) return;
        Entry.BoxedValue = val;
        ChangeValueFunction(val);
        OnBoxedValueChange?.Invoke(val);
    }
    public object GetBoxedValue() => Entry.BoxedValue;
    public ConfigDefinition GetDefinition() => Entry.Definition;
    public Type GetSettingType() => Entry.SettingType;
    public object GetDefaultValue() => Entry.DefaultValue;
}