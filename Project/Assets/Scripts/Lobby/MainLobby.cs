using UnityEngine;
using System.Collections;

public class MainLobby : MonoBehaviour {
	
    private string roomName;
    private Vector2 scrollPos = Vector2.zero;	

    void Awake()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v0.1");

        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));		
		
		roomName = PhotonNetwork.playerName + " room";
    }

    void OnGUI()
    {
        if (!PhotonNetwork.connected){
            ShowConnectingGUI();
            return;
        }


        if (PhotonNetwork.room != null)
            return;


        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Main Menu");

        //Player name
        GUILayout.BeginHorizontal();
        GUILayout.Label("Player name:", GUILayout.Width(150));
        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
        if (GUI.changed)//Save name
            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);


        //Join room by title
        GUILayout.BeginHorizontal();
        GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("GO"))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        GUILayout.EndHorizontal();

        //Create a room (fails if exist!)
        GUILayout.BeginHorizontal();
        GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("GO"))
        {
            PhotonNetwork.CreateRoom(roomName, true, true, 10);
        }
        GUILayout.EndHorizontal();

        //Join random room
        GUILayout.BeginHorizontal();
        GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("..no games available...");
        }
        else
        {
            if (GUILayout.Button("GO"))
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.Label("ROOM LISTING:");
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("..no games available..");
        }
        else
        {
            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
                if (GUILayout.Button("JOIN"))
                {
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        GUILayout.EndArea();
    }


    void ShowConnectingGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
}