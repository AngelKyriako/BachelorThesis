using UnityEngine;
using System.Collections;

public class CharacterWindow: MonoBehaviour {

    private const int STATS_WINDOW_ID = 0;
    private const string STATS_WINDOW_TEXT = "Character Stats";
    private static readonly Rect windowRect = new Rect(Screen.width / 2, Screen.height / 2, 200, 300);

    private PlayerCharacter character;

    private bool isVisible;

    private void Awake() {
        character = (PlayerCharacter)gameObject.AddComponent("PlayerCharacter");
        character.Awake();

        isVisible = false;
    }

    private void OnGui(){
        GUI.Window(STATS_WINDOW_ID, windowRect, StatsWindow, STATS_WINDOW_TEXT);
    }

    private void StatsWindow(int id) {
        GUI.DragWindow();
    }
}