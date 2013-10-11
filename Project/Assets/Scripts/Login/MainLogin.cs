using UnityEngine;
using System.Collections;


public class MainLogin: MonoBehaviour{
	
	//GUI
	private Rect fullScreenRect;
	public Texture2D background;
	
	public Rect layoutRect;
	public int spaceBig;	
	public int spaceSmall;	
	
	public GUIStyle bigLabelStyle;
	public GUIStyle smallLabelStyle;
	public GUIStyle inputTextFieldStyle;
	public GUIStyle buttonStyle;
	
	//Logic
	private string playerName;


	void Awake(){
		playerName = "";
		fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);		
		layoutRect = new Rect((Screen.width - layoutRect.width)/2, (Screen.height - layoutRect.height)/2,
							   layoutRect.width, layoutRect.height);			
	}
	
	void OnGUI(){
        
		GUI.DrawTexture(fullScreenRect, background, ScaleMode.StretchToFill);
		
		GUILayout.BeginArea(layoutRect);
	        GUILayout.Label("Welcome to Dragongarden BITCH !!!", bigLabelStyle);
			GUILayout.Space(spaceBig);
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
			        GUILayout.Label("Enter name:", smallLabelStyle);
					GUILayout.Space(spaceSmall);
			        playerName = GUILayout.TextField(playerName, inputTextFieldStyle);
			        if (GUI.changed)
			            PlayerPrefs.SetString("playerName", playerName);
				GUILayout.EndHorizontal();
				GUILayout.Space(spaceBig);
		        if (GUILayout.Button("Login", buttonStyle) && playerName.Trim().Length != 0)
					Application.LoadLevel ("Lobby"); 			
	        GUILayout.EndVertical();   					
		GUILayout.EndArea();
	}
}