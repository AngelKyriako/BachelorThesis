using UnityEngine;

public class GUIGamePreferences: MonoBehaviour {

    private const KeyCode TOGGLE_BUTTON = KeyCode.G;

    void Start() {
        gameObject.GetComponent<dfPanel>().IsVisible = false;
    }

	void Update () {
        if (Input.GetKeyUp(TOGGLE_BUTTON))
            gameObject.GetComponent<dfPanel>().IsVisible = !gameObject.GetComponent<dfPanel>().IsVisible;
	}
}
