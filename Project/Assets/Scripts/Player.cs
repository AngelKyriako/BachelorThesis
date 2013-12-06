using UnityEngine;
using System.Collections;

public class Player {

    private string name;
    private PlayerTeam team;
    private PlayerColor color;

    private static Player instance = new Player();

    private Player() {
        name = PlayerPrefs.GetString("name");
        team = PlayerTeam.None;
        color = PlayerColor.None;
	}

    public static Player Instance {
        get { return Player.instance; }
    }

#region Setters and Getters
    public string Name {
        get { return name; }
    }
    public PlayerTeam Team {
        get { return team; }
        set { team = value; }
    }
    public PlayerColor Color {
        get { return color; }
        set { color = value; }
    }
#endregion
}
