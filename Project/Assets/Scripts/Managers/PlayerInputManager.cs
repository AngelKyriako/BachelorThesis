using UnityEngine;
using System;

public class PlayerInputManager: SingletonPhotonMono<PlayerInputManager> {

    private const int LEFT_CLICK = 0, RIGHT_CLICK = 1;

    //public delegate void CameraMovementEvent();
    //public event CameraMovementEvent OnCameraMovementInput;
    public delegate void CharacterMovementEvent(Ray ray);
    public event CharacterMovementEvent OnCharacterMovementInput;
    public delegate void SkillSelectEvent(CharacterSkillSlot _slot);
    public event SkillSelectEvent OnSkillSelectInput;

    private PlayerInputManager() { }

    void Start() {
    }

	void Update () {
        //Camera Input
        
        //Character Input
        if (Input.GetMouseButtonDown(RIGHT_CLICK)) {
            OnCharacterMovementInput(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        //Skill Input
        if (Input.GetKeyUp(KeyCode.Q) || CharacterInfoPanel.Instance.IsSkillButtonPressed((int)CharacterSkillSlot.Q))
            OnSkillSelectInput(CharacterSkillSlot.Q);
        else if (Input.GetKeyUp(KeyCode.W) || CharacterInfoPanel.Instance.IsSkillButtonPressed((int)CharacterSkillSlot.W))
            OnSkillSelectInput(CharacterSkillSlot.W);
        else if (Input.GetKeyUp(KeyCode.E) || CharacterInfoPanel.Instance.IsSkillButtonPressed((int)CharacterSkillSlot.E))
            OnSkillSelectInput(CharacterSkillSlot.E);
        else if (Input.GetKeyUp(KeyCode.R) || CharacterInfoPanel.Instance.IsSkillButtonPressed((int)CharacterSkillSlot.R))
            OnSkillSelectInput(CharacterSkillSlot.R);
	}

    public Vector3 MousePosition {
        get { return Input.mousePosition; }
    }
}
