using UnityEngine;
using System.Collections.Generic;

public enum CharacterWindowState {
    Statistics,
    Skills
}

public class CharacterWindow: MonoBehaviour {

#region Gui constants
    private const int CHARACTER_WINDOW_ID = 0;
    private const string CHARACTER_WINDOW_TEXT = "",
                         MENU_ITEM_STATS = "Statistics",
                         MENU_ITEM_SKILLS = "Skills";
#endregion

#region Model attributes
    private PlayerCharacterModel playerCharModel;
    private Dictionary<string, BaseSkill> availableSkills;
#endregion

#region Gui attributes
    //Layout rects
    public int spaceOffset = 5, mainWidth = 600, mainHeight = 400,
               menuHeight = 70,
               descriptionHeight = 100;

    private Rect windowRect, menuRect, descriptionRect;
    private Rect statsRect, attributesRect, skillBookRect, skillbarRect;

    //menu
    public GUIStyle statisticsItem, skillsItem;

    private int normalFontSize, selectedFontSize;
    private Color selectedFontColor, normalFontColor;
    //main
    public GUIStyle statLabel, AttributeLabel, valueLabel;
    public int statModifyButtonSize = 25;
    private bool isVisible;
    private CharacterWindowState displayState;
#endregion

    void Awake() {
        enabled = false;
    }

    void Start() {
        playerCharModel = GameManager.Instance.PlayerCharacter.GetComponent<PlayerCharacterModel>();

        //Rects
        windowRect = new Rect((Screen.width - mainWidth / 2) / 2,
                              (Screen.height - mainHeight / 2) / 2,
                              mainWidth,
                              mainHeight);
        menuRect = new Rect(spaceOffset,spaceOffset, mainWidth, menuHeight);
        descriptionRect = new Rect(spaceOffset,
                                   windowRect.height - descriptionHeight,
                                   mainWidth,
                                   descriptionHeight);

        statsRect = new Rect(spaceOffset,
                             menuRect.y + menuRect.height + spaceOffset,
                             mainWidth / 2,
                             mainHeight);
        attributesRect = new Rect(statsRect.x + statsRect.width + spaceOffset,
                                  menuRect.y + menuRect.height + spaceOffset,
                                  mainWidth - statsRect.width,
                                  mainHeight);

        //menu
        normalFontSize = statisticsItem.fontSize;
        selectedFontSize = (int)(normalFontSize * 1.3);
        normalFontColor = statisticsItem.normal.textColor;
        selectedFontColor = statisticsItem.active.textColor;

        displayState = default(CharacterWindowState);
        statisticsItem.fontSize = selectedFontSize;
        statisticsItem.normal.textColor = selectedFontColor;

        availableSkills = new Dictionary<string, BaseSkill>();
        availableSkills.Add("skill 1", new BaseSkill("skill 1", "skill 1 description", null));
        availableSkills.Add("skill 2", new BaseSkill("skill 2", "skill 2 description", null));
        availableSkills.Add("skill 3", new BaseSkill("skill 3", "skill 3 description", null));

        isVisible = true;
    }

    void Update() {
    }

    void OnGUI(){
        if(isVisible)
            windowRect = ClampToScreen(GUI.Window(CHARACTER_WINDOW_ID, windowRect, MainWindow, CHARACTER_WINDOW_TEXT));

    }

    private Rect ClampToScreen(Rect r) {
        r.x = Mathf.Clamp(r.x, 0, Screen.width - r.width);
        r.y = Mathf.Clamp(r.y, 0, Screen.height - r.height);
        return r;
    }

    private void MainWindow(int windowID) {
        Menu();
        if (displayState.Equals(CharacterWindowState.Statistics)) {
            Stats();
            Attributes();
        }
        else if (displayState.Equals(CharacterWindowState.Skills)) {
            AvailableSkills();
            SkillBar();
        }
        Description();
        GUI.DragWindow();
    }
    
    private void Menu() {
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(MENU_ITEM_STATS, statisticsItem)) {
            displayState = CharacterWindowState.Statistics;
            statisticsItem.normal.textColor = selectedFontColor;
            statisticsItem.fontSize = selectedFontSize;
            skillsItem.normal.textColor = normalFontColor;
            skillsItem.fontSize = normalFontSize;
        }
        if (GUILayout.Button(MENU_ITEM_SKILLS, skillsItem)) {
            displayState = CharacterWindowState.Skills;
            statisticsItem.normal.textColor = normalFontColor;
            statisticsItem.fontSize = normalFontSize;
            skillsItem.normal.textColor = selectedFontColor;
            skillsItem.fontSize = selectedFontSize;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void Stats() {
        GUILayout.BeginArea(statsRect);
        for (int i = 0; i < playerCharModel.GetStatsLength(); ++i) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(playerCharModel.GetStat((StatType)i).Name + ": ", statLabel);
            GUILayout.Label(playerCharModel.GetStat((StatType)i).FinalValue.ToString(), valueLabel);
            if (GUILayout.Button("-", GUILayout.Width(statModifyButtonSize), GUILayout.Height(statModifyButtonSize))
               && (playerCharModel.TrainingPoints < playerCharModel.MAX_TRAINING_POINTS)) {
                --playerCharModel.GetStat((StatType)i).BaseValue;
                ++playerCharModel.TrainingPoints;
                playerCharModel.UpdateAttributes();
            }
            if (GUILayout.Button("+", GUILayout.Width(statModifyButtonSize), GUILayout.Height(statModifyButtonSize))
               && (playerCharModel.TrainingPoints > 0)){
                ++playerCharModel.GetStat((StatType)i).BaseValue;
                --playerCharModel.TrainingPoints;
                playerCharModel.UpdateAttributes();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
    private void Attributes() {
        GUILayout.BeginArea(attributesRect);
        for (int i = 0; i < playerCharModel.GetVitalsLength(); ++i) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(playerCharModel.GetVital((VitalType)i).Name + ": ", AttributeLabel);
            GUILayout.Label(playerCharModel.GetVital((VitalType)i).FinalValue.ToString(), valueLabel);
            GUILayout.EndHorizontal();
        }
        for (int i = 0; i < playerCharModel.GetAttributesLength(); ++i) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(playerCharModel.GetAttribute((AttributeType)i).Name + ": ", AttributeLabel);
            GUILayout.Label(playerCharModel.GetAttribute((AttributeType)i).FinalValue.ToString(), valueLabel);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
    private void AvailableSkills() {

    }
    private void SkillBar() {

    }
    private void Description() {

    }

    public bool IsVisible {
        get { return isVisible; }
        set { isVisible = value; }
    }
}