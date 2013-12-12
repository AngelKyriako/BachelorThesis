using UnityEngine;
using System;

public class PlayerInputManager: SingletonPhotonMono<PlayerInputManager> {

    private const int LEFT_CLICK = 0, RIGHT_CLICK = 1;

    private CharacterSkillSlot currentTargetedSlot;

    public delegate void CharacterMovementEvent(Ray ray);
    public event CharacterMovementEvent OnCharacterMovementInput;
    public delegate void SkillPressEvent(CharacterSkillSlot _slot);
    public event SkillPressEvent SkillQWERorLeftClick, SkillRightClick;

    private PlayerInputManager() { }

    void Start() {
        currentTargetedSlot = CharacterSkillSlot.None;
    }

	void Update () {
        //Character movement input and skill unselect
        if (Input.GetMouseButtonDown(RIGHT_CLICK)) {
            OnCharacterMovementInput(Camera.main.ScreenPointToRay(Input.mousePosition));
            SkillRightClick(CharacterSkillSlot.None);
        }
        //Skill select, casting
        //@TODO: Prevent this part of input if in Heaven
        if (Input.GetKeyUp(KeyCode.Q)){// || CharacterInfoPanel.Instance.IsSkillButtonPressed(CharacterSkillSlot.Q)) {
            SkillQWERorLeftClick(CharacterSkillSlot.Q);
        }
        else if (Input.GetKeyUp(KeyCode.W)) {// || CharacterInfoPanel.Instance.IsSkillButtonPressed(CharacterSkillSlot.W)) {
            SkillQWERorLeftClick(CharacterSkillSlot.W);
        }
        else if (Input.GetKeyUp(KeyCode.E)) {// || CharacterInfoPanel.Instance.IsSkillButtonPressed(CharacterSkillSlot.E)) {
            SkillQWERorLeftClick(CharacterSkillSlot.E);
        }
        else if (Input.GetKeyUp(KeyCode.R)) {// || CharacterInfoPanel.Instance.IsSkillButtonPressed(CharacterSkillSlot.R)) {
            SkillQWERorLeftClick(CharacterSkillSlot.R);
        }
        else if (Input.GetMouseButtonDown(LEFT_CLICK)) {
            SkillQWERorLeftClick(currentTargetedSlot);
        }
	}

    public Vector3 MousePosition {
        get { return Input.mousePosition; }
    }
    public CharacterSkillSlot CurrentTargetedSlot {
        get { return currentTargetedSlot; }
        set { currentTargetedSlot = value; }
    }
}
