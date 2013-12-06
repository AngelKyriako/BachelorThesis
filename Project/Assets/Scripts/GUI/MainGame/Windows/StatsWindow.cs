using UnityEngine;
using System.Collections;

public class StatsWindow: DraggableWindow {

    private const string TITLE = "Stats";
    private const int ID = 1;
    private const int WIDTH = 150, HEIGHT = 200;
    private const KeyCode TOGGLE_BUTTON = KeyCode.C;

    void Start() {
        SetUpGUI(TITLE, ID, (Screen.width-WIDTH) / 2, Screen.height / 2 + HEIGHT / 2, WIDTH, HEIGHT, true, TOGGLE_BUTTON);
    }

    public override void MainWindow(int windowID) {
        base.MainWindow(windowID);
    }
}