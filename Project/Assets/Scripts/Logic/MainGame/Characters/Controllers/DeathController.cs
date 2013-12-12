using UnityEngine;
using System.Collections;

public class DeathController: MonoBehaviour {

    private BaseCharacterModel model;

    void Awake() {
        model = GetComponent<BaseCharacterModel>();
        enabled = false;
    }

    public void Enable(bool isMasterClient) {
        enabled = isMasterClient;
    }
    public void Disable() {
        enabled = false;
    }
}
