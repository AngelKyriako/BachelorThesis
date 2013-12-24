using UnityEngine;
using System.Collections;

public class PlayerCharacterDeathController: MonoBehaviour {

    void Awake() {
        enabled = false;
    }

    void Update() {
        GameManager.Instance.MyCharacterModel.RespawnTimer -= Time.deltaTime;
        if (GameManager.Instance.MyCharacterModel.RespawnTimer == 0) {
            GameManager.Instance.MyCharacterModel.VitalsToFull();
            enabled = false;
        }
    }

    void OnGUI() {
        GUI.Label(new Rect((Screen.width / 2) - 30, 0, 60, 30), Utilities.Instance.TimeCounterDisplay(GameManager.Instance.MyCharacterModel.RespawnTimer));
    }

    public void Enable() {
        ClearEffects();
        GameManager.Instance.MyCharacterModel.Died();
        enabled = true;
    }

    private void ClearEffects() {
        BaseEffect[] effects = gameObject.GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            effects[i].Deactivate();
    }
}
