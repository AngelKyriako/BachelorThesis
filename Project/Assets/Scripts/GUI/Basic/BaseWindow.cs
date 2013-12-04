using UnityEngine;
using System.Collections;

public abstract class BaseWindow: MonoBehaviour {

    private Rect windowRect;    
    private string windowTitle;
    private int windowId;

    void Awake() {
        enabled = false;
    }

    public void SetUpGUI(string _winTitle, int _winID, int _width, int _height) {
        windowTitle = _winTitle;
        windowId = _winID;
        windowRect = new Rect((Screen.width - _width / 2) / 2,
                              (Screen.height - _height / 2) / 2,
                              _width,
                              _height);
        enabled = true;
    }

    public void SetUpGUI(string _winTitle, int _winID, int _x, int _y, int _width, int _height) {
        windowTitle = _winTitle;
        windowId = _winID;
        windowRect = new Rect(_x,
                              _y,
                              _width,
                              _height);
        enabled = true;
    }

    public virtual void OnGUI() {
        windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(windowId, windowRect, MainWindow, windowTitle));
    }

    public abstract void MainWindow(int windowID);
}
