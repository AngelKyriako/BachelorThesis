using UnityEngine;
using System.Collections;

public class TerrainMap: SingletonMono<TerrainMap> {

    private const int MAIN_WIDTH = 100, MAIN_HEIGHT = 100;

    private Rect windowRect;
    private bool isVisible;
    private string windowText;
    private int windowId, windowWidth, windowHeight;
    private TerrainMap() { }

    void Start(){
        isVisible = false;
        SetUpGUI();
    }

    public void SetUpGUI() {
        windowText = "Map";
        windowId = 10;
        windowWidth = 200;
        windowHeight = 500;
        windowRect = new Rect((Screen.width - windowWidth / 2) / 2,
                              (Screen.height - windowHeight / 2) / 2,
                              windowWidth,
                              windowHeight);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.M))
            isVisible = !isVisible;
    }

    void OnGUI() {
        if (isVisible)
            windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(windowId, windowRect, MainWindow, windowText));
    }

        void MainWindow(int windowID){
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
            GUILayout.Label(_name +"'s position: "+GameManager.Instance.GetCharacter(_name).transform.position.ToString());
        }
    }
}
