using UnityEngine;

public class PlayerInputManager: SingletonPhotonMono<PlayerInputManager> {

    private const int LEFT_CLICK = 0, RIGHT_CLICK = 1;

    public delegate void CharacterMovementEvent(Ray ray);
    public event CharacterMovementEvent OnCharacterMovementInput;
    public delegate void CameraMovementEvent();
    public event CameraMovementEvent OnCameraMovementInput;

    private PlayerInputManager() { }

	void Update () {
        //Character Input
        if (Input.GetMouseButtonDown(RIGHT_CLICK)) {
            OnCharacterMovementInput(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        //Camera Input
	}
}
