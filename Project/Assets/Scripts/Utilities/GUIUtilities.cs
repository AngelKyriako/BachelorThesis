using UnityEngine;
using System.Collections.Generic;

public class GUIUtilities {

    private static GUIUtilities instance = new GUIUtilities();

    private GUIUtilities() { }

    public static GUIUtilities Instance {
        get { return instance; }
    }

    public KeyValuePair<string, T> ButtonOptions<T, K>(ref K editingField, K targetedState, KeyValuePair<string, T> selectedPair,
                                                       Dictionary<string, T> availablePairs, int width) {

        if (editingField.Equals(default(K)) &&
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

    public void RoomPropertySkillSet() {
    }
}