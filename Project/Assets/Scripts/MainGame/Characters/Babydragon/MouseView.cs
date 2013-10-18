using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)

public class MouseLook: MonoBehaviour {

    private enum MouseRotation {
        XAndY = 0,
        X = 1,
        Y = 2
    }
    private MouseRotation axes = MouseRotation.XAndY;
    private float sensitivityX = 15F;
    private float sensitivityY = 15F;

    private float minimumX = -360F;
    private float maximumX = 360F;

    private float minimumY = -60F;
    private float maximumY = 60F;

    private float rotationY = 0F;

    private void Start() {
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }

    private void Update() {
        if (axes == MouseRotation.XAndY) {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == MouseRotation.X) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}