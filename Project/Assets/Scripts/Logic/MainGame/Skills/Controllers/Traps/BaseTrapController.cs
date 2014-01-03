using UnityEngine;
using System.Collections;

public class BaseTrapController: BaseSkillController {

    public bool triggersOnAlly = false, triggersOnEnemy = true;
    public float timeToLive = 20f;

    private bool isTriggered;
    private float startTime;

    public override void SetUp(BaseSkill _skill, BaseCharacterModel _model, Vector3 _destination) {
        base.SetUp(_skill, _model, _destination);
        isTriggered = false;
    }

    public override void Start() {
        transform.LookAt(Destination);
        transform.Rotate(90, 0, 0);//Check what it needs
        transform.position = Skill.OwnerModel.transform.position;//check if I need to put y=0;
        gameObject.renderer.enabled = true;
        startTime = Time.time;
    }

	public override void Update () {        
        if (IsMySkill && !isTriggered) {
            if (Time.time - startTime > timeToLive)
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