using UnityEngine;

public interface IBaseSkill {

    void Target();
    void Cast();
    void Update();

    string Title { get; set; }
    string Description { get; set; }
    Texture2D Icon { get; set; }

    bool LineOfSight { get; set; }
    GameObject TargetEffect { get; set; }
    GameObject CastEffect { get; set; }

    float CoolDownTime { get; set; }
    float CoolDownTimer { get; }
    bool IsReady { get; }
}