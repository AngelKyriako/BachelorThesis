using UnityEngine;
using System.Collections;

public class TerrainMap: SingletonMono<TerrainMap> {

    private const float MAP_POINT_SIZE = 20;
    private const int VISIBLE_ENEMY_LAYER = 9;
    
    private Rect windowRect;
    private bool isVisible;
    private string windowText;
    private int windowId, windowWidth, windowHeight;
    private float terrainMapWidthScale, terrainMapHeightScale;

    private TerrainMap() { }

    void Start(){
        isVisible = false;
        SetUpGUI();
    }

    public void SetUpGUI() {
        windowText = "";
        windowId = 10;
        windowWidth = 150;
        windowHeight = 150;
        windowRect = new Rect((Screen.width - windowWidth / 2) / 2,
                              (Screen.height - windowHeight / 2) / 2,
                              windowWidth,
                              windowHeight);
        terrainMapWidthScale = Terrain.activeTerrain.terrainData.size.x / windowWidth;
        terrainMapHeightScale = Terrain.activeTerrain.terrainData.size.z / windowHeight;
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.M))
            isVisible = !isVisible;
    }

    void OnGUI() {
        if (isVisible)
            windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(windowId, windowRect, MainWindow, windowText));
    }

    public void MainWindow(int windowID){
        float pointX, pointY;
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {

            pointX = (GameManager.Instance.GetCharacter(_name).transform.position.x
                      / terrainMapWidthScale) - MAP_POINT_SIZE / 2;
            pointY = ((Terrain.activeTerrain.terrainData.size.z - GameManager.Instance.GetCharacter(_name).transform.position.z)
                      / terrainMapHeightScale) - MAP_POINT_SIZE / 2;

            if (GameManager.Instance.Me.Character.name.Equals(_name))
                GUI.Label(new Rect(pointX, pointY, MAP_POINT_SIZE, MAP_POINT_SIZE), _name);
            else if (GameManager.Instance.GetCharacter(_name).layer.Equals(VISIBLE_ENEMY_LAYER))
                GUI.Label(new Rect(pointX, pointY, MAP_POINT_SIZE, MAP_POINT_SIZE), _name);
        }
        GUI.DragWindow();
    }
}
