using UnityEngine;
using System.Collections;


public class MainLogin: MonoBehaviour{
	
	private string playerName;
	private Rect fullScreenRect;
	
	public Texture2D background;
	public Rect layoutRect;
	public GUIStyle bigLabelStyle;
	public GUIStyle smallLabelStyle;
	public GUIStyle inputTextFieldStyle;
	public GUIStyle bigButtonStyle;	
	public int spaceBig;	
	public int spaceSmall;	

	void Awake(){
		playerName = "";
		fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);		
		layoutRect = new Rect((Screen.width - layoutRect.width)/2, (Screen.height - layoutRect.height)/2,
							   layoutRect.width, layoutRect.height);			
	}
	
	void OnGUI(){
        
		GUI.DrawTexture(fullScreenRect, background, ScaleMode.StretchToFill);

		GUILayout.BeginArea(layoutRect);
			GUILayout.Space(spaceBig);
			GUILayout.Space(spaceBig);				
	        GUILayout.Label("Welcome to Dragon Garden", bigLabelStyle);
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
		        if (GUILayout.Button("Login", bigButtonStyle) && playerName.Trim().Length != 0)
					Application.LoadLevel ("Lobby"); 			
	        GUILayout.EndVertical();   					
		GUILayout.EndArea();
	}
}