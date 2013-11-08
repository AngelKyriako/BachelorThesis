using UnityEngine;

public interface IBaseSkill {

    string Title { get; set; }
    string Description { get; set; }
    Texture2D Icon { get; set; }
    void AddEffect(BaseEffect effect);
    void RemoveEffect(BaseEffect effect);
    BaseEffect GetEffect(int index);
    int EffectsCount { get; }

    SkillType GetSkillType();
}