using UnityEngine;
using System.Collections;

public class DFPlayerCharacterModel: MonoBehaviour {

    private const int DEFAULT_VITAL = 100;
    
    private PlayerCharacterModel myModel;
	
    void Start () {
        //myModel = GameManager.Instance.MyCharacterModel;
	}

    #region Vitals
    public float CurrentHealth {
        get { return myModel ? myModel.GetVital(0).CurrentValue : MaxHealth; }
    }
    public float MaxHealth {
        get { return myModel ? myModel.GetVital(0).FinalValue : DEFAULT_VITAL; }
    }

    public float CurrentMana {
        get { return myModel ? myModel.GetVital(1).CurrentValue : MaxMana; }
    }
    public float MaxMana {
        get { return myModel ? myModel.GetVital(1).FinalValue : DEFAULT_VITAL; }
    }

    public uint CurrentExp {
        get { return myModel ? myModel.CurrentExp : MaxExp; }
    }
    public uint MaxExp {
        get { return myModel ? myModel.ExpToLevel : DEFAULT_VITAL; }
    }
    #endregion

    #region Skills
    public BaseSkill QSkill {
        get { return myModel.GetSkill(CharacterSkillSlot.Q); }
    }
    public string QIcon {
        get { return SkillBook.Instance.GetIcon(myModel.GetSkill(CharacterSkillSlot.Q).Id); }
    }

    public BaseSkill WSkill {
        get { return myModel.GetSkill(CharacterSkillSlot.W); }
    }
    public string WIcon {
        get { return SkillBook.Instance.GetIcon(myModel.GetSkill(CharacterSkillSlot.W).Id); }
    }

    public BaseSkill ESkill {
        get { return myModel.GetSkill(CharacterSkillSlot.E); }
    }
    public string EIcon {
        get { return SkillBook.Instance.GetIcon(myModel.GetSkill(CharacterSkillSlot.E).Id); }
    }

    public BaseSkill RSkill {
        get { return myModel.GetSkill(CharacterSkillSlot.R); }
    }
    public string RIcon {
        get { return SkillBook.Instance.GetIcon(myModel.GetSkill(CharacterSkillSlot.R).Id); }
    }
    #endregion
}