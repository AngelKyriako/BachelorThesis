using UnityEngine;
using System.Collections;

public class CameraController: MonoBehaviour {

    private float lockOnTargetOffset = 50f;

    public Collider target;
    public bool lockedOnTarget = true;
    public int scrollOffset = 150;
    public float movementSpeed = 35,
                 scrollSpeed = 35,
                 rotateAmount = 10,
                 rotateSpeed = 100,
                 minCameraHeight = 10,
                 maxCameraHeight = 50;


    private void Update() {
        if (lockedOnTarget)
            ApplyMovementOnTarget();
        else
            ApplyRTSMovement(ReceiveMovementInput());
        ApplyRotation(ReceiveRotationInput());
    }

    ////////////////////////////////// Lock on character Movement  //////////////////////////////////

    private void ApplyMovementOnTarget() {
        Camera.main.transform.position.Set(target.transform.position.x,
                                            Camera.main.transform.position.y,
                                            target.transform.position.z - lockOnTargetOffset);
    }

    ////////////////////////////////// RTS Movement  //////////////////////////////////

    private Vector3 HorizontalMovement(Vector3 move) {
        if (!Input.GetKey(KeyCode.LeftAlt)) {
            float xpos = Input.mousePosition.x;
            if (xpos >= 0 && xpos < scrollOffset)
                move.x -= movementSpeed;
            else if (xpos <= Screen.width && xpos > Screen.width - scrollOffset)
                move.x += movementSpeed;
        }
        return move;
    }

    private Vector3 VerticalMovement(Vector3 move) {
        if (!Input.GetKey(KeyCode.LeftAlt)) {
            float ypos = Input.mousePosition.y;
            if (ypos >= 0 && ypos < scrollOffset)
                move.z -= movementSpeed;
            else if (ypos <= Screen.height && ypos > Screen.height - scrollOffset)
                move.z += movementSpeed;
        }
        return move;
    }

    private Vector3 ZoomMovement(Vector3 move) {
        move.y = -(scrollSpeed * Input.GetAxis("Mouse ScrollWheel"));
        return move;
    }

    private Vector3 ReceiveMovementInput() {
        Vector3 movement = new Vector3(0, 0, 0);

        movement = HorizontalMovement(movement);
        movement = VerticalMovement(movement);
        movement = Camera.main.transform.TransformDirection(movement);
        movement = ZoomMovement(movement);
        return movement;
    }

    private void ApplyRTSMovement(Vector3 movement) {
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;

        destination += movement;

        if (destination != origin) {
            destination = ValidateMovementDestination(destination);
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * scrollSpeed);
        }
    }

    private Vector3 ValidateMovementDestination(Vector3 destination) {
        if (destination.y > maxCameraHeight) {
            destination.y = maxCameraHeight;
        }
        else if (destination.y < minCameraHeight) {
            destination.y = minCameraHeight;
        }
        return destination;
    }

    ////////////////////////////////// RTS Rotation  //////////////////////////////////
    private Vector3 ReceiveRotationInput() {
        Vector3 rotation = new Vector3(0, 0, 0);

        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1)) {
            rotation.x = -Input.GetAxis("Mouse Y") * rotateAmount;
            rotation.y = Input.GetAxis("Mouse X") * rotateAmount;
        }
        return rotation;
    }

    private Vector3 ValidateRotationDestination(Vector3 destination) {
        return destination;
    }

    private void ApplyRotation(Vector3 rotation) {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        destination += rotation;

        if (destination != origin) {
            destination = ValidateRotationDestination(destination);
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }
}