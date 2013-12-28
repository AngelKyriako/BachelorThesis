using UnityEngine;
using System.Collections;

public class GUIPreferenceSlot: MonoBehaviour {

    public int type;

    public enum PreferenceType{
        Title,
        Mode,
        Map,
        Difficulty,
        TargetKills,
    }

	void Start () {
        dfControl control = gameObject.GetComponent<dfControl>();
        dfLabel nameLabel = control.Find<dfLabel>("Name");
        dfLabel valueLabel = control.Find<dfLabel>("Value");
        nameLabel.Color = valueLabel.Color = GameManager.Instance.MyColor;

        switch ((PreferenceType)type) {
            case PreferenceType.Title:
                nameLabel.Text = "Room:";
                valueLabel.Text = GameVariables.Instance.Title;
                break;
            case PreferenceType.Mode:
                nameLabel.Text = "Mode:";
                valueLabel.Text = GameVariables.Instance.Mode.Key;
                break;
            case PreferenceType.Map:
                nameLabel.Text = "Map:";
                valueLabel.Text = GameVariables.Instance.Map.Key;
                break;
            case PreferenceType.Difficulty:
                nameLabel.Text = "Difficulty:";
                valueLabel.Text = GameVariables.Instance.Difficulty.Key;
                break;
            case PreferenceType.TargetKills:
                nameLabel.Text = "Target Kills:";
                valueLabel.Text = GameVariables.Instance.TargetKills.Key;
                break;
        }
	}
}
