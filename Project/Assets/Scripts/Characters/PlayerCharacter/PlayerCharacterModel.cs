using System;
using System.Collections.Generic;

public enum CharacterSkillSlots{
    Q, W, E, R
}

public class PlayerCharacterModel: BaseCharacterModel {

    public readonly int MAX_TRAINING_POINTS = 50;

    private int trainingPoints;
    private BaseSkill[] skills;

    public override void Awake() {
        base.Awake();
        trainingPoints = MAX_TRAINING_POINTS;
        skills = new BaseSkill[Enum.GetValues(typeof(CharacterSkillSlots)).Length];
    }

    public int TrainingPoints {
        get { return trainingPoints; }
        set { trainingPoints = (value < 0) ? 0 : ((value > MAX_TRAINING_POINTS)?MAX_TRAINING_POINTS:value); }
    }
    public BaseSkill GetSkill(int index) {
        return skills[index];
    }
    public BaseSkill SetSkill(int index, BaseSkill skill) {
        return skills[index] = skill;
    }
    public int SkillCount {
        get { return skills.Length; }
    }
}
