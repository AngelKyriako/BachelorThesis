using UnityEngine;
using System.Collections;

public enum PlayerTeam{
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

public enum PlayerColor{
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
