using UnityEngine;
using System.Collections;

public class PlayersInfoWindow: DraggableWindow {

    private const string TITLE = "";
    private const int ID = 8;
    private const int WIDTH = 300, SINGLE_SLOT_HEIGHT = 50;
    private const KeyCode TOGGLE_BUTTON = KeyCode.P;

    private Rect characterTabRect;

    void Start() {
        SetUpGUI(TITLE, ID, Screen.width, 0, WIDTH, SINGLE_SLOT_HEIGHT * GameManager.Instance.AllPlayerKeys.Count, true, TOGGLE_BUTTON);
        characterTabRect = new Rect(5, 0, WIDTH - 10, SINGLE_SLOT_HEIGHT);
    }

    public override void MainWindow(int windowID) {
        base.MainWindow(windowID);
        characterTabRect.y = 0;
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
            GUI.Label(characterTabRect,
                      GameManager.Instance.GetPlayer(_name).name + " "+
                      ((PlayerColor)GameManager.Instance.GetPlayer(_name).customProperties["Color"]).ToString() +
                      "(" + GameManager.Instance.GetPlayerModel(_name).GetVital((int)VitalType.Health).ToString()+ " " +
                      "Kills: " + GameManager.Instance.GetPlayerModel(_name).Kills+", "+
                      ((PlayerTeam)GameManager.Instance.GetPlayer(_name).customProperties["Team"]).ToString());

            characterTabRect.y += SINGLE_SLOT_HEIGHT;
        }
    }
}
