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
            Vector3 winSpawnPoint = GameObject.Find("LosersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyPlayerColor).transform.position;
            Vector3 loseSpawnPoint = GameObject.Find("WinnersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyPlayerColor).transform.position;

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

    public PlayerTeam WinningTeam {
        get { return winningTeam; }
        set { winningTeam = value; }
    }
}
