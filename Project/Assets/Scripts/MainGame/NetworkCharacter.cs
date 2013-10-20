using UnityEngine;
using System.Collections;

public class NetworkCharacter: Photon.MonoBehaviour {

    CameraController cameraComponent;
    RTSController controllerComponent;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;

    public float smooth = 5f;

    void Awake() {
        cameraComponent = GetComponent<CameraController>();
        controllerComponent = GetComponent<RTSController>();
        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;
    }

    void Start() {
        if (photonView.isMine) {
            cameraComponent.enabled = true;
            controllerComponent.enabled = true;
            transform.parent = GameObject.Find("Characters/BabyDragons").transform;
            //Camera.main.transform.parent = transform;
            //Camera.main.transform.localPosition = new Vector3(0, 2, -10);
            //Camera.main.transform.localEulerAngles = new Vector3(10, 0, 0);
        }
        else {
            cameraComponent.enabled = false;
            controllerComponent.enabled = true;
        }
        controllerComponent.SetIsRemote(!photonView.isMine);
        gameObject.name = "BabyDragon" + photonView.viewID.ToString();
    }

    void Update() {
        if (!photonView.isMine) {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, Time.deltaTime * smooth);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, Time.deltaTime * smooth);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // send the local character's data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(rigidbody.velocity);

        }
        else { // receive data from remote characters
            correctPlayerPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRotation = (Quaternion)stream.ReceiveNext();
            //rigidbody.velocity = (Vector3)stream.ReceiveNext();
        }
    }
}
