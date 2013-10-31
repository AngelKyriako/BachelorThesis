using UnityEngine;

public interface IBaseSkill {

    string Title { get; set; }
    string Description { get; set; }
    Texture2D Icon { get; set; }

    bool LineOfSight { get; set; }
    GameObject TargetEffect { get; set; }
    GameObject CastEffect { get; set; }

    float CoolDownTime { get; set; }
    float CoolDownTimer { get; }
    bool IsReady { get; }

    void Cast();
    void Update();
}