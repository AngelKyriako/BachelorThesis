using UnityEngine;
using System.Collections.Generic;

public class RoomNetController: BaseNetController {

    private const float RPC_COOLDOWN_TIME = 2.0f;

    private float lastPropertySyncTime;
    
    public override void Awake() {
        base.Awake();
        enabled = false;
    }

    void Start() {
        RequestForRoomState(GameManager.Instance.MyPlayer);
        lastPropertySyncTime = Time.time;        
    }

    void Update() {
        if (IsMasterClient && (Time.time - lastPropertySyncTime > RPC_COOLDOWN_TIME)) {
            SyncGameVariables();
            for (int i = 0; i < MainRoomModel.Instance.PlayerSlotsLength; ++i) {
                //@TODO: FIX THAT SHIT
                //if (!MainRoomModel.Instance.IsSlotEmpty(i) && !GameManager.Instance.PlayerExists(MainRoomModel.Instance.GetPlayerInSlot(i).ID.ToString())) {
                //    Utilities.Instance.LogMessageToChat("Yo its: " + MainRoomModel.Instance.GetPlayerInSlot(i).ID +" and I am gonna leave now");
                //    Utilities.Instance.LogMessageToChat("Because " + (PlayerColor)i + " is not empty");
                //    MasterClientClearSlot(i, MainRoomModel.Instance.GetPlayerInSlot(i));
                //}
            }
            lastPropertySyncTime = Time.time;
        }
    }

    void OnLeftRoom() {
        if(PhotonNetwork.connected)
            MasterClientClearSlot((int)MainRoomModel.Instance.MySlot, GameManager.Instance.MyPlayer);
    }

    #region Game preferences
    public void SyncGameVariables() {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "SyncGameVariables", "This function is only available for the master client.");
        photonView.RPC("SetGameVariables", PhotonTargets.Others, GameVariables.Instance.Title, GameVariables.Instance.Mode.Key,
                        GameVariables.Instance.Map.Key, GameVariables.Instance.Difficulty.Key, GameVariables.Instance.MaxPlayers.Key,
                        GameVariables.Instance.TargetKills.Key, GameVariables.Instance.Timer.Key);

        PhotonNetwork.room.name = GameVariables.Instance.Title;
        PhotonNetwork.room.maxPlayers = GameVariables.Instance.MaxPlayers.Value;
        PhotonNetwork.room.customProperties["Mode"] = GameVariables.Instance.Mode.Value;
        PhotonNetwork.room.customProperties["Map"] = GameVariables.Instance.Map.Value;
        PhotonNetwork.room.customProperties["Difficulty"] = GameVariables.Instance.Difficulty.Value;
        PhotonNetwork.room.customProperties["Target kills"] = GameVariables.Instance.TargetKills.Value;
        PhotonNetwork.room.customProperties["Timer"] = GameVariables.Instance.Timer.Value;
        GameManager.Instance.UpdateRoomProperties();
    }

    #region RPCs
    [RPC]
    private void SetGameVariables(string _title, string _mode, string _map, string _difficulty, string _maxPlayers, string _targetKills, string _timer) {
        GameVariables.Instance.Title = _title;
        GameVariables.Instance.Mode = new KeyValuePair<string, GameMode>(_mode, GameVariables.Instance.AvailableModes[_mode]);
        GameVariables.Instance.Map = new KeyValuePair<string, GameMap>(_map, GameVariables.Instance.AvailableMaps[_map]);
        GameVariables.Instance.Difficulty = new KeyValuePair<string, GameDifficulty>(_difficulty, GameVariables.Instance.AvailableDifficulties[_difficulty]);
        GameVariables.Instance.MaxPlayers = new KeyValuePair<string, int>(_maxPlayers, GameVariables.Instance.AvailableMaxPlayers[_maxPlayers]);
        GameVariables.Instance.TargetKills = new KeyValuePair<string, int>(_targetKills, GameVariables.Instance.AvailableTargetKills[_targetKills]);
        GameVariables.Instance.Timer = new KeyValuePair<string, double>(_timer, GameVariables.Instance.AvailableTimers[_timer]);
    }
    #endregion
    #endregion

    #region Player Slots
    private void RequestForRoomState(PhotonPlayer _playerRequested) {
        if (!IsMasterClient)
            photonView.RPC("MasterClientRequestForRoomState", PhotonNetwork.masterClient, _playerRequested);
    }

    public void MasterClientPlayerToSlot(int _slotNum, PhotonPlayer _player) {
        if (IsMasterClient)
            BroadCastPlayerToSlot(_slotNum, _player);
        else
            photonView.RPC("BroadCastPlayerToSlot", PhotonNetwork.masterClient, _slotNum, _player);
    }

    public void MasterClientClearSlot(int _slotNum, PhotonPlayer _player) {
        if (IsMasterClient)
            BroadCastClearSlot(_slotNum, _player);
        else
            photonView.RPC("BroadCastClearSlot", PhotonNetwork.masterClient, _slotNum, _player);
    }

    public void MasterClientKickPlayerInSlot(int _slotNum, PhotonPlayer _player) {
        if (IsMasterClient)
            BroadCastKickPlayer(_slotNum, _player);
    }

    #region RPCs
    //all room slots
    [RPC]
    private void MasterClientRequestForRoomState(PhotonPlayer _playerRequested) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]MasterClientRequestForRoomState", "This RPC is only available for the master client.");
        for (int i = 0; i < MainRoomModel.Instance.PlayerSlotsLength; ++i)
            photonView.RPC("SetPlayerToSlot", _playerRequested, i, MainRoomModel.Instance.GetPlayerInSlot(i));
    }
    //player in slot
    [RPC]
    private void BroadCastPlayerToSlot(int _slotNum, PhotonPlayer _player) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastPlayerToSlot", "This RPC is only available for the master client.");
        if (MainRoomModel.Instance.IsSlotEmpty(_slotNum)) {
            SetPlayerToSlot(_slotNum, _player);
            photonView.RPC("SetPlayerToSlot", PhotonTargets.Others, _slotNum, _player);
        }
    }

    [RPC]
    private void SetPlayerToSlot(int _slotNum, PhotonPlayer _player) {
        MainRoomModel.Instance.SetPlayerInSlot(_slotNum, _player);
        if (GameManager.Instance.MyPlayer.Equals(_player)) {
            MainRoomModel.Instance.MySlot = (PlayerColor)_slotNum;
            _player.customProperties["Color"] = (PlayerColor)_slotNum;
            GameManager.Instance.UpdatePlayerColorProperty();
        }
    }
    //clear slot
    [RPC]
    private void BroadCastClearSlot(int _slotNum, PhotonPlayer _player) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastClearSlot", "This RPC is only available for the master client.");        
        if (_slotNum != (int)PlayerColor.None && MainRoomModel.Instance.SlotOwnedByPlayer(_slotNum, _player)) {
            ClearSlot(_slotNum, _player);
            photonView.RPC("ClearSlot", PhotonTargets.Others, _slotNum, _player);
        }
    }

    [RPC]
    private void ClearSlot(int _slotNum, PhotonPlayer _player) {
        if (GameManager.Instance.MyPlayer.Equals(_player)) {
            MainRoomModel.Instance.MySlot = PlayerColor.None;
            _player.customProperties["Color"] = MainRoomModel.Instance.MySlot;
        }
        MainRoomModel.Instance.EmptySlot(_slotNum);
    }

    //kick player
    [RPC]
    private void BroadCastKickPlayer(int _slotNum, PhotonPlayer _player) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastClearSlot", "This RPC is only available for the master client.");
        _player.customProperties["Color"] = PlayerColor.None;
        if (_slotNum != (int)PlayerColor.None) {
            ClearSlot(_slotNum, _player);
            photonView.RPC("ClearSlot", PhotonTargets.Others, _slotNum, _player);
            photonView.RPC("LeaveRoom", _player);
        }
    }

    [RPC]
    private void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }

    #endregion
    #endregion
}
