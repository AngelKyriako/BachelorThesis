using UnityEngine;
using System.Collections;

public class GUIToggleWindow: MonoBehaviour {

    public dfControl controlWindow;
    public bool isToggleWindow = true;
    public string iconForMaximizing = "vscroll-down-hover", iconForMinimizing = "vscroll-up-hover";

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<dfSprite>(), "GUIToggleWindow", "Awake", "dfSprite must be component of the gameObject.");
    }

    void OnClick(){
        controlWindow.IsVisible = !controlWindow.IsVisible;
        if (isToggleWindow)
            gameObject.GetComponent<dfSprite>().SpriteName = controlWindow.IsVisible ? iconForMinimizing : iconForMaximizing;
    }
}