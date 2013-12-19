/* Copyright 2013 Daikon Forge */
using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GUISkill))]
public class GUISkillInspector: Editor {

    public override void OnInspectorGUI() {
        InspectGUISkill();
    }

    private void InspectGUISkill() {
        var control = target as GUISkill;

        var assignable = EditorGUILayout.Toggle("Is Action Slot", control.IsActionSlot);
        if (assignable != control.IsActionSlot) {
            dfEditorUtil.MarkUndo(control, "Change Action Slot flag");
            control.IsActionSlot = assignable;
        }

        //@TODO: fix bug here
        if (assignable) {
            var slotList = GetSlotList();
            var assignedIndex = Array.IndexOf(slotList, control.Slot);
            var index = EditorGUILayout.Popup("Slot Button", assignedIndex, slotList);
            if (index != assignedIndex) {
                dfEditorUtil.MarkUndo(control, "Assign slot");
                control.Slot = (CharacterSkillSlot)index;
            }
        }
        control.Id = 0;
    }

    private string[] GetSlotList() {
        return Enum.GetNames(typeof(CharacterSkillSlot)).ToArray();
    }
}