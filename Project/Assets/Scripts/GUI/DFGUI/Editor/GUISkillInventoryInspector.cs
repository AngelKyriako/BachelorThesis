/* Copyright 2013 Daikon Forge */
using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GUISkillInventory))]
public class GUISkillInventoryInspector: Editor {

    public override void OnInspectorGUI() {
        InspectGUISkillInventory();
    }

    private void InspectGUISkillInventory() {
        var control = target as GUISkillInventory;

        var skillList = GetSkillList();
        var assignedIndex = Array.IndexOf(skillList, control.Id);
        var index = EditorGUILayout.IntPopup(assignedIndex, GetSkillStringList(), skillList);
        if (index != assignedIndex) {
            dfEditorUtil.MarkUndo(control, "Assign skill");
            control.Id = skillList[index];
        }
    }

    private int[] GetSkillList() {
        return SkillBook.Instance.AllSkillsKeys.ToArray();
    }

    private string[] GetSkillStringList() {
        string[] skillList = new string[SkillBook.Instance.AllSkillsKeys.Count];
        for (int i = 0; i < GetSkillList().Length; ++i) {
            skillList[i] = DFSkillModel.Instance.Title(i);
        }
        return skillList;
    }
}