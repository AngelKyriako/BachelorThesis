using UnityEngine;
using System.Collections;

public class RoomNetController: BaseNetController {

    private MainRoom room;

    public override void Awake() {
        base.Awake();
        room = GetComponent<MainRoom>();
    }
}
