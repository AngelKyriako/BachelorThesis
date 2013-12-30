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

    public void Enable(string _killer, string _dead, Vector3 _deathPoint) {
        ClearEffects();
        CombatManager.Instance.BroadCastPlayerKill(_killer, _dead, _deathPoint);
        TeleportManager.Instance.StandardTeleportation(false);

        enabled = true;
    }

    public void MakeAllPlayersVisible() {
        foreach (string _name in GameManager.Instance.AllPlayerKeys)
            if (!CombatManager.Instance.IsAlly(_name))
                Utilities.Instance.SetGameObjectLayer(GameManager.Instance.GetCharacter(_name), 8);
    }

    private void ClearEffects() {
        BaseEffect[] effects = gameObject.GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            effects[i].Deactivate();
    }
}
