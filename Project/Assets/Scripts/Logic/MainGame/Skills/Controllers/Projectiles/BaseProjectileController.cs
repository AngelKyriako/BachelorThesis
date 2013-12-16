using UnityEngine;
using System.Collections;

public class BaseProjectileController: BaseSkillController {

    private const int directionMultiplier = 1000;

    public int movementSpeed=20, range=20;
    private bool isTriggered;

    public override void SetUp(BaseSkill _skill, BaseCharacterModel _model, Vector3 _destination) {
        base.SetUp(_skill, _model, new Vector3(_destination.normalized.x * directionMultiplier,
                                               _destination.normalized.y,
                                               _destination.normalized.z * directionMultiplier));

        transform.LookAt(Destination);
        transform.Rotate(0, 180, 0);
        transform.position = Origin = CasterModel.ProjectileOriginPosition;

        isTriggered = false;
    }

    public override void Start() { }    

	public override void Update () {
        transform.position = Vector3.MoveTowards(transform.position, Destination, movementSpeed * Time.deltaTime);
        if (PhotonNetwork.isMasterClient && Vector3.Distance(Origin, transform.position) > range)
            CombatManager.Instance.MasterClientDestroySceneObject(gameObject);

	}

    public override void OnTriggerEnter(Collider other) {
        PlayerCharacterModel otherModel;
        if (PhotonNetwork.isMasterClient && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void")) && !isTriggered){
            if (other.CompareTag("Player")) {
                otherModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);

                Utilities.Instance.LogMessageToChat(CasterModel.name + "'s projectile collided with " + otherModel.name);
                Utilities.Instance.LogMessageToChat("Are allies: " + CombatManager.Instance.AreAllies(CasterModel.name, otherModel.name));
                
                if (otherModel && !CombatManager.Instance.AreAllies(CasterModel.name, otherModel.name)) {
                    isTriggered = true;
                    Skill.ActivateOffensiveEffects(CasterModel, otherModel);
                    Utilities.Instance.LogMessageToChat(otherModel.name + " is an opponent. Skill Triggered.");
                    Skill.Trigger(other.transform.position, Quaternion.identity);
                    CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
                }
            }
            else {
                Utilities.Instance.LogMessageToChat(CasterModel.name + "'s projectile collided with " + other.name+ ". Projectile destroyed");
                Skill.Trigger(other.transform.position, Quaternion.identity);
                CombatManager.Instance.MasterClientDestroySceneObject(gameObject);
            }
        }
    }
}