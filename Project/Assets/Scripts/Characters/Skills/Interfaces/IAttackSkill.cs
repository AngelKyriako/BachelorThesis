using UnityEngine;

public interface IAttackSkill: IBaseSkill {
    float DamageValue { get; set; }
    float Range { get; set; }
    GameObject Projectile { set; get; }
}