using UnityEngine;
using System.Collections;

public class GUIMatchInfoSlot : MonoBehaviour {

    private string playerId;

    void Awake() {
        enabled = false;
    }
    //
    public void SetUp(string _playerName) {
        playerId = _playerName;
        enabled = true;
    }

	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

//    public override void MainWindow(int windowID) {
//        base.MainWindow(windowID);
//        characterTabRect.y = 0;
//        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
//            GUI.Label(characterTabRect,
//                      GameManager.Instance.GetPlayer(_name).name + " "+
//                      ((PlayerColor)GameManager.Instance.GetPlayer(_name).customProperties["Color"]).ToString() +
//                      GameManager.Instance.GetPlayerModel(_name).GetVital((int)VitalType.Health).ToString()+ " " +
//                      "Kills: " + GameManager.Instance.GetPlayerModel(_name).Kills+", "+
//                      "Deaths: " + GameManager.Instance.GetPlayerModel(_name).Deaths + ", " +
//                      ((PlayerTeam)GameManager.Instance.GetPlayer(_name).customProperties["Team"]).ToString());

//            characterTabRect.y += SINGLE_SLOT_HEIGHT;
//        }
//    }