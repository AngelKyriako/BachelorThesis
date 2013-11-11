using UnityEngine;

public interface IBaseSkill {

    string Title { get; set; }
    string Description { get; set; }
    Texture2D Icon { get; set; }
    void AddEffect(BaseEffect effect);
    void RemoveEffect(BaseEffect effect);
    BaseEffect GetEffect(int index);
    int EffectsCount { get; }

    void Target(PlayerCharacterModel _caster);
    void Cast(PlayerCharacterModel _caster);
    void Trigger(PlayerCharacterModel _caster, PlayerCharacterModel _receiver);
    void ActivateEffects(PlayerCharacterModel _caster, PlayerCharacterModel _receiver);

    SkillType Type { get; }
}