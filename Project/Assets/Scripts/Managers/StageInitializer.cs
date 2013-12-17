using UnityEngine;
using System.Collections;

public class StageInitializer: MonoBehaviour {

    public bool testingMode = true;

    void Awake() {
        if (!testingMode)
            GameManager.Instance.InitMainStage();
        MonoBehaviour.Destroy(this);
	}
}
