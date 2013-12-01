using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetupScreen: Photon.MonoBehaviour {

    private const float ROOM_INFO_HEIGHT_PERCENT = 0.3f, READY_BUTTON_HEIGHT_PERCENT = 0.1f;
    private const float RPC_COOLDOWN_TIME = 2.0f;

    private VariableType editingField;
    private string readyText = "Ready";
    private bool isVisible;
    private float lastRPCTime;

    public Texture2D background;
    public int space = 10;

    private Rect fullscreen, roomInfoRect, playerInfoRect, readyButtonRect;

    void Awake() {
        fullscreen = new Rect(0, 0, Screen.width, Screen.height);
        roomInfoRect = new Rect(0, 0, Screen.width, ROOM_INFO_HEIGHT_PERCENT * Screen.height);
        readyButtonRect = new Rect(0, Screen.height - READY_BUTTON_HEIGHT_PERCENT * Screen.height, Screen.width, READY_BUTTON_HEIGHT_PERCENT * Screen.height);
        playerInfoRect = new Rect(0, roomInfoRect.height, Screen.width, Screen.height - roomInfoRect.height - readyButtonRect.height);
        enabled = false;
    }

	void Start () {
        editingField = default(VariableType);
        isVisible = true;
        lastRPCTime = 0f;
	}
	
	void Update () {
        if (Input.GetKeyUp(KeyCode.G))
            isVisible = !isVisible;
	}

    void OnGUI() {
        if (isVisible) {
            if (PhotonNetwork.isMasterClient) {
                EditableRoomInfoGUI();
                PlayerInfoGUI();
                StartGameButton();
            }
            else {
                ViewRoomInfoGUI();
                ReadyToggleButton();
            }
        }
    }

    #region joined player
    private void ViewRoomInfoGUI() {
        GUILayout.BeginArea(roomInfoRect);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Room:" + GameVariables.Instance.Title);
        //GUILayout.Space(space * 5);
        //GUILayout.Label("Mode:" + GameVariables.Instance.Mode.Key);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Players: " + GameVariables.Instance.MaxPlayers.Key);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty: "+GameVariables.Instance.Difficulty.Key);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Countdown timer: " + GameVariables.Instance.Timer.Key);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal))
            GUILayout.Label("Target kills: " + GameVariables.Instance.TargetKills.Key);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void ReadyToggleButton() {
        GUILayout.BeginArea(readyButtonRect);
        GUILayout.BeginHorizontal();
        readyText = GUIUtilities.Instance.ToggleTextButton(readyText, "Ready", "Not ready");
        if (!((bool)PhotonNetwork.player.customProperties["IsReady"]).Equals(readyText.Equals("Ready")))
            PhotonNetwork.player.customProperties["IsReady"] = readyText.Equals("Ready");
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    #endregion

    #region master client
    private void EditableRoomInfoGUI() {
        GUILayout.BeginArea(roomInfoRect);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Room: " + GameVariables.Instance.Title);
        //GUILayout.Label("Mode:");
        //GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingField, VariableType.Mode,
        //                                                       GameVariables.Instance.Mode,
        //                                                       GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:");
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:");
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:");
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, VariableType>(ref editingField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal)) {
            GUILayout.Label("Target kills:");
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void StartGameButton() {
        GUILayout.BeginArea(readyButtonRect);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Game")) {
            if (AllPlayersReady())
                //    PhotonNetwork.LoadLevel("MainStage"+GameVariables.Instance.Mode.Key);
                Utilities.Instance.LogMessage("ALL Ready !!!");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    #endregion

    private void PlayerInfoGUI() {
        GUILayout.BeginArea(playerInfoRect);

        for (int i = 0; i < GameVariables.Instance.MaxPlayers.Value; ++i) {
            GUILayout.BeginHorizontal();
            PlayerSlot();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndArea();
    }

    private void PlayerSlot() {
    }


    private bool AllPlayersReady() {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
            if (!(bool)player.customProperties["IsReady"] && !player.isMasterClient)
                return false;

        return true;
    }


}
