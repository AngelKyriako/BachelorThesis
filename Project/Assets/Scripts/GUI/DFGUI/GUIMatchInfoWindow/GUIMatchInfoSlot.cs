using UnityEngine;
using System.Collections;

public class GUIMatchInfoSlot : MonoBehaviour {

    private string playerId;
    private dfLabel kdaLabel, teamLabel;

    void Awake() {        
        enabled = false;
    }

    public void SetUp(string _playerId) {
        playerId = _playerId;

        dfControl control = gameObject.GetComponent<dfControl>();
        control.GetComponent<dfPanel>().Color = DFCharacterModel.Instance.PlayerInfoColor(playerId);

        dfLabel nameLabel = control.Find<dfLabel>("Name");
        nameLabel.Text = DFCharacterModel.Instance.PlayerInfoName(playerId);        

        kdaLabel = control.Find<dfLabel>("K_D_A");
        kdaLabel.Text = DFCharacterModel.Instance.PlayerInfoCounters(playerId);

        teamLabel = control.Find<dfLabel>("Team");
        teamLabel.Text = DFCharacterModel.Instance.PlayerInfoTeam(playerId);

        enabled = true;
    }

	void Start () {
	    
	}
	
	void Update () {
        kdaLabel.Text = DFCharacterModel.Instance.PlayerInfoCounters(playerId);
        teamLabel.Text = DFCharacterModel.Instance.PlayerInfoTeam(playerId);
	}
}