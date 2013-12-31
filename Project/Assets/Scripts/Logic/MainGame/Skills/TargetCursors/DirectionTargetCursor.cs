using UnityEngine;
using System.Collections;

public class DirectionTargetCursor: BaseTargetCursor {

    public override Vector3 Destination {
        get { return transform.position - GameManager.Instance.MyCharacter.transform.position; }
    }
}
