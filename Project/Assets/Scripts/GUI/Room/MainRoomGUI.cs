using UnityEngine;
using System.Collections.Generic;

public class MainRoomGUI: MonoBehaviour {

    private const int SPACE = 12, PLAYER_SLOT_HEIGHT = 50,
                      SOUTH_CHAT_HEIGHT = 100, SOUTH_BUTTONS_WIDTH = 250, SOUTH_BUTTONS_HEIGHT = 50,
                      PREFERENCES_WIDTH = 250;

    public GUIStyle style;
    public Texture2D background;

    private Rect westPreferencesRect,
                 eastSlotsRect, playerSlotRect,
                 southChatRect, southButtonRect;
    
    private VariableType editingPreferencesField;
    private bool teamSelecting, allPlayersReady;
    private float lastPlayersReadyUpdate;
    private RoomNetController networkController;

	void Awake () {
        enabled = false;
        networkController = GetComponent<RoomNetController>();
	}

    void Start() {
        #region Init Rects
        westPreferencesRect = new Rect(0, 0, PREFERENCES_WIDTH, Screen.height);

        eastSlotsRect = new Rect(westPreferencesRect.width + 10, 0, Screen.width - westPreferencesRect.width, Screen.height);
        playerSlotRect = new Rect(0, 0, eastSlotsRect.width/2, eastSlotsRect.height);

        southChatRect = new Rect(0, Screen.height - SOUTH_CHAT_HEIGHT, Screen.width - SOUTH_BUTTONS_WIDTH, SOUTH_CHAT_HEIGHT);
        southButtonRect = new Rect(Screen.width - SOUTH_BUTTONS_WIDTH, Screen.height - SOUTH_BUTTONS_HEIGHT, SOUTH_BUTTONS_WIDTH, SOUTH_BUTTONS_HEIGHT);
        #endregion

        editingPreferencesField = default(VariableType);
        teamSelecting = false;
        allPlayersReady = true;
        lastPlayersReadyUpdate = Time.time;
    }

    void OnGUI() {
        GUI.DrawTexture(GUIUtilities.Instance.FullScreenRect, background, ScaleMode.StretchToFill);
        if (!networkController.IsMasterClient)
            WestGamePreferencesViewed();
        else
            WestGamePreferencesEditable();
        EastPlayerSlots();
        SouthChat();
        SouthButtons();
    }

    void Update() {
        if (networkController.IsMasterClient && Time.time - lastPlayersReadyUpdate > 5) {
            allPlayersReady = GameManager.Instance.AllPlayersReady();
            lastPlayersReadyUpdate = Time.time;
        }
    }

    #region Preferences
    private void WestGamePreferencesEditable() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Room: ", style);
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(200));
        GUILayout.EndHorizontal();
        
        GUILayout.Space(SPACE);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:", style);
        GUILayout.BeginVertical();
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingPreferencesField, VariableType.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(SPACE);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map:", style);
        GUILayout.BeginVertical();
        GameVariables.Instance.Map = GUIUtilities.Instance.ButtonOptions<GameMap, VariableType>(ref editingPreferencesField, VariableType.Map,
                                                               GameVariables.Instance.Map,
                                                               GameVariables.Instance.AvailableMaps, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(SPACE);

        GUILayout.BeginHorizontal();
        if (GameVariables.Instance.Mode.Value == GameMode.Conquerors) {
            GUILayout.Label("Target kills:", style);
            GUILayout.BeginVertical();
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.TargetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndVertical();
        }
        else if (GameVariables.Instance.Mode.Value == GameMode.BattleRoyal) {
            GUILayout.Label("Starting Lifes:", style);
            GUILayout.BeginVertical();
            GameVariables.Instance.StartingLifes = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.StartingLifes,
                                                              GameVariables.Instance.StartingLifes,
                                                              GameVariables.Instance.AvailableStartingLifes, 40);
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(SPACE);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:", style);
        GUILayout.BeginVertical();
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(SPACE);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:", style);
        GUILayout.BeginVertical();
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingPreferencesField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(SPACE);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Start Timer:", style);
        GUILayout.BeginVertical();
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<float, VariableType>(ref editingPreferencesField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void WestGamePreferencesViewed() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.Label("Room: " + GameVariables.Instance.Title, style, GUILayout.MaxWidth(300));
        GUILayout.Space(SPACE);
        GUILayout.Label("Mode: " + GameVariables.Instance.Mode.Key, style);
        GUILayout.Space(SPACE);
        GUILayout.Label("Map: " + GameVariables.Instance.Map.Key, style);
        GUILayout.Space(SPACE);
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.Conquerors))
            GUILayout.Label("Target kills: " + GameVariables.Instance.TargetKills.Key, style);            
        else if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal))
            GUILayout.Label("Starting Lifes: " + GameVariables.Instance.StartingLifes.Key, style);
        GUILayout.Space(SPACE);
        GUILayout.Label("Max players: " + GameVariables.Instance.MaxPlayers.Key, style);
        GUILayout.Space(SPACE);
        GUILayout.Label("Difficulty: " + GameVariables.Instance.Difficulty.Key, style);
        GUILayout.Space(SPACE);
        GUILayout.Label("Timer: " + GameVariables.Instance.Timer.Key, style);
        GUILayout.EndArea();
    }
    #endregion

    #region Player slots
    private void EastPlayerSlots() {
        GUILayout.BeginArea(eastSlotsRect);
        for (int i = 0; i < MainRoomModel.Instance.PlayerSlotsLength; ++i) {
            PlayerSlot(i);
            PlayerSlot(i);
        }
        GUILayout.EndArea();
    }

    private void PlayerSlot(int _slotNum) {
        if (_slotNum % 2 == 0) {
            playerSlotRect.x = 0;
            playerSlotRect.y = (_slotNum / 2) * PLAYER_SLOT_HEIGHT;
        }
        else {
            playerSlotRect.x = playerSlotRect.width;
        }

        GUILayout.BeginArea(playerSlotRect);
        GUILayout.BeginHorizontal();
        if (networkController.IsMasterClient)
            KickButton(_slotNum);
        MainSlot(_slotNum);
        TeamButton(_slotNum);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void KickButton(int _slotNum) {
        if (!IsEmptySlot(_slotNum) && !IsMySlot(_slotNum) && GUILayout.Button("Kick", GUILayout.Width(40)))
            networkController.MasterClientKickPlayerInSlot(_slotNum, MainRoomModel.Instance.GetPlayerInSlot(_slotNum));
    }

    private void MainSlot(int _slotNum) {
        if (GUILayout.Button(MainRoomModel.Instance.GetSlotColor(_slotNum) + " [ " + MainRoomModel.Instance.GetPlayerNameInSlot(_slotNum) + " ]"))
            if(IsEmptySlot(_slotNum)) {
                networkController.MasterClientClearSlot((int)MainRoomModel.Instance.MySlot, GameManager.Instance.MyPlayer);
                networkController.MasterClientPlayerToSlot(_slotNum, GameManager.Instance.MyPlayer);
            }
            else if(IsMySlot(_slotNum))
                networkController.MasterClientClearSlot(_slotNum, GameManager.Instance.MyPlayer);
    }

    private void TeamButton(int _slotNum) {
        GUILayout.BeginVertical();
        if (IsMySlot(_slotNum)) {            
            GameManager.Instance.MyPlayer.customProperties["Team"] = GUIUtilities.Instance.ButtonOptions<PlayerTeam, bool>(ref teamSelecting, true,
                                                                     (PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"],
                                                                     MainRoomModel.Instance.AvailableTeams, 60);
            if (!MainRoomModel.Instance.MyTeam.Equals((PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"])) {
                MainRoomModel.Instance.MyTeam = (PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"];
                GameManager.Instance.UpdatePlayerTeamProperty();
            }
        }
        else if (MainRoomModel.Instance.GetPlayerInSlot(_slotNum) != null)
            GUILayout.Button(((PlayerTeam)MainRoomModel.Instance.GetPlayerInSlot(_slotNum).customProperties["Team"]).ToString(), GUILayout.Width(60));
        GUILayout.EndVertical();        
    }

    private bool IsMySlot(int _slotNum) {
        return MainRoomModel.Instance.SlotOwnedByPlayer(_slotNum, GameManager.Instance.MyPlayer);
    }
    private bool IsEmptySlot(int _slotNum) {
        return MainRoomModel.Instance.IsSlotEmpty(_slotNum);
    }
    #endregion

    private void SouthChat() {
        GUILayout.BeginArea(southChatRect);
        GUILayout.EndArea();
    }

    private void SouthButtons() {
        GUILayout.BeginArea(southButtonRect);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("To Lobby"))
            PhotonNetwork.LoadLevel("Lobby");
        if (networkController.IsMasterClient)
            StartGameButton();
        else
            ReadyButton();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void StartGameButton() {
        GUILayout.BeginVertical();
        style.fontSize = 9;
        if (GUILayout.Button("Start Game")) {
            networkController.SyncGameVariables();
            if (allPlayersReady = GameManager.Instance.AllPlayersReady())
                GameManager.Instance.MasterClientLoadMainStage();
        }
        if(!MainRoomModel.Instance.LocalClientOwnsSlot)
            GUILayout.Label("Please choose a slot", style);
        else if (!allPlayersReady)
            GUILayout.Label("Wait for joined players", style);
        style.fontSize = 12;
        GUILayout.EndVertical();
    }

    private void ReadyButton() {
        GUILayout.BeginVertical();
        style.fontSize = 9;
        if ((bool)GameManager.Instance.MyPlayer.customProperties["IsReady"]) {
            if (GUILayout.Button("Ready"))
                if (MainRoomModel.Instance.LocalClientOwnsSlot) {
                    GameManager.Instance.MyPlayer.customProperties["IsReady"] = false;
                    GameManager.Instance.UpdatePlayerIsReadyProperty();
                }
        }
        else
            if (GUILayout.Button("Not Ready")) {
                GameManager.Instance.MyPlayer.customProperties["IsReady"] = true;
                GameManager.Instance.UpdatePlayerIsReadyProperty();
            }

        if (!MainRoomModel.Instance.LocalClientOwnsSlot)
            GUILayout.Label("Please choose a slot", style);
        style.fontSize = 12;
        GUILayout.EndVertical();
    }
}