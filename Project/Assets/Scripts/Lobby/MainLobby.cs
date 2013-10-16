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
	private RoomProperties createRoomProperties;
	
	public int northHeight;
	public int westWidth;	
	public Texture2D background;
	public GUIStyle titleStyle, welcomeLabelStyle, roomStyle;
	public GUIStyle westButtonStyle, propertyLabel, propertyValue, goButtonStyle;
	public int space;
	
    void Awake(){
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v0.1");		
		
		fullscreen = new Rect(0, 0, Screen.width, Screen.height);
		north = new Rect(0, 0, Screen.width, northHeight);
		south = new Rect(Screen.width, Screen.height, 0, 0);
		west = new Rect(0, north.y + north.height,
 		  	   westWidth, Screen.height - (north.y + north.height));
		east = new Rect(west.x + west.width+50, north.y + north.height+50,
		  	   Screen.width - (west.x + west.width + 50), south.y - (north.y + north.height));

        PhotonNetwork.playerName = PlayerPrefs.GetString("name");
		joinRoomName = "";
		createRoomName = PhotonNetwork.playerName.EndsWith("s")?PhotonNetwork.playerName+"' room"
														 	   :PhotonNetwork.playerName+"'s room";
		buttonPressedLast = ButtonState.none;
		createRoomProperties = new RoomProperties(createRoomName, PhotonNetwork.playerName);
    }
	
    private void ConnectingGUI(){
    	GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        	GUILayout.Label("Connecting to Photon server.");
        GUILayout.EndArea();
    }	
	
	private void NorthGUI(){
		GUILayout.BeginArea(north);
		GUILayout.Space(space);	
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
	}
	
	private void JoinRoomGUI(){
		GUILayout.BeginVertical();
        joinRoomName = GUILayout.TextField(joinRoomName, GUILayout.MaxWidth(180));
		GUILayout.Space(space/2);	
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
		GUILayout.Space(space);
		CreateRoomGUI();
		GUILayout.Space(space);
		EnterQueueGUI();
		GUILayout.Space(2*space);	
		RoomsGUI();
		GUILayout.Space(space);	
		JoinRoomGUI();		
		GUILayout.EndArea();
	}
	
	private void CreatePropertiesGUI(){
		createRoomProperties.GetTitle(); // 1
		createRoomProperties.GetMode(); // 1
		createRoomProperties.GetMaxKills(); // 1
		createRoomProperties.GetMaxPlayers(); // 1
		createRoomProperties.GetTimer(); // 1
		createRoomProperties.GetUsedSkills();
		createRoomProperties.GetBannedSkills();
		
		GUILayout.BeginVertical();		
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Title:", propertyLabel);
		createRoomProperties.SetTitle(GUILayout.TextField(createRoomProperties.GetTitle(), GUILayout.MaxWidth(180)));
		GUILayout.EndHorizontal();		

		GUILayout.BeginHorizontal();
		GUILayout.Label("Mode:", propertyLabel);
		string[] str1 = {"Battle Royal", "Conquerors", "Capture the flag"};
		GUILayout.SelectionGrid(3, str1, str1.Length, propertyValue);
		GUILayout.EndHorizontal();		
		
		
		if (createRoomProperties.IsBattleRoyal()){
			GUILayout.BeginHorizontal();
			GUILayout.Label("Max kills:", propertyLabel);
			string[] str2 = {"10", "20", "50", "75", "100", "200", "500"};
			GUILayout.SelectionGrid(3, str2, str2.Length, propertyValue);
			GUILayout.EndHorizontal();
		}
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Max players:", propertyLabel);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Timer:", propertyLabel);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Go", goButtonStyle))
			PhotonNetwork.CreateRoom(createRoomName, true, true, createRoomProperties.GetMaxPlayers());				
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		
		
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