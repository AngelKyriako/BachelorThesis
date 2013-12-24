using UnityEngine;
using System.Collections;

public class ExpRadiusSphere: MonoBehaviour {

    private const float TIME_TO_LIVE = 3f;

    private uint expWorth;
    private float timeStart;
    private bool b = true;//DELETE THIS
    void Awake() {
        enabled = false;
    }

    public void SetUp(uint _exp) {
        expWorth = _exp;
        enabled = true;
    }

    void Start() {
        timeStart = Time.time;
    }

    void Update() {
        if ((Time.time - timeStart) >= TIME_TO_LIVE)
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
            if (b) {
                GameManager.Instance.LogMessageToMasterClient("exp collided with: " + playerModel.name + ", myname: " + GameManager.Instance.MyCharacter.name);
                b = false;
            }
            if (playerModel && GameManager.Instance.ItsMe(playerModel.name) && !playerModel.IsDead) {
                GameManager.Instance.MyCharacterModel.GainExp(expWorth);
                Destroy(gameObject);
            }
        }
    }

}
