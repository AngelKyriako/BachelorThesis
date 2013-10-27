public class BaseStatistic {

    private int baseValue, buffValue;

    public BaseStatistic() {
        baseValue = 0;
        buffValue = 0;
    }

    public BaseStatistic(int baseV) {
        baseValue = baseV;
        buffValue = 0;
    }

    public virtual int FinalValue {
        get { return baseValue + buffValue; }
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