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
    public string StatName0 {
        get { return GetStatName(0); }
    }
    public string StatValue0 {
        get { return GetStatValue(0); }
    }

    public string StatName1 {
        get { return GetStatName(1); }
    }
    public string StatValue1 {
        get { return GetStatValue(1); }
    }

    public string StatName2 {
        get { return GetStatName(2); }
    }
    public string StatValue2 {
        get { return GetStatValue(2); }
    }

    public string StatName3 {
        get { return GetStatName(3); }
    }
    public string StatValue3 {
        get { return GetStatValue(3); }
    }

    public string StatName4 {
        get { return GetStatName(4); }
    }
    public string StatValue4 {
        get { return GetStatValue(4); }
    }
    private string GetStatName(int _index) {
        return myModel != null ? myModel.GetStat(_index).Name : DEFAULT_STRING;
    }
    private string GetStatValue(int _index) {
        return myModel != null ? myModel.GetStat(_index).DisplayFinalValue : DEFAULT_STRING;
    }
    #endregion

    #region Vitals

    public float CurrentHealth {
        get { return VitalCurrent(0); }
    }
    public float MaxHealth {
        get { return VitalFinal(0); }
    }
    public string HealthName {
        get { return VitalCurrentStr(0); }
    }
    public string HealthMaxValue {
        get { return VitalFinalStr(0); }
    }

    public float CurrentMana {
        get { return VitalCurrent(1); }
    }
    public float MaxMana {
        get { return VitalFinal(1); }
    }
    public string ManaName {
        get { return VitalCurrentStr(1); }
    }
    public string ManaMaxValue {
        get { return VitalFinalStr(1); }
    }

    private float VitalCurrent(int _index) {
        return myModel != null ? myModel.GetVital(_index).CurrentValue : DEFAULT_INT;
    }
    private float VitalFinal(int _index) {
        return myModel != null ? myModel.GetVital(_index).FinalValue : DEFAULT_INT;
    }
    private string VitalCurrentStr(int _index) {
        return myModel != null ? myModel.GetVital(_index).Name : DEFAULT_STRING;
    }
    private string VitalFinalStr(int _index) {
        return myModel != null ? myModel.GetVital(_index).DisplayFinalValue : DEFAULT_STRING;
    }
    #endregion

    #region Exp
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
        get { return GetAttributeName(0); }
    }
    public string AttributeValue0 {
        get { return GetAttributeValue(0); }
    }

    public string AttributeName1 {
        get { return GetAttributeName(1); }
    }
    public string AttributeValue1 {
        get { return GetAttributeValue(1); }
    }

    public string AttributeName2 {
        get { return GetAttributeName(2); }
    }
    public string AttributeValue2 {
        get { return GetAttributeValue(2); }
    }

    public string AttributeName3 {
        get { return GetAttributeName(3); }
    }
    public string AttributeValue3 {
        get { return GetAttributeValue(3); }
    }

    public string AttributeName4 {
        get { return GetAttributeName(4); }
    }
    public string AttributeValue4 {
        get { return GetAttributeValue(4); }
    }

    public string AttributeName5 {
        get { return GetAttributeName(5); }
    }
    public string AttributeValue5 {
        get { return GetAttributeValue(5); }
    }

    public string AttributeName6 {
        get { return GetAttributeName(6); }
    }
    public string AttributeValue6 {
        get { return GetAttributeValue(6); }
    }

    public string AttributeName7 {
        get { return GetAttributeName(7); }
    }
    public string AttributeValue7 {
        get { return GetAttributeValue(7); }
    }

    public string AttributeName8 {
        get { return GetAttributeName(8); }
    }
    public string AttributeValue8 {
        get { return GetAttributeValue(8); }
    }

    public string AttributeName9 {
        get { return GetAttributeName(9); }
    }
    public string AttributeValue9 {
        get { return GetAttributeValue(9); }
    }

    public string AttributeName10 {
        get { return GetAttributeName(10); }
    }
    public string AttributeValue10 {
        get { return GetAttributeValue(10); }
    }

    public string AttributeName11 {
        get { return GetAttributeName(11); }
    }
    public string AttributeValue11 {
        get { return GetAttributeValue(11); }
    }

    private string GetAttributeName(int _index) {
        return myModel != null ? myModel.GetAttribute(_index).Name : DEFAULT_STRING;
    }
    private string GetAttributeValue(int _index) {
        return myModel != null ? myModel.GetAttribute(_index).DisplayFinalValue : DEFAULT_STRING;
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