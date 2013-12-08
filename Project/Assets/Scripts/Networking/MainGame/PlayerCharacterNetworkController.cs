using UnityEngine;
using System.Collections;

public class PlayerCharacterNetworkController: SerializableNetController {

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
        GameManager.Instance.AddPlayerCharacter(photonView.name, photonView.owner);
        if (IsLocalClient && !PhotonNetwork.isMasterClient) {
            GameManager.Instance.Me = new PlayerCharacterPair(photonView.owner, gameObject);
            GameManager.Instance.MasterClientRequestConnectedPlayers();
        }
        else if (IsLocalClient && PhotonNetwork.isMasterClient)
            GameManager.Instance.MasterClient = GameManager.Instance.Me = new PlayerCharacterPair(photonView.owner, gameObject);

        enabled = false;        
    }

    public void SetUp() {
        transform.parent = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterPath).transform;

        model = gameObject.GetComponent<PlayerCharacterModel>();
        model.enabled = true;

        cameraController = gameObject.GetComponent<CameraController>();
        cameraController.enabled = IsLocalClient;

        movementController = gameObject.GetComponent<MovementController>();
        movementController.enabled = true;

        visionController = gameObject.GetComponent<VisionController>();               
        if (IsLocalClient) {//@TODO or are allies with Me
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("Allies"));
            visionController.enabled = true;
        }
        else {
            Utilities.Instance.SetGameObjectLayer(gameObject, LayerMask.NameToLayer("HiddenEnemies"));
            visionController.enabled = false;
        }

        enabled = true;
    }

    public override void Start() {
        base.Start();
        if(IsLocalClient)
            model.AddListeners();
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
        //base.SendData(_stream);
        //_stream.SendNext(rigidbody.velocity);
        //_stream.SendNext(movementController.AnimatorMovementSpeed);
    }

    public override void ReceiveData(PhotonStream _stream) {
        //base.ReceiveData(_stream);
        //rigidbody.velocity = (Vector3)_stream.ReceiveNext();
        //movementController.AnimatorMovementSpeed = (float)_stream.ReceiveNext();
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

    public void AttachEffectToPlayer(PlayerCharacterNetworkController _caster, PlayerCharacterNetworkController _receiver, string _effectTitle) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "PlayerCharacterNetworkController", "AttachEffectToPlayer", "This method is only available for the master client.");
        //Utilities.Instance.LogMessage("local client is: " + photonView.name);
        //Utilities.Instance.LogMessage("local client owner is: " + photonView.owner.name);
        if (photonView.Equals(_receiver.photonView))
            _receiver.AttachEffect(_caster.name, _receiver.name, _effectTitle);
        else {
            //Utilities.Instance.LogMessage("Send RPC TO !!!");
            //Utilities.Instance.LogMessage("remote client is: " + _receiver.photonView.name);
            //Utilities.Instance.LogMessage("remote client owner is: " + _receiver.photonView.owner.name);
            photonView.RPC("AttachEffect", _receiver.photonView.owner, _caster.name, _receiver.name, _effectTitle);
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
    public void AttachEffect(string _casterName, string _receiverName, string _effectTitle) {
        LogMessageToMasterClient(_receiverName + " just attached to themselves the effect" + _effectTitle + ", of caster" + _casterName);
        BaseEffect effectToAttach = EffectBook.Instance.GetEffect(_effectTitle);
        BaseEffect tempEffect = (BaseEffect)GameManager.Instance.GetCharacter(_receiverName).AddComponent(effectToAttach.GetType());
        tempEffect.SetUpEffect(GameManager.Instance.GetPlayerModel(_casterName), effectToAttach);
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
    private void SyncCharacterVital(int _index, float _baseValue, float _buffValue, float _currentValue) {
        model.GetVital(_index).BaseValue = _baseValue;
        model.GetVital(_index).BuffValue = _buffValue;
        model.GetVital(_index).CurrentValue = _currentValue;
    }
    [RPC]
    private void SyncAttributesBonusStatValues() {
        model.UpdateAttributesBasedOnStats();
        model.UpdateVitalsBasedOnStats();
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