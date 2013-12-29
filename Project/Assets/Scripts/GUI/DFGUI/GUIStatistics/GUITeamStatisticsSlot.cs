using UnityEngine;
using System.Collections;

public class GUITeamStatisticsSlot : MonoBehaviour {

    private PlayerTeam team;
    private dfLabel killsLabel, deathsLabel, assistsLabel,
                    teamLabel;

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<dfPanel>() != null, "GUITeamStatisticsSlot", "Awake", "dfPanel must be a component of the game object");
        enabled = false;
    }
    //@TODO: delete the ifs when assists are added to the game
    public void SetUp(PlayerTeam _team) {
        team = _team;

        dfControl control = gameObject.GetComponent<dfControl>();
        control.Color = GameOverManager.Instance.WinningTeam == _team ? Color.green : Color.red;

        dfLabel nameLabel = control.Find<dfLabel>("TeamName");
        nameLabel.Text = DFCharacterModel.Instance.TeamName(team);

        killsLabel = control.Find<dfLabel>("Kills");
        if (killsLabel)
            killsLabel.Text = DFCharacterModel.Instance.TeamKills(team);

        deathsLabel = control.Find<dfLabel>("Deaths");
        if (deathsLabel)
            deathsLabel.Text = DFCharacterModel.Instance.TeamDeaths(team);

        assistsLabel = control.Find<dfLabel>("Assists");
        if (assistsLabel)
            assistsLabel.Text = DFCharacterModel.Instance.TeamAssists(team);

        enabled = true;
    }
}
