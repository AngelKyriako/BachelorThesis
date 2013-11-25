using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private CombatManager() { }

    public bool AreAllies(string _name1, string _name2) {
        return GameManager.Instance.GetPlayer(_name1).customProperties["team"].Equals(
               GameManager.Instance.GetPlayer(_name2).customProperties["team"]);
    }

    public void HostInstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneObject(_obj, _position, _rotation);
        else
            photonView.RPC("InstantiateSceneObject", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation);
    }

    public void HostInstantiateSceneProjectile(string _obj, Vector3 _position, Quaternion _rotation,
                                               string _skillName, string _casterName, Vector3 _destination) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneProjectile(_obj, _position, _rotation, _skillName, _casterName, _destination);
        else {
            Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name+" is sending message to masterclient to instantiate a projectile !");
            photonView.RPC("InstantiateSceneProjectile", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation, _skillName, _casterName, _destination);
        }
    }

    public void HostDestroySceneObject(GameObject _obj) {
        if (PhotonNetwork.isMasterClient)
            DestroySceneObject(_obj);
        else
            photonView.RPC("DestroySceneObject", PhotonNetwork.masterClient, _obj);
    }

    #region RPCs (To be used only by the master client)
    [RPC]
    private void InstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name + " is instantiating scene object !");
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
    }
    
    [RPC]
    private void InstantiateSceneProjectile(string _obj, Vector3 _position, Quaternion _rotation,
                                            string _skillName, string _casterName, Vector3 _destination) {
        Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name + " is instantiating scene projectile !");
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        GameObject obj = PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
        obj.GetComponent<BaseProjectile>().SetUpProjectile( new Pair<BaseSkill, BaseCharacterModel>(
                                                                    SkillBook.Instance.GetSkill(_skillName),
                                                                    GameManager.Instance.GetPlayerModel(_casterName)), _destination);
    }
    [RPC]
    private void DestroySceneObject(GameObject _obj) {
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        PhotonNetwork.Destroy(_obj);
    }
#endregion
}
