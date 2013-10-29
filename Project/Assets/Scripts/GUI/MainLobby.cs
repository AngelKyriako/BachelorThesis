﻿using UnityEngine;
using System.Collections.Generic;

public enum EditProperty {
    None,
    Title,
    Mode,
    Difficulty,
    MaxPlayers,
    targetKills,
    Timer
};

public enum Action {
    None,
    CreateRoom,
    ViewRoom,
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

    private void Awake() {
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
        GameVariables.Instance.Title = createRoomName;
        GameVariables.Instance.Host = PhotonNetwork.playerName;

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
        GUILayout.Space(space);
        GUILayout.Label("Welcome " + PhotonNetwork.playerName, welcomeLabelStyle);
        GUILayout.EndArea();
    }

    private void SouthGUI() {
        GUILayout.BeginArea(south);
        GUILayout.EndArea();
    }

    private void CreateRoomGUI() {
        if(GUILayout.Button("Create", westButtonStyle, GUILayout.Width(130)))
            buttonPressedLast = Action.CreateRoom;
    }

    private void MatchMakingGUI() {
        if(GUILayout.Button("Matchmaking", westButtonStyle, GUILayout.Width(250)))
            buttonPressedLast = Action.MatchMaking;
    }

    private void RoomsGUI() {
        if(PhotonNetwork.GetRoomList().Length == 0)
            GUILayout.Label("...no rooms available.", roomStyle);
        else {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach(RoomInfo room in PhotonNetwork.GetRoomList()) {
                if (GUILayout.Button(room.name + " (" + room.playerCount + "/" + room.maxPlayers + ")", roomStyle)) {
                    buttonPressedLast = Action.ViewRoom;
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
            PhotonNetwork.JoinRoom(joinRoomName);
            PhotonNetwork.LoadLevel("MeetingPoint");
        }
        GUILayout.EndVertical();
    }

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

    private void CreatePropertiesGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:", propertyLabel);
        GameVariables.Instance.Title = GUILayout.TextField(GameVariables.Instance.Title, GUILayout.MaxWidth(250));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode:", propertyLabel);
        GameVariables.Instance.Mode = GUIUtilities.Instance.ButtonOptions<GameMode, EditProperty>(ref editingField, EditProperty.Mode,
                                                               GameVariables.Instance.Mode,
                                                               GameVariables.Instance.AvailableModes, 110);
        GUILayout.EndHorizontal();
        if(GameVariables.Instance.Mode.Value == GameMode.BattleRoyal) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max kills:", propertyLabel);
            GameVariables.Instance.TargetKills = GUIUtilities.Instance.ButtonOptions<int, EditProperty>(ref editingField, EditProperty.targetKills,
                                                              GameVariables.Instance.TargetKills,
                                                              GameVariables.Instance.AvailableTargetKills, 40);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players:", propertyLabel);
        GameVariables.Instance.MaxPlayers = GUIUtilities.Instance.ButtonOptions<int, EditProperty>(ref editingField, EditProperty.MaxPlayers,
                                                          GameVariables.Instance.MaxPlayers,
                                                          GameVariables.Instance.AvailableMaxPlayers, 28);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty:", propertyLabel);
        GameVariables.Instance.Difficulty = GUIUtilities.Instance.ButtonOptions<GameDifficulty, EditProperty>(ref editingField, EditProperty.Difficulty,
                                                                 GameVariables.Instance.Difficulty,
                                                                 GameVariables.Instance.AvailableDifficulties, 60);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer:", propertyLabel);
        GameVariables.Instance.Timer = GUIUtilities.Instance.ButtonOptions<double, EditProperty>(ref editingField, EditProperty.Timer,
                                                             GameVariables.Instance.Timer,
                                                             GameVariables.Instance.AvailableTimers, 39);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Go", goButtonStyle)) {
            //@TODO: Add properties on server map
            PhotonNetwork.CreateRoom(createRoomName, true, true, GameVariables.Instance.MaxPlayers.Value);
            PhotonNetwork.LoadLevel("MeetingPoint");
        }
        GUILayout.EndHorizontal();
    }

    private void JoinPropertiesGUI() {
        GUILayout.BeginHorizontal();
        //@TODO: Game properties here
        if (GUILayout.Button("Go", goButtonStyle)) {
            PhotonNetwork.JoinRoom(joinRoomName);
            if (PhotonNetwork.room != null)
                PhotonNetwork.LoadLevel("MeetingPoint");
        }
        GUILayout.EndHorizontal();
    }

    private void MatchmakingPropertiesGUI() {
        //@TODO: Game properties here
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Go", goButtonStyle)) {
            PhotonNetwork.JoinRandomRoom();
            if (PhotonNetwork.room != null)
                PhotonNetwork.LoadLevel("MeetingPoint");
        }
        GUILayout.EndHorizontal();
    }

    private void EastGUI() {
        GUILayout.BeginArea(east);
        if(buttonPressedLast == Action.ViewRoom)
            JoinPropertiesGUI();
        else if (buttonPressedLast == Action.CreateRoom)
            CreatePropertiesGUI();
        else if(buttonPressedLast == Action.MatchMaking)
            MatchmakingPropertiesGUI();
        GUILayout.EndArea();
    }

    private void OnGUI() {
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