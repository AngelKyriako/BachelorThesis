using UnityEngine;
using System.Collections.Generic;

public class MainRoomGUI: MonoBehaviour {

    private const int SOUTH_HEIGHT = 100, SOUTH_BUTTONS_WIDTH = 200,
                      PREFERENCES_WIDTH = 400;

    public Texture2D background;

    private Rect westPreferencesRect,
                 eastSlotsRect, playerSlotRect,
                 southChatRect, southButtonRect;

    private string myName, readyText = "Ready";
    private VariableType editingPreferencesField;    

    private RoomNetController networkController;

	void Awake () {
        enabled = false;
        networkController = GetComponent<RoomNetController>();
        networkController.MasterClientRequestForRoomState();        
	}

    void Start() {
        #region Init Rects
        westPreferencesRect = new Rect(0, 0, PREFERENCES_WIDTH, Screen.height - SOUTH_HEIGHT);

        eastSlotsRect = new Rect(westPreferencesRect.width, 0, Screen.width - westPreferencesRect.width, Screen.height - SOUTH_HEIGHT);
        playerSlotRect = new Rect(0, 0, eastSlotsRect.width, eastSlotsRect.height / MainRoomModel.Instance.PlayerSlotsLength);

        southChatRect = new Rect(0, Screen.height - SOUTH_HEIGHT, Screen.width - SOUTH_BUTTONS_WIDTH, SOUTH_HEIGHT);
        southButtonRect = new Rect(southChatRect.width, Screen.height - SOUTH_HEIGHT, SOUTH_BUTTONS_WIDTH, SOUTH_HEIGHT);
        #endregion
        myName = GameManager.Instance.MyPlayer.name;
        editingPreferencesField = default(VariableType);
    }

    void OnGUI() {
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
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingPreferencesField, VariableType.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndHorizontal();
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal)) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max kills:");
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:");
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingPreferencesField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:");
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingPreferencesField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:");
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, VariableType>(ref editingPreferencesField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void WestGamePreferencesViewed() {
        GUILayout.BeginArea(westPreferencesRect);
        GUILayout.Label("Room: " + GameVariables.Instance.Title, GUILayout.MaxWidth(300));
        GUILayout.Label("Mode: " + GameVariables.Instance.Mode.Key);
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
        for (int i = 0; i < MainRoomModel.Instance.PlayerSlotsLength; ++i)
            PlayerSlot(i);
        GUILayout.EndArea();
    }

    private void PlayerSlot(int _slotNum) {
        playerSlotRect.y = _slotNum * playerSlotRect.height;

        string slotName = MainRoomModel.Instance.GetPlayerNameInSlot(_slotNum);        

        GUILayout.BeginArea(playerSlotRect);
        GUILayout.BeginHorizontal();
        ClearButton(_slotNum);
        MainSlot(_slotNum);
        TeamButton(_slotNum);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void ClearButton(int _slotNum) {
        if (IsMySlot(_slotNum) && GUILayout.Button("Clear"))
            networkController.MasterClientClearSlot(_slotNum, myName);
    }

    private void MainSlot(int _slotNum) {
        if (GUILayout.Button(MainRoomModel.Instance.GetSlotColor(_slotNum) + " [ " + MainRoomModel.Instance.GetPlayerNameInSlot(_slotNum) + " ]") &&
            IsEmptySlot(_slotNum)) {
            networkController.MasterClientClearSlot((int)MainRoomModel.Instance.MySlot, myName);
            networkController.MasterClientPlayerToSlot(_slotNum, myName);
        }
    }

    private void TeamButton(int _slotNum) {//@TODO: ????
        string state = default(string);

        GUILayout.BeginVertical();        
        //if(IsMySlot(_slotNum))
        //    GameManager.Instance.MyPlayer.customProperties["Team"] = GUIUtilities.Instance.ButtonOptions<PlayerTeam, string>(ref state, state,
        //                                                         (PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"],
        //                                                         MainRoomModel.Instance.AvailableTeams, 40);
        GUILayout.EndVertical();        
    }

    private bool IsMySlot(int _slotNum) {
        return MainRoomModel.Instance.SlotOwnedByPlayer(_slotNum, myName);
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
        //if ((bool)GameManager.Instance.Me.Player.customProperties["IsReady"]) {
        //    if (GUILayout.Button("Ready")) {
        //        networkController.SyncGameVariables();
        //        GameManager.Instance.Me.Player.customProperties["IsReady"] = false;
        //    }
        //}
        //else {
        //    if (GUILayout.Button("Not Ready")) {
        //        networkController.SyncGameVariables();
        //        GameManager.Instance.Me.Player.customProperties["IsReady"] = true;
        //    }
        //}
    }

    private void ReadyToggleButton() {
        readyText = GUIUtilities.Instance.ToggleTextButton(readyText, "Ready", "Not ready");
        if (!((bool)GameManager.Instance.Me.Player.customProperties["IsReady"]).Equals(readyText.Equals("Ready")))
            GameManager.Instance.Me.Player.customProperties["IsReady"] = readyText.Equals("Ready");
    }
}