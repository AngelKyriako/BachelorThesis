using UnityEngine;
using System.Collections;

public class TerrainMap: DraggableWindow {

    private const string TITLE = "Map";
    private const int ID = 10;
    private const int MAP_SIZE = 150;
    private const KeyCode TOGGLE_BUTTON = KeyCode.M;

    private const float MAP_POINT_SIZE = 20;
    private const int VISIBLE_ENEMY_LAYER = 9;
    
    private float terrainMapWidthScale, terrainMapHeightScale;

    void Start(){
        SetUpGUI(TITLE, ID, Screen.width, Screen.height, MAP_SIZE, MAP_SIZE, true, TOGGLE_BUTTON);
    }

    public void SetUpGUI(string _winTitle, int _winID, int _x, int _y, int _width, int _height, bool _isVisible, KeyCode _toggleButton) {
        base.SetUpGUI(_winTitle, _winID, _x, _y, _width, _height, _isVisible, _toggleButton);

        terrainMapWidthScale = Terrain.activeTerrain.terrainData.size.x / _width;
        terrainMapHeightScale = Terrain.activeTerrain.terrainData.size.z / _height;
    }

    public override void MainWindow(int windowID){
        base.MainWindow(windowID);

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
    }
}
