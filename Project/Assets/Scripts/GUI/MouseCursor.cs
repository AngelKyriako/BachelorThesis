using UnityEngine;
using System.Collections;

public class MouseCursor: MonoBehaviour {

    public Texture2D defaultCursor,
                     westCursor,
                     eastSideCursor,
                     northSideCursor,
                     southSideCursor;

    private Texture2D cursorIcon;

    void Awake() {
        enabled = false;
    }

    void Start() {
        cursorIcon = defaultCursor;
    }

    void OnGUI() {
        GUI.DrawTexture(GUIUtilities.Instance.ClampToScreen(new Rect(Input.mousePosition.x,
                                                                     Screen.height - Input.mousePosition.y,
                                                                     cursorIcon.width,
                                                                     cursorIcon.height)), cursorIcon);
    }

    public Texture2D CursorIcon {
        get { return cursorIcon; }
        set { cursorIcon = value; }
    }
}
