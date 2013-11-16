using UnityEngine;
using System.Collections;

public class NetworkController: Photon.MonoBehaviour {

    #region attributes
    // references to local gameObjects
    private PlayerCharacterModel model;
    private CameraController cameraController;
    private MovementController movementController;
    private VisionController visionController;
    // interpolation shit
    private float syncTime = 0f, syncDelay = 0f, lastSynchronizationTime = 0f;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
    private float currentSpeed;
#endregion

    void Awake() {
        model = gameObject.GetComponent<PlayerCharacterModel>();
        cameraController = gameObject.GetComponent<CameraController>();
        movementController = gameObject.GetComponent<MovementController>();
        visionController = gameObject.GetComponent<VisionController>();
        cameraController.enabled = photonView.isMine;
        movementController.enabled = photonView.isMine;        
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
        SyncRemoteCharacter();
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
            currentSpeed = (float)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
        }
    }

    private void SyncRemoteCharacter(){
        if (!photonView.isMine) {
            syncTime += Time.deltaTime;
            //Check if able to smooth (last argument probably should be parameterized based on:
            //                                  Vector3.Distance(transform.position, correctPlayerPosition);)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, 0);

            Utilities.Instance.LogMessage("syncDelay: " + syncDelay);
            Utilities.Instance.LogMessage("syncTime: " + syncTime);
            Utilities.Instance.LogMessage("syncTime / syncDelay: " + syncTime / syncDelay);
        }
    }

    #region RPCs
    //Stats
    [RPC]
    private void SyncCharacterStat(int _index, int _baseValue, int _buffValue) {
        model.GetStat(_index).BaseValue = _baseValue;
        model.GetStat(_index).BuffValue = _buffValue;
    }
    [RPC]
    private void SyncCharacterAttribute(int _index, int _baseValue, int _buffValue) {
        model.GetAttribute(_index).BaseValue = _baseValue;
        model.GetAttribute(_index).BuffValue = _buffValue;
    }
    [RPC]
    private void SyncCharacterVital(int _index, int _baseValue, int _buffValue, int _currentValue) {
        model.GetVital(_index).BaseValue = _baseValue;
        model.GetVital(_index).BuffValue = _buffValue;
        model.GetVital(_index).CurrentValue = _currentValue;
    }
    [RPC]
    private void SyncAttributesBasedOnStats() {
        model.UpdateAttributes();
    }    
#endregion

    #region Accessors
    public PhotonView PhotonView{
        get { return photonView; }
    }
    public PlayerCharacterModel Model {
        get { return model; }
    }
    public CameraController CameraController {
        get { return cameraController; }
    }
    public MovementController MovementController {
        get { return movementController; }
    }
    public VisionController VisionController {
        get { return visionController; }
    }
#endregion
}