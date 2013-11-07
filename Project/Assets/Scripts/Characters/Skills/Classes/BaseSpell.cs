using UnityEngine;

public class BaseSpell: BaseSkill, IBaseSpell {

    #region attributes
    private float range;
    private bool lineOfSight;
    private float coolDownTimer, coolDownTime;
    private GameObject targetEffect, castEffect, triggerEffect, projectile;
#endregion

    #region constructors
    public BaseSpell()
        : base() {
        lineOfSight = true;
        targetEffect = null;
        coolDownTimer = coolDownTime = 0f;
        castEffect = null;
        triggerEffect = null;
    }

    public BaseSpell(string _title, string _desc, Texture2D _icon, float _cd, GameObject _target, GameObject _cast, GameObject _trigger, GameObject _projectile)
        : base(_title, _desc, _icon) {
        lineOfSight = true;
        targetEffect = _target;
        coolDownTimer = coolDownTime = _cd;
        castEffect = _cast;
        triggerEffect = _trigger;
        projectile = _projectile;
    }
#endregion

    public void Target() {
        if(targetEffect)
            GameObject.Instantiate(targetEffect);
    }

    public virtual void Cast() {
        coolDownTimer = coolDownTime;
        if (castEffect)
            GameObject.Instantiate(castEffect);
    }

    public virtual void Trigger(PlayerCharacterModel _caster, PlayerCharacterModel _receiver) {
        for (int i = 0; i < EffectsCount; ++i) {
            Utilities.Instance.LogMessage("Activate effect: " + GetEffect(i).Title);
            GetEffect(i).Activate(_caster, _receiver);
        }
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
    }
    public float CoolDownTime {
        get { return coolDownTime; }
        set { coolDownTime = value; }
    }
    public bool IsReady {
        get { return (coolDownTimer == 0f); }
    }

    public GameObject TargetEffect {
        get { return targetEffect; }
        set { targetEffect = value; }
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

    public override SkillType GetSkillType() { return SkillType.BaseSpell_T; }
}
