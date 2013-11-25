using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterInfoPanel: SingletonMono<CharacterInfoPanel> {

    #region Gui constants
    private const int MAIN_X = 300, MAIN_HEIGHT = 125, MAIN_WIDTH = 300;
#endregion

    #region Model attributes
    private PlayerCharacterModel playerCharModel;
    private bool[] skillButtonPressed;
    #endregion

    #region Gui attributes
    public Rect layoutRect;
    public GUIStyle vitalBar;
#endregion

    private CharacterInfoPanel() { }

    void Awake() {
        enabled = false;
    }

	void Start () {
        playerCharModel = GameManager.Instance.MyCharacterModel;
        layoutRect = new Rect(MAIN_X, Screen.height - MAIN_HEIGHT, MAIN_WIDTH, MAIN_HEIGHT);

        skillButtonPressed = new bool[Enum.GetNames(typeof(CharacterSkillSlot)).Length];
	}
	
	void Update () {
        
	}

    void OnGUI() {
        GUILayout.BeginArea(layoutRect);
        SkillsPanel();
        VitalsPanel();
        GUILayout.EndArea();
    }

    private void SkillsPanel() {
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        for (int i = 0; i < playerCharModel.SkillCount; ++i) {
            skillButtonPressed[i] = playerCharModel.GetSkill(i) != null
                                    && GUILayout.Button(playerCharModel.GetSkill(i).Title, GUILayout.Width(50), GUILayout.Height(50));
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }

    private void VitalsPanel() {
        for (int i = 0; i < playerCharModel.VitalsLength; ++i)
            GUILayout.Label(playerCharModel.GetVital(i).Name + " (" + playerCharModel.GetVital(i).CurrentValue + "/" +
                                                                      playerCharModel.GetVital(i).FinalValue + ")", vitalBar);
    }

    public bool IsSkillButtonPressed(int _index) {
        return skillButtonPressed[_index];
    }

}
