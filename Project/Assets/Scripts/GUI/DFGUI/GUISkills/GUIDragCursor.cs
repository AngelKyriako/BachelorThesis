using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIDragCursor: GUICursor {

    public override void Start() {
        base.Start();
        // We don't want the drag cursor visible unless it is being used
        Sprite.Hide();

        // We don't want the drag cursor to intercept mouse messages
        Sprite.IsInteractive = false;
        Sprite.IsEnabled = false;
    }

    public override void Update() {
        if (Sprite.IsVisible)
            SetPosition(Input.mousePosition);
    }

    /// <summary>
    /// Displays the drag cursor, which will follow the mouse until hidden
    /// </summary>
    /// <param name="sprite">The sprite to display in the drag cursor</param>
    /// <param name="position">The initial position of the drag cursor</param>
    /// <param name="offset">The mouse offset within the dragged object</param>
    public static void Show(dfSprite sprite, Vector2 position, Vector2 offset) {
        CursorOffset = offset;

        SetPosition(position);

        Sprite.Size = sprite.Size;
        Sprite.Atlas = sprite.Atlas;
        Sprite.SpriteName = sprite.SpriteName;
        Sprite.IsVisible = true;
        Sprite.BringToFront();

    }

    public static void Hide() {
        Sprite.IsVisible = false;
    }
}