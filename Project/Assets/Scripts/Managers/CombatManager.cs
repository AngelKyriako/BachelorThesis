using UnityEngine;
using System.Collections;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private float expRadius;

    private CombatManager() { }

    void Start(){
        switch (GameVariables.Instance.Difficulty.Value){
            case GameDifficulty.Easy:
                expRadius = 30f;
		    break;
            case GameDifficulty.Medium:
                expRadius = 20f;
		    break;
            case GameDifficulty.Hard:
                expRadius = 13f;
            break;
	    }
    }

    #region Instantiation of network objects methods
    public void InstantiateNetworkObject(string _obj, Vector3 _position, Quaternion _rotation) {
        PhotonNetwork.Instantiate(_obj, _position, _rotation, 0, null);
    }

    public void InstantiateNetworkSkill(string _obj, Vector3 _position, Quaternion _rotation,
                                        int _skillId, string _casterName, Vector3 _destination) {
        GameObject obj = PhotonNetwork.Instantiate(_obj, _position, _rotation, 0, null);
        obj.GetComponent<BaseSkillController>().SetUp(SkillBook.Instance.GetSkill(_skillId), GameManager.Instance.GetPlayerModel(_casterName), _destination);
    }

    public void DestroyNetworkObject(GameObject _obj) {
        Utilities.Instance.LogColoredMessageToChat("Dafuck bro !!!", Color.red);
        PhotonNetwork.Destroy(_obj);
    }
    #endregion

    #region Messages to Master client
    public void BroadCastPlayerKill(string _killerName, string _deadName, Vector3 _position) {
        Utilities.Instance.PreCondition(GameManager.Instance.ItsMe(_deadName), "CombatManager", "DeadPlayerBroadCastKill", "This method is only available for the master client.");

        if (!_killerName.Equals(_deadName))
            GameManager.Instance.MyPhotonView.RPC("PlayerKill", GameManager.Instance.GetPlayer(_killerName), _killerName, _deadName, _position);
        GameManager.Instance.MyNetworkController.PlayerDeath(_deadName);
        GameManager.Instance.MyPhotonView.RPC("PlayerDeath", PhotonTargets.Others, _deadName);       
    }
    #endregion

    #region Local Accessors
    public bool IsAlly(string _name) {
        return GameManager.Instance.IsAlly(_name);
    }

    public bool AreAllies(string _name1, string _name2) {
        return ((PlayerTeam)GameManager.Instance.GetPlayer(_name1).customProperties["Team"]).Equals(
                (PlayerTeam)GameManager.Instance.GetPlayer(_name2).customProperties["Team"]);
    }

    public float ExpRadius {
        get { return expRadius; }
    }
    #endregion
}
