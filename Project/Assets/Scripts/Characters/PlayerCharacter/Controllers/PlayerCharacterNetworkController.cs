using UnityEngine;
using System.Collections;

public class PlayerCharacterNetworkController: BaseNetworkController {

    #region attributes
    // references to local gameObjects
    private PlayerCharacterModel model;
    private CameraController cameraController;
    private MovementController movementController;
    private VisionController visionController;
    // stat sync delay
    private float statSyncDelay = 1.5f, statSyncTimer = 0f;
#endregion

    public override void Awake() {
        base.Awake();
        transform.parent = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterPath).transform;

        model = gameObject.GetComponent<PlayerCharacterModel>();
        cameraController = gameObject.GetComponent<CameraController>();
        movementController = gameObject.GetComponent<MovementController>();
        visionController = gameObject.GetComponent<VisionController>();
        cameraController.enabled = IsLocalClient;
        movementController.enabled = true;
        visionController.enabled = IsLocalClient;//@TODO: for team play this should be only for enemies

        GameManager.Instance.AddPlayerCharacter(photonView.name, photonView.owner);
        if (IsLocalClient) {
            GameManager.Instance.Me = new PlayerCharacterPair(photonView.owner, gameObject);
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Allies"));
            if (!PhotonNetwork.isMasterClient)
                GameManager.Instance.RequestConnectedPlayerCharacters();
            else
                GameManager.Instance.MasterClient = new PlayerCharacterPair(photonView.owner, gameObject);
        }
        else {
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("HiddenEnemies"));//@TODO Must be called when game starts when everyone is connected
        }
    }

    public override void Start() {
        base.Start();
        if(IsLocalClient)
            model.SetUpModel();
    }

    public void OnDestroy() {
        GameManager.Instance.RemovePlayerCharacter(name);
    }

    public override void Update() {
        base.Update();
        if (IsLocalClient && statSyncTimer > statSyncDelay) {
            SendCharacterStats();
            SendCharacterAttributes();
            statSyncTimer = 0;
        }
        statSyncTimer += Time.deltaTime;
    }

    public override void SendData(PhotonStream _stream) {
        base.SendData(_stream);
        _stream.SendNext(rigidbody.velocity);
        _stream.SendNext(movementController.AnimatorMovementSpeed);
    }

    public override void ReceiveData(PhotonStream _stream) {
        base.ReceiveData(_stream);
        rigidbody.velocity = (Vector3)_stream.ReceiveNext();
        movementController.AnimatorMovementSpeed = (float)_stream.ReceiveNext();
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

    public void AttachEffectToPlayer(PlayerCharacterNetworkController _receiver, string _effectTitle, string _casterName) {
        PhotonPlayer _receiverPlayer = _receiver.photonView.owner;
        Utilities.Instance.LogMessage("local client is: " + photonView.name);
        Utilities.Instance.LogMessage("local client owner is: " + photonView.owner.name);
        Utilities.Instance.LogMessage("remote client is masterclient: " + _receiverPlayer.isMasterClient);
        if (photonView.Equals(_receiver.photonView))
            _receiver.AttachEffect(_effectTitle, _casterName);
        else {
            Utilities.Instance.LogMessage("master client is not the receiver, RPC this to " + _receiverPlayer.name);
            photonView.RPC("AttachEffect", _receiverPlayer, _effectTitle, _casterName);
        }
    }
    
    public void LogMessageToMasterClient(string _str){
        photonView.RPC("PrintShit", PhotonNetwork.masterClient, _str);
    }
    [RPC]
    void PrintShit(string _str, PhotonMessageInfo info) {
        Utilities.Instance.LogMessage(info.sender.name +" sent: "+ _str);
    }

    #region Effects RPCs
    [RPC]
    private void AttachEffect(string _effectTitle, string _casterName) {
        BaseEffect effectToAttach = EffectBook.Instance.GetEffect(_effectTitle);
        BaseEffect tempEffect = (BaseEffect)gameObject.AddComponent(effectToAttach.GetType());
        tempEffect.SetUpEffect(GameManager.Instance.GetPlayerModel(_casterName), effectToAttach);
        LogMessageToMasterClient(photonView.name + " just attached to themselves the effect" + _effectTitle + ", of caster" + _casterName);
    }

    #endregion

    #region Stats RPCs
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