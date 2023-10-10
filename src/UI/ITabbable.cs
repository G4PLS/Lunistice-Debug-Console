using System;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace Lunistice_DebugConsole.UI;

public interface ITabbable
{
    public abstract void SetTab(int tabIndex);

    protected abstract void DisableTab(int tabIndex);

    protected abstract void AddTab<T>(GameObject tabGroup, string label, object[] args) where T : TabPageUI;
}