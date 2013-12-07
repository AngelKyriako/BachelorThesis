using UnityEngine;
using System.Collections;

public class RoomNetController: BaseNetController {

    public override void Awake() {
        base.Awake();
    }

    public void MasterClientRequestForRoomState() {
        if (!IsMasterClient)
            photonView.RPC("RequestForRoomState", PhotonNetwork.masterClient);
    }

    public void MasterClientPlayerToSlot(PlayerColor _slot, string _playerName) {
        if (IsMasterClient)
            BroadCastPlayerToSlot((int)_slot, _playerName);
        else
            photonView.RPC("BroadCastPlayerToSlot", PhotonNetwork.masterClient, (int)_slot, _playerName);
    }

    public void MasterClientClearSlot(PlayerColor _slot) {
        if (IsMasterClient)
            BroadCastClearSlot((int)_slot);
        else
            photonView.RPC("BroadCastClearSlot", PhotonNetwork.masterClient, (int)_slot);
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
    private void BroadCastPlayerToSlot(int _slot, string _playerName) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastPlayerToSlot", "This RPC is only available for the master client.");
        if (MainRoomModel.Instance.IsSlotEmpty(_slot)) {
            SetPlayerToSlot(_slot, _playerName);
            photonView.RPC("SetPlayerToSlot", PhotonTargets.Others, _slot, _playerName);
        }
    }

    [RPC]
    private void SetPlayerToSlot(int _slot, string _playerName) {        
        MainRoomModel.Instance.SetPlayerNameInSlot(_slot, _playerName);
    }
    //clear slot
    [RPC]
    private void BroadCastClearSlot(int _slot) {
        Utilities.Instance.PreCondition(IsMasterClient, "RoomNetController", "[RPC]BroadCastClearSlot", "This RPC is only available for the master client.");
        ClearSlot(_slot);
        photonView.RPC("ClearSlot", PhotonTargets.Others, _slot);
    }

    [RPC]
    private void ClearSlot(int _slot) {
        MainRoomModel.Instance.EmptySlot(_slot);        
    }
    #endregion
}
