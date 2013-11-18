using UnityEngine;
using System.Collections;

public class NetworkController: Photon.MonoBehaviour {

    #region attributes
    // references to local gameObjects
    private PlayerCharacterModel model;
    private CameraController cameraController;
    private MovementController movementController;
    private VisionController visionController;
    // stat sync delay
    private float statSyncDelay = 1.5f, statSyncTimer = 0f;
    // interpolation shit
    private float syncTime = 0f, syncDelay = 0f, lastSynchronizationTime = 0f;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
#endregion

    void Awake() {
        model = gameObject.GetComponent<PlayerCharacterModel>();
        cameraController = gameObject.GetComponent<CameraController>();
        movementController = gameObject.GetComponent<MovementController>();
        visionController = gameObject.GetComponent<VisionController>();
        cameraController.enabled = photonView.isMine;
        movementController.enabled = true;
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
    }

    void Update() {
        if (!photonView.isMine)
            SyncRemoteCharacter();
        else if (statSyncTimer > statSyncDelay) {
            SyncCharacterStats();
            SyncCharacterAttributes();
            statSyncTimer = 0;
        }
        statSyncTimer += Time.deltaTime;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // send the local character's data
            //positioning
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.velocity);
            //animating
            stream.SendNext(movementController.AnimatorMovementSpeed);
        }
        else { // receive data from remote characters
            //positioning
            correctPlayerPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRotation = (Quaternion)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();
            //animating
            movementController.AnimatorMovementSpeed = (float)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = (Time.time - lastSynchronizationTime)+0.001f;
            lastSynchronizationTime = Time.time;
        }
    }

    private void SyncRemoteCharacter(){

        transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, syncTime / syncDelay);
        transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, syncTime / syncDelay);

        syncTime += Time.deltaTime;
    }

    private void SyncCharacterStats() {
        for (int i = 0; i < model.StatsLength; ++i)
            photonView.RPC("SyncCharacterStat", PhotonTargets.Others,
                            i, model.GetStat(i).BaseValue, model.GetStat(i).BuffValue);
        photonView.RPC("SyncAttributesBonusStatValues", PhotonTargets.Others);
    }
    private void SyncCharacterAttributes() {
        for (int i = 0; i < model.AttributesLength; ++i)
            photonView.RPC("SyncCharacterAttribute", PhotonTargets.Others,
                            i, model.GetAttribute(i).BaseValue, model.GetAttribute(i).BuffValue);
        for (int i = 0; i < model.VitalsLength; ++i)
            photonView.RPC("SyncCharacterVital", PhotonTargets.Others,
                           i, model.GetVital(i).BaseValue, model.GetVital(i).BuffValue, model.GetVital(i).CurrentValue);
    }

    #region RPCs
    [RPC]
    private void SyncCharacterStat(int _index, float _baseValue, float _buffValue) {
        model.GetStat(_index).BaseValue = _baseValue;
        model.GetStat(_index).BuffValue = _buffValue;
    }
    [RPC]
    private void SyncCharacterAttribute(int _index, float _baseValue, float _buffValue) {
        model.GetAttribute(_index).BaseValue = _baseValue;
        model.GetAttribute(_index).BuffValue = _buffValue;
    }
    [RPC]
    private void SyncCharacterVital(int _index, float _baseValue, float _buffValue, int _currentValue) {
        model.GetVital(_index).BaseValue = _baseValue;
        model.GetVital(_index).BuffValue = _buffValue;
        model.GetVital(_index).CurrentValue = _currentValue;        
    }
    [RPC]
    private void SyncAttributesBonusStatValues() {
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