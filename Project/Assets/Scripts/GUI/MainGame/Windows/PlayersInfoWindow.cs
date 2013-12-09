using UnityEngine;
using System.Collections;

public class PlayersInfoWindow: DraggableWindow {

    private const string TITLE = "";
    private const int ID = 8;
    private const int WIDTH = 220, HEIGHT = 300;
    private const KeyCode TOGGLE_BUTTON = KeyCode.P;

    private Rect characterTabRect;

    void Start() {
        SetUpGUI(TITLE, ID, Screen.width, 0, WIDTH, HEIGHT, true, TOGGLE_BUTTON);
        characterTabRect = new Rect(5, 5, WIDTH - 10, HEIGHT/10);
    }

    public override void MainWindow(int windowID) {
        base.MainWindow(windowID);
        int playerCount = 0;
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
            GUI.Label(new Rect(5, 5 + playerCount * (HEIGHT / 10), WIDTH - 10, HEIGHT / 10),
                      GameManager.Instance.GetPlayer(_name).name + ": (" +
                      GameManager.Instance.GetPlayerModel(_name).GetVital((int)VitalType.Health).CurrentValue + " / " +
                      GameManager.Instance.GetPlayerModel(_name).GetVital((int)VitalType.Health).FinalValue +
                      ") Kills " + GameManager.Instance.GetPlayerModel(_name).Kills);
            ++playerCount;
        }
    }
}
