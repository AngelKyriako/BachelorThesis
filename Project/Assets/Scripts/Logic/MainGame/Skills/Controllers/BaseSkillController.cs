using UnityEngine;
using System.Collections;

public class BaseSkillController: MonoBehaviour {

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
    }    

    public virtual void Update() {
    }

    public virtual void OnTriggerEnter(Collider other) {
        Utilities.Instance.LogMessageToChat("OnTriggedEnter with: " + other.name);
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
    #endregion
}