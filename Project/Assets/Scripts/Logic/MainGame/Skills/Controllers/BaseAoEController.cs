using UnityEngine;
using System.Collections.Generic;

public class BaseAoEController: MonoBehaviour {
   
    private int radius = 5;
    private int maxAlliesAffected = 10, maxEnemiesAffected = 10;
    private bool activeOnSelf = true;
    private float timeToLive = -1, activationFrequency = -1;

    private float startTime, lastActivationTime;
    private BaseSkill skill;
    private Dictionary<string, bool> alliesAffected, enemiesAffected;

    void Awake() {
        Utilities.Instance.Assert(gameObject.GetComponent<SphereCollider>() != null, "BaseAoEController", "Awake", "GameObject needs to have a SphereCollider component");

        alliesAffected = new Dictionary<string, bool>();
        enemiesAffected = new Dictionary<string, bool>();
        enabled = false;
    }

    public void SetUp(BaseSkill _skill, int _radius, int _maxAllies, int _maxEnemies, bool _activeOnSelf, bool _attachedOnCaster, float _ttl, float _freq) {
        skill = _skill;
        radius = _radius;
        maxAlliesAffected = _maxAllies;
        maxEnemiesAffected = _maxEnemies;
        activeOnSelf = _activeOnSelf;
        timeToLive = _ttl;
        activationFrequency = _freq;

        gameObject.GetComponent<SphereCollider>().radius = radius;
        if (_attachedOnCaster)
            gameObject.transform.parent = _skill.OwnerModel.gameObject.transform;

        enabled = true;
    }

    void Start() {
        startTime = Time.time;
        lastActivationTime = 0;
    }

    void Update() {
        if (IsMySkill) {
            if (Time.time - lastActivationTime >= activationFrequency) {
                //Utilities.Instance.LogColoredMessageToChat("Attaching Effects dude, lastActivationTime: " + lastActivationTime, Color.red);
                AttachEffects();
                lastActivationTime = Time.time;
            }

            if (Time.time - startTime > timeToLive) {
                //Utilities.Instance.LogColoredMessageToChat("Time's up", Color.red);
                Destroy(gameObject);
            }
        }
    }

    private void AttachEffects() {
        BaseCharacterModel model;
        foreach (string _name in GameManager.Instance.AllPlayerKeys) {
            model = GameManager.Instance.GetPlayerModel(_name);
            if (Vector3.Distance(transform.position, model.transform.position) <= radius)
                if (!CombatManager.Instance.AreAllies(skill.OwnerModel.name, model.name))
                    TriggerOnEnemy(model);
                else if (activeOnSelf || !GameManager.Instance.ItsMe(model.name))
                    TriggerOnAlly(model);
        }
        ClearAffectedAllies();
        ClearAffectedEnemies();
    }

    //public virtual void OnTriggerStay(Collider other) {
    //    if (IsMySkill && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void"))) {
    //        BaseCharacterModel model = Utilities.Instance.GetPlayerCharacterModel(other.transform);
    //        if (!CombatManager.Instance.AreAllies(skill.OwnerModel.name, model.name))
    //            TriggerOnEnemy(model);
    //        else if (activeOnSelf || !GameManager.Instance.ItsMe(model.name))
    //            TriggerOnAlly(model);
    //    }
    //}

    public void TriggerOnEnemy(BaseCharacterModel _model) {
        if (!IsEnemyAffected(_model.name) && !ReachedMaxEnemies) {
            Skill.ActivateOffensiveEffects(skill.OwnerModel, _model);
            AffectEnemy(_model.name);
        }
    }

    public void TriggerOnAlly(BaseCharacterModel _model) {
        if (!IsAllyAffected(_model.name) && !ReachedMaxAllies) {
            Skill.ActivateSupportEffects(skill.OwnerModel, _model);
            AffectAlly(_model.name);
        }
    }

    #region Affect Characters
    public void AffectAlly(string _charName) {
        alliesAffected.Add(_charName, true);
    }
    public void ClearAffectedAllies() {
        alliesAffected.Clear();
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
    public void ClearAffectedEnemies() {
        enemiesAffected.Clear();
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
