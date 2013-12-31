using UnityEngine;
using System.Collections;

public class BaseSkillController: MonoBehaviour {

    private BaseSkill skill;
    private BaseCharacterModel casterModel;

    private Vector3 origin, destination;    
    
    void Awake() {
        enabled = false;
        gameObject.renderer.enabled = false;
    }

    public virtual void SetUp(BaseSkill _skill, BaseCharacterModel _model, Vector3 _destination) {
        skill = _skill;
        casterModel = _model;
        destination = _destination;
        origin = CasterModel.transform.position;
        
        enabled = true;
    }

    public virtual void Start() {        
        transform.position = destination;
        gameObject.renderer.enabled = true;
        Trigger(null);
    }    

    public virtual void Update() { }

    public virtual void OnTriggerEnter(Collider other) { }

    public virtual void Trigger(BaseCharacterModel _characterHit) {
        if (!IsAoE)
            CombatManager.Instance.DestroyNetworkObject(gameObject);
        else
            ActivateAoE();
        Skill.Trigger(transform.position, Quaternion.identity);        
    }

    public virtual void ActivateAoE() {        
        gameObject.GetComponent<BaseAoEController>().SetUp(skill);
        enabled = false;
    }
    #region Accessors
    public BaseSkill Skill {
        get { return skill; }
        set { skill = value; }
    }
    public BaseCharacterModel CasterModel {
        get { return casterModel; }
        set { casterModel = value; }
    }

    public Vector3 Origin {
        get { return origin; }
        set { origin = value; }
    }
    public Vector3 Destination {
        get { return destination; }
    }

    public bool IsMySkill {
        get { return casterModel && GameManager.Instance.ItsMe(casterModel.name); }
    }

    public bool IsAoE {
        get { return gameObject.GetComponent<BaseAoEController>() != null; }
    }
    #endregion
}