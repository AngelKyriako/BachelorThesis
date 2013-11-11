using UnityEngine;

public interface IBaseSpell: IBaseSkill {    

    void Update();

    float Range { get; set; }
    bool LineOfSight { get; set; }

    float CoolDownTimer { get; set; }
    bool IsReady { get; }

    GameObject TargetCursor { get; set; }
    GameObject CastEffect { get; set; }
    GameObject TriggerEffect { get; set; }
    GameObject Projectile { set; get; }
}