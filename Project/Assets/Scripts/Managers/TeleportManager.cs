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
        Camera.main.transform.position.Set(Camera.main.transform.position.x, Camera.main.transform.position.y + distanceToHeaven, Camera.main.transform.position.z);
    }

    public void TeleportMeToStage() {
        GameManager.Instance.MyCharacter.transform.position = stageSpawnPoint;
        GameManager.Instance.MyCharacter.transform.rotation = Quaternion.identity;
        Camera.main.transform.position.Set(Camera.main.transform.position.x, Camera.main.transform.position.y - distanceToHeaven, Camera.main.transform.position.z);
    }

    //public Vector3 HeavenSpawnPoint {
    //    get { return heavenSpawnPoint; }
    //}
    
    //public Vector3 StageSpawnPoint {
    //    get { return stageSpawnPoint; }
    //}
}
