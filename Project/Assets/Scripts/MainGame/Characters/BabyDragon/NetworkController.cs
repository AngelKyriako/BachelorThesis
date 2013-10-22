using UnityEngine;
using System.Collections;

public class NetworkController: Photon.MonoBehaviour {

    private CameraController cameraController;
    private CharacterController characterController;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;

    void Awake() {
        cameraController = GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;
    }

    void Start() {
        characterController.SetIsLocal(photonView.isMine);
        gameObject.name = "BabyDragon" + photonView.viewID.ToString();
        if (photonView.isMine) {
            cameraController.enabled = true;
            characterController.enabled = true;
            transform.parent = GameObject.Find("Characters/BabyDragons").transform;
        }
        else {
            cameraController.enabled = true;
            characterController.enabled = true;
        }
    }

    void Update() {
        if (!photonView.isMine) {
            //@TODO: Smooth this bitch (last argument should be parameterized based on:
            //                                  Vector3.Distance(transform.position, correctPlayerPosition);)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, 0);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // send the local character's data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.velocity);

        }
        else { // receive data from remote characters
            correctPlayerPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRotation = (Quaternion)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();
        }
    }
}