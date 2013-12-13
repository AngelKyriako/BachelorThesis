using UnityEngine;
using System.Collections;

public class TeleportationToPoint: BaseSkillController {

    private Vector3 spawnPointToTeleport;

    public override void OnTriggerEnter(Collider other) {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            PlayerCharacterModel playerModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            Teleport(playerModel);
        }
    }

    private void Teleport(PlayerCharacterModel _playerModel) {
        if (CombatManager.Instance.AreAllies(_playerModel.name, CasterModel.name) && !_playerModel.IsDead)
            TeleportManager.Instance.TeleportToPoint(spawnPointToTeleport);
    }

    public Vector3 SpawnPointToTeleport {
        get { return spawnPointToTeleport; }
        set { spawnPointToTeleport = value; }
    }
}
