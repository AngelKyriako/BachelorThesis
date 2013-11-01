using UnityEngine;

public class BaseSkill: IBaseSkill {

#region attributes
    private string title, description;
    private Texture2D icon;
    private bool lineOfSight;
    private GameObject targetEffect, castEffect;
    private float coolDownTimer, coolDownTime;
#endregion

#region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        lineOfSight = true;
        targetEffect = null;
        castEffect = null;
        coolDownTimer = coolDownTime = 2.0f;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon) {
        title = _title;
        description = _desc;
        icon = _icon;
        lineOfSight = true;
        targetEffect = null;
        castEffect = null;
        coolDownTimer = coolDownTime = 2.0f;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon, GameObject target, GameObject cast, float cd) {
        title = _title;
        description = _desc;
        icon = _icon;
        lineOfSight = true;
        targetEffect = target;
        castEffect = cast;
        coolDownTimer = coolDownTime = cd;
    }
#endregion

    public void Target() {
        GameObject.Instantiate(targetEffect);
    }

    public void Cast() {
        coolDownTimer = coolDownTime;
        GameObject.Instantiate(castEffect);
    }

    public void Update() {
        throw new System.NotImplementedException();
    }

#region Setters and Getters
    public string Title {
        get { return title; }
        set { title = value; }
    }
    public string Description {
        get { return description; }
        set { description = value; }
    }
    public Texture2D Icon {
        get { return icon; }
        set { icon = value; }
    }
    public bool LineOfSight {
        get { return LineOfSight; }
        set { LineOfSight = value; }
    }
    public GameObject TargetEffect {
        get { return targetEffect; }
        set { targetEffect = value; }
    }
    public GameObject CastEffect {
        get { return castEffect; }
        set { castEffect = value; }
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
#endregion
}
