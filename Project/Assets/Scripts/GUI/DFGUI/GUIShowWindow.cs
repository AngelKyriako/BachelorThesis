using UnityEngine;
using System.Collections;

public class GUIShowWindow: MonoBehaviour {

    public string windowName = string.Empty;
    public KeyCode key;
    public bool isCharacterWindow = false;

    private bool busy = false;
    private bool isVisible = false;
    private dfControl window;

    void OnEnable() {
        Utilities.Instance.PreCondition(!windowName.Equals(string.Empty), "GUIShowWindow", "OnEnable", "window is not defined");
        Utilities.Instance.PreCondition(key!=null, "GUIShowWindow", "OnEnable", "window is not defined");

        window = GameObject.Find(windowName).GetComponent<dfControl>();
        window.IsVisible = false;

        Utilities.Instance.PostCondition(window != null, "GUIShowWindow", "OnEnable", "window object not found");
    }

    void Update() {
        if (Input.GetKeyUp(key) && !busy) {
            ToggleWindow();
        }
    }

    void OnClick() {
        if (!busy)
            ToggleWindow();
    }

    private void ToggleWindow() {
        StopAllCoroutines();
        if (!isVisible)
            StartCoroutine(showWindow(window));
        else
            StartCoroutine(hideWindow(window));
    }

    IEnumerator hideWindow(dfControl window) {
        busy = true;
        if (isCharacterWindow)
            GUIModelController.Instance.Hide();

        isVisible = false;

        window.IsVisible = true;
        window.GetManager().BringToFront(window);

        var opacity = new dfAnimatedFloat(1f, 0f, 0.33f);
        while (opacity > 0.05f) {
            window.Opacity = opacity;
            yield return null;
        }

        window.Opacity = 0f;

        busy = false;
    }

    IEnumerator showWindow(dfControl window) {
        isVisible = true;
        busy = true;

        window.IsVisible = true;
        window.GetManager().BringToFront(window);

        var opacity = new dfAnimatedFloat(0f, 1f, 0.33f);
        while (opacity < 0.95f) {
            window.Opacity = opacity;
            yield return null;
        }

        window.Opacity = 1f;

        busy = false;
        if (isCharacterWindow)
            GUIModelController.Instance.Show();
        isVisible = true;
    }
}
