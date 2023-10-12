using System;
using BepInEx.Configuration;

namespace Lunistice_DebugConsole;

public class ConfigWrapper<T>
{
    public delegate void ChangeValue(T val);
    
    private ConfigFile _config;
    public ConfigEntry<T> Entry { get; private set; }
    /// <summary>
    /// Gets called when the Value Changes
    /// </summary>
    public Action<T> OnValueChange;
    /// <summary>
    /// Gets called when the Boxed Value Changes
    /// </summary>
    public Action<object> OnBoxedValueChange;
    
    /// <summary>
    /// Function that gets called when a value gets changed
    /// </summary>
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
        ChangeValueFunction?.Invoke(value);
        OnValueChange?.Invoke(value);
    }
    public T GetValue() => Entry.Value;
    public void SetBoxedValue(object value)
    {
        if (value is not T val) return;
        Entry.BoxedValue = val;
        ChangeValueFunction?.Invoke(val);
        OnBoxedValueChange?.Invoke(val);
    }
    public object GetBoxedValue() => Entry.BoxedValue;
    public ConfigDefinition GetDefinition() => Entry.Definition;
    public Type GetSettingType() => Entry.SettingType;
    public object GetDefaultValue() => Entry.DefaultValue;
    public void ResetValue() => SetBoxedValue(Entry.DefaultValue);
}