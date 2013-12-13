using UnityEngine;
using System.Collections;

public class PlayerCharacterDeathController: MonoBehaviour {

    private PlayerCharacterModel model;

    void Awake() {
        enabled = false;
    }

    public void Setup(bool isLocalPlayer) {
        if (isLocalPlayer)
            model = GameManager.Instance.MyCharacterModel;
    }

    void Update() {
        model.RespawnTimer -= Time.deltaTime;
        if (model.RespawnTimer == 0) {
            model.RefreshVitals();
            enabled = false;
        }
    }

    void OnGUI() {
        GUI.Label(new Rect((Screen.width / 2) - 30, 0, 60, 30), Utilities.Instance.TimeCounterDisplay(model.RespawnTimer));
    }

    public void Enable() {
        model.Died();
        ClearEffects();
        enabled = true;
    }

    private void ClearEffects() {
        BaseEffect[] effects = gameObject.GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            effects[i].Deactivate();
    }
}
