using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterInfoPanel: SingletonMono<CharacterInfoPanel> {

    #region Gui constants
    private const int MAIN_X = 350, MAIN_HEIGHT = 125, MAIN_WIDTH = 300;
#endregion

    #region Model attributes
    private PlayerCharacterModel playerCharModel;
    private Dictionary<CharacterSkillSlot, bool> skillButtonPressed;
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

        skillButtonPressed = new Dictionary<CharacterSkillSlot, bool>() {
            { CharacterSkillSlot.Q, false },
            { CharacterSkillSlot.W, false },
            { CharacterSkillSlot.E, false },
            { CharacterSkillSlot.R, false },
        };

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
        foreach (CharacterSkillSlot _key in playerCharModel.AllSkillKeys)
            skillButtonPressed[_key] = playerCharModel.SkillExists(_key) &&
                                       GUILayout.Button(/*playerCharModel.GetSkill(i).Icon, */((int)playerCharModel.GetSkill(_key).CoolDownTimer).ToString(), GUILayout.Width(48), GUILayout.Height(48));
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }

    private void VitalsPanel() {
        for (int i = 0; i < playerCharModel.VitalsLength; ++i)
            GUILayout.Label(playerCharModel.GetVital(i).Name + " (" + playerCharModel.GetVital(i).CurrentValue + "/" +
                                                                      playerCharModel.GetVital(i).FinalValue + ")", vitalBar);
    }

    public bool IsSkillButtonPressed(CharacterSkillSlot _key) {
        return skillButtonPressed[_key];
    }

}
