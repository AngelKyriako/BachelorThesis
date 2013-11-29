using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    public int movementSpeed=10, range=25;

    private BaseSkill skill;
    private BaseCharacterModel casterModel;
    private Vector3 origin, destination;

    void Awake() {
        enabled = false;
    }

	void Start () {
        origin = transform.position;
        transform.LookAt(destination);
        transform.Rotate(0, 180, 0);

        //@TODO: somehow transfer this to movement controller if possible 
        casterModel.gameObject.transform.LookAt(destination);
	}

    public virtual void SetUpProjectile(BaseSkill _skill, BaseCharacterModel _model, Vector3 _dest) {
        skill = _skill;
        casterModel = _model;
        destination = _dest;
        enabled = true;
    }

	public virtual void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) > range)
            CombatManager.Instance.MasterClientDestroySceneObject(gameObject);

	}

    public virtual void OnTriggerEnter(Collider other) {
        if (PhotonNetwork.isMasterClient && !casterModel.name.Equals(other.name))
            skill.Trigger(casterModel, other.GetComponent<PlayerCharacterModel>());
    }

    public Vector3 Destination {
        get { return destination; }
        set { destination = value; }
    }
}
