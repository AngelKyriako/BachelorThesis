using UnityEngine;
using System;
using System.Collections.Generic;

public enum EditProperty {
    None,
    Title,
    Mode,
    Difficulty,
    MaxPlayers,
    MaxKills,
    Timer
};

public enum Action {
    None,
    Create,
    Join,
    MatchMaking
};

public class MainLobby: MonoBehaviour {

    private Action buttonPressedLast;
    private EditProperty editingField;
    private string createRoomName, joinRoomName;
    private Vector2 scrollPos = Vector2.zero;
    private Rect fullscreen, north, south, west, east;

    public int northHeight;
    public int westWidth;
    public Texture2D background;
    public GUIStyle titleStyle, welcomeLabelStyle, roomStyle;
    public GUIStyle westButtonStyle, propertyLabel, propertyValue, goButtonStyle;
    public int space;

    void Awake() {
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v0.1");

        fullscreen = new Rect(0, 0, Screen.width, Screen.height);
        north = new Rect(0, 0, Screen.width, northHeight);
        south = new Rect(Screen.width, Screen.height, 0, 0);
        west = new Rect(0, north.y + north.height,
               westWidth, Screen.height - (north.y + north.height));
        east = new Rect(west.x + west.width + 50, north.y + north.height + 50,
               Screen.width - (west.x + west.width + 50), south.y - (north.y + north.height));

        PhotonNetwork.playerName = PlayerPrefs.GetString("name");
        joinRoomName = "";
        createRoomName = PhotonNetwork.playerName.EndsWith("s") ? PhotonNetwork.playerName + "' room"
                                                                : PhotonNetwork.playerName + "'s room";
        RoomProperties.Instance.SetTitle(createRoomName);
        RoomProperties.Instance.SetHost(PhotonNetwork.playerName);

        buttonPressedLast = Action.None;
        editingField = EditProperty.None;
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
        GUILayout.Label("Welcome " + PhotonNetwork.playerName, welcomeLabelStyle);
        GUILayout.EndArea();
    }

    private void SouthGUI() {
        GUILayout.BeginArea(south);
        GUILayout.EndArea();
    }

    private void CreateRoomGUI() {
        if(GUILayout.Button("Create", westButtonStyle))
            buttonPressedLast = Action.Create;
    }

    private void JoinRoomGUI() {
        GUILayout.BeginVertical();
        joinRoomName = GUILayout.TextField(joinRoomName, GUILayout.MaxWidth(180));
        GUILayout.Space(space / 2);
        if(GUILayout.Button("Join", westButtonStyle))
            buttonPressedLast = Action.Join;
        GUILayout.EndVertical();
    }

    private void MatchMakingGUI() {
        if(GUILayout.Button("Matchmaking", westButtonStyle))
            buttonPressedLast = Action.MatchMaking;
    }

    private void RoomsGUI() {
        if(PhotonNetwork.GetRoomList().Length == 0)
            GUILayout.Label("...no rooms available.", roomStyle);
        else {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach(RoomInfo room in PhotonNetwork.GetRoomList()) {
                if(GUILayout.Button(room.name + " (" + room.playerCount + "/" + room.maxPlayers + ")", roomStyle))
                    joinRoomName = room.name;
            }
            GUILayout.EndScrollView();
        }
    }

    private void WestGUI() {
        GUILayout.BeginArea(west);
        GUILayout.Space(space);
        CreateRoomGUI();
        GUILayout.Space(space);
        MatchMakingGUI();
        GUILayout.Space(2 * space);
        RoomsGUI();
        GUILayout.Space(space);
        JoinRoomGUI();
        GUILayout.EndArea();
    }

    private void CreatePropertiesGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:", propertyLabel);
        RoomProperties.Instance.SetTitle(GUILayout.TextField(RoomProperties.Instance.GetTitle(), GUILayout.MaxWidth(250)));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:", propertyLabel);
        RoomProperties.Instance.SetMode(
            MyGUIHolder.Instance.RoomPropertyOptions<GameMode>(ref editingField, EditProperty.Mode,
                                                               RoomProperties.Instance.GetMode(),
                                                               RoomProperties.Instance.GetAvailableModes(), 110)
        );
        GUILayout.EndHorizontal();
        if(RoomProperties.Instance.GetMode() == GameMode.BattleRoyal) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max kills:", propertyLabel);
            RoomProperties.Instance.SetMaxKills(
                MyGUIHolder.Instance.RoomPropertyOptions<int>(ref editingField, EditProperty.MaxKills,
                                                              RoomProperties.Instance.GetMaxKills(),
                                                              RoomProperties.Instance.GetAvailableMaxKills(), 40)
            );
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:", propertyLabel);
        RoomProperties.Instance.SetMaxPlayers(
            MyGUIHolder.Instance.RoomPropertyOptions<int>(ref editingField, EditProperty.MaxPlayers,
                                                          RoomProperties.Instance.GetMaxPlayers(),
                                                          RoomProperties.Instance.GetAvailableMaxPlayers(), 28)
        );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:", propertyLabel);
        RoomProperties.Instance.SetDifficulty(
            MyGUIHolder.Instance.RoomPropertyOptions<Difficulty>(ref editingField, EditProperty.Difficulty,
                                                                 RoomProperties.Instance.GetDifficulty(),
                                                                 RoomProperties.Instance.GetAvailableDifficulties(), 60)
        );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:", propertyLabel);
        RoomProperties.Instance.SetTimer(
            MyGUIHolder.Instance.RoomPropertyOptions<double>(ref editingField, EditProperty.Timer,
                                                             RoomProperties.Instance.GetTimer(),
                                                             RoomProperties.Instance.GetAvailableTimers(), 39)
        );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Go", goButtonStyle)) {
            //paikse mpalitsa me ta properties
            PhotonNetwork.CreateRoom(createRoomName, true, true, RoomProperties.Instance.GetMaxPlayers());
        }
        GUILayout.EndHorizontal();
    }

    private void JoinPropertiesGUI() {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Go", goButtonStyle))
            PhotonNetwork.JoinRoom(joinRoomName);
        GUILayout.EndHorizontal();
    }

    private void QueuePropertiesGUI() {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Go", goButtonStyle))
            PhotonNetwork.JoinRandomRoom();
        GUILayout.EndHorizontal();
    }

    private void EastGUI() {
        GUILayout.BeginArea(east);
        if(buttonPressedLast == Action.Join)
            JoinPropertiesGUI();
        else if(buttonPressedLast == Action.Create)
            CreatePropertiesGUI();
        else if(buttonPressedLast == Action.MatchMaking)
            QueuePropertiesGUI();
        GUILayout.EndArea();
    }

    void OnGUI() {
        if(!PhotonNetwork.connected) {
            GUI.DrawTexture(fullscreen, background, ScaleMode.StretchToFill);
            ConnectingGUI();
        } else if(PhotonNetwork.room == null) {
            GUI.DrawTexture(fullscreen, background, ScaleMode.StretchToFill);
            NorthGUI();
            SouthGUI();
            WestGUI();
            EastGUI();
        }
    }
}