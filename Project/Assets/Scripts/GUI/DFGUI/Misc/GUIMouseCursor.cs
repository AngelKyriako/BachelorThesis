using UnityEngine;
using System.Collections;

public class GUIMouseCursor: SingletonMono<GUIMouseCursor> {

    public string defaultCursor,
                  horizontal,
                  vertical,
                  bottomLeftTopRight,
                  bottomRightTopLeft;

    private dfSprite sprite;

    private GUIMouseCursor() { }

    void Awake() {
        sprite = GetComponent<dfSprite>();
    }

    void Start() {
        // We don't want the drag cursor to intercept mouse messages
        sprite.IsInteractive = false;
        sprite.IsEnabled = false;

        Screen.showCursor = false;
        SetCursor(defaultCursor);
    }

    //@TODO: Set mouse cursor when at edges of screen or targeting

    void Update() {
        sprite.RelativePosition = sprite.GetManager().ScreenToGui(Input.mousePosition);
        sprite.BringToFront();
    }

    public void SetCursor(string _sprite) {
        sprite.SpriteName = _sprite;
    }
}
