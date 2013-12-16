using UnityEngine;
using System.Collections;

public class PlayerCharacterNetworkController: SerializableNetController {

    #region constants
    private const float STAT_SYNC_DELAY = 10f, ATTRIBUTES_SYNC_DELAY = 1.5f;
    #endregion

    #region attributes
    // references to local components
    private PlayerCharacterModel model;
    private MovementController movementController;
    private VisionController visionController;
    // stat sync delay
    private float statSyncTimer, attributeSyncTimer;
#endregion

    public override void Awake() {        
        base.Awake();

        name = photonView.owner.ID.ToString();

        transform.parent = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterPath).transform;

        GameManager.Instance.AddPlayerCharacter(name, photonView.owner);
        if (IsLocalClient && !PhotonNetwork.isMasterClient) {
            GameManager.Instance.Me = GameManager.Instance.GetPlayerCharacterPair(name);
            GameManager.Instance.MasterClientRequestConnectedPlayers();            
        }
        else if (IsLocalClient && PhotonNetwork.isMasterClient)
            GameManager.Instance.MasterClient = GameManager.Instance.Me = GameManager.Instance.GetPlayerCharacterPair(name);
        enabled = false;
    }

    public void SetUp() {
        gameObject.GetComponent<ColorPicker>().enabled = true;

        model = gameObject.GetComponent<PlayerCharacterModel>();
        model.enabled = true;

        movementController = gameObject.GetComponent<MovementController>();
        movementController.enabled = true;

        visionController = gameObject.GetComponent<VisionController>();
        visionController.SetUp(CombatManager.Instance.IsAlly(name), GameVariables.Instance.Difficulty.Value);

        gameObject.GetComponent<PlayerCharacterDeathController>().Setup(IsLocalClient);

        if (IsLocalClient)
            model.AddListeners();

        enabled = true;
    }

    public override void Start() {
        base.Start();
        statSyncTimer = 0f;
        attributeSyncTimer = 0f;
    }

    public void OnDestroy() {
        GameManager.Instance.RemovePlayerCharacter(name);
    }

    public override void Update() {
        base.Update();
        if (IsLocalClient) {
            if(statSyncTimer > STAT_SYNC_DELAY) {
                SendCharacterStats();
                statSyncTimer = 0f;
            }
            if (attributeSyncTimer > ATTRIBUTES_SYNC_DELAY) {
                SendCharacterAttributes();
                attributeSyncTimer = 0f;
            }
            statSyncTimer += Time.deltaTime;
            attributeSyncTimer += Time.deltaTime;
        }
    }

    public override void SendData(PhotonStream _stream) {
        if (enabled) {
            base.SendData(_stream);
            //_stream.SendNext(rigidbody.velocity);
            _stream.SendNext(movementController.AnimatorMovementSpeed);
        }
    }

    public override void ReceiveData(PhotonStream _stream) {
        if (enabled) {
            base.ReceiveData(_stream);
            //rigidbody.velocity = (Vector3)_stream.ReceiveNext();
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
        //Utilities.Instance.LogMessageToChat("local client is: " + photonView.name);
        //Utilities.Instance.LogMessageToChat("local client owner is: " + photonView.owner.name);
        if (photonView.Equals(_receiver.photonView))
            _receiver.AttachEffect(_caster.name, _receiver.name, _effectTitle);
        else {
            //Utilities.Instance.LogMessageToChat("Send RPC TO !!!");
            //Utilities.Instance.LogMessageToChat("remote client is: " + _receiver.photonView.name);
            //Utilities.Instance.LogMessageToChat("remote client owner is: " + _receiver.photonView.owner.name);
            photonView.RPC("AttachEffect", _receiver.photonView.owner, _caster.name, _receiver.name, _effectTitle);
        }
    }

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

    #region Effects RPCs
    [RPC]
    public void AttachEffect(string _casterName, string _receiverName, string _effectTitle) {
        GameManager.Instance.LogMessageToMasterClient(_receiverName + ": just attached to himself the effect " + _effectTitle + ", of caster: " + _casterName);
        BaseEffect effectToAttach = EffectBook.Instance.GetEffect(_effectTitle);
        BaseEffect tempEffect = (BaseEffect)GameManager.Instance.GetCharacter(_receiverName).AddComponent(effectToAttach.GetType());
        tempEffect.SetUpEffect(GameManager.Instance.GetPlayerModel(_casterName), effectToAttach);
    }
    #endregion

    #region Accessors
    public PlayerCharacterModel Model {
        get { return model; }
    }
    public MovementController MovementController {
        get { return movementController; }
    }
    public VisionController VisionController {
        get { return visionController; }
    }
    #endregion
}