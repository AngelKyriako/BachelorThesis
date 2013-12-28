using UnityEngine;
using System.Collections;

public class SilenceEffect: LastingEffect {

    public override void Activate() {
        base.Activate();
        Receiver.IsSilenced = true;
    }

    public override void Deactivate() {
        Receiver.IsSilenced = false;
        base.Deactivate();
    }
}
