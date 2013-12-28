using UnityEngine;
using System.Collections.Generic;

public class GameOverManager: SingletonMono<GameOverManager> {    

    private GameOverManager() { }

    private PlayerTeam winningTeam;

    void Awake () {
        enabled = false;
	}

    void OnLevelWasLoaded(int level) {
        if (level == 4) {
            Vector3 winSpawnPoint = GameObject.Find("LosersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyTeam).transform.position;
            Vector3 loseSpawnPoint = GameObject.Find("WinnersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyTeam).transform.position;

            if (GameManager.Instance.IsMyTeam(winningTeam))
                TeleportManager.Instance.TeleportToPoint(winSpawnPoint);
            else
                TeleportManager.Instance.TeleportToPoint(loseSpawnPoint);

            GameManager.Instance.MyCharacter.transform.LookAt(Camera.main.transform);            
        }
    }

    public void SetUp(PlayerTeam _winningTeam) {
        winningTeam = _winningTeam;

        GameManager.Instance.MyCharacterModel.IsSilenced = true;
        GameManager.Instance.MyCharacterModel.IsStunned = true;
        enabled = true;
    }

    void OnGUI() {
        GUILayout.Button("Game Over");
        for(int i=0; i<GameManager.Instance.TeamsCount; ++i )
            if (GameManager.Instance.TeamKills(i) > 0)
                GUILayout.Button((PlayerTeam)i + ": " + GameManager.Instance.TeamKills(i) + " kills");
    }
}
