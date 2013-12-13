using UnityEngine;
using System.Collections.Generic;

public class CharacterInfoPanel: SingletonMono<CharacterInfoPanel> {

    #region Gui constants
    private const int MAIN_X = 350, MAIN_HEIGHT = 150, MAIN_WIDTH = 250;
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

    void Start() {
        playerCharModel = GameManager.Instance.MyCharacterModel;
        layoutRect = new Rect((Screen.width - MAIN_WIDTH) / 2, Screen.height - MAIN_HEIGHT, MAIN_WIDTH, MAIN_HEIGHT);

        skillButtonPressed = new Dictionary<CharacterSkillSlot, bool>() {
            { CharacterSkillSlot.Q, false },
            { CharacterSkillSlot.W, false },
            { CharacterSkillSlot.E, false },
            { CharacterSkillSlot.R, false },
        };

    }

    void OnGUI() {
        GUILayout.BeginArea(layoutRect);
        SkillsPanel();
        VitalsPanel();
        LevelBarPanel();
        GUILayout.EndArea();
    }

    private void VitalsPanel() {
        for (int i = 0; i < playerCharModel.VitalsLength; ++i)
            GUILayout.Label(playerCharModel.GetVital(i).ToString());
    }

    private void SkillsPanel() {
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        CharacterSkillSlot _key = CharacterSkillSlot.Q;
        for (int i = 1; i < playerCharModel.SkillSlotsLength; _key = (CharacterSkillSlot)(++i)) {
            skillButtonPressed[_key] = playerCharModel.SkillExists(_key) &&
                                       GUILayout.Button(/*playerCharModel.GetSkill(_key).Icon, */Utilities.Instance.TimeCounterDisplay(playerCharModel.GetSkill(_key).CoolDownTimer), GUILayout.Width(48), GUILayout.Height(48));
            //GUILayout.Label(/*playerCharModel.GetSkill(_key).Icon, */((int)playerCharModel.GetSkill(_key).CoolDownTimer).ToString(), GUILayout.Width(48), GUILayout.Height(48));
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }

    private void LevelBarPanel() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level: "+playerCharModel.Level + " ");
        GUILayout.Label("(" + playerCharModel.CurrentExp + "/" + playerCharModel.ExpToLevel + ")");
        GUILayout.EndHorizontal();
    }

    public bool IsSkillButtonPressed(CharacterSkillSlot _key) {
        return skillButtonPressed[_key];
    }
}