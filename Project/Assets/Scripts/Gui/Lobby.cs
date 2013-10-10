using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {

	public Texture2D babyDragons;

	public int width;
	public int height;	
	public int x;
	public int y;	
	
    void Start(){
		width = 800;
		height = 600;	
		x = Screen.width/2-width/2;
		y = Screen.height/2-height/2;	
    }
	
	void OnGUI () {
		GUI.Box(new Rect(x, y, width, height), new GUIContent(babyDragons));
	}
}
