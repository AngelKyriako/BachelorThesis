using UnityEngine;
using System.Collections;

public class NetworkController: Photon.MonoBehaviour {

    private CameraController cameraController;
    private MovementController movementController;
    private VisionController visionController;

    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
    private float currentSpeed;

    void Awake() {
        cameraController = gameObject.GetComponent<CameraController>();
        cameraController.enabled = photonView.isMine;
        movementController = gameObject.GetComponent<MovementController>();
        movementController.enabled = photonView.isMine;
        visionController = gameObject.GetComponent<VisionController>();
        visionController.enabled = photonView.isMine;//@TODO: for team play this should be only for enemies

        gameObject.transform.parent = GameObject.Find("Characters/BabyDragons").transform;
        gameObject.name = photonView.viewID.ToString();
        if (photonView.isMine) {
            GameManager.Instance.Me = new PlayerCharacterPair(photonView.owner, gameObject);
            Utilities.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Allies"));
        }
        else {
            //GameManager.Instance.AddPlayerCharacter(new PlayerCharacterPair(photonView.owner, gameObject));
            Utilities.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("HiddenEnemies"));//@TODO Must be called when game starts when everyone is connected
        }

        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;
        currentSpeed = 0;                
    }

    void Update() {
        if (!photonView.isMine) {
            //Check if able to smooth (last argument probably should be parameterized based on:
            //                                  Vector3.Distance(transform.position, correctPlayerPosition);)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, 0);
            movementController.MovementSpeed = currentSpeed;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // send the local character's data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.velocity);
            stream.SendNext(movementController.MovementSpeed);
        }
        else { // receive data from remote characters
            correctPlayerPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRotation = (Quaternion)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();
            currentSpeed = (float)stream.ReceiveNext();
        }
    }
}