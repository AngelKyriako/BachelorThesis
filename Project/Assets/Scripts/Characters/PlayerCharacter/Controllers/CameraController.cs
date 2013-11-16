﻿using UnityEngine;
using System.Collections;

/// <summary>
/// A camera controller that gives an RTS view perspective.
/// Locks on the target character when it is in a lock state. (hit 'L' to lock and unlock).
/// Drag and drop the character you wish to target.
/// </summary>
public class CameraController: MonoBehaviour {

    private Vector3 originPosition, originRotation;
    private bool lockedOnTarget;

    public Transform target;
    public float targetOffsetX = 0f,
                 targetOffsetZ = -15f,
                 smoothLockOn = 5f;

    public int scrollOffset = 100;
    public float movementInputWeight = 35,
                 minMovementSpeed = 25, maxMovementSpeed = 100,
                 rotateInputWeight = 35,
                 rotateSpeed = 35;

    public float minCameraX = 10f, maxCameraX = 80f,
                 minCameraY = 10f, maxCameraY = 40f,
                 minCameraZ = 10f, maxCameraZ = 80f;

    private void Awake() {
        Utilities.Instance.Assert(target, "CameraController", "Awake", "target transform is not defined");
        //@TODO: Init fixed camera rotation
        lockedOnTarget = true;
    }

    private void Update() {
        Vector3 movement;
        
        ToggleCameraLock();

        if (lockedOnTarget)
            movement = ReceiveMovementToTarget();
        else {
            movement = ReceiveMovementInput();
            movement = Camera.main.transform.TransformDirection(movement);
        }
        movement = ZoomMovement(movement);

        originPosition = Camera.main.transform.position;
        ApplyMovement(ValidateMovementDestination(GetMovementDestination(movement)));
        //originRotation = Camera.main.transform.eulerAngles;
        //ApplyRotation(ValidateRotationDestination(GetRotationDestination(ReceiveRotationInput())));
    }

    ////////////////////////////////// Movement  //////////////////////////////////
    private void ToggleCameraLock() {
        if (Input.GetKeyUp(KeyCode.L))
            lockedOnTarget = !lockedOnTarget;
    }

    private Vector3 ReceiveMovementToTarget() {
        float movementX = ((target.position.x - Camera.main.transform.position.x + targetOffsetX));
        float movementZ = ((target.position.z - Camera.main.transform.position.z + targetOffsetZ));
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

    private Vector3 ValidateMovementDestination(Vector3 destination) {
        if (destination != originPosition) {
            if (destination.x > maxCameraX)
                destination.x = maxCameraX;
            else if (destination.x < minCameraX)
                destination.x = minCameraX;

            if (destination.y > maxCameraY)
                destination.y = maxCameraY;
            else if (destination.y < minCameraY)
                destination.y = minCameraY;

            if (destination.z > maxCameraZ)
                destination.z = maxCameraZ;
            else if (destination.z < minCameraZ)
                destination.z = minCameraZ;
        }
        return destination;
    }

    private void ApplyMovement(Vector3 destination) {
        if (destination != originPosition)
            Camera.main.transform.position = Vector3.MoveTowards(originPosition, destination, Time.deltaTime * minMovementSpeed);
    }

    ////////////////////////////////// Rotation  //////////////////////////////////
    private Vector3 ReceiveRotationInput() {
        Vector3 rotation = new Vector3(0, 0, 0);

        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1)) {
            rotation.x = -Input.GetAxis("Mouse Y") * rotateInputWeight;
            rotation.y = Input.GetAxis("Mouse X") * rotateInputWeight;
        }
        return rotation;
    }

    private Vector3 GetRotationDestination(Vector3 rotation) {
        Vector3 origin = originRotation;
        return origin += rotation;
    }

    private Vector3 ValidateRotationDestination(Vector3 destination) {
        return destination;
    }
    
    private void ApplyRotation(Vector3 destination) {
        if (destination != originRotation)
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(originRotation, destination, Time.deltaTime * rotateSpeed);
    }
}