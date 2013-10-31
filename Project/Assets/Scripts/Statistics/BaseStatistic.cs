public class BaseStatistic {

    private string name, description;
    private int baseValue, buffValue;

    public BaseStatistic() {
        name = string.Empty;
        description = string.Empty;
        baseValue = 0;
        buffValue = 0;
    }

    public BaseStatistic(string _name, string _desc, int _val) {
        name = _name;
        description = _desc;
        baseValue = _val;
        buffValue = 0;
    }

    public virtual int FinalValue {
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
    public int BaseValue {
        get { return baseValue; }
        set { baseValue = value; }
    }
    public int BuffValue {
        get { return buffValue; }
        set { buffValue = value; }
    }
}