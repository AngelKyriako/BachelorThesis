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
        if (IsMySkill && !isTriggered && Vector3.Distance(Origin, transform.position) > range)
            TriggerAndDestroy();
	}

    ////////////////////////////////////////////////////////////////////////////////////////////
    // I have to put many subclasses of base projectile controller with different behaviors.  //
    // It will seem as if there is a huge variety of skills in game                           //
    ////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// This controller is activated for the first player character it hits.
    /// If an enemy it activates the offensive effects. If an ally(not caster himself) it activates the support effects.
    /// It is destroyed afterwards.
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other) {
        if (IsMySkill && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void")) && !isTriggered) {
            PlayerCharacterModel otherModel = Utilities.Instance.GetPlayerCharacterModel(other.transform);
            if (otherModel != null) {
                if (!CombatManager.Instance.AreAllies(CasterModel.name, otherModel.name)) {
                    Skill.ActivateOffensiveEffects(CasterModel, otherModel);
                    Utilities.Instance.LogMessageToChat(otherModel.name + " is an opponent. Activate offensive effects.");
                }
                else if (!GameManager.Instance.ItsMe(otherModel.name)) {
                    Skill.ActivateSupportEffects(CasterModel, otherModel);
                    Utilities.Instance.LogMessageToChat(otherModel.name + " is an ally. Activate support effects.");
                }
            }
            else
                Utilities.Instance.LogMessageToChat(CasterModel.name + "'s projectile collided with " + other.name+ ". Projectile destroyed");

            if (!GameManager.Instance.ItsMe(otherModel.name))
                TriggerAndDestroy();
        }
    }

    private void TriggerAndDestroy() {
        isTriggered = true;
        Skill.Trigger(transform.position, Quaternion.identity);
        CombatManager.Instance.DestroyNetworkObject(gameObject);
    }
}