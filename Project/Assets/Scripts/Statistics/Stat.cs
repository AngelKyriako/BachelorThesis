public enum StatType {
    Strength,
    Agility,
    Stamina,
    Intelligence,
    Charisma
}

public class Stat: BaseStatistic{
    public Stat(): base(){ }
    public Stat(string _name, string _desc, int _val) : base(_name, _desc, _val) { }
}