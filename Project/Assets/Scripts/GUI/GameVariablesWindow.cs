using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameVariablesWindow : Photon.MonoBehaviour {

    private const int GAME_VARIABLES_WINDOW_ID = 1, MAIN_WIDTH = 300, MAIN_HEIGHT = 300;
    private const string GAME_VARIABLES_WINDOW_TEXT = "Game Properties";
    private const float RPC_COOLDOWN_TIME = 2.0f;

    private VariableType editingField;
    private Rect windowRect;
    private bool isVisible;
    private float lastRPCTime, lastToggleTime;

	void Start () {
        editingField = default(VariableType);
        windowRect = new Rect((Screen.width - MAIN_WIDTH / 2) / 2,
                              (Screen.height - MAIN_HEIGHT / 2) / 2,
                              MAIN_WIDTH,
                              MAIN_HEIGHT);
        isVisible = false;
        lastRPCTime = 0f;
        lastToggleTime = 0f;
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
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(200));
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

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Game")) {
            SyncGameVariables();
            //if (AllPlayersReady())
            //    PhotonNetwork.LoadLevel("MainStage"+GameVariables.Instance.Mode.Key);
        }
        GUILayout.EndHorizontal();

        SyncGameVariables();
    }

    private bool AllPlayersReady() {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
            if (!(bool)player.customProperties["IsReady"])
                return false;

        return true;
    }

    private void SyncGameVariables() {
        if (Time.time - lastRPCTime > RPC_COOLDOWN_TIME) {
            photonView.RPC("SetGameVariables", PhotonTargets.Others, GameVariables.Instance.Title, GameVariables.Instance.Mode.Key, GameVariables.Instance.Difficulty.Key,
                            GameVariables.Instance.MaxPlayers.Key, GameVariables.Instance.TargetKills.Key, GameVariables.Instance.Timer.Key);

            PhotonNetwork.room.name = GameVariables.Instance.Title;
            PhotonNetwork.room.maxPlayers = GameVariables.Instance.MaxPlayers.Value;
            PhotonNetwork.room.customProperties["Mode"] = GameVariables.Instance.Mode.Value;
            PhotonNetwork.room.customProperties["Difficulty"] = GameVariables.Instance.Difficulty.Value;
            PhotonNetwork.room.customProperties["Target kills"] = GameVariables.Instance.TargetKills.Value;
            PhotonNetwork.room.customProperties["Timer"] = GameVariables.Instance.Timer.Value;

            lastRPCTime = Time.time;
        }
    }

    [RPC]
    private void SetGameVariables(string _title, string _mode, string _difficulty, string _maxPlayers, string _targetKills, string _timer) {
        GameVariables.Instance.Title = _title;
        GameVariables.Instance.Mode = new KeyValuePair <string, GameMode>(_mode, GameVariables.Instance.AvailableModes[_mode]);
        GameVariables.Instance.Difficulty = new KeyValuePair<string, GameDifficulty>(_difficulty, GameVariables.Instance.AvailableDifficulties[_difficulty]);
        GameVariables.Instance.MaxPlayers = new KeyValuePair<string, int>(_maxPlayers, GameVariables.Instance.AvailableMaxPlayers[_maxPlayers]);
        GameVariables.Instance.TargetKills = new KeyValuePair<string, int>(_targetKills, GameVariables.Instance.AvailableTargetKills[_targetKills]);
        GameVariables.Instance.Timer = new KeyValuePair<string, double>(_timer, GameVariables.Instance.AvailableTimers[_timer]);
    }
}
