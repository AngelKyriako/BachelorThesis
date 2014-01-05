using UnityEngine;
using System.Collections;

public class DFSkillModel: SingletonMono<DFSkillModel> {

    public delegate void SkillEventHandler(int _id);
    public event SkillEventHandler SkillActivated;

    private DFSkillModel() { }

    public void CastSkill(CharacterSkillSlot _slot) {
        if (SkillActivated != null)
            SkillActivated(GameManager.Instance.MyCharacterModel.GetSkill(_slot).Id);
    }

    public void SetSkillAvailable(int _id, bool b) {
        SkillBook.Instance.SetSkillAvailable(_id, b);
    }

    public string Title(int _id) {
        return SkillBook.Instance.GetSkill(_id).Title;
    }
    public string Icon(int _id) {
        return SkillBook.Instance.GetIcon(_id);
    }
    public string Description(int _id) {
        return SkillBook.Instance.GetSkill(_id).Description;
    }
    public float ManaCost(int _skillId, CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key).ManaCost : SkillBook.Instance.GetSkill(_skillId).ManaCost;
    }
    public float Cooldown(int _id, CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key).CasterBasedCoolDown : SkillBook.Instance.GetSkill(_id).CoolDown;
    }
    public float CooldownTimer(int _id, CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key).CoolDownTimer : SkillBook.Instance.GetSkill(_id).CoolDown;
    }

    public BaseSkill Skill(int _id, CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key) : SkillBook.Instance.GetSkill(_id);
    }

    public string IsSkillAvailable(int _id) {
        return SkillBook.Instance.GetSkill(_id).RequirementsFulfilled() && SkillBook.Instance.IsSkillAvailable(_id)
               ? "[color #00FF00]O[/color]" : "[color #ff0000]X[/color]";
    }

    private bool IsOnActionSlot(CharacterSkillSlot _key){
        return !_key.Equals(CharacterSkillSlot.None);
    }
}
