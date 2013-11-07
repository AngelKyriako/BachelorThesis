using UnityEngine;

public interface IBaseSpell: IBaseSkill {

    void Target();
    void Cast();
    void Trigger(PlayerCharacterModel _caster, PlayerCharacterModel _receiver);
    void Update();

    float Range { get; set; }
    bool LineOfSight { get; set; }
    
    float CoolDownTime { get; set; }
    float CoolDownTimer { get; }
    bool IsReady { get; }

    GameObject TargetEffect { get; set; }
    GameObject CastEffect { get; set; }
    GameObject TriggerEffect { get; set; }
    GameObject Projectile { set; get; }
}