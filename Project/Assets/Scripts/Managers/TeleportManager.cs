using UnityEngine;
using System.Collections.Generic;

public class TeleportManager {

    private Vector3 stageSpawnPoint, heavenSpawnPoint;
    private float distanceToHeaven;

    private static TeleportManager instance;
    public static TeleportManager Instance {
        get {
            if (instance != null)
                return instance;
            else
                return (instance = new TeleportManager());
        }
    }

    private TeleportManager() {
        PlayerTeam myTeam = (PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"];
        heavenSpawnPoint = GameObject.Find("Heaven/SpawnPointArea/SpawnPoint" + (int)myTeam).transform.position;
        stageSpawnPoint = GameObject.Find("Terrain/SpawnPointArea/SpawnPoint" + (int)myTeam).transform.position;
        distanceToHeaven = heavenSpawnPoint.y - stageSpawnPoint.y;
    }

    public void TeleportToPoint(Vector3 _spawnPoint) {
        GameManager.Instance.MyCharacter.transform.position = _spawnPoint;
        GameManager.Instance.MyCharacter.transform.rotation = Quaternion.identity;
        GameManager.Instance.MyMovementController.StandStillBitch();
    }

    public void StandardTeleportation(bool teleportToStage) {
        if (teleportToStage)
            TeleportMeToMainStage();
        else
            TeleportMeToHeaven();
    }

    private void TeleportMeToHeaven() {
        TeleportToPoint(heavenSpawnPoint);
        GameManager.Instance.MyCameraController.EnterHeavenMode();
    }

    private void TeleportMeToMainStage() {
        TeleportToPoint(stageSpawnPoint);
        GameManager.Instance.MyCameraController.EnterMainStageMode();
    }
}
