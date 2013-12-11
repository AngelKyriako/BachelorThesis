using UnityEngine;
using System.Collections;

public class PlayerCharacterDeathController: MonoBehaviour {

    public Vector3 deathSpawnPoint;

    private PlayerCharacterModel model;
    private CameraController camera;

    void Awake() {
        Utilities.Instance.Assert(deathSpawnPoint != null, "PlayerCharacterDeathController", "Awake", "Death spawn point must have a value");
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

    public void Enable() {
        model.Died();
        ClearEffects();
        TeleportToAfterLife();
        camera.transform.position.Set(50, 115, 30);
        enabled = true;
    }

    private void ClearEffects() {
        BaseEffect[] effects = gameObject.GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            effects[i].Deactivate();
    }

    private void TeleportToAfterLife() {
        transform.position = deathSpawnPoint;
    }
}
