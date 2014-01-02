using UnityEngine;
using System.Collections;

public class BaseProjectileController: BaseSkillController {

    private const int directionMultiplier = 1000;

    public bool triggersOnAlly = false, triggersOnEnemy = true;
    public int movementSpeed=20, range=20;

    private bool isTriggered;

    public override void SetUp(BaseSkill _skill, BaseCharacterModel _model, Vector3 _destination) {
        base.SetUp(_skill, _model, new Vector3(_destination.normalized.x * directionMultiplier,
                                               _destination.normalized.y,
                                               _destination.normalized.z * directionMultiplier));
        isTriggered = false;
    }

    public override void Start() {
        transform.LookAt(Destination);
        transform.Rotate(0, 180, 0);     
        transform.position = Origin = CasterModel.ProjectileOriginPosition;
        gameObject.renderer.enabled = true;
    }

	public override void Update () {        
        if (IsMySkill && !isTriggered) {
            transform.position = Vector3.MoveTowards(transform.position, Destination, movementSpeed * Time.deltaTime);
            if (Vector3.Distance(Origin, transform.position) > range)
                Trigger(null);
        }
	}

    public override void OnTriggerEnter(Collider other) {
        if (IsMySkill && !isTriggered && !other.gameObject.layer.Equals(LayerMask.NameToLayer("Void")))            
            Trigger(Utilities.Instance.GetPlayerCharacterModel(other.transform));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    // I have to put many subclasses of base projectile controller with different behaviors.  //
    // It will seem as if there is a huge variety of skills in game                           //
    ////////////////////////////////////////////////////////////////////////////////////////////

    // This controller is activated for the first player character it hits.
    // If an enemy it activates the offensive effects. If an ally(not caster himself) it activates the support effects.
    // It is destroyed afterwards.
    public override void Trigger(BaseCharacterModel _characterHit) {
        if (_characterHit == null || !GameManager.Instance.ItsMe(_characterHit.name)) {

            isTriggered = true;
            Skill.Trigger(transform.position, Quaternion.identity);

            if (_characterHit != null && !IsAoE) {
                if (triggersOnEnemy && !CombatManager.Instance.AreAllies(CasterModel.name, _characterHit.name))
                    Skill.ActivateOffensiveEffects(CasterModel, _characterHit);
                else if (triggersOnAlly && CombatManager.Instance.AreAllies(CasterModel.name, _characterHit.name) && !GameManager.Instance.ItsMe(_characterHit.name))
                    Skill.ActivateSupportEffects(CasterModel, _characterHit);
                CombatManager.Instance.DestroyNetworkObject(gameObject);            
            }
            else if (IsAoE)
                ActivateAoE();
            else
                CombatManager.Instance.DestroyNetworkObject(gameObject);            
        }
    }
}