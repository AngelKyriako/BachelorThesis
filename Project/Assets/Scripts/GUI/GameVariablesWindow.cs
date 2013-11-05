using UnityEngine;
using System.Collections;

public class GameVariablesWindow : Photon.MonoBehaviour {

    private const int GAME_VARIABLES_WINDOW_ID = 1, MAIN_WIDTH = 300, MAIN_HEIGHT = 300;
    private const string GAME_VARIABLES_WINDOW_TEXT = "Game Properties";

    private VariableType editingField;
    private Rect windowRect;
    private bool isVisible;

	void Start () {
        editingField = default(VariableType);
        windowRect = new Rect((Screen.width - MAIN_WIDTH / 2) / 2,
                              (Screen.height - MAIN_HEIGHT / 2) / 2,
                              MAIN_WIDTH,
                              MAIN_HEIGHT);
        isVisible = true;
	}

    void OnGUI() {
        if (isVisible)
            windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(GAME_VARIABLES_WINDOW_ID, windowRect, MainWindow, GAME_VARIABLES_WINDOW_TEXT));
    }

    private void MainWindow(int windowID) {
        if (PhotonNetwork.player.isMasterClient)
            GameVariablesEditableGUI();
        else
            GameVariablesViewGUI();
        GUI.DragWindow();
    }

    private void GameVariablesViewGUI() {
        GUILayout.Label("Title: " + GameVariables.Instance.Title, GUILayout.MaxWidth(300));
        GUILayout.Label("Mode: " + GameVariables.Instance.Mode.Key);
        if (GameVariables.Instance.Mode.Value == GameMode.BattleRoyal)
            GUILayout.Label("Target kills: "+GameVariables.Instance.TargetKills.Key);
        GUILayout.Label("Max players: "+GameVariables.Instance.MaxPlayers.Key);
        GUILayout.Label("Difficulty: "+GameVariables.Instance.Difficulty.Key);
        GUILayout.Label("Timer: " + GameVariables.Instance.Timer.Key);
    }

    private void GameVariablesEditableGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:");
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(250));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:");
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingField, VariableType.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndHorizontal();
        if (GameVariables.Instance.Mode.Value == GameMode.BattleRoyal) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max kills:");
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:");
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:");
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:");
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, VariableType>(ref editingField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndHorizontal();
    }
}
