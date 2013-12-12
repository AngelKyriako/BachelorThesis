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

    public void TeleportMeToHeaven() {
        TeleportMe(heavenSpawnPoint);
        GameManager.Instance.MyCameraController.EnterHeavenMode();
    }

    public void TeleportMeToMainStage() {
        TeleportMe(stageSpawnPoint);
        GameManager.Instance.MyCameraController.EnterMainStageMode();
    }

    private void TeleportMe(Vector3 _spawnPoint) {
        GameManager.Instance.MyCharacter.transform.position = _spawnPoint;
        GameManager.Instance.MyCharacter.transform.rotation = Quaternion.identity;
        GameManager.Instance.MyMovementController.StandStillBitch();
    }
}
