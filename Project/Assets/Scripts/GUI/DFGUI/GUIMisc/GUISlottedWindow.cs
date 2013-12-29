using UnityEngine;
using System.Collections;

public abstract class GUISlottedWindow : MonoBehaviour {

    public dfPanel slot;

    void Start() {
        Utilities.Instance.Assert(slot != null, "GUISlottedWindow", "Start", "slot not set");
        Utilities.Instance.Assert(gameObject.GetComponent<dfScrollPanel>() != null, "GUISlottedWindow", "Start", "dfScrollPanel must be a component of the game object");
        
        SetUpWindow();
    }

    public abstract void SetUpWindow();
}
