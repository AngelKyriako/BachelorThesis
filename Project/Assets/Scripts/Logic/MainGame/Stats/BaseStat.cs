public class BaseStat {

    private string name, description;
    private float baseValue, buffValue;

    public BaseStat(string _name, string _desc, float _val) {
        name = _name;
        description = _desc;
        baseValue = _val;
        buffValue = 0;
    }

    public virtual float FinalValue {
        get { return baseValue + buffValue; }
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }
    public string Description {
        get { return description; }
        set { description = value; }
    }
    public float BaseValue {
        get { return baseValue; }
        set { baseValue = value; }
    }
    public float BuffValue {
        get { return buffValue; }
        set { buffValue = value; }
    }
}