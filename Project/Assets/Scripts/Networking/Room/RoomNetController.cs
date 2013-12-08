using UnityEngine;
using System.Collections.Generic;

public class RoomNetController: BaseNetController {

    private const float RPC_COOLDOWN_TIME = 1.0f;

    private float lastRPCTime;
    
    public override void Awake() {
        base.Awake();
        lastRPCTime = Time.time;
    }

    void Update() {
        if (IsMasterClient && (Time.time - lastRPCTime > RPC_COOLDOWN_TIME)) {
            SyncGameVariables();
            lastRPCTime = Time.time;
        }
    }

    #region Game preferences
    public void SyncGameVariables() {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "SyncGameVariables", "This function is only available for the master client.");
        photonView.RPC("SetGameVariables", PhotonTargets.Others, GameVariables.Instance.Title, GameVariables.Instance.Mode.Key, GameVariables.Instance.Difficulty.Key,
                        GameVariables.Instance.MaxPlayers.Key, GameVariables.Instance.TargetKills.Key, GameVariables.Instance.Timer.Key);

        PhotonNetwork.room.name = GameVariables.Instance.Title;
        PhotonNetwork.room.maxPlayers = GameVariables.Instance.MaxPlayers.Value;
        PhotonNetwork.room.customProperties["Mode"] = GameVariables.Instance.Mode.Value;
        PhotonNetwork.room.customProperties["Difficulty"] = GameVariables.Instance.Difficulty.Value;
        PhotonNetwork.room.customProperties["Target kills"] = GameVariables.Instance.TargetKills.Value;
        PhotonNetwork.room.customProperties["Timer"] = GameVariables.Instance.Timer.Value;
    }

    #region RPCs
    [RPC]
    private void SetGameVariables(string _title, string _mode, string _difficulty, string _maxPlayers, string _targetKills, string _timer) {
        GameVariables.Instance.Title = _title;
        GameVariables.Instance.Mode = new KeyValuePair<string, GameMode>(_mode, GameVariables.Instance.AvailableModes[_mode]);
        GameVariables.Instance.Difficulty = new KeyValuePair<string, GameDifficulty>(_difficulty, GameVariables.Instance.AvailableDifficulties[_difficulty]);
        GameVariables.Instance.MaxPlayers = new KeyValuePair<string, int>(_maxPlayers, GameVariables.Instance.AvailableMaxPlayers[_maxPlayers]);
        GameVariables.Instance.TargetKills = new KeyValuePair<string, int>(_targetKills, GameVariables.Instance.AvailableTargetKills[_targetKills]);
        GameVariables.Instance.Timer = new KeyValuePair<string, double>(_timer, GameVariables.Instance.AvailableTimers[_timer]);
    }
    #endregion
    #endregion

    #region Player Slots
    public void MasterClientRequestForRoomState() {
        if (!IsMasterClient)
            photonView.RPC("RequestForRoomState", PhotonNetwork.masterClient);
    }

    public void MasterClientPlayerToSlot(int _slotNum, string _playerName) {
        if (IsMasterClient)
            BroadCastPlayerToSlot(_slotNum, _playerName);
        else
            photonView.RPC("BroadCastPlayerToSlot", PhotonNetwork.masterClient, _slotNum, _playerName);
    }

    public void MasterClientClearSlot(int _slotNum, string _playerName) {
        if (IsMasterClient)
            BroadCastClearSlot(_slotNum, _playerName);
        else
            photonView.RPC("BroadCastClearSlot", PhotonNetwork.masterClient, _slotNum, _playerName);
    }

    #region RPCs
    //all room slots
    [RPC]
    private void RequestForRoomState(PhotonMessageInfo info) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]MasterClientRequestForRoomState", "This RPC is only available for the master client.");
        for (int i = 0; i < MainRoomModel.Instance.PlayerSlotsLength; ++i )
            photonView.RPC("SetPlayerToSlot", info.sender, i, MainRoomModel.Instance.GetPlayerNameInSlot(i));
    }
    //player in slot
    [RPC]
    private void BroadCastPlayerToSlot(int _slotNum, string _playerName) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastPlayerToSlot", "This RPC is only available for the master client.");
        if (MainRoomModel.Instance.IsSlotEmpty(_slotNum)) {
            SetPlayerToSlot(_slotNum, _playerName);
            photonView.RPC("SetPlayerToSlot", PhotonTargets.Others, _slotNum, _playerName);
        }
    }

    [RPC]
    private void SetPlayerToSlot(int _slotNum, string _playerName) {        
        MainRoomModel.Instance.SetPlayerNameInSlot(_slotNum, _playerName);
        if (GameManager.Instance.MyPlayer.name.Equals(_playerName))
            MainRoomModel.Instance.MySlot = (PlayerColor)_slotNum;
    }
    //clear slot
    [RPC]
    private void BroadCastClearSlot(int _slotNum, string _playerName) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastClearSlot", "This RPC is only available for the master client.");
        Utilities.Instance.LogMessage("SlotOwnedByPlayer: " + MainRoomModel.Instance.SlotOwnedByPlayer(_slotNum, _playerName));
        Utilities.Instance.LogMessage("GetPlayerNameInSlot: " + MainRoomModel.Instance.GetPlayerNameInSlot(_slotNum));

        if (MainRoomModel.Instance.SlotOwnedByPlayer(_slotNum, _playerName)) {
            ClearSlot(_slotNum);
            photonView.RPC("ClearSlot", PhotonTargets.Others, _slotNum);
        }
    }

    [RPC]
    private void ClearSlot(int _slotNum) {
        MainRoomModel.Instance.EmptySlot(_slotNum);        
    }
    #endregion
    #endregion
}
