using UnityEngine;
using System.Collections;

public class PlayerCharacterDeathController: MonoBehaviour {

    private PlayerCharacterModel model;
    private CameraController camera;

    void Awake() {
        enabled = false;
    }

    public void Setup(bool isLocalPlayer) {
        if (isLocalPlayer) {
            model = GameManager.Instance.MyCharacterModel;
            camera = gameObject.GetComponent<CameraController>();
        }
    }

    void Update() {
        model.RespawnTimer -= Time.deltaTime;
        if (model.RespawnTimer == 0)
            enabled = false;
    }

    void OnGUI() {
        //@TODO: respawn time counter !!!
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
