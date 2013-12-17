using UnityEngine;
using System.Collections;

public class StageInitializer: MonoBehaviour {

    public bool buildingStage = true;

    void Awake() {
        if(!buildingStage)
            GameManager.Instance.InitMainStage();
        MonoBehaviour.Destroy(this);
	}
}
