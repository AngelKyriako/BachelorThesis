using UnityEngine;

public class BaseSpell: BaseSkill, IBaseSpell {

    #region attributes
    private float range;
    private bool lineOfSight;
    private float coolDownTimer, coolDownTime;
    private GameObject targetCursor, castEffect, triggerEffect, projectile;
#endregion

    #region constructors
    public BaseSpell()
        : base() {
        lineOfSight = true;
        targetCursor = null;
        coolDownTimer = coolDownTime = 0f;
        castEffect = null;
        triggerEffect = null;
    }

    public BaseSpell(string _title, string _desc, Texture2D _icon, float _cd, GameObject _target, GameObject _cast, GameObject _trigger, GameObject _projectile)
        : base(_title, _desc, _icon) {
        lineOfSight = true;
        targetCursor = _target;
        coolDownTimer = coolDownTime = _cd;
        castEffect = _cast;
        triggerEffect = _trigger;
        projectile = _projectile;
    }
#endregion

    public override void Target(PlayerCharacterModel _caster) {
        if (targetCursor)
            GameObject.Instantiate(targetCursor);
        else
            Cast(_caster);
    }

    public override void Cast(PlayerCharacterModel _caster) {
        GameObject obj;
        coolDownTimer = coolDownTime;
        if (castEffect)
            GameObject.Instantiate(castEffect, _caster.transform.position, Quaternion.identity);

        if (projectile) {
            obj = (GameObject)GameObject.Instantiate(projectile, _caster.transform.position, Quaternion.identity);
            obj.GetComponent<BaseProjectile>().SkillCasterPair = new Pair<BaseSkill, PlayerCharacterModel>(this, _caster);
            obj.GetComponent<BaseProjectile>().Destination = targetCursor != null ? targetCursor.transform.position : Vector3.zero;
            obj.GetComponent<BaseProjectile>().enabled = true;
        }
        else
            Trigger(_caster, _caster);
    }

    //for AoE skills we need a certain behavior of the triggerEffect
    public override void Trigger(PlayerCharacterModel _caster, PlayerCharacterModel _receiver) {
        if (triggerEffect)
            GameObject.Instantiate(triggerEffect,
                                   targetCursor != null ? targetCursor.transform.position : _caster.transform.position,
                                   Quaternion.identity);
        ActivateEffects(_caster, _receiver);
    }

    public override void ActivateEffects(PlayerCharacterModel _caster, PlayerCharacterModel _receiver) {
        for (int i = 0; i < EffectsCount; ++i)
            _receiver.AddEffectAttached(new AttachedEffect(new BaseEffect(GetEffect(i)), _caster));
    }

    public virtual void Update(){
    }

    #region Accessors
    public float Range {
        get { return range; }
        set { range = value; }
    }
    public bool LineOfSight {
        get { return LineOfSight; }
        set { LineOfSight = value; }
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
