using UnityEngine;
using System.Collections;

public class GUIShowSkillWindow: MonoBehaviour {

    private bool busy = false;
    private bool isVisible = false;
    private dfControl skillBookWindow;
    void OnEnable() {
        skillBookWindow = GameObject.Find("SkillBookWindow").GetComponent<dfControl>();
        skillBookWindow.IsVisible = false;
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.K)) {
            if (!isVisible)
                StartCoroutine(showWindow(skillBookWindow));
            else
                StartCoroutine(hideWindow(skillBookWindow));
        }
    }

    void OnClick() {

        if (busy)
            return;

        StopAllCoroutines();
        if (!isVisible)
            StartCoroutine(showWindow(skillBookWindow));
        else
            StartCoroutine(hideWindow(skillBookWindow));

    }

    IEnumerator hideWindow(dfControl window) {
        busy = true;
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
        isVisible = true;
    }

}
