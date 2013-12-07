using UnityEngine;
using System.Collections;

public class Player {

    private string name;
    private PlayerTeam team;
    private PlayerColor color;

    public Player(string _name, PlayerTeam _team, PlayerColor _color) {
        name = _name;
        team = _team;
        color = _color;
	}

    #region Setters and Getters
    public string Name {
        get { return name; }
    }
    public PlayerTeam Team {
        get { return team; }
    }
    public PlayerColor Color {
        get { return color; }
    }
    #endregion
}
