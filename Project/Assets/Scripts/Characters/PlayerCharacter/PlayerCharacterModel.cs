using System;
using System.Collections.Generic;

public enum CharacterSkillSlots{
    Q, W, E, R
}

public class PlayerCharacterModel: BaseCharacterModel {

    private BaseSkill[] skills;

    public override void Awake() {
        base.Awake();
        skills = new BaseSkill[Enum.GetValues(typeof(CharacterSkillSlots)).Length];
    }

    public BaseSkill GetSkill(CharacterSkillSlots slot) {
        return skills[(int)slot];
    }
}
