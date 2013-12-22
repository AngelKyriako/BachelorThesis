using UnityEngine;
using System.Collections;

public class GUICursor: MonoBehaviour {//make this work

    private static dfSprite _sprite;
    private static Vector2 _cursorOffset;

    public virtual void Start() {
        // Obtain a reference to the Sprite that this component is attached to
        _sprite = GetComponent<dfSprite>();
    }

    public virtual void Update() {
        SetPosition(PlayerInputManager.Instance.MousePosition);
    }

    public static void SetPosition(Vector2 position) {
        // Convert position from "screen coordinates" to "gui coordinates"
        position = _sprite.GetManager().ScreenToGui(position);
        // Center the control on the mouse/touch
        _sprite.RelativePosition = position - _cursorOffset;
    }

    public static dfSprite Sprite {
        get { return GUICursor._sprite; }
        set { GUICursor._sprite = value; }
    }
    public static Vector2 CursorOffset {
        get { return GUICursor._cursorOffset; }
        set { GUICursor._cursorOffset = value; }
    }
}
