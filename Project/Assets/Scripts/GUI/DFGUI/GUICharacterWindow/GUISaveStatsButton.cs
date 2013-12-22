using UnityEngine;
using System.Collections;

public class GUISaveStatsButton : MonoBehaviour {
    void OnClick() {
        DFCharacterModel.Instance.SaveUpdatedStats();
    }
}
