using UnityEngine;
using System.Collections;

public class GameOverManager: MonoBehaviour {

	void Awake () {
	
	}
	
	void Update () {
	
	}

    void OnGUI() {
        GUILayout.Button("Game Over");
        for(int i=0; i<GameManager.Instance.TeamsCount; ++i )
            GUILayout.Button((PlayerTeam)i + ": " + GameManager.Instance.TeamKills(i) + " kills");
    }
}
