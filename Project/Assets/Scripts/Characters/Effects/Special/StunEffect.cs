using UnityEngine;
using System.Collections;

public class StunEffect: LastingEffect {

    public override void Activate() {
        base.Activate();
        Receiver.IsStunned = true;
    }

    public override void Deactivate() {
        Receiver.IsStunned = false;
        base.Deactivate();
    }
}
