using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    private const int directionMultiplier = 1000;

    public int movementSpeed=20, range=20;

    private BaseSkill skill;
    private BaseCharacterModel casterModel;
    private Vector3 origin, destination;

    void Awake() {
        enabled = false;
    }

    public virtual void SetUpProjectile(BaseSkill _skill, BaseCharacterModel _model, Vector3 _direction) {
        skill = _skill;
        casterModel = _model;
        destination = _direction.normalized;
        destination.x *= directionMultiplier;
        destination.z *= directionMultiplier;
        enabled = true;
    }

	void Start () {
        //@TODO: somehow transfer this to movement controller or somewhere to run localy
        casterModel.gameObject.transform.LookAt(destination);

        origin = casterModel.ProjectileOriginPosition;
        transform.position = origin;
        transform.LookAt(destination);
        transform.Rotate(0, 180, 0);
	}    

	public virtual void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (PhotonNetwork.isMasterClient && Vector3.Distance(origin, transform.position) > range)
            CombatManager.Instance.MasterClientDestroySceneObject(gameObject);

	}

    public virtual void OnTriggerEnter(Collider other) {
        PlayerCharacterModel otherModel;
        if (PhotonNetwork.isMasterClient){
            if (other.CompareTag("Player")) {
                otherModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
                if (otherModel && !CombatManager.Instance.IsAlly(otherModel.name)) {
                    skill.ActivateOffensiveEffects(casterModel, other.GetComponent<PlayerCharacterModel>());
                    Utilities.Instance.LogMessage(casterModel.name + "'s projectile collided with " + otherModel.name + ". Projectile destroyed");
                    skill.Trigger(casterModel, other.GetComponent<PlayerCharacterModel>(), other.transform.position, Quaternion.identity);
                    CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
                }
            }
            else {
                Utilities.Instance.LogMessage(casterModel.name + "'s projectile collided with " + other.name+ ". Projectile destroyed");
                skill.Trigger(casterModel, other.GetComponent<PlayerCharacterModel>(), other.transform.position, Quaternion.identity);
                CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
            }
        }
    }
}
