using UnityEngine;
using System.Collections.Generic;

public class GameOverManager: MonoBehaviour {

    public Vector3 winnerSpot = Vector3.zero;
    public int winnerSpotVariance = 5;

    void Awake () {
        winnerSpot += new Vector3(Random.Range(-winnerSpotVariance, winnerSpotVariance),
                                  0,
                                  Random.Range(-winnerSpotVariance, winnerSpotVariance));
        enabled = false;
	}

    void SetUp(string _winnerId) {
        foreach (string _id in GameManager.Instance.AllPlayerKeys)
            if (CombatManager.Instance.IsAlly(_winnerId))
                TeleportManager.Instance.TeleportToPoint(winnerSpot);
        enabled = true;
    }

    void OnGUI() {
        GUILayout.Button("Game Over");
        for(int i=0; i<GameManager.Instance.TeamsCount; ++i )
            GUILayout.Button((PlayerTeam)i + ": " + GameManager.Instance.TeamKills(i) + " kills");
    }
}
