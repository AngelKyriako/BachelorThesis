using UnityEngine;
using System.Collections;

public class ExpRadiusSphere: MonoBehaviour {

    private const float TIME_TO_LIVE = 1f;

    private uint expWorth;
    private float timeLived;

    void Awake() {
        enabled = false;
    }

    public void SetUp(uint _exp) {
        expWorth = _exp;
        enabled = true;
    }

    void Start() {
        timeLived = 0f;
    }

    void Update() {
        if ((timeLived += Time.deltaTime) >= TIME_TO_LIVE)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) {
        Trigger(other);
    }

    void OnTriggerStay(Collider other) {
        Trigger(other);
    }

    private void Trigger(Collider other) {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            PlayerCharacterModel playerModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (playerModel && GameManager.Instance.MyCharacter.name.Equals(playerModel.name) && !playerModel.IsDead) {
                GameManager.Instance.MyCharacterModel.GainExp(expWorth);
                Destroy(gameObject);
            }
        }
    }
}
