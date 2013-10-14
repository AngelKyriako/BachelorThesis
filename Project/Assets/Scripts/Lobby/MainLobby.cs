using UnityEngine;
using System.Collections;

public class MainLobby: MonoBehaviour{
	
	private enum ButtonState{
		none,
		create,
		join,
		enterQueue
	};
	private ButtonState buttonPressedLast;
    private string createRoomName, joinRoomName;
    private Vector2 scrollPos = Vector2.zero;
	private Rect fullscreen, north, south, west, east;
	
	
	public int northHeight;
	public int westWidth;	
	public Texture2D background;
	public GUIStyle titleStyle, welcomeLabelStyle, roomStyle;
	public GUIStyle westButtonStyle, goButtonStyle, inputTextFieldStyle;
	public int bigSpace;
	public int smallSpace;
	
    void Awake(){
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v0.1");		
		
		fullscreen = new Rect(0, 0, Screen.width, Screen.height);
		north = new Rect(0, 0, Screen.width, northHeight);
		south = new Rect(Screen.width, Screen.height, 0, 0);
		west = new Rect(0, north.y + north.height,
 		  	   westWidth, Screen.height - (north.y + north.height));
		east = new Rect(west.x + west.width, north.y + north.height,
		  	   Screen.width - (west.x + west.width), Screen.height - (south.y - (north.y + north.height)));

        PhotonNetwork.playerName = PlayerPrefs.GetString("name");
		joinRoomName = "";
		createRoomName = PhotonNetwork.playerName.EndsWith("s")?PhotonNetwork.playerName+"' room"
														 	   :PhotonNetwork.playerName+"'s room";
		buttonPressedLast = ButtonState.none;
    }
	
    private void ConnectingGUI(){
    	GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        	GUILayout.Label("Connecting to Photon server.");
        GUILayout.EndArea();
    }	
	
	private void NorthGUI(){
		GUILayout.BeginArea(north);
		GUILayout.Space(bigSpace);	
		GUILayout.Label("Dragonborn Lobby", titleStyle);
		GUILayout.Label("Welcome " + PhotonNetwork.playerName, welcomeLabelStyle);
		GUILayout.EndArea();
	}
	
	private void SouthGUI(){
		GUILayout.BeginArea(south);
		GUILayout.EndArea();
	}
	
	private void CreateRoomGUI(){
        if (GUILayout.Button("Create", westButtonStyle))
			buttonPressedLast = ButtonState.create;
            //PhotonNetwork.CreateRoom(createRoomName, true, true, 10);
	}
	
	private void JoinRoomGUI(){
		GUILayout.BeginVertical();
        joinRoomName = GUILayout.TextField(joinRoomName, inputTextFieldStyle);
		GUILayout.Space(bigSpace);	
        if (GUILayout.Button("Join", westButtonStyle))
			buttonPressedLast = ButtonState.join;
            //PhotonNetwork.JoinRoom(joinRoomName);
		GUILayout.EndVertical();
	}	
	
	private void EnterQueueGUI(){
        if (GUILayout.Button("Enter queue", westButtonStyle))
			buttonPressedLast = ButtonState.enterQueue;
        	//PhotonNetwork.JoinRandomRoom();
	}	
	
	private void RoomsGUI(){
        if (PhotonNetwork.GetRoomList().Length == 0)
            GUILayout.Label("...no rooms available.", roomStyle);
        else{
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo room in PhotonNetwork.GetRoomList()){
                if (GUILayout.Button(room.name+" ("+room.playerCount+"/"+room.maxPlayers+")", roomStyle))
					PhotonNetwork.JoinRoom(room.name);                    
            }
            GUILayout.EndScrollView();
        }		
	}
	
	private void WestGUI(){
		GUILayout.BeginArea(west);
		CreateRoomGUI();
		GUILayout.Space(bigSpace);	
		EnterQueueGUI();
		GUILayout.Space(3*bigSpace);	
		RoomsGUI();
		GUILayout.Space(bigSpace);	
		JoinRoomGUI();		
		GUILayout.EndArea();
	}		
	
	private void CreatePropertiesGUI(){
		GUILayout.Label("Create properties", goButtonStyle);
	}
	private void JoinPropertiesGUI(){
		GUILayout.Label("Join properties", goButtonStyle);
	}
	private void QueuePropertiesGUI(){
		GUILayout.Label("Queue properties", goButtonStyle);
	}		
	
	private void EastGUI(){
		GUILayout.BeginArea(east);
		if (buttonPressedLast == ButtonState.join)
			JoinPropertiesGUI();
		else if(buttonPressedLast == ButtonState.create)
			CreatePropertiesGUI();
		else if(buttonPressedLast == ButtonState.enterQueue)
			QueuePropertiesGUI();
		GUILayout.EndArea();
	}
	
    void OnGUI(){		
        if (!PhotonNetwork.connected){
			GUI.DrawTexture(fullscreen, background, ScaleMode.StretchToFill);
            ConnectingGUI();
		}
        else if (PhotonNetwork.room == null){
			GUI.DrawTexture(fullscreen, background, ScaleMode.StretchToFill);
			NorthGUI();
			SouthGUI();
			WestGUI();
			EastGUI();
		}
    }
}