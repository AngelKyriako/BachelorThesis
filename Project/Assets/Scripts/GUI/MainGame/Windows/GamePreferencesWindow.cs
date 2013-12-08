using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePreferencesWindow: Photon.MonoBehaviour {

    private const int GAME_VARIABLES_WINDOW_ID = 1, MAIN_WIDTH = 300, MAIN_HEIGHT = 300;
    private const string GAME_VARIABLES_WINDOW_TEXT = "Room info";

    private Rect windowRect;
    private bool isVisible;

    void Awake() {
        enabled = false;
    }

	void Start () {
        windowRect = new Rect((Screen.width - MAIN_WIDTH / 2) / 2,
                              (Screen.height - MAIN_HEIGHT / 2) / 2,
                              MAIN_WIDTH,
                              MAIN_HEIGHT);
        isVisible = false;
	}

    void Update() {
        if (Input.GetKeyUp(KeyCode.G))
            isVisible = !isVisible;
    }

    void OnGUI() {
        if (isVisible)
            windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(GAME_VARIABLES_WINDOW_ID, windowRect, MainWindow, GAME_VARIABLES_WINDOW_TEXT));
    }

    private void MainWindow(int windowID) {
        GameVariablesViewGUI();
        GUI.DragWindow();
    }

    private void GameVariablesViewGUI() {
        GUILayout.Label("Room: " + GameVariables.Instance.Title, GUILayout.MaxWidth(300));
        GUILayout.Label("Mode: " + GameVariables.Instance.Mode.Key);
        GUILayout.Label("Map: " + GameVariables.Instance.Map.Key);
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal))
            GUILayout.Label("Target kills: "+GameVariables.Instance.TargetKills.Key);
        GUILayout.Label("Max players: "+GameVariables.Instance.MaxPlayers.Key);
        GUILayout.Label("Difficulty: "+GameVariables.Instance.Difficulty.Key);
        GUILayout.Label("Timer: " + GameVariables.Instance.Timer.Key);
    }
}
