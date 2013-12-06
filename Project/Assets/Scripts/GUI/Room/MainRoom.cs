using UnityEngine;
using System.Collections;

public enum PlayerTeam {
    None,
    Team1,
    Team2,
    Team3,
    Team4,
    Team5,
    Team6,
    Team7,
    Team8,
    Team9,
    Team10,
}

public enum PlayerColor {
    None,
    Red,
    Blue,
    Green,
    Purple,
    Yellow,
    Pink,
    Orange,
    Brown,
    Gray,
    Black
}

public class MainRoom: MonoBehaviour {

    private RoomNetController networkController;

	void Awake () {
        networkController = GetComponent<RoomNetController>();
	}
	
	void Update () {
	
	}

    void OnGUI() {
    }
}