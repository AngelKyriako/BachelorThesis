using UnityEngine;
using System.Collections.Generic;

public class CombatManager: SingletonPhotonMono<CombatManager> {

    private float expRadius;

    private CombatManager() {
        InitSkillCounters();
    }

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
        PhotonNetwork.Destroy(_obj);
    }
    #endregion

    #region Messages to Master client
    public void BroadCastPlayerKill(string _killerName, string _deadName, Vector3 _position) {
        Utilities.Instance.PreCondition(GameManager.Instance.ItsMe(_deadName), "CombatManager", "DeadPlayerBroadCastKill", "This method is only available for the master client.");

        if (!_killerName.Equals(_deadName)) {
            GameManager.Instance.MyPhotonView.RPC("PlayerKill", GameManager.Instance.GetPlayer(_killerName), _killerName, _deadName, _position);
            GameManager.Instance.BroadcastChatMessage(SystemMessages.Instance.Kill(GameManager.Instance.GetPlayerName(_killerName),
                                                                                   GameManager.Instance.GetPlayerName(_deadName)));
        }
        else
            GameManager.Instance.BroadcastChatMessage(SystemMessages.Instance.Suicide(GameManager.Instance.GetPlayerName(_deadName)));

        GameManager.Instance.MyNetworkController.PlayerDeath(_deadName);
        GameManager.Instance.MyPhotonView.RPC("PlayerDeath", PhotonTargets.Others, _deadName);       
    }
    #endregion

    #region Local Methods
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

    public void AttachEffectToSelf(string _casterName, BaseEffect _effectToAttach) {
        BaseEffect tempEffect = (BaseEffect)GameManager.Instance.MyCharacter.AddComponent(_effectToAttach.GetType());
        tempEffect.SetUpEffect(GameManager.Instance.GetPlayerModel(_casterName), _effectToAttach);
    }
    #endregion

    #region Statistics shit
    private int[] SkillCastingCounters;

    private void InitSkillCounters() {
        SkillCastingCounters = new int[SkillBook.Instance.AllSkillsKeys.Count];
        for (int i = 0; i < SkillCastingCounters.Length; ++i)
            SkillCastingCounters[i] = 0;
    }

    public void RaiseSkillUsageCount(int skillId) {
        ++SkillCastingCounters[skillId];
    }

    public string SkillCastingCountersToString() {
        string txt = "Skill usage count:\n";
        for (int i = 0; i < SkillCastingCounters.Length; ++i)
            txt += SkillBook.Instance.GetSkill(i).Title +":\t"+SkillCastingCounters[i].ToString()+"\n";
        return txt;
    }
    #endregion
    
}
