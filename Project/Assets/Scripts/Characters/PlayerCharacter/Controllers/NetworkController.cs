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
    private float syncTime, syncDelay, lastSynchronizationTime;
    private Vector3 correctPlayerPosition;
    private Quaternion correctPlayerRotation;
#endregion

    void Awake() {
        model = gameObject.GetComponent<PlayerCharacterModel>();
        cameraController = gameObject.GetComponent<CameraController>();
        movementController = gameObject.GetComponent<MovementController>();
        visionController = gameObject.GetComponent<VisionController>();
        cameraController.enabled = IsLocalClient;
        movementController.enabled = true;
        visionController.enabled = IsLocalClient;//@TODO: for team play this should be only for enemies

        transform.parent = GameObject.Find("Characters/BabyDragons").transform;
        name = photonView.viewID.ToString();
        if (IsLocalClient) {
            GameManager.Instance.Me = new PlayerCharacterPair(photonView.owner, gameObject);
            GameManager.Instance.HostAddToAll(photonView.name);
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Allies"));
        }
        else {
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("HiddenEnemies"));//@TODO Must be called when game starts when everyone is connected
        }
    }

    void Start() {
        model.SetUpModel(photonView.name);

        correctPlayerPosition = transform.position;
        correctPlayerRotation = Quaternion.identity;

        syncTime = syncDelay = 0.001f;
        lastSynchronizationTime = Time.time;
    }

    void Update() {
        if (PhotonNetwork.connectionState.Equals(ConnectionState.Connected)) {
            if (!photonView.isMine)
                SyncWithRemoteCharacter();
            else if (statSyncTimer > statSyncDelay) {
                SendCharacterStats();
                SendCharacterAttributes();
                statSyncTimer = 0;
            }
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

            syncTime = 0.001f;
            syncDelay = (Time.time - lastSynchronizationTime)+0.001f;
            lastSynchronizationTime = Time.time;
        }
    }

    private void SyncWithRemoteCharacter() {
        Utilities.Instance.PreCondition((!float.IsNaN(correctPlayerPosition.x) && !float.IsNaN(correctPlayerPosition.y) && !float.IsNaN(correctPlayerPosition.z)),
                                        "NetworkController", "SyncRemoteCharacter", "Fucking NaN Values: " + correctPlayerPosition);
        Utilities.Instance.PreCondition((!float.IsNaN(correctPlayerRotation.x) && !float.IsNaN(correctPlayerRotation.y) && !float.IsNaN(correctPlayerRotation.z) && !float.IsNaN(correctPlayerRotation.w)),
                                        "NetworkController", "SyncRemoteCharacter", "Fucking NaN Values: " + correctPlayerRotation);
        Utilities.Instance.PreCondition(!float.IsNaN(syncTime / syncDelay), "NetworkController", "SyncRemoteCharacter", "Fucking NaN!!! syncTime: " + syncTime + ", syncDelay" + syncDelay);

        transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, syncTime / syncDelay);
        transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRotation, syncTime / syncDelay);

        syncTime += Time.deltaTime;
    }

    private void SendCharacterStats() {
        for (int i = 0; i < model.StatsLength; ++i)
            photonView.RPC("SyncCharacterStat", PhotonTargets.Others,
                            i, model.GetStat(i).BaseValue, model.GetStat(i).BuffValue);
        photonView.RPC("SyncAttributesBonusStatValues", PhotonTargets.Others);
    }
    private void SendCharacterAttributes() {
        for (int i = 0; i < model.AttributesLength; ++i)
            photonView.RPC("SyncCharacterAttribute", PhotonTargets.Others,
                            i, model.GetAttribute(i).BaseValue, model.GetAttribute(i).BuffValue);
        for (int i = 0; i < model.VitalsLength; ++i)
            photonView.RPC("SyncCharacterVital", PhotonTargets.Others,
                           i, model.GetVital(i).BaseValue, model.GetVital(i).BuffValue, model.GetVital(i).CurrentValue);
    }

    public void AttachEffectToRemotePlayer(PhotonView _receiverView, BaseEffect _effect, BaseCharacterModel _caster) {
        Utilities.Instance.LogMessage(_caster.Name + " attaching the effect <" + _effect.Title + "> to: " + _receiverView.owner.name);
        photonView.RPC("AttachEffect", _receiverView.owner, _effect.Title, _caster.Name);
    }

    #region RPCs

    #region Effects
    [RPC]
    private void AttachEffect(string _effectTitle, string _casterName) {
        BaseEffect effectToAttach = EffectBook.Instance.GetEffect(_effectTitle);
        BaseEffect tempEffect = (BaseEffect)gameObject.AddComponent(effectToAttach.GetType());
        Utilities.Instance.LogMessage("Effect attached and ready to setup");
        tempEffect.SetUpEffect(GameManager.Instance.GetPlayerModel(_casterName), EffectBook.Instance.GetEffect(_effectTitle));
        //@TODO: SOMEHOW Get the corrent PlayerCharacterModel from the master client
    }
    #endregion

    #region Stats
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

#endregion

    #region Accessors    
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
    public bool IsLocalClient {
        get { return photonView.isMine || PhotonNetwork.connectionState.Equals(ConnectionState.Disconnected); }
    }
#endregion
}