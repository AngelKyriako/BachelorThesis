using UnityEngine;
using System.Collections.Generic;

public class GUIMatchInfoWindow : MonoBehaviour {

    public dfPanel infoSlot;

    private const KeyCode TOGGLE_BUTTON = KeyCode.I;

	void Start () {        
        dfPanel _nextSlot;

        //I want the local player's score to be first
        _nextSlot = (dfPanel)Instantiate(infoSlot);
        _nextSlot.gameObject.GetComponent<GUIMatchInfoSlot>().SetUp(GameManager.Instance.MyCharacter.name);

        gameObject.GetComponent<dfPanel>().AddControl(_nextSlot.GetComponent<dfPanel>());
        //Then the rest of the players
        foreach (string _name in GameManager.Instance.AllPlayerKeys)
            if (!GameManager.Instance.ItsMe(_name)) {
                _nextSlot = (dfPanel)Instantiate(infoSlot);
                _nextSlot.gameObject.GetComponent<GUIMatchInfoSlot>().SetUp(_name);

                gameObject.GetComponent<dfPanel>().AddControl(_nextSlot.GetComponent<dfPanel>());
            }
	}
	
	void Update () {
        if (Input.GetKeyUp(TOGGLE_BUTTON))
            gameObject.GetComponent<dfPanel>().IsVisible = !gameObject.GetComponent<dfPanel>().IsVisible;
	}
}