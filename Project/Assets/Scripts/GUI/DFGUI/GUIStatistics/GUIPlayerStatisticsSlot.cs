using UnityEngine;
using System.Collections;

public class GUIPlayerStatisticsSlot: MonoBehaviour {

    private string playerId;
    private dfLabel killsLabel, deathsLabel, assistsLabel,
                    teamLabel;

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<dfPanel>() != null, "GUIPlayerStatisticsSlot", "Awake", "dfPanel must be a component of the game object");
        enabled = false;
    }
    //@TODO: delete the ifs when assists are added to the game
    public void SetUp(string _playerId) {
        playerId = _playerId;

        dfControl control = gameObject.GetComponent<dfControl>();
        control.Color = DFCharacterModel.Instance.PlayerInfoColor(playerId);

        dfLabel nameLabel = control.Find<dfLabel>("PlayerName");
        if (nameLabel)
            nameLabel.Text = DFCharacterModel.Instance.PlayerInfoName(playerId);

        killsLabel = control.Find<dfLabel>("KillsValue");
        if (killsLabel)
            killsLabel.Text = DFCharacterModel.Instance.PlayerKills(playerId);

        deathsLabel = control.Find<dfLabel>("DeathsValue");
        if (deathsLabel)
            deathsLabel.Text = DFCharacterModel.Instance.PlayerDeaths(playerId);

        assistsLabel = control.Find<dfLabel>("AssistsValue");
        if (assistsLabel)
            assistsLabel.Text = DFCharacterModel.Instance.PlayerAssists(playerId);

        teamLabel = control.Find<dfLabel>("Team");
        if (teamLabel)
            teamLabel.Text = DFCharacterModel.Instance.PlayerTeam(playerId);

        enabled = true;
    }
	
	void Update () {
        if (killsLabel)
            killsLabel.Text = DFCharacterModel.Instance.PlayerKills(playerId);
        if (deathsLabel)
            deathsLabel.Text = DFCharacterModel.Instance.PlayerDeaths(playerId);
        if (assistsLabel)
            assistsLabel.Text = DFCharacterModel.Instance.PlayerAssists(playerId);      
	}
}