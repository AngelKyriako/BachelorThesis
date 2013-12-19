using UnityEngine;
using System.Collections;

public class DFSkillModel: SingletonMono<DFSkillModel> {

    private DFSkillModel() { }

    public delegate void SkillEventHandler(BaseSkill skill);
    public event SkillEventHandler SkillActivated;
    public event SkillEventHandler SkillDeactivated;

    public string Title(int _id) {
        return SkillBook.Instance.GetSkill(_id).Title;
    }
    public string Icon(int _id) {
        return SkillBook.Instance.GetIcon(_id);
    }
    public string Description(int _id) {
        return SkillBook.Instance.GetSkill(_id).Description;
    }

    public BaseSkill Skill(int _id, CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key) : SkillBook.Instance.GetSkill(_id);
    }

    public float ManaCost(CharacterSkillSlot _key) {
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key).ManaCost : 0;
    }

    public float Cooldown(int _id, CharacterSkillSlot _key){
        return IsOnActionSlot(_key) ?
               GameManager.Instance.MyCharacterModel.GetSkill(_key).CasterCoolDown : SkillBook.Instance.GetSkill(_id).CoolDown;
    }

    public string Requirement(int _id) {
        return SkillBook.Instance.GetSkill(_id).RequirementsFulfilled() ? "[color #00FF00]O[/color]" : "[color #ff0000]X[/color]";
    }


    private bool IsOnActionSlot(CharacterSkillSlot _key){
        return !_key.Equals(CharacterSkillSlot.None);
    }
}
