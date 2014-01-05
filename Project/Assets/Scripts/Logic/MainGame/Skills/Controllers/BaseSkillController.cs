using UnityEngine;
using System.Collections;

public class BaseSkillController: MonoBehaviour {

    //AoE vars
    public bool IsAoE = false;
    public string aoePrefabName = string.Empty;
    public int aoeRadius = 5;
    public int aoeMaxAlliesAffected = 10, aoeMaxEnemiesAffected = 10;
    public bool aoeActiveOnSelf = true, aoeAttachedOnPlayer = false;
    public float aoeTimeToLive = -1, aoeActivationFrequency = -1;

    private BaseSkill skill;
    private BaseCharacterModel casterModel;

    private Vector3 origin, destination;    
    
    void Awake() {
        enabled = false;
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
        if (gameObject.renderer)
            gameObject.renderer.enabled = true;
        if (IsMySkill)
            Trigger(null);
    }

    public virtual void Update() { }

    public virtual void OnTriggerEnter(Collider other) { }

    public virtual void Trigger(BaseCharacterModel _characterHit) {
        Skill.Trigger(skill.OwnerModel.transform.position, Quaternion.identity);        
        if (IsAoE)
            ActivateAoE();
        else
            CombatManager.Instance.DestroyNetworkObject(gameObject);        
    }

    public virtual void ActivateAoE() {
        if (gameObject.renderer)
            gameObject.renderer.enabled = false;
        GameObject obj;
        if (aoePrefabName == null || aoePrefabName.Equals(string.Empty)) {
            obj = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            if (obj.renderer)
                obj.renderer.enabled = false;
        }
        else
            obj = (GameObject)GameObject.Instantiate(Resources.Load(ResourcesPathManager.Instance.AoEObjectPath(aoePrefabName)),
                                                     transform.position, transform.rotation);

        obj.AddComponent<BaseAoEController>().SetUp(skill, aoeRadius, aoeMaxAlliesAffected, aoeMaxEnemiesAffected,
                                                    aoeActiveOnSelf, aoeAttachedOnPlayer, aoeTimeToLive, aoeActivationFrequency);
        CombatManager.Instance.DestroyNetworkObject(gameObject);
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
    #endregion
}