using UnityEngine;
using System.Collections;

public class DFCharacterModel: SingletonMono<DFCharacterModel> {

    private const int DEFAULT_VITAL = 100;
    
    private PlayerCharacterModel myModel;

    private DFCharacterModel() { }

    void Start () {
        myModel = GameManager.Instance.MyCharacterModel;
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

    #region Set Action Skills
    public static void SetActionSkill(CharacterSkillSlot _slot, int _id){
        ClearActionSkill(_slot, _id);
        GameManager.Instance.MyCharacterModel.AddSkill(_slot, SkillBook.Instance.GetSkill(_id));
        SkillBook.Instance.SetSkillAvailable(_id, false);
    }
    public static void ClearActionSkill(CharacterSkillSlot _slot, int _id) {
        if (GameManager.Instance.MyCharacterModel.SkillExists(_slot)) {
            SkillBook.Instance.SetSkillAvailable(GameManager.Instance.MyCharacterModel.GetSkill(_slot).Id, true);
            GameManager.Instance.MyCharacterModel.RemoveSkill(_slot);
        }
    }
    #endregion
}