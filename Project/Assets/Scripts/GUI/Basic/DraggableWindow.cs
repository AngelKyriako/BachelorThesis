using UnityEngine;
using System.Collections;

public class DraggableWindow: ToggledWindow {

    public override void MainWindow(int windowID) {
        GUI.DragWindow();
    }
}
