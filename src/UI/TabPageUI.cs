using UnityEngine;
using UniverseLib.UI.Models;

namespace Lunistice_DebugConsole.UI;

public abstract class TabPageUI : UIModel
{
    public GameObject Parent { get; }
    public override GameObject UIRoot => _uiRoot;
    protected GameObject _uiRoot;

    public TabPageUI(GameObject parent)
    {
        Parent = parent;
    }

    public abstract override void ConstructUI(GameObject parent);
}