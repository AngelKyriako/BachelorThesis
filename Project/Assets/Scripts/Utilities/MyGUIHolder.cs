using UnityEngine;
using System.Collections.Generic;

public class MyGUIHolder {

    private static MyGUIHolder instance = new MyGUIHolder();

    private MyGUIHolder() { }

    public static MyGUIHolder Instance {
        get { return instance;  }
    }

    public T RoomPropertyOptions<T>(ref EditProperty editingField, EditProperty targetedState,
                                    T selectedField, Dictionary<T, string> dictionary, int width) {
        if(!editingField.Equals(targetedState))
            if(GUILayout.Button(dictionary[selectedField], GUILayout.Width(width)))
                editingField = targetedState;

        if(editingField.Equals(targetedState))
            foreach(KeyValuePair<T, string> pair in dictionary) {
                if(GUILayout.Button(pair.Value, GUILayout.Width(width))) {
                    editingField = EditProperty.None;
                    selectedField = pair.Key;
                }
            }
        return selectedField;
    }

    public string RoomPropertyText() {
        return "";
    }

    public void RoomPropertySkillSet() {
    }
}