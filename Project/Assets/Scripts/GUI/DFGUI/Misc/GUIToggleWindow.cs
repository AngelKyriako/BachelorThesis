using UnityEngine;
using System.Collections;

public class GUIToggleWindow: MonoBehaviour {

    public dfControl controlWindow;
    public bool isToggleWindow = true;
    public string iconForMaximizing = "vscroll-down-hover", iconForMinimizing = "vscroll-up-hover";

    void OnClick(){
        controlWindow.IsVisible = !controlWindow.IsVisible;
        if (isToggleWindow)
            gameObject.GetComponent<dfSprite>().SpriteName = controlWindow.IsVisible?iconForMinimizing:iconForMaximizing;
    }
}
