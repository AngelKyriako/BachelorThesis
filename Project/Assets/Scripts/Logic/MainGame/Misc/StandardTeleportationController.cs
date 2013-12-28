using UnityEngine;
using System.Collections;

public class StandardTeleportationController : MonoBehaviour {

    public bool isHeavenPortal = true;

    void OnTriggerEnter(Collider other) {
        Trigger(other);
    }

    void OnTriggerStay(Collider other) {
        Trigger(other);
    }

    private void Trigger(Collider other){
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            PlayerCharacterModel playerModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (GameManager.Instance.MyCharacter.name.Equals(playerModel.name) && !playerModel.IsDead)
                TeleportManager.Instance.StandardTeleportation(isHeavenPortal);
        }
    }
}