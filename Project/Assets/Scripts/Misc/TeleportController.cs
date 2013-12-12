using UnityEngine;
using System.Collections;

public class TeleportController : MonoBehaviour {

    private bool playerTeleported;
    //@TODO: Reset playerTeleported = false;
	void Awake () {
        playerTeleported = false;
	}

    void OnTriggerEnter(Collider other) {//@TODO: && RespawnTimer == 0
        if (!playerTeleported  && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            PlayerCharacterModel otherModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (GameManager.Instance.MyCharacter.name.Equals(otherModel.name)) {
                TeleportManager.Instance.TeleportMeToMainStage();
                playerTeleported = true;
            }
        }
    }
}