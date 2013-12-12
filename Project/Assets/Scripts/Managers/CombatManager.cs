using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private CombatManager() { }

    #region Local methods
    public bool IsAlly(string _name) {
        return ((PlayerTeam)GameManager.Instance.MyPlayer.customProperties["Team"]).Equals(
                (PlayerTeam)GameManager.Instance.GetPlayer(_name).customProperties["Team"]);
    }

    public bool AreAllies(string _name1, string _name2) {
        return ((PlayerTeam)GameManager.Instance.GetPlayer(_name1).customProperties["Team"]).Equals(
                (PlayerTeam)GameManager.Instance.GetPlayer(_name2).customProperties["Team"]);
    }
    #endregion

    #region Messages to Master Client
    public void MasterClientInstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneObject(_obj, _position, _rotation);
        else
            photonView.RPC("InstantiateSceneObject", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation);
    }

    public void MasterClientInstantiateSceneSkill(string _obj, Vector3 _position, Quaternion _rotation,
                                                  string _skillName, string _casterName, Vector3 _destination) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneSkill(_obj, _position, _rotation, _skillName, _casterName, _destination);
        else
            photonView.RPC("InstantiateSceneSkill", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation, _skillName, _casterName, _destination);
    }

    public void MasterClientDestroySceneObject(GameObject _obj) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "MasterClientDestroySceneObject", "This method is only available for the master client.");
        PhotonNetwork.Destroy(_obj);
    }

    #region RPCs
    [RPC]
    private void InstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        //Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name + " is instantiating scene object !");
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
    }

    [RPC]
    private void InstantiateSceneSkill(string _obj, Vector3 _position, Quaternion _rotation,
                                       string _skillName, string _casterName, Vector3 _destination) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        GameObject obj = PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
        obj.GetComponent<BaseSkillController>().SetUp(SkillBook.Instance.GetSkill(_skillName),
                                                      GameManager.Instance.GetPlayerModel(_casterName),
                                                      _destination);
    }
    #endregion
    #endregion

    #region Messages to all

    public void KillHappened(string _killerName, string _deadName, Vector3 _position) {
        MasterClientInstantiateSceneObject(ResourcesPathManager.Instance.ExpRadiusSphere, _position, Quaternion.identity);
        photonView.RPC("KilledEnemyPlayer", GameManager.Instance.GetPlayer(_killerName), _deadName);
    }

    #region RPCs
    [RPC]
    private void KilledEnemyPlayer(string _killedEnemyName) {
        GameManager.Instance.MyCharacterModel.KilledEnemy(GameManager.Instance.GetPlayerModel(_killedEnemyName));
    }
    #endregion
    #endregion
}
