using UnityEngine;

public class BaseSpell: BaseSkill, IBaseSpell {

    #region attributes
    private float range;
    private float coolDownTimer, coolDownTime;
    private GameObject targetCursor, castEffect, triggerEffect, projectile;
#endregion

    #region constructors
    public BaseSpell()
        : base() {
        targetCursor = null;
        coolDownTimer = coolDownTime = 0f;
        castEffect = null;
        triggerEffect = null;
    }

    public BaseSpell(string _title, string _desc, Texture2D _icon, float _cd, GameObject _target, GameObject _cast, GameObject _trigger, GameObject _projectile)
        : base(_title, _desc, _icon) {
        targetCursor = _target;
        coolDownTimer = coolDownTime = _cd;
        castEffect = _cast;
        triggerEffect = _trigger;
        projectile = _projectile;
    }
#endregion

    public override void Target(BaseCharacterModel _caster) {
        GameObject obj;
        if (targetCursor) {
            obj = (GameObject)GameObject.Instantiate(targetCursor);
            obj.GetComponent<TargetCursor>().SkillCasterPair = new Pair<BaseSkill, BaseCharacterModel>(this, _caster);
            obj.GetComponent<TargetCursor>().enabled = true;
        }
        else
            Cast(_caster);
    }

    public override void Cast(BaseCharacterModel _caster) {
        GameObject obj;
        coolDownTimer = coolDownTime;
        if (castEffect)
            GameObject.Instantiate(castEffect, _caster.transform.position, Quaternion.identity);

        if (projectile) {
            obj = (GameObject)GameObject.Instantiate(projectile, _caster.transform.position, Quaternion.identity);
            obj.GetComponent<BaseProjectile>().SkillCasterPair = new Pair<BaseSkill, BaseCharacterModel>(this, _caster);
            obj.GetComponent<BaseProjectile>().Destination = targetCursor != null ? targetCursor.transform.position : Vector3.zero;
            obj.GetComponent<BaseProjectile>().enabled = true;
        }
        else
            Trigger(_caster, null);
    }

    //for AoE skills we need a certain behavior of the triggerEffect
    public override void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        if (triggerEffect)
            GameObject.Instantiate(triggerEffect,
                                   targetCursor != null ? targetCursor.transform.position : _caster.transform.position,
                                   Quaternion.identity);
        ActivateEffects(_caster, _receiver);
    }

    public override void ActivateEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        BaseEffect tempEffect;
        for (int i = 0; i < EffectsCount; ++i) {
            if (!GetEffect(i).IsPassive && _receiver)
                tempEffect = (BaseEffect)_receiver.gameObject.AddComponent(GetEffect(i).GetType());
            else
                tempEffect = (BaseEffect)_caster.gameObject.AddComponent(GetEffect(i).GetType());
            tempEffect.SetUpEffect(_caster, GetEffect(i));
        }
    }

    #region Accessors
    public float Range {
        get { return range; }
        set { range = value; }
    }

    public float CoolDownTimer {
        get { return coolDownTimer; }
        set { coolDownTimer = value; }
    }
    public bool IsReady {
        get { return (coolDownTimer == 0f); }
    }

    public GameObject TargetCursor {
        get { return targetCursor; }
        set { targetCursor = value; }
    }
    public GameObject CastEffect {
        get { return castEffect; }
        set { castEffect = value; }
    }
    public GameObject TriggerEffect {
        get { return triggerEffect; }
        set { triggerEffect = value; }
    }
    public GameObject Projectile {
        get { return projectile; }
        set { projectile = value; }
    }
#endregion

    public override SkillType Type{
        get { return SkillType.BaseSpell_T; }
    }
}
