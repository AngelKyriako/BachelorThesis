using UnityEngine;

public class CameraManager: SingletonMono<CameraManager> {

    public enum CameraMode {
        Heaven,
        MainStage
    }

    #region editable attributes
    public Vector3 defaultPosition;

    public float defaultRotationX = 80f,
                 targetOffsetX = 0f,
                 targetOffsetZ = -5f,
                 smoothLockOn = 5f;

    public int scrollOffset = 100;
    public float movementInputWeight = 35,
                 minMovementSpeed = 25, maxMovementSpeed = 100;

    public float minCameraX = 27f, maxCameraX = 73f,
                 minStageCameraY = 20f, maxStageCameraY = 25f,
                 minHeavenCameraY = 75f, maxHeavenCameraY = 85f,
                 minCameraZ = 10f, maxCameraZ = 79f;
    #endregion

    #region attributes
    private float lastMainStageY;
    private Vector3 originPosition, destination;
    
    private GameObject target;
    private bool lockedOnTarget;

    private CameraMode mode;

    private DispatchTable<CameraMode, Vector3, Vector3> HeightValidationDispatcher;
    #endregion

    private CameraManager() { }

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

        enabled = false;
    }

    public void SetUp() {
        target = GameManager.Instance.MyCharacter;
        enabled = true;
    }

    void Start() {
        Utilities.Instance.Assert(target!=null, "CameraManager", "Start", "Invalid target value, a gameobject must be assigned to this variable.");
        Camera.main.transform.position = destination = defaultPosition;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(defaultRotationX, 0, 0));

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
        get { return target.transform.position.x - Camera.main.transform.position.x + targetOffsetX; }
    }
    private float MovementInAxisZ {
        get { return target.transform.position.z - Camera.main.transform.position.z + targetOffsetZ; }
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

    public void EnterHeavenMode() {
        lastMainStageY = Camera.main.transform.position.y;
        mode = CameraMode.Heaven;
        destination = new Vector3(PositioningInAxisX, minHeavenCameraY, PositioningInAxisZ);
    }

    public void EnterMainStageMode() {
        mode = CameraMode.MainStage;
        destination.Set(PositioningInAxisX, lastMainStageY, PositioningInAxisZ);
        ValidateDestination();
        Camera.main.transform.position = destination;
    }

    private float PositioningInAxisX {
        get { return target.transform.position.x + targetOffsetX; }
    }
    private float PositioningInAxisZ {
        get { return target.transform.position.z + targetOffsetZ; }
    }
}