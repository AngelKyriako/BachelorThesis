using UnityEngine;

public abstract class DirectEffect: StatModifierEffect {

    public DirectEffect()
        : base() {
    }

    public DirectEffect(string _title, string _descr, Texture2D _icon)
        : base(_title, _descr, _icon) {
    }

}
