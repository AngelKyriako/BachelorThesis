using UnityEngine;
using System.Collections;

public class DFCharacterModel: SingletonMono<DFCharacterModel> {

    private const int DEFAULT_INT = 100;
    private const string DEFAULT_STRING = "";

    private PlayerCharacterModel myModel;

    private DFCharacterModel() { }

    void Start () {
        myModel = GameManager.Instance.MyCharacterModel;
	}

    #region Stats
    #endregion

    #region Vitals
    public float CurrentHealth {
        get { return myModel != null ? myModel.GetVital(0).CurrentValue : DEFAULT_INT; }
    }
    public float MaxHealth {
        get { return myModel != null ? myModel.GetVital(0).FinalValue : DEFAULT_INT; }
    }
    public string HealthName {
        get { return myModel != null ? myModel.GetVital(0).Name : DEFAULT_STRING; }
    }
    public string HealthMaxValue {
        get { return myModel != null ? myModel.GetVital(0).DisplayFinalValue : DEFAULT_STRING; }
    }

    public float CurrentMana {
        get { return myModel != null ? myModel.GetVital(1).CurrentValue : DEFAULT_INT; }
    }
    public float MaxMana {
        get { return myModel != null ? myModel.GetVital(1).FinalValue : DEFAULT_INT; }
    }
    public string ManaName {
        get { return myModel != null ? myModel.GetVital(1).Name : DEFAULT_STRING; }
    }
    public string ManaMaxValue {
        get { return myModel != null ? myModel.GetVital(1).DisplayFinalValue : DEFAULT_STRING; }
    }

    public uint CurrentExp {
        get { return myModel != null ? myModel.CurrentExp : DEFAULT_INT; }
    }
    public uint MaxExp {
        get { return myModel != null ? myModel.ExpToLevel : DEFAULT_INT; }
    }
    public string CurrentExpStr {
        get { return myModel != null ? myModel.CurrentExp.ToString() : DEFAULT_STRING; }
    }
    public string MaxExpStr {
        get { return myModel != null ? myModel.ExpToLevel.ToString() : DEFAULT_STRING; }
    }
    #endregion

    #region Attributes
    public string AttributeName0 {
        get { return myModel != null ? myModel.GetAttribute(0).Name : DEFAULT_STRING; }
    }
    public string AttributeValue0 {
        get { return myModel != null ? myModel.GetAttribute(0).DisplayFinalValue : DEFAULT_STRING; }
    }

    public string AttributeName1 {
        get { return myModel.GetAttribute(1).Name; }
    }
    public string AttributeValue1 {
        get { return myModel.GetAttribute(1).DisplayFinalValue; }
    }

    public string AttributeName2 {
        get { return myModel.GetAttribute(2).Name; }
    }
    public string AttributeValue2 {
        get { return myModel.GetAttribute(2).DisplayFinalValue; }
    }

    public string AttributeName3 {
        get { return myModel.GetAttribute(3).Name; }
    }
    public string AttributeValue3 {
        get { return myModel.GetAttribute(3).DisplayFinalValue; }
    }

    public string AttributeName4 {
        get { return myModel.GetAttribute(4).Name; }
    }
    public string AttributeValue4 {
        get { return myModel.GetAttribute(4).DisplayFinalValue; }
    }

    public string AttributeName5 {
        get { return myModel.GetAttribute(5).Name; }
    }
    public string AttributeValue5 {
        get { return myModel.GetAttribute(5).DisplayFinalValue; }
    }

    public string AttributeName6 {
        get { return myModel.GetAttribute(6).Name; }
    }
    public string AttributeValue6 {
        get { return myModel.GetAttribute(6).DisplayFinalValue; }
    }

    public string AttributeName7 {
        get { return myModel.GetAttribute(7).Name; }
    }
    public string AttributeValue7 {
        get { return myModel.GetAttribute(7).DisplayFinalValue; }
    }

    public string AttributeName8 {
        get { return myModel.GetAttribute(8).Name; }
    }
    public string AttributeValue8 {
        get { return myModel.GetAttribute(8).DisplayFinalValue; }
    }

    public string AttributeName9 {
        get { return myModel.GetAttribute(9).Name; }
    }
    public string AttributeValue9 {
        get { return myModel.GetAttribute(9).DisplayFinalValue; }
    }

    public string AttributeName10 {
        get { return myModel.GetAttribute(10).Name; }
    }
    public string AttributeValue10 {
        get { return myModel.GetAttribute(10).DisplayFinalValue; }
    }

    public string AttributeName11 {
        get { return myModel.GetAttribute(11).Name; }
    }
    public string AttributeValue11 {
        get { return myModel.GetAttribute(11).DisplayFinalValue; }
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