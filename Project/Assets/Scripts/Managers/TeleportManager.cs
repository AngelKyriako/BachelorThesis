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
        GameManager.Instance.MyCharacter.transform.position = heavenSpawnPoint;
        GameManager.Instance.MyCharacter.transform.rotation = Quaternion.identity;
        GameManager.Instance.MyCameraController.EnterHeavenMode();
    }

    public void TeleportMeToStage() {
        GameManager.Instance.MyCharacter.transform.position = stageSpawnPoint;
        GameManager.Instance.MyCharacter.transform.rotation = Quaternion.identity;
        GameManager.Instance.MyCameraController.EnterStageMode();
    }

    //public Vector3 HeavenSpawnPoint {
    //    get { return heavenSpawnPoint; }
    //}
    
    //public Vector3 StageSpawnPoint {
    //    get { return stageSpawnPoint; }
    //}
}
