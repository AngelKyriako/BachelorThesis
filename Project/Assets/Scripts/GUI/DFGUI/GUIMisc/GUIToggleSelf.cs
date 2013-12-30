using UnityEngine;
using System.Collections;

public class GUIToggleSelf : MonoBehaviour {

    public KeyCode key;

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<dfControl>(), "GUIToggleSelf", "Awake", "dfControl must be component of the gameObject.");
    }

	void Update () {
        if (Input.GetKeyUp(key))
            gameObject.GetComponent<dfControl>().IsVisible = !gameObject.GetComponent<dfControl>().IsVisible;
	}
}
