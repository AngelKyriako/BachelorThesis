using UnityEngine;
using System;

public class DFCharacterModel: SingletonMono<DFCharacterModel> {

    private const int DEFAULT_INT = 100;
    private const string DEFAULT_STRING = "";

    private PlayerCharacterModel myModel;
    private StatsUpdater updater;

    private DFCharacterModel() { }

    void Start () {
        myModel = GameManager.Instance.MyCharacterModel;
        updater = new StatsUpdater(myModel.StatsLength);
	}

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

    public void UpdateStatButtonClick(int _index) {
        updater.ToggleStatUpdatingState(_index);
    }

    public void SaveUpdatedStats() {
        updater.SaveUpdatedStats();
    }

    public class StatsUpdater {

        private int updatesCount;
        private bool[] currentlyUpdatingStats;

        public StatsUpdater(int _statsLength) {
            updatesCount = 0;
            currentlyUpdatingStats = new bool[_statsLength];
            for (int i = 0; i < currentlyUpdatingStats.Length; ++i)
                currentlyUpdatingStats[i] = false;
        }

        public void ToggleStatUpdatingState(int _index) {
            if (!currentlyUpdatingStats[_index] && GameManager.Instance.MyCharacterModel.TrainingPoints != 0) {
                currentlyUpdatingStats[_index] = true;
                --GameManager.Instance.MyCharacterModel.TrainingPoints;
            }
            else if (currentlyUpdatingStats[_index]) {
                currentlyUpdatingStats[_index] = false;
                ++GameManager.Instance.MyCharacterModel.TrainingPoints;
            }
        }

        public void SaveUpdatedStats() {
            if (PlayerCanUpdate) {
                for (int i = 0; i < currentlyUpdatingStats.Length; ++i)
                    if (currentlyUpdatingStats[i]) {
                        ++GameManager.Instance.MyCharacterModel.GetStat(i).BaseValue;
                        currentlyUpdatingStats[i] = false;
                    }
                ++updatesCount;
                GameManager.Instance.MyCharacterModel.UpdateAttributesBasedOnStats();
                GameManager.Instance.MyCharacterModel.UpdateVitalsBasedOnStats();
            }
            else
                GUIMessageDisplay.Instance.AddMessage("No more updates left");
        }

        public string StatUpdateText(int _index) {
            return PlayerCanUpdate ? (!currentlyUpdatingStats[_index] ? "+" : "+1") : "";
        }

        public bool PlayerCanUpdate {
            get { return updatesCount < GameManager.Instance.MyCharacterModel.Level; }
        }
    }

    // Binding Properties
    #region Character window
    #region Stats
    public string TrainingPoints {
        get { return myModel != null && myModel.TrainingPoints != 0 ? "Training points left: " + myModel.TrainingPoints.ToString() : ""; }
    }

    public string StatValue0 {
        get { return GetStatValue(0); }
    }
    public string StatUpdateState0 {
        get { return updater != null ? updater.StatUpdateText(0) : "+"; }
    }

    public string StatValue1 {
        get { return GetStatValue(1); }
    }
    public string StatUpdateState1 {
        get { return updater != null ? updater.StatUpdateText(1) : "+"; }
    }

    public string StatValue2 {
        get { return GetStatValue(2); }
    }
    public string StatUpdateState2 {
        get { return updater != null ? updater.StatUpdateText(2) : "+"; }
    }

    public string StatValue3 {
        get { return GetStatValue(3); }
    }
    public string StatUpdateState3 {
        get { return updater != null ? updater.StatUpdateText(3) : "+"; }
    }

    public string StatValue4 {
        get { return GetStatValue(4); }
    }
    public string StatUpdateState4 {
        get { return updater != null ? updater.StatUpdateText(4) : "+"; }
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
        get { return VitalName(0); }
    }
    public string HealthValue {
        get { return VitalValueStr(0); }
    }

    public float CurrentMana {
        get { return VitalCurrent(1); }
    }
    public float MaxMana {
        get { return VitalFinal(1); }
    }
    public string ManaName {
        get { return VitalName(1); }
    }
    public string ManaValue {
        get { return VitalValueStr(1); }
    }

    private float VitalCurrent(int _index) {
        return myModel != null ? myModel.GetVital(_index).CurrentValue : DEFAULT_INT;
    }
    private float VitalFinal(int _index) {
        return myModel != null ? myModel.GetVital(_index).FinalValue : DEFAULT_INT;
    }
    private string VitalName(int _index) {
        return myModel != null ? myModel.GetVital(_index).Name : DEFAULT_STRING;
    }
    private string VitalValueStr(int _index) {
        return myModel != null ? myModel.GetVital(_index).ToString() : DEFAULT_STRING;
    }

    public uint CurrentExp {
        get { return myModel != null ? myModel.CurrentExp : DEFAULT_INT; }
    }
    public uint MaxExp {
        get { return myModel != null ? myModel.ExpToLevel : DEFAULT_INT; }
    }
    public string ExpStr {
        get { return myModel != null ? myModel.CurrentExp.ToString() + "/" + myModel.ExpToLevel.ToString() : DEFAULT_STRING; }
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

    public string PlayerName {
        get { return PhotonNetwork.player.name; }
    }
    public string LevelStr {
        get { return myModel != null ? "level " + myModel.Level : DEFAULT_STRING; }
    }
    public bool StatsCanBeUpdated {
        get { return updater != null && myModel != null ? updater.PlayerCanUpdate && myModel.TrainingPoints > 0 : true; }
    }
    #endregion

    #region RespawnTimer Window
    public string RespawnTimer {
        get { return myModel != null ? Utilities.Instance.TimeCounterDisplay(myModel.RespawnTimer) : string.Empty; }
    }

    public bool IsPlayerRespawning {
        get { return myModel != null && myModel.RespawnTimer != 0; }
    }
    #endregion

    // Statistics Accessors
    #region Player
    public Color PlayerInfoColor(string _id) {
        return GameManager.Instance.GetPlayerRGBColor(_id);
    }

    public string PlayerInfoName(string _id) {
        return GameManager.Instance.GetPlayerName(_id);
    }

    public string PlayerKills(string _id) {
        return "[color #00ff00]" + GameManager.Instance.GetPlayerModel(_id).Kills + "[/color]";
        ;
    }

    public string PlayerDeaths(string _id) {
        return "/ [color #ff0000]" + GameManager.Instance.GetPlayerModel(_id).Deaths + "[/color]";
    }

    public string PlayerAssists(string _id) {
        return "";//"/ [color #0000ff]" + GameManager.Instance.GetPlayerModel(_id).Assists + "[/color]";
    }

    public string PlayerTeam(string _id) {
        return GameManager.Instance.GetPlayerTeam(_id).ToString();
    }
    #endregion

    #region Team
    public string TeamName(PlayerTeam _team) {
        return _team.ToString();
    }
    public string TeamKills(PlayerTeam _team) {
        return "[color #00ff00]" + GameManager.Instance.TeamKills((int)_team).ToString() + "[/color]";;
    }
    public string TeamDeaths(PlayerTeam _team) {
        return "[color #ff0000]" + "" + "[/color]";
    }
    public string TeamAssists(PlayerTeam _team) {
        return "[color #ff0000]" + "" + "[/color]";
    }
    #endregion
}