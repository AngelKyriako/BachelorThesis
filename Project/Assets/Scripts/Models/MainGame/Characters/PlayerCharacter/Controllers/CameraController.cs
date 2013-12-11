using UnityEngine;
using System.Collections;

/// <summary>
/// A camera controller that gives an RTS view perspective.
/// Locks on the target character when it is in a lock state. (hit 'L' to lock and unlock).
/// Drag and drop the character you wish to target.
/// </summary>
public class CameraController: MonoBehaviour {

    public enum CameraMode {
        Heaven,
        Stage
    }

    public Vector3 defaultRotation;
    public float targetOffsetX = 0f,
                 targetOffsetZ = -15f,
                 smoothLockOn = 5f;

    public int scrollOffset = 100;
    public float movementInputWeight = 35,
                 minMovementSpeed = 25, maxMovementSpeed = 100;

    public float minCameraX = 30f, maxCameraX = 70f,
                 minStageCameraY = 25f, maxStageCameraY = 30f,
                 minHeavenCameraY = 115f, maxHeavenCameraY = 160f,
                 minCameraZ = 10f, maxCameraZ = 70f;

    private Vector3 originPosition;
    private bool lockedOnTarget;

    private CameraMode mode;
    private DispatchTable<CameraMode, Vector3, Vector3> HeightValidationDispatcher;

    void Awake() {
        HeightValidationDispatcher = new DispatchTable<CameraMode, Vector3, Vector3>();
        HeightValidationDispatcher.AddAction(CameraMode.Heaven, delegate(Vector3 _destination) {            
            if (_destination.y > maxHeavenCameraY)
                _destination.y = maxHeavenCameraY;
            else if (_destination.y < minHeavenCameraY)
                _destination.y = minHeavenCameraY;

            return _destination;
        });

        HeightValidationDispatcher.AddAction(CameraMode.Stage, delegate(Vector3 _destination) {
            if (_destination.y > maxStageCameraY)
                _destination.y = maxStageCameraY;
            else if (_destination.y < minStageCameraY)
                _destination.y = minStageCameraY;

            return _destination;
        });
    }

    void Start() {
        Camera.main.transform.rotation = Quaternion.LookRotation(defaultRotation);
        mode = default(CameraMode);
        lockedOnTarget = true;
    }

    void Update() {
        ToggleCameraLock();
        Move();
    }

    //////////////////////////////////  Locking  //////////////////////////////////
    private void ToggleCameraLock() {
        if (Input.GetKeyUp(KeyCode.L))
            lockedOnTarget = !lockedOnTarget;
    }

    ////////////////////////////////// Movement  //////////////////////////////////
    private void Move() {
        Vector3 movement;

        if (lockedOnTarget)
            movement = ReceiveMovementToTarget();
        else
            movement = Camera.main.transform.TransformDirection(ReceiveMovementInput());
        movement = ZoomMovement(movement);

        originPosition = Camera.main.transform.position;
        ApplyMovement(ValidateMovementDestination(GetMovementDestination(movement)));
    }

    private Vector3 ReceiveMovementToTarget() {
        float movementX = ((transform.position.x - Camera.main.transform.position.x + targetOffsetX));
        float movementZ = ((transform.position.z - Camera.main.transform.position.z + targetOffsetZ));
        return new Vector3(movementX, 0, movementZ);
    }

    private Vector3 HorizontalMovement(Vector3 move) {
        if (!Input.GetKey(KeyCode.LeftAlt)) {
            float xpos = Input.mousePosition.x;
            if (xpos >= 0 && xpos < scrollOffset)
                move.x -= movementInputWeight;
            else if (xpos <= Screen.width && xpos > Screen.width - scrollOffset)
                move.x += movementInputWeight;
        }
        return move;
    }

    private Vector3 VerticalMovement(Vector3 move) {
        if (!Input.GetKey(KeyCode.LeftAlt)) {
            float ypos = Input.mousePosition.y;
            if (ypos >= 0 && ypos < scrollOffset)
                move.z -= movementInputWeight;
            else if (ypos <= Screen.height && ypos > Screen.height - scrollOffset)
                move.z += movementInputWeight;
        }
        return move;
    }

    private Vector3 ZoomMovement(Vector3 move) {
        float input = Input.GetAxis("Mouse ScrollWheel");
        move.y = -(movementInputWeight * input);
        return move;
    }

    private Vector3 ReceiveMovementInput() {
        Vector3 movement = new Vector3(0, 0, 0);

        movement = HorizontalMovement(movement);
        movement = VerticalMovement(movement);
        return movement;
    }

    private Vector3 GetMovementDestination(Vector3 movement) {
        Vector3 origin = originPosition;
        return origin += movement;
    }

    private Vector3 ValidateMovementDestination(Vector3 _destination) {
        if (_destination != originPosition) {
            if (_destination.x > maxCameraX)
                _destination.x = maxCameraX;
            else if (_destination.x < minCameraX)
                _destination.x = minCameraX;
            
            if (_destination.z > maxCameraZ)
                _destination.z = maxCameraZ;
            else if (_destination.z < minCameraZ)
                _destination.z = minCameraZ;

            _destination = HeightValidationDispatcher.Dispatch(mode, _destination);
        }
        return _destination;
    }

    private void ApplyMovement(Vector3 destination) {
        if (destination != originPosition)
            Camera.main.transform.position = Vector3.MoveTowards(originPosition, destination, Time.deltaTime * minMovementSpeed);
    }

    public void EnterHeavenMode() {
        mode = CameraMode.Heaven;
        Camera.main.transform.position = new Vector3(transform.position.x, minHeavenCameraY, transform.position.z);
    }

    public void EnterStageMode() {
        mode = CameraMode.Stage;
        Camera.main.transform.position = new Vector3(transform.position.x, minStageCameraY, transform.position.z);
    }
}