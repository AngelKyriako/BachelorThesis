using UnityEngine;
using System.Collections.Generic;

public class BaseAoEController: MonoBehaviour {
   
    public int radius = 5;
    public int maxAlliesAffected = 10, maxEnemiesAffected = 10;
    public bool activeOnSelf = true;
    public float timeToLive = -1, activationFrequency = -1;

    private float startTime, lastActivationTime;
    private BaseSkill skill;
    private Dictionary<string, bool> alliesAffected, enemiesAffected;

    void Awake() {
        //Utilities.Instance.Assert(activationFrequency <=);
        alliesAffected = new Dictionary<string, bool>();
        enemiesAffected = new Dictionary<string, bool>();
        enabled = false;
    }

    public void SetUp(BaseSkill _skill) {
        skill = _skill;

        startTime = Time.time;
        lastActivationTime = Time.time;

        gameObject.GetComponent<SphereCollider>().radius = radius;
        enabled = true;
    }    

    void Update() {
        if (IsMySkill) {
            if (Time.time - startTime > timeToLive)
                CombatManager.Instance.DestroyNetworkObject(gameObject);
            if (Time.time - lastActivationTime >= activationFrequency) {
                lastActivationTime = Time.time;
                //free affected players
            }
        }
    }

    public virtual void OnTriggerStay(Collider other) {
        if (IsMySkill && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
            BaseCharacterModel model = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (!CombatManager.Instance.AreAllies(skill.OwnerModel.name, model.name))
                TriggerOnEnemy(model);
            else if (activeOnSelf || !GameManager.Instance.ItsMe(model.name))
                TriggerOnAlly(model);
        }
    }

    public void TriggerOnAlly(BaseCharacterModel _model) {
        if (IsAllyAffected(_model.name) && !ReachedMaxAllies) {
            Skill.ActivateOffensiveEffects(skill.OwnerModel, _model);
            AffectAlly(_model.name);
        }
    }

    public void TriggerOnEnemy(BaseCharacterModel _model) {
        if (IsEnemyAffected(_model.name) && !ReachedMaxEnemies) {
            Skill.ActivateSupportEffects(skill.OwnerModel, _model);
            AffectEnemy(_model.name);
        }
    }

    #region Affected characters
    public void AffectAlly(string _charName) {
        alliesAffected.Add(_charName, true);
    }
    public void FreeAlly(string _charName) {
        alliesAffected.Remove(_charName);
    }
    public bool IsAllyAffected(string _charName) {
        return alliesAffected.ContainsKey(_charName);
    }
    public bool ReachedMaxAllies {
        get { return alliesAffected.Count == maxAlliesAffected; }
    }

    public void AffectEnemy(string _charName) {
        enemiesAffected.Add(_charName, true);
    }
    public void FreeEnemy(string _charName) {
        enemiesAffected.Remove(_charName);
    }
    public bool IsEnemyAffected(string _charName) {
        return enemiesAffected.ContainsKey(_charName);
    }
    public bool ReachedMaxEnemies {
        get { return enemiesAffected.Count == maxEnemiesAffected; }
    }
    #endregion

    #region Accessors
    public BaseSkill Skill {
        get { return skill; }
        set { skill = value; }
    }
    
    public bool IsMySkill {
        get { return skill != null && GameManager.Instance.ItsMe(skill.OwnerModel.name); }
    }
    #endregion
}
