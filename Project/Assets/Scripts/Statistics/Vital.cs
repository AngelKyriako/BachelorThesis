public enum VitalType {
    Health,
    Mana
}

public class Vital: Attribute {

    private int currentValue;

    public Vital(): base() {
        currentValue = base.FinalValue;
    }

    public int CurrentValue {
        get { return currentValue; }
        set {
            if (value > base.FinalValue)
                currentValue = base.FinalValue;
            else if (value < 0)
                currentValue = 0;
            else
                currentValue = value;
        }
    }
}