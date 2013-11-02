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
    public BaseSkill GetSkill(CharacterSkillSlots slot) {
        return skills[(int)slot];
    }
}
