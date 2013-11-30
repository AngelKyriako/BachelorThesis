using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    private const int directionMultiplier = 1000;

    public int movementSpeed=10, range=20;

    private BaseSkill skill;
    private BaseCharacterModel casterModel;
    private Vector3 origin, direction;

    void Awake() {
        enabled = false;
    }

    public virtual void SetUpProjectile(BaseSkill _skill, BaseCharacterModel _model, Vector3 _direction) {
        skill = _skill;
        casterModel = _model;
        direction = _direction.normalized;
        direction.x *= directionMultiplier;
        direction.z *= directionMultiplier;
        enabled = true;
    }

	void Start () {
        origin = transform.position;
        transform.LookAt(direction);
        transform.Rotate(0, 180, 0);

        //@TODO: somehow transfer this to movement controller if possible
        casterModel.gameObject.transform.LookAt(direction);
	}    

	public virtual void Update () {
        transform.position = Vector3.MoveTowards(transform.position, direction, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) > range)
            CombatManager.Instance.MasterClientDestroySceneObject(gameObject);

	}

    public virtual void OnTriggerEnter(Collider other) {
        if (PhotonNetwork.isMasterClient && !casterModel.name.Equals(other.name))
            skill.Trigger(casterModel, other.GetComponent<PlayerCharacterModel>(), other.transform.position, Quaternion.identity);
    }

    public Vector3 Direction {
        get { return direction; }
        set { direction = value; }
    }
}
