using UnityEngine;
using System.Collections;

public class PlayerCharacterNetworkController: SerializableNetController {

    #region attributes
    // references to local components
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

        GameManager.Instance.AddPlayerCharacter(name, photonView.owner);
        if (IsLocalClient && !PhotonNetwork.isMasterClient) {
            GameManager.Instance.Me = GameManager.Instance.GetPlayerCharacterPair(name);
            GameManager.Instance.MasterClientRequestConnectedPlayers();            
        }
        else if (IsLocalClient && PhotonNetwork.isMasterClient)
            GameManager.Instance.MasterClient = GameManager.Instance.Me = GameManager.Instance.GetPlayerCharacterPair(name);
        //Utilities.Instance.LogMessage("me: " + GameManager.Instance.Me);
        //Utilities.Instance.LogMessage("my player: " + GameManager.Instance.MyPlayer.name);
        //Utilities.Instance.LogMessage("my object: " + GameManager.Instance.MyCharacter.name);
        //Utilities.Instance.LogMessage("my model: " + GameManager.Instance.MyCharacterModel.GetVital(0).Name);
        enabled = false;
    }

    public void SetUp() {
        gameObject.GetComponent<ColorPicker>().enabled = true;

        model = gameObject.GetComponent<PlayerCharacterModel>();
        model.enabled = true;

        cameraController = gameObject.GetComponent<CameraController>();
        cameraController.enabled = IsLocalClient;

        movementController = gameObject.GetComponent<MovementController>();
        movementController.enabled = true;

        visionController = gameObject.GetComponent<VisionController>();
        visionController.SetUp(CombatManager.Instance.IsAlly(name), GameVariables.Instance.Difficulty.Value);

        if (IsLocalClient)
            model.AddListeners();

        enabled = true;
    }

    public void OnDestroy() {
        GameManager.Instance.RemovePlayerCharacter(name);
    }

    public override void Update() {
        base.Update();
        if (IsLocalClient && model && statSyncTimer > statSyncDelay) {
            SendCharacterStats();
            SendCharacterAttributes();
            statSyncTimer = 0;
        }
        statSyncTimer += Time.deltaTime;
    }

    public override void SendData(PhotonStream _stream) {
        if (enabled) {
            base.SendData(_stream);
            _stream.SendNext(rigidbody.velocity);
            _stream.SendNext(movementController.AnimatorMovementSpeed);
        }
    }

    public override void ReceiveData(PhotonStream _stream) {
        if (enabled) {
            base.ReceiveData(_stream);
            rigidbody.velocity = (Vector3)_stream.ReceiveNext();
            movementController.AnimatorMovementSpeed = (float)_stream.ReceiveNext();
        }
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
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "PlayerCharacterNetworkController", "[RPC]PrintShit", "This RPC is only available for the master client.");
        Utilities.Instance.LogMessage(info.sender.name +" sent: "+ _str);
    }

    #region Effects RPCs
    [RPC]
    public void AttachEffect(string _casterName, string _receiverName, string _effectTitle) {
        LogMessageToMasterClient(_receiverName + " just attached to themself the effect" + _effectTitle + ", of caster" + _casterName);
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