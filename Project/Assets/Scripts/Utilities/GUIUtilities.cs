using UnityEngine;
using System.Collections.Generic;

public class GUIUtilities {

    private Rect fullScreenRect;    

    private static GUIUtilities instance = new GUIUtilities();
    public static GUIUtilities Instance {
        get { return instance; }
    }

    private GUIUtilities() {
        fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);
    }


    public KeyValuePair<string, T> ButtonOptions<T, K>(ref K editingField, K targetedState, KeyValuePair<string, T> selectedPair,
                                                       Dictionary<string, T> availablePairs, int width) {

        if (!editingField.Equals(targetedState) &&
            GUILayout.Button(selectedPair.Key, GUILayout.Width(width)))
            editingField = targetedState;

        if (editingField.Equals(targetedState))
            foreach (KeyValuePair<string, T> pair in availablePairs) {
                if (GUILayout.Button(pair.Key, GUILayout.Width(width))) {
                    editingField = default(K);
                    selectedPair = pair;
                }
            }
        return selectedPair;
    }

    public string ToggleTextButton(string _current, string _first, string _second) {
        if (GUILayout.Button(_current)) {
            if (_current.Equals(_first))
                _current = _second;
            else
                _current = _first;
        }
        return _current;
    }

    public Rect ClampToScreen(Rect r) {
        r.x = Mathf.Clamp(r.x, 0, Screen.width - r.width);
        r.y = Mathf.Clamp(r.y, 0, Screen.height - r.height);
        return r;
    }

    public Rect FullScreenRect {
        get { return fullScreenRect; }
    }
}