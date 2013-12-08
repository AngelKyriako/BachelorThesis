using UnityEngine;
using System.Collections;

public class StageInitializer: MonoBehaviour {

    void Awake() {
        GameManager.Instance.InitMainStage();
        MonoBehaviour.Destroy(this);
	}

}
