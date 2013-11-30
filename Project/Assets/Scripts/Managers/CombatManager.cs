using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private CombatManager() { }

    public void MasterClientInstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneObject(_obj, _position, _rotation);
        else
            photonView.RPC("InstantiateSceneObject", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation);
    }

    public void MasterClientInstantiateSceneProjectile(string _obj, Vector3 _position, Quaternion _rotation,
                                               string _skillName, string _casterName, Vector3 _direction) {
        if (PhotonNetwork.isMasterClient)
            InstantiateSceneProjectile(_obj, _position, _rotation, _skillName, _casterName, _direction);
        else {
            Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name+" is sending message to masterclient to instantiate a projectile !");
            photonView.RPC("InstantiateSceneProjectile", PhotonNetwork.masterClient,
                                                   _obj, _position, _rotation, _skillName, _casterName, _direction);
        }
    }

    public void MasterClientDestroySceneObject(GameObject _obj) {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(_obj);
    }

    public bool AreAllies(string _name1, string _name2) {
        return GameManager.Instance.GetPlayer(_name1).customProperties["team"].Equals(
               GameManager.Instance.GetPlayer(_name2).customProperties["team"]);
    }

    #region RPCs (To be sent only to master client)
    [RPC]
    private void InstantiateSceneObject(string _obj, Vector3 _position, Quaternion _rotation) {
        Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name + " is instantiating scene object !");
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
    }
    
    [RPC]
    private void InstantiateSceneProjectile(string _obj, Vector3 _position, Quaternion _rotation,
                                            string _skillName, string _casterName, Vector3 _direction) {
        Utilities.Instance.LogMessage(GameManager.Instance.MyPhotonView.name + " is instantiating scene projectile !");
        Utilities.Instance.PreCondition(PhotonNetwork.isMasterClient, "CombatManager", "[RPC]InstantiateSceneObject", "This RPC is only available for the master client.");
        GameObject obj = PhotonNetwork.InstantiateSceneObject(_obj, _position, _rotation, 0, null);
        obj.GetComponent<BaseProjectile>().SetUpProjectile(SkillBook.Instance.GetSkill(_skillName), 
                                                           GameManager.Instance.GetPlayerModel(_casterName),
                                                           _direction);
    }
#endregion
}
