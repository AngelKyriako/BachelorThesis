public enum StatType {
    STR,
    STA,
    AGI,
    INT,
    CHA
}

public class Stat: BaseStat{
    public Stat(string _name, string _desc, float _val) : base(_name, _desc, _val) { }
}