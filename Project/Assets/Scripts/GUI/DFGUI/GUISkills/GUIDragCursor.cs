using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIDragCursor: MonoBehaviour {

    private static dfSprite sprite;
    private static Vector2 cursorOffset;

    void Start() {
        // Obtain a reference to the Sprite that this component is attached to
        sprite = GetComponent<dfSprite>();
        // We don't want the drag cursor to intercept mouse messages
        sprite.IsInteractive = false;
        sprite.IsEnabled = false;
        // We don't want the drag cursor visible unless it is being used
        sprite.Hide();
    }

    void Update() {
        if (sprite.IsVisible)
            SetPosition(Input.mousePosition);
    }

    /// <summary>
    /// Displays the drag cursor, which will follow the mouse until hidden
    /// </summary>
    /// <param name="sprite">The sprite to display in the drag cursor</param>
    /// <param name="position">The initial position of the drag cursor</param>
    /// <param name="offset">The mouse offset within the dragged object</param>
    public static void Show(dfSprite _sprite, Vector2 _position, Vector2 _offset) {
        cursorOffset = _offset;

        SetPosition(_position);

        sprite.Size = _sprite.Size;
        sprite.Atlas = _sprite.Atlas;
        sprite.SpriteName = _sprite.SpriteName;
        sprite.IsVisible = true;
        sprite.BringToFront();

    }

    public static void Hide() {
        sprite.IsVisible = false;
    }

    private static void SetPosition(Vector2 position) {
        // Convert position from "screen coordinates" to "gui coordinates"
        position = sprite.GetManager().ScreenToGui(position);
        // Center the control on the mouse/touch
        sprite.RelativePosition = position - cursorOffset;
    }
}