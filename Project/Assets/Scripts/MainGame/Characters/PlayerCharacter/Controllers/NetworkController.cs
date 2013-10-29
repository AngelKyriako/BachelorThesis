using UnityEngine;
using System.Collections;

public class NetworkController: Photon.MonoBehaviour {

    private CameraController cameraController;
    private CharacterController characterController;
    private VisionController visionController;

    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
    private float currentSpeed;

    void Awake() {
        cameraController = GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        visionController = GetComponent<VisionController>();

        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;
        currentSpeed = 0;
    }

    private static void SetGameObjectLayer(GameObject obj, int l) {
        if (obj == null)
            return;
        obj.layer = l;
        foreach (Transform child in obj.transform)
            if (child)
                SetGameObjectLayer(child.gameObject, l);
    }

    void Start() {
        characterController.SetIsLocal(photonView.isMine);
        gameObject.name = "BabyDragon" + photonView.viewID.ToString();
        transform.parent = GameObject.Find("Characters/BabyDragons").transform;
        if (photonView.isMine) {
            cameraController.enabled = true;
            characterController.enabled = true;
            visionController.enabled = true;
            SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Allies"));
        }
        else {
            cameraController.enabled = false;
            characterController.enabled = false;
            visionController.enabled = false;//@TODO: for team play this should be only for enemies
            SetGameObjectLayer(gameObject, LayerMask.NameToLayer("HiddenEnemies"));
        }
    }

    void Update() {
        if (!photonView.isMine) {
            //Check if able to smooth (last argument proly should be parameterized based on:
            //                                  Vector3.Distance(transform.position, correctPlayerPosition);)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, 0);
            characterController.SetCurrentSpeed(currentSpeed);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // send the local character's data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.velocity);
            stream.SendNext(characterController.GetCurrentSpeed());

        }
        else { // receive data from remote characters
            correctPlayerPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRotation = (Quaternion)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();
            currentSpeed = (float)stream.ReceiveNext();
        }
    }
}