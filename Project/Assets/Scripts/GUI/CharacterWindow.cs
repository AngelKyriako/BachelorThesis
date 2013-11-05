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
    private BaseSkill lastSelectedSkill;
#endregion

    #region Gui attributes
    public GUISkin characterWindowSkin;
    //Layout rects
    public int spaceOffset = 5, mainWidth = 600, mainHeight = 400,
               menuHeight = 50,
               statsWidth = 300, statisticsHeight = 320,
               availableSkillsHeight = 250, skillsWidth = 300;

    private Rect windowRect, menuRect;
    private Rect statsRect, attributesRect, statDescriptionRect;
    private Rect availableSkillsRect, selectedSkillsRect, skillDescriptionRect;

    //menu
    public GUIStyle statisticsItem, skillsItem;

    private CharacterWindowState displayState;
    private int normalFontSize, selectedFontSize;
    private Color selectedFontColor, normalFontColor;

    //statistics
    public GUIStyle statLabel, AttributeLabel, valueLabel;
    private int statModifyButtonSize = 28;
    private float repeatButtonCooldown = 0.1f, lastClickTime;
    //skills
    private Vector2 availableSkillScrollPos;
    private int skillButtonSize = 60;
    //misc
    private bool isVisible;
    private float lastToggleTime;
    
#endregion

    void Awake() {
        enabled = false;
    }

    void Start() {
        playerCharModel = GameManager.Instance.Me.Character.GetComponent<PlayerCharacterModel>();
        lastSelectedSkill = null;
    #region Layout Rects
        windowRect          = new Rect((Screen.width - mainWidth / 2) / 2,
                                       (Screen.height - mainHeight / 2) / 2,
                                       mainWidth,
                                       mainHeight);
        menuRect            = new Rect(0,
                                       0,
                                       mainWidth,
                                       menuHeight);
        //statistics
        statsRect           = new Rect(spaceOffset,
                                       menuRect.y + menuRect.height,
                                       statsWidth - spaceOffset,
                                       statisticsHeight);
        attributesRect      = new Rect(statsRect.x + statsRect.width,
                                       menuRect.y + menuRect.height,
                                       mainWidth - statsRect.width,
                                       statisticsHeight);
        statDescriptionRect = new Rect(0,
                                       attributesRect.y + attributesRect.height,
                                       mainWidth,
                                       windowRect.height - (attributesRect.y + attributesRect.height));
        //skills
        availableSkillsRect = new Rect(0,
                                       menuRect.y + menuRect.height,
                                       skillsWidth,
                                       availableSkillsHeight);
        selectedSkillsRect  = new Rect(0,
                                       availableSkillsRect.y + availableSkillsRect.height,
                                       skillsWidth,
                                       windowRect.height - (availableSkillsRect.y + availableSkillsRect.height));
        skillDescriptionRect= new Rect(selectedSkillsRect.x + selectedSkillsRect.width,
                                       menuRect.y + menuRect.height,
                                       windowRect.width - selectedSkillsRect.width,
                                       windowRect.height - (menuRect.y + menuRect.height));
#endregion
        //menu
        normalFontSize = statisticsItem.fontSize;
        selectedFontSize = (int)(normalFontSize * 1.3);
        normalFontColor = statisticsItem.normal.textColor;
        selectedFontColor = statisticsItem.active.textColor;
        displayState = default(CharacterWindowState);
        statisticsItem.fontSize = selectedFontSize;
        statisticsItem.normal.textColor = selectedFontColor;
        //statistics
        lastClickTime = Time.time;
        //skills
        availableSkillScrollPos = Vector2.zero;
        //misc
        isVisible = false;
        lastToggleTime = 0;
    }

    void Update() {
        if(!IsInvoking("GetInput")){
            InvokeRepeating("GetInput", 0, Utilities.Instance.TOGGLE_KEY_DELAY);
        }
    }

    private void GetInput() {
        if (Input.GetKey(KeyCode.C) && Time.time - lastToggleTime > Utilities.Instance.TOGGLE_KEY_DELAY)
            isVisible = !isVisible;
    }

    void OnGUI(){
        GUI.skin = characterWindowSkin;
        if(isVisible)
            windowRect = GUIUtilities.Instance.ClampToScreen(GUI.Window(CHARACTER_WINDOW_ID, windowRect, MainWindow, CHARACTER_WINDOW_TEXT));

    }

    private void MainWindow(int windowID) {
        Menu();
        if (displayState.Equals(CharacterWindowState.Statistics)) {
            Stats();
            Attributes();
            StatisticDescription();
        }
        else if (displayState.Equals(CharacterWindowState.Skills)) {
            AvailableSkills();
            SelectedSkills();
            SkillDescription();
        }
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

    #region Statistics Panel
    private void Stats() {
        GUILayout.BeginArea(statsRect);
        for (int i = 0; i < playerCharModel.GetStatsLength(); ++i) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(playerCharModel.GetStat((StatType)i).Name + ": ", statLabel);
            GUILayout.Label(playerCharModel.GetStat((StatType)i).FinalValue.ToString(), valueLabel);
            if (GUILayout.RepeatButton("-", GUILayout.Width(statModifyButtonSize), GUILayout.Height(statModifyButtonSize))
               && (Time.time - lastClickTime > repeatButtonCooldown)
               && (playerCharModel.TrainingPoints < playerCharModel.MAX_TRAINING_POINTS)) {
                --playerCharModel.GetStat((StatType)i).BaseValue;
                ++playerCharModel.TrainingPoints;
                playerCharModel.UpdateAttributes();

                lastClickTime = Time.time;
            }
            if (GUILayout.RepeatButton("+", GUILayout.Width(statModifyButtonSize), GUILayout.Height(statModifyButtonSize))
               && (Time.time - lastClickTime > repeatButtonCooldown)
               && (playerCharModel.TrainingPoints > 0)){
                ++playerCharModel.GetStat((StatType)i).BaseValue;
                --playerCharModel.TrainingPoints;
                playerCharModel.UpdateAttributes();

                lastClickTime = Time.time;
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
    private void StatisticDescription() {
        GUILayout.BeginArea(statDescriptionRect);
        GUILayout.EndArea();
    }
#endregion
    
    #region Skills Panel
    private void AvailableSkills() {
        int count = 0;
        GUILayout.BeginArea(availableSkillsRect);
        availableSkillScrollPos = GUILayout.BeginScrollView(availableSkillScrollPos);
        GUILayout.BeginHorizontal();
        foreach (BaseSkill skill in SkillBook.Instance.AvailableSkills) {
            if (GUILayout.Button(skill.Title, GUILayout.Width(skillButtonSize), GUILayout.Height(skillButtonSize)))
                lastSelectedSkill = skill;
            if (++count % 4 == 0) {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    private void SelectedSkills() {
        GUILayout.BeginArea(selectedSkillsRect);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < playerCharModel.SkillCount; ++i ) {
            if (playerCharModel.GetSkill(i) != null){
                if (GUILayout.Button(playerCharModel.GetSkill(i).Title, GUILayout.Width(skillButtonSize), GUILayout.Height(skillButtonSize))
                    && lastSelectedSkill != null) {
                    SkillBook.Instance.AvailableSkills.Add(playerCharModel.GetSkill(i));
                    playerCharModel.SetSkill(i, lastSelectedSkill);
                    SkillBook.Instance.AvailableSkills.Remove(lastSelectedSkill);
                    lastSelectedSkill = null;
                }
            }
            else
                if (GUILayout.Button("Empty", GUILayout.Width(skillButtonSize), GUILayout.Height(skillButtonSize))
                    && lastSelectedSkill != null) {
                    playerCharModel.SetSkill(i, lastSelectedSkill);
                    SkillBook.Instance.AvailableSkills.Remove(lastSelectedSkill);
                    lastSelectedSkill = null;
                }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    private void SkillDescription() {
        GUILayout.BeginArea(skillDescriptionRect);
        GUILayout.EndArea();
    }
#endregion

    public bool IsVisible {
        get { return isVisible; }
        set { isVisible = value; }
    }
}