using UnityEngine;
using System.Collections.Generic;

public class MainRoomGUI: MonoBehaviour {

    private const int PLAYER_SLOT_HEIGHT = 50,
                      SOUTH_CHAT_HEIGHT = 100, SOUTH_BUTTONS_WIDTH = 100, SOUTH_BUTTONS_HEIGHT = 50,
                      PREFERENCES_WIDTH = 250;

    public Texture2D background;

    private Rect westPreferencesRect,
                 eastSlotsRect, playerSlotRect,
                 southChatRect, southButtonRect;
    
    private string readyText = "Ready";
    private VariableType editingPreferencesField;
    private bool teamSelecting;
    
    private RoomNetController networkController;

	void Awake () {
        enabled = false;
        networkController = GetComponent<RoomNetController>();
	}

    void Start() {
        #region Init Rects
        westPreferencesRect = new Rect(0, 0, PREFERENCES_WIDTH, Screen.height - SOUTH_CHAT_HEIGHT);

        eastSlotsRect = new Rect(westPreferencesRect.width + 10, 0, Screen.width - westPreferencesRect.width, Screen.height);
        playerSlotRect = new Rect(0, 0, eastSlotsRect.width/2, eastSlotsRect.height);

        southChatRect = new Rect(0, Screen.height - SOUTH_CHAT_HEIGHT, Screen.width - SOUTH_BUTTONS_WIDTH, SOUTH_CHAT_HEIGHT);
        southButtonRect = new Rect(Screen.width - SOUTH_BUTTONS_WIDTH, Screen.height - SOUTH_BUTTONS_HEIGHT, SOUTH_BUTTONS_WIDTH, SOUTH_BUTTONS_HEIGHT);
        #endregion

        editingPreferencesField = default(VariableType);
        teamSelecting = false;
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

    #region Preferences
    private void WestGamePreferencesEditable() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Room: " + GameVariables.Instance.Title, GUILayout.MaxWidth(300));
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:");
        GUILayout.BeginVertical();
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingPreferencesField, VariableType.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map:");
        GUILayout.BeginVertical();
        GameVariables.Instance.Map = GUIUtilities.Instance.ButtonOptions<GameMap, VariableType>(ref editingPreferencesField, VariableType.Map,
                                                               GameVariables.Instance.Map,
                                                               GameVariables.Instance.AvailableMaps, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal)) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max kills:");
            GUILayout.BeginVertical();
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:");
        GUILayout.BeginVertical();
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:");
        GUILayout.BeginVertical();
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingPreferencesField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:");
        GUILayout.BeginVertical();
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, VariableType>(ref editingPreferencesField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void WestGamePreferencesViewed() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.Label("Room: " + GameVariables.Instance.Title, GUILayout.MaxWidth(300));
        GUILayout.Label("Mode: " + GameVariables.Instance.Mode.Key);
        GUILayout.Label("Map: " + GameVariables.Instance.Map.Key);
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal))
            GUILayout.Label("Target kills: " + GameVariables.Instance.TargetKills.Key);
        GUILayout.Label("Max players: " + GameVariables.Instance.MaxPlayers.Key);
        GUILayout.Label("Difficulty: " + GameVariables.Instance.Difficulty.Key);
        GUILayout.Label("Timer: " + GameVariables.Instance.Timer.Key);
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

        string slotName = MainRoomModel.Instance.GetPlayerNameInSlot(_slotNum);        

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
            networkController.MasterClientKickPlayerInSlot(_slotNum, GameManager.Instance.MyPlayer);
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
        if (networkController.IsMasterClient)
            StartGameButton();
        else
            ReadyButton();
        GUILayout.EndArea();
    }

    private void StartGameButton() {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Game")) {
            networkController.SyncGameVariables();
            if (GameManager.Instance.AllPlayersReady())
                GameManager.Instance.StartGame();
            else
                ;//@TODO: message not all players are ready
        }
        GUILayout.EndHorizontal();

    }

    private void ReadyButton() {
        if ((bool)GameManager.Instance.MyPlayer.customProperties["IsReady"]) {
            if (GUILayout.Button("Ready"))
                if (MainRoomModel.Instance.LocalClientOwnsSlot){
                    GameManager.Instance.MyPlayer.customProperties["IsReady"] = false;
                    GameManager.Instance.UpdatePlayerIsReadyProperty();
                }
                else
                    ;//@TODO message down of button
        }
        else
            if (GUILayout.Button("Not Ready")) {
                GameManager.Instance.MyPlayer.customProperties["IsReady"] = true;
                GameManager.Instance.UpdatePlayerIsReadyProperty();
            }

    }

    //private void ReadyToggleButton() {
    //    readyText = GUIUtilities.Instance.ToggleTextButton(readyText, "Ready", "Not ready");
    //    if (!((bool)GameManager.Instance.MyPlayer.customProperties["IsReady"]).Equals(readyText.Equals("Ready")))
    //        GameManager.Instance.MyPlayer.customProperties["IsReady"] = readyText.Equals("Ready");
    //}
}