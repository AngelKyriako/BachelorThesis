﻿public enum StatType {
    Strength,
    Agility,
    Stamina,
    Intelligence,
    Charisma
}

public class Stat: BaseStat{
    public Stat(string _name, string _desc, float _val) : base(_name, _desc, _val) { }
}