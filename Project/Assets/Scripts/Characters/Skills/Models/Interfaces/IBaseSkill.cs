using UnityEngine;

public interface IBaseSkill {

    string Title { get; set; }
    string Description { get; set; }
    Texture2D Icon { get; set; }
    void AddEffect(BaseEffect effect);
    void RemoveEffect(BaseEffect effect);
    BaseEffect GetEffect(int index);
    int EffectsCount { get; }

    void Target(BaseCharacterModel _caster);
    void Cast(BaseCharacterModel _caster);
    void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    void ActivateEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);

    SkillType Type { get; }
}