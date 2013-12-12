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
        MainStage
    }

    public Vector3 defaultPosition, defaultRotation;
    public float targetOffsetX = 0f,
                 targetOffsetZ = -8f,
                 smoothLockOn = 5f;

    public int scrollOffset = 100;
    public float movementInputWeight = 35,
                 minMovementSpeed = 25, maxMovementSpeed = 100;

    public float minCameraX = 30f, maxCameraX = 70f,
                 minStageCameraY = 25f, maxStageCameraY = 30f,
                 minHeavenCameraY = 65f, maxHeavenCameraY = 80f,
                 minCameraZ = 10f, maxCameraZ = 70f;

    private float lastMainStageY;
    private Vector3 originPosition, destination;
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

        HeightValidationDispatcher.AddAction(CameraMode.MainStage, delegate(Vector3 _destination) {
            if (_destination.y > maxStageCameraY)
                _destination.y = maxStageCameraY;
            else if (_destination.y < minStageCameraY)
                _destination.y = minStageCameraY;

            return _destination;
        });
    }

    void Start() {
        Camera.main.transform.position = destination = defaultPosition;
        Camera.main.transform.rotation = Quaternion.Euler(defaultRotation);

        lastMainStageY = maxStageCameraY;
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
        destination = originPosition + movement;

        ValidateDestination();
        ApplyMovement();
    }

    private Vector3 ReceiveMovementToTarget() {
        return new Vector3(MovementInAxisX, 0, MovementInAxisZ);
    }
    private float MovementInAxisX {
        get { return transform.position.x - Camera.main.transform.position.x + targetOffsetX; }
    }
    private float MovementInAxisZ {
        get { return transform.position.z - Camera.main.transform.position.z + targetOffsetZ; }
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
        move.y = -(movementInputWeight * Input.GetAxis("Mouse ScrollWheel"));
        if (mode.Equals(CameraMode.MainStage))
            lastMainStageY = Camera.main.transform.position.y + move.y;
        return move;
    }

    private Vector3 ReceiveMovementInput() {
        Vector3 movement = new Vector3(0, 0, 0);

        movement = HorizontalMovement(movement);
        movement = VerticalMovement(movement);
        return movement;
    }

    private void ValidateDestination() {
        if (destination != originPosition) {
            if (destination.x > maxCameraX)
                destination.x = maxCameraX;
            else if (destination.x < minCameraX)
                destination.x = minCameraX;

            if (destination.z > maxCameraZ)
                destination.z = maxCameraZ;
            else if (destination.z < minCameraZ)
                destination.z = minCameraZ;

            destination = HeightValidationDispatcher.Dispatch(mode, destination);
        }
    }

    private void ApplyMovement() {
        if (destination != originPosition)
            Camera.main.transform.position = Vector3.MoveTowards(originPosition, destination, Time.deltaTime * minMovementSpeed);
    }

    //@TODO: Change temporarity the speed of the camera to make an awesome follow the character effect
    public void EnterHeavenMode() {
        mode = CameraMode.Heaven;
        destination = new Vector3(PositioningInAxisX, minHeavenCameraY, PositioningInAxisZ);
    }

    public void EnterMainStageMode() {
        mode = CameraMode.MainStage;
        destination = new Vector3(PositioningInAxisX, lastMainStageY, PositioningInAxisZ);
    }

    private float PositioningInAxisX {
        get { return transform.position.x + targetOffsetX; }
    }
    private float PositioningInAxisZ {
        get { return transform.position.z + targetOffsetZ; }
    }
}