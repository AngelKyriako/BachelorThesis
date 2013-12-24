using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private CombatManager() { }

    #region Local methods
    public bool IsAlly(string _name) {
        return GameManager.Instance.IsAlly(_name);
    }

    public bool AreAllies(string _name1, string _name2) {
        return ((PlayerTeam)GameManager.Instance.GetPlayer(_name1).customProperties["Team"]).Equals(
                (PlayerTeam)GameManager.Instance.GetPlayer(_name2).customProperties["Team"]);
    }
    #endregion

    #region Messages to Master Client
    public void MasterClientInstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        if (PhotonNetwork.isMasterClient)
            GameManager.Instance.MyNetworkController.InstantiateSceneObject(_obj, _position, _rotation);
        else
            GameManager.Instance.MyPhotonView.RPC("InstantiateSceneObject", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation);
    }

    public void MasterClientInstantiateSceneSkill(string _obj, Vector3 _position, Quaternion _rotation,
                                                  int _skillId, string _casterName, Vector3 _destination) {
        if (PhotonNetwork.isMasterClient)
            GameManager.Instance.MyNetworkController.InstantiateSceneSkill(_obj, _position, _rotation, _skillId, _casterName, _destination);
        else
            GameManager.Instance.MyPhotonView.RPC("InstantiateSceneSkill", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation, _skillId, _casterName, _destination);
    }

    public void MasterClientDestroySceneObject(GameObject _obj) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "MasterClientDestroySceneObject", "This method is only available for the master client.");
        PhotonNetwork.Destroy(_obj);
    }

    public void DeadPlayerBroadCastKill(string _killerName, string _deadName, Vector3 _position) {
        Utilities.Instance.PreCondition(GameManager.Instance.ItsMe(_deadName), "CombatManager", "DeadPlayerBroadCastKill", "This method is only available for the master client.");
        GameManager.Instance.MyPhotonView.RPC("PlayerKill", GameManager.Instance.GetPlayer(_killerName), _killerName, _deadName, _position);
    }

    public void DeadPlayerBroadCastDeath(string _deadName) {
        Utilities.Instance.PreCondition(GameManager.Instance.ItsMe(_deadName), "CombatManager", "DeadPlayerBroadCastDeath", "This method is only available for the master client.");
        GameManager.Instance.MyPhotonView.RPC("PlayerDeath", PhotonTargets.Others, _deadName);
    }

    #endregion
}
