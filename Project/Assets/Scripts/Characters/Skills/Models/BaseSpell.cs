using UnityEngine;

public class BaseSpell: BaseSkill {

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

    public override void Target(BaseCharacterModel _caster, CharacterSkillSlot _slot) {
        GameObject obj;
        if (targetCursor) {
            obj = (GameObject)GameObject.Instantiate(targetCursor);
            obj.GetComponent<TargetCursor>().SetUpTargetCursor(new Pair<BaseSkill, BaseCharacterModel>(this, _caster), _slot);
        }
        else
            Cast(_caster, Vector3.zero);
        IsSelected = true;
        coolDownTimer = coolDownTime;
    }

    public override void Cast(BaseCharacterModel _caster, Vector3 _destination) {
        coolDownTimer = coolDownTime;
        if (castEffect)
            CombatManager.Instance.HostInstantiateSceneObject(ResourcesPathManager.Instance.CastEffectPath(castEffect.name),
                                                              _caster.transform.position, Quaternion.identity);

        if (projectile)
            CombatManager.Instance.HostInstantiateSceneProjectile(ResourcesPathManager.Instance.ProjectilePath(projectile.name),
                                                                  _caster.transform.position, Quaternion.identity, Title, _caster.name, _destination);
        else
            Trigger(_caster, null);
        IsSelected = false;
    }

    //for AoE skills we need a certain behavior of the triggerEffect
    public override void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        if (triggerEffect)
            CombatManager.Instance.HostInstantiateSceneObject(ResourcesPathManager.Instance.TriggerEffectPath(triggerEffect.name),
                                                          targetCursor != null ? targetCursor.transform.position : _caster.transform.position, Quaternion.identity);
        ActivateEffects(_caster, _receiver);
    }

    public override void ActivateEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        BaseEffect tempEffect;
        foreach (string key in Effects.Keys) {
            if (!GetEffect(key).IsPassive && _receiver) {
                Utilities.Instance.LogMessage("Attaching an effect bitch");
                _caster.NetworkController.AttachEffectToRemotePlayer(_receiver.NetworkController.photonView, GetEffect(key), _caster);
            }
            else {
                tempEffect = (BaseEffect)_caster.gameObject.AddComponent(GetEffect(key).GetType());
                tempEffect.SetUpEffect(_caster, GetEffect(key));
            }
        }
    }

    public override void Update() {
        CoolDownTimer -= Time.deltaTime;
    }

    #region Accessors
    public float Range {
        get { return range; }
        set { range = value; }
    }

    public float CoolDownTimer {
        get { return coolDownTimer; }
        set { coolDownTimer = value > 0 ? value : 0; }
    }
    public override bool IsReady(BaseCharacterModel _char) {
        return base.IsReady(_char) && (coolDownTimer == 0f);
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
}
