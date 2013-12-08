using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public enum VariableType {
    None,
    Title,
    Mode,
    Map,
    Difficulty,
    MaxPlayers,
    targetKills,
    Timer
};

public enum LobbyAction {
    None,
    CreateRoom,
    ViewRoom,
    MatchMaking
};

public class MainLobby: MonoBehaviour {

    public int northHeight;
    public int westWidth;
    public Texture2D background;
    public GUIStyle titleStyle, welcomeLabelStyle, roomStyle;
    public GUIStyle westButtonStyle, propertyLabel, goButtonStyle;
    public int space;

    private LobbyAction buttonPressedLast;
    private VariableType editingField;
    private string createRoomName, joinRoomName;
    private Vector2 scrollPos = Vector2.zero;
    private Rect north, south, west, east;

    void Awake() {
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0");

        north = new Rect(0, 0, Screen.width, northHeight);
        south = new Rect(Screen.width, Screen.height, 0, 0);
        west = new Rect(0, north.y + north.height,
               westWidth, Screen.height - (north.y + north.height));
        east = new Rect(west.x + west.width + 50, north.y + north.height + 50,
               Screen.width - (west.x + west.width + 50), south.y - (north.y + north.height));

        PhotonNetwork.playerName = PlayerPrefs.GetString("name");
        PhotonNetwork.player.customProperties["IsReady"] = false;
        PhotonNetwork.player.customProperties["Team"] = PlayerTeam.Team1;
        PhotonNetwork.player.customProperties["Color"] = PlayerColor.None;

        joinRoomName = "";
        createRoomName = PhotonNetwork.playerName.EndsWith("s") ? PhotonNetwork.playerName + "' room"
                                                                : PhotonNetwork.playerName + "'s room";
        GameVariables.Instance.Title = createRoomName;

        buttonPressedLast = default(LobbyAction);
        editingField = default(VariableType);
    }

    void OnGUI() {
        GUI.DrawTexture(GUIUtilities.Instance.FullScreenRect, background, ScaleMode.StretchToFill);
        if (!PhotonNetwork.connected)
            ConnectingGUI();
        else if (PhotonNetwork.room == null) {
            NorthGUI();
            SouthGUI();
            WestGUI();
            EastGUI();
        }
    }

    private void ConnectingGUI() {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        GUILayout.Label("Connecting to Photon server.");
        GUILayout.EndArea();
    }

    private void NorthGUI() {
        GUILayout.BeginArea(north);
        GUILayout.Space(space);
        GUILayout.Label("Dragonborn Lobby", titleStyle);
        GUILayout.Space(space);
        GUILayout.Label("Welcome " + PhotonNetwork.playerName, welcomeLabelStyle);
        GUILayout.EndArea();
    }

    private void SouthGUI() {
        GUILayout.BeginArea(south);
        GUILayout.EndArea();
    }

    #region West GUI
    private void WestGUI() {
        GUILayout.BeginArea(west);
        CreateRoomGUI();
        GUILayout.Space(3 * space);
        MatchMakingGUI();
        GUILayout.Space(3 * space);
        RoomsGUI();
        GUILayout.Space(3 * space);
        JoinRoomGUI();
        GUILayout.EndArea();
    }

    private void CreateRoomGUI() {
        if (GUILayout.Button("Create", westButtonStyle, GUILayout.Width(130)))
            buttonPressedLast = LobbyAction.CreateRoom;
    }

    private void MatchMakingGUI() {
        if (GUILayout.Button("Matchmaking", westButtonStyle, GUILayout.Width(250)))
            buttonPressedLast = LobbyAction.MatchMaking;
    }

    private void RoomsGUI() {
        if (PhotonNetwork.GetRoomList().Length == 0)
            GUILayout.Label("...no rooms available.", roomStyle);
        else {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
                if (GUILayout.Button(room.name + " (" + room.playerCount + "/" + room.maxPlayers + ")", roomStyle)) {
                    buttonPressedLast = LobbyAction.ViewRoom;
                    joinRoomName = room.name;
                }
            }
            GUILayout.EndScrollView();
        }
    }

    private void JoinRoomGUI() {
        GUILayout.BeginVertical();
        joinRoomName = GUILayout.TextField(joinRoomName, GUILayout.MaxWidth(180));
        GUILayout.Space(space / 2);
        if (GUILayout.Button("Join", westButtonStyle, GUILayout.Width(90))) {
            PhotonNetwork.LoadLevel("Room");
            PhotonNetwork.JoinRoom(joinRoomName);
            if (GUILayout.Button("Go", goButtonStyle)) {
                PhotonNetwork.LoadLevel("Room");
                PhotonNetwork.JoinRoom(joinRoomName);
            }
        }
        GUILayout.EndVertical();
    }
#endregion

    #region East GUI
    private void EastGUI() {
        GUILayout.BeginArea(east);
        if(buttonPressedLast == LobbyAction.ViewRoom)
            JoinPropertiesGUI();
        else if (buttonPressedLast == LobbyAction.CreateRoom)
            CreatePropertiesGUI();
        else if(buttonPressedLast == LobbyAction.MatchMaking)
            MatchmakingPropertiesGUI();
        GUILayout.EndArea();
    }

    private void CreatePropertiesGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:", propertyLabel);
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(250));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:", propertyLabel);
        GUILayout.BeginVertical();
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, VariableType>(ref editingField, VariableType.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map:", propertyLabel);
        GUILayout.BeginVertical();
        GameVariables.Instance.Map = GUIUtilities.Instance.ButtonOptions<GameMap, VariableType>(ref editingField, VariableType.Map,
                                                               GameVariables.Instance.Map,
                                                               GameVariables.Instance.AvailableMaps, 110);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (GameVariables.Instance.Mode.Value == GameMode.BattleRoyal) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Target kills:", propertyLabel);
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:", propertyLabel);
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, VariableType>(ref editingField, VariableType.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:", propertyLabel);
        GUILayout.BeginVertical();
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, VariableType>(ref editingField, VariableType.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:", propertyLabel);
        GUILayout.BeginVertical();
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, VariableType>(ref editingField, VariableType.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Go", goButtonStyle)) {

            string[] roomPropsInLobby = { "Mode", "Difficulty", "Target kills", "Timer" };
            string[] roomPropsInLobbyBR = { "Mode", "Difficulty", "Timer" };

            PhotonNetwork.CreateRoom(createRoomName, true, true, GameVariables.Instance.MaxPlayers.Value,
                                     new Hashtable() {  {"Mode", GameVariables.Instance.Mode.Value},
                                                        {"Difficulty", GameVariables.Instance.Difficulty.Value},
                                                        {"Target kills", GameVariables.Instance.TargetKills.Value},
                                                        {"Timer", GameVariables.Instance.Timer.Value} },
                                     GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal)?roomPropsInLobbyBR:roomPropsInLobby);

            PhotonNetwork.player.customProperties["IsReady"] = true;
            PhotonNetwork.LoadLevel("Room");
        }
        GUILayout.EndHorizontal();
    }

    private void JoinPropertiesGUI() {
        GUILayout.BeginHorizontal();
        //@TODO: Game properties here
        if (GUILayout.Button("Go", goButtonStyle)) {
            PhotonNetwork.LoadLevel("Room");
            PhotonNetwork.JoinRoom(joinRoomName);
        }
        GUILayout.EndHorizontal();
    }

    private void MatchmakingPropertiesGUI() {
        //@TODO: Game properties here
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Go", goButtonStyle)) {
            PhotonNetwork.LoadLevel("Room");
            PhotonNetwork.JoinRandomRoom();
        }
        GUILayout.EndHorizontal();
    }


#endregion
}