using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterWindowState {
    Statistics,
    Skills
}

public class CharacterWindow: MonoBehaviour {

#region constants
    private const int CHARACTER_WINDOW_ID = 0;
    private const string CHARACTER_WINDOW_TEXT = "";

    public Rect menu, content, description;
#endregion

#region attributes
    private PlayerCharacterModel characterModel;
    private Dictionary<string, BaseSkill> availableSkills;
    private bool isVisible;
    private CharacterWindowState displayState;
    private Rect windowRect = new Rect((Screen.width - 400 / 2) / 2, (Screen.height - 350 / 2) / 2, 400, 350);
#endregion

    void Awake() {
        enabled = false;
    }

    void Start() {
        characterModel = GameManager.Instance.PlayerCharacter.GetComponent<PlayerCharacterModel>();

        availableSkills = new Dictionary<string, BaseSkill>();
        availableSkills.Add("skill 1", new BaseSkill("skill 1", "skill 1 description", null));
        availableSkills.Add("skill 2", new BaseSkill("skill 2", "skill 2 description", null));
        availableSkills.Add("skill 3", new BaseSkill("skill 3", "skill 3 description", null));

        isVisible = true;
        displayState = default(CharacterWindowState);
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
        GUI.DragWindow(new Rect(0, 0, windowRect.width, 100));
    }
    
    private void Menu() {
        GUI.Label(new Rect(0, 0, 50, 50), characterModel.GetStat(StatType.Agility).BaseValue.ToString());
    }
    private void Stats() {

    }
    private void Attributes() {

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