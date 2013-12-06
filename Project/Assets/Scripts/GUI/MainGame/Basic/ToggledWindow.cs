using UnityEngine;
using System.Collections;

public abstract class ToggledWindow: BaseWindow {

    private bool isVisible;
    private KeyCode toggleButton;

    public void SetUpGUI(string _winTitle, int _winID, int _width, int _height, bool _isVisible, KeyCode _toggleButton) {
        base.SetUpGUI(_winTitle, _winID, _width, _height);
        isVisible = _isVisible;
        toggleButton = _toggleButton;
    }

    public void SetUpGUI(string _winTitle, int _winID, int _x, int _y, int _width, int _height, bool _isVisible, KeyCode _toggleButton) {
        base.SetUpGUI(_winTitle, _winID, _x, _y, _width, _height);
        isVisible = _isVisible;
        toggleButton = _toggleButton;
    }

    void Update() {
        if (Input.GetKeyUp(toggleButton))
            isVisible = !isVisible;
    }

    public override void OnGUI() {
        if (isVisible)
            base.OnGUI();
    }
}