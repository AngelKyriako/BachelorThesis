using UnityEngine;
using System;

public class PlayerInputManager: SingletonPhotonMono<PlayerInputManager> {

    private const int LEFT_CLICK = 0, RIGHT_CLICK = 1;

    public delegate void CameraMovementEvent();
    public event CameraMovementEvent OnCameraMovementInput;
    public delegate void CharacterMovementEvent(Ray ray);
    public event CharacterMovementEvent OnCharacterMovementInput;
    public delegate void SkillSelectEvent(CharacterSkillSlots _slot);
    public event SkillSelectEvent OnSkillSelectInput;

    private DispatchTable<KeyCode> SkillDispatcher;

    private PlayerInputManager() { }

    void Start() {
        SkillDispatcher = new DispatchTable<KeyCode>();

        SkillDispatcher.AddAction(KeyCode.Q, new Action(() => OnSkillSelectInput(CharacterSkillSlots.Q)));
        SkillDispatcher.AddAction(KeyCode.W, new Action(() => OnSkillSelectInput(CharacterSkillSlots.W)));
        SkillDispatcher.AddAction(KeyCode.E, new Action(() => OnSkillSelectInput(CharacterSkillSlots.E)));
        SkillDispatcher.AddAction(KeyCode.R, new Action(() => OnSkillSelectInput(CharacterSkillSlots.R)));
    }

	void Update () {
        //Camera Input
        
        //Character Input
        if (Input.GetMouseButtonDown(RIGHT_CLICK)) {
            OnCharacterMovementInput(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        //Skill Input
        if(Input.GetKey(KeyCode.Q))
            OnSkillSelectInput(CharacterSkillSlots.Q);
        else if (Input.GetKey(KeyCode.W))
            OnSkillSelectInput(CharacterSkillSlots.W);
        else if (Input.GetKey(KeyCode.E))
            OnSkillSelectInput(CharacterSkillSlots.E);
        else if (Input.GetKey(KeyCode.R))
            OnSkillSelectInput(CharacterSkillSlots.R);
	}    
}
