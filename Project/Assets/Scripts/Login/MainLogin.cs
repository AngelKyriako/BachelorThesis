using UnityEngine;
using System.Collections;


public class MainLogin: MonoBehaviour{
	
	private string playerName;
	private Rect fullscreen;
	
	public Texture2D background;
	public Rect layoutRect;
	public GUIStyle bigLabelStyle, smallLabelStyle, inputTextFieldStyle, bigButtonStyle;	
	public int bigSpace;	
	public int smallSpace;	

	void Awake(){
		playerName = "";
		fullscreen = new Rect(0, 0, Screen.width, Screen.height);
		layoutRect = new Rect((Screen.width - layoutRect.width)/2, (Screen.height - layoutRect.height)/2,
							   layoutRect.width, layoutRect.height);			
	}
	
	void OnGUI(){
		GUI.DrawTexture(fullscreen, background, ScaleMode.StretchToFill);

		GUILayout.BeginArea(layoutRect);
		GUILayout.Space(bigSpace*2);				
        GUILayout.Label("Welcome to Dragonborn", bigLabelStyle);
		GUILayout.Space(bigSpace);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
        GUILayout.Label("Enter name:", smallLabelStyle);
		GUILayout.Space(smallSpace);
        playerName = GUILayout.TextField(playerName, inputTextFieldStyle);
        if (GUI.changed)
            PlayerPrefs.SetString("name", playerName);
		GUILayout.EndHorizontal();
		GUILayout.Space(bigSpace);
        if (GUILayout.Button("Login", bigButtonStyle) && playerName.Trim().Length != 0)
			Application.LoadLevel ("Lobby"); 			
        GUILayout.EndVertical();   					
		GUILayout.EndArea();
	}
}