using UnityEngine;
using System.Collections;

public class RoomDetails: MonoBehaviour{
	
	public void CreatePropertiesGUI(RoomProperties room, string roomName){
		GUILayout.Label("Create properties");
	}
	
	public void JoinPropertiesGUI(){
		GUILayout.Label("Join properties");
	}
	
	public void QueuePropertiesGUI(){
		GUILayout.Label("Queue properties");
	}
}
