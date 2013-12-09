using UnityEngine;
using System.Collections;

public class BaseProjectile: MonoBehaviour {

    private const int directionMultiplier = 1000;

    public int movementSpeed=20, range=20;

    private BaseSkill skill;
    private BaseCharacterModel casterModel;
    private Vector3 origin, destination;
    private bool isTriggered;

    void Awake() {
        enabled = false;
    }

    public virtual void SetUpProjectile(BaseSkill _skill, BaseCharacterModel _model, Vector3 _direction) {
        skill = _skill;
        casterModel = _model;
        destination = _direction.normalized;
        destination.x *= directionMultiplier;
        destination.z *= directionMultiplier;
        isTriggered = false;
        enabled = true;
    }

	void Start () {
        //@TODO: somehow transfer this to movement controller or somewhere to run localy
        //casterModel.gameObject.transform.LookAt(destination);

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
        if (PhotonNetwork.isMasterClient && !isTriggered){
            if (other.CompareTag("Player")) {
                otherModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
                Utilities.Instance.LogMessage(casterModel.name + "'s projectile collided with " + otherModel.name);
                if (otherModel && !CombatManager.Instance.AreAllies(casterModel.name, otherModel.name)) {
                    isTriggered = true;
                    skill.ActivateOffensiveEffects(casterModel, otherModel);
                    Utilities.Instance.LogMessage(otherModel.name + " is an opponent. Skill Triggered.");
                    skill.Trigger(other.transform.position, Quaternion.identity);
                    CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
                }
            }
            else {
                Utilities.Instance.LogMessage(casterModel.name + "'s projectile collided with " + other.name+ ". Projectile destroyed");
                skill.Trigger(other.transform.position, Quaternion.identity);
                CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
            }
        }
    }
}
