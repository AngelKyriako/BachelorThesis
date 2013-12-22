using UnityEngine;
using System;

public class GUIStatUpgradeButton: MonoBehaviour {

    public int statIndex;
    
    private bool busy = false;

    void OnEnable() {
        Utilities.Instance.Assert(statIndex >= 0 && statIndex < Enum.GetValues(typeof(StatType)).Length, "GUIStatUpgradeButton", "OnEnable", "Invalid stat index value");
    }

    void OnClick() {
        DFCharacterModel.Instance.UpdateStatButtonClick(statIndex);
    }
}